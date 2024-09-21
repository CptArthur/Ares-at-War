using System;
using System.Collections.Generic;
using AresAtWar.API;
using AresAtWar.War;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
using static AresAtWar.War.WarSim;

namespace AresAtWar.SessionCore
{
    public class CustomMissionMapping
    {
        public static List<string> ActiveDestinations = new List<string>();
        public struct TransportMissionType
        {
            public string Name;
            public string Description;
            public int BaseReward;
            public int Difficulty;
            public float CollateralPercentage;
            public int ReputationReward;
            public int FailReputationPrice;

            public TransportMissionType(string name, string description, int baseReward, int difficulty, float collateral, int reputationReward, int failReputationPrice)
            {
                Name = name;
                Description = description;
                BaseReward = baseReward;
                Difficulty = difficulty;
                CollateralPercentage = collateral;
                ReputationReward = reputationReward;
                FailReputationPrice = failReputationPrice;
            }
        }

        private static readonly List<TransportMissionType> MissionTypes = new List<TransportMissionType>
        {
            // Medical Emergency in a Military Zone
            /*
            new TransportMissionType(
                "Medical Emergency Response",
                "Urgently deliver medical supplies. Failure will result in high collateral damage.",
                60000, 3, 20000, 30, 10
            ),
            */
            
            // Cargo Shipment in Urban Area
            new TransportMissionType(
                "Cargo Delivery",
                "Deliver a shipment.",
                55000, 2, 0.05f, 10, 5
            ),


        };


        public static Dictionary<string, string> InternalMissionMapping(string profileSubtypeId, string spawnGroupName, List<string> Tags, Vector3D storeblockposition)
        {
            if (Tags.Contains("Transport"))
            {
                return TransportMissionMapping(profileSubtypeId, spawnGroupName, Tags, storeblockposition);
            }

            return new Dictionary<string, string>();
        }

        public static Dictionary<string, string> TransportMissionMapping(string profileSubtypeId, string spawnGroupName, List<string> Tags, Vector3D storeblockposition)
        {
            var startingEncounter = GetStaticEncounter(spawnGroupName, storeblockposition);
            var destinationEncounter = GetStaticEncounter(GetNextDestination(startingEncounter.Spawngroupname), storeblockposition);

            var mapping = new Dictionary<string, string>();


            if (destinationEncounter != null)
            {
                var missionType = GetRandomMissionType(startingEncounter, destinationEncounter);
                var distance = Vector3D.Distance(startingEncounter.Coords, destinationEncounter.Coords);
                var reward = CalculateReward(missionType.BaseReward, distance, missionType.Difficulty);

                // Calculate duration based on speed (5 meters per second)
                const double speedMetersPerSecond = 5.0;
                var durationMinutes = (distance / speedMetersPerSecond) / 60.0; // Convert seconds to minutes

                int Collateral = (int)(reward * missionType.CollateralPercentage);

                mapping.Add("{DestinationLocation}", $"{{X:{destinationEncounter.Coords.X} Y:{destinationEncounter.Coords.Y} Z:{destinationEncounter.Coords.Z}}}");
                mapping.Add("{EventName}", $"{missionType.Name} to {destinationEncounter.Spawngroupname}");
                mapping.Add("{Description}", $"{missionType.Description} Distance: {distance:F2} km.");
                mapping.Add("{Reward}", $"{reward}");
                mapping.Add("{Collateral}", $"{Collateral}");
                mapping.Add("{ReputationReward}", $"{missionType.ReputationReward}");
                mapping.Add("{FailReputationPrice}", $"{missionType.FailReputationPrice}");
                mapping.Add("{Duration}", $"{(int)durationMinutes}");

            }

            return mapping;
        }

        private static TransportMissionType GetRandomMissionType(StaticEncounter from, StaticEncounter to)
        {


            //Sameplanet offplanet 

            
            int index = AaWSessionCore.GetRandomNumber(0, MissionTypes.Count);
            return MissionTypes[index];
        }

        private static int CalculateReward(int baseReward, double distance, int difficulty)
        {
            // Example reward calculation
            int distanceBonus = (int)(distance / 100) * 1000; // Example: 1000 units per 100 km
            int difficultyMultiplier = difficulty; // Reward increases with difficulty
            return baseReward + distanceBonus * difficultyMultiplier;
        }

        public static string GetNextDestination(string currentLocation)
        {
            List<string> possibleDestinations = new List<string>();
            List<int> destinationWeights = new List<int>();
            int totalWeight = 0;

            foreach (var tradeRoute in _TradeRoutes)
            {
                if (tradeRoute.Locations.Contains(currentLocation))
                {
                    foreach (var location in tradeRoute.Locations)
                    {
                        if (location == currentLocation) continue;

                        if (ActiveDestinations.Contains(currentLocation + location)) continue;

                        possibleDestinations.Add(location);
                        destinationWeights.Add(tradeRoute.Importance);
                        totalWeight += tradeRoute.Importance;
                    }
                }
            }

            if (totalWeight == 0)
                return null;

            int randomValue = AaWSessionCore.GetRandomNumber(0, totalWeight);
            int cumulativeWeight = 0;

            for (int i = 0; i < possibleDestinations.Count; i++)
            {
                cumulativeWeight += destinationWeights[i];
                if (randomValue < cumulativeWeight)
                {
                    ActiveDestinations.Add(currentLocation + possibleDestinations[i]);
                    AaWSessionCore.counter3 = 0;
                    return possibleDestinations[i];
                }
            }

            return null;
        }

        public static StaticEncounter GetStaticEncounter(string spawnGroupName, Vector3D position)
        {
            foreach (var staticEncounter in _staticEncounters)
            {
                if (staticEncounter.Spawngroupname == spawnGroupName)
                {
                    return staticEncounter;
                }
            }

            MyVisualScriptLogicProvider.ShowNotificationToAll($"Couldn't find {spawnGroupName}, trying to get the closest", 7000, "Red");

            return GetClosestStaticEncounter(position);
        }

        public static StaticEncounter GetClosestStaticEncounter(Vector3D position)
        {
            StaticEncounter closestEncounter = null;
            double closestDistance = double.MaxValue;

            foreach (var staticEncounter in _staticEncounters)
            {
                double distance = Vector3D.Distance(staticEncounter.Coords, position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEncounter = staticEncounter;
                }
            }

            return closestEncounter;
        }
    }
}
