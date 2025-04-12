using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class CustomMissionMapping
    {
        public static List<string> ActiveDestinations = new List<string>();
        public static List<string> ActiveContracts = new List<string>();

        public enum ContextMode
        {
            WarFrontLine, //For example AHE-HQ, in UNION controlled. This is on the front
            WarSupport,//For example AHE-HQ, in UNION controlled, but far away War. Supply missions to front
            Peace, //For example AHE-HQ, in UNION controlled, no War
            HoldingUndermineWarEffort, //For example SYN in CIV but in enemy Node
            Holding // For example SYN in CIV but in peace with GC Node. Can still smuggle things to the front
        }
        public enum MissionType
        {
            Transport,
            SupplyMission,
            Bounty,
            Smuggle
        }

        public struct MissionData
        {
            public MissionType Type;
            public List<string> Tags;
            public string Name;
            public string Description;
            public int BaseReward;
            public int Difficulty;
            public float CollateralPercentage;
            public int ReputationReward;
            public int FailReputationPrice;

            public MissionData(MissionType type, List<string> tags, string name, string description, int baseReward, int difficulty, float collateral, int reputationReward, int failReputationPrice)
            {
                Type = type;
                Tags = tags;
                Name = name;
                Description = description;
                BaseReward = baseReward;
                Difficulty = difficulty;
                CollateralPercentage = collateral;
                ReputationReward = reputationReward;
                FailReputationPrice = failReputationPrice;
            }
        }



        private static readonly List<MissionData> MissionDatas = new List<MissionData>
        {

            //Transport
            new MissionData(
                MissionType.Smuggle,
                new List<string>(),
                "Smuggle Cargo to {destinationEncounter}",
                "Ensure timely and safe delivery of the shipment to {destinationEncounter}",
                55000, 2, 0.05f, 10, 5
            ),


            //Transport
            new MissionData(
                MissionType.Transport,
                new List<string>(),
                "Deliver Cargo to {destinationEncounter}",
                "Ensure timely and safe delivery of the shipment to {destinationEncounter}",
                55000, 2, 0.05f, 10, 5
            ),

           new MissionData(
                MissionType.Transport,
                new List<string>{"Settlement"},
                "Deliver civilian cargo to {destinationEncounter}",
                "Ensure timely and safe delivery of the shipment to {destinationEncounter}",
                55000, 2, 0.05f, 10, 5
            ),


            new MissionData(
                MissionType.Transport,
                new List<string>{"MilitaryBase"},
                "Deliver Military supplies to {destinationEncounter}",
                "Ensure timely and safe delivery of the shipment to {destinationEncounter}",
                60000, 3, 0.2f, 20, 10
            ),


            //Bounty
            new MissionData(
                MissionType.Bounty,
                new List<string>(),
                "Bounty Hunt",
                "Track down and eliminate the leader of a local rebel group causing unrest.",
                80000, 4, 0.1f, 15, 10
            ),
            new MissionData(
                MissionType.Bounty,
                new List<string>{"Urban", },
                "Urban Gang Bust",
                "Assist in a high-profile operation to apprehend a notorious gang in the city.",
                70000, 3, 0.07f, 12, 8
            ),

        };



        private static MissionData GetRandomMissionData(MissionType missionType,List<string> tags)
        {
            List<MissionData> matchingMissions = new List<MissionData>();

            foreach (var mission in MissionDatas)
            {
                if (mission.Type != missionType)
                    continue;

                // Check if all tags in the mission data are present in the input tags
                bool allTagsMatch = true;
                foreach (var missionTag in mission.Tags)
                {
                    if (!tags.Contains(missionTag))
                    {
                        allTagsMatch = false;
                        break;
                    }
                }

                // Add the mission to the list if all tags match
                if (allTagsMatch)
                {
                    // Add the mission multiple times based on the number of matched tags
                    for (int i = 0; i < 1 + mission.Tags.Count; i++)
                    {
                        matchingMissions.Add(mission);
                    }
                }
            }

            // If no matches, return a random mission as a fallback
            if (matchingMissions.Count == 0)
            {
                MyVisualScriptLogicProvider.SendChatMessage($"Can't find mission data for {missionType}", "AaW");
                return MissionDatas[AaWSessionCore.GetRandomNumber(0, MissionDatas.Count)];
            }

            // Return a random mission from the filtered list
            int index = AaWSessionCore.GetRandomNumber(0, matchingMissions.Count);
            return matchingMissions[index];
        }


        public static Dictionary<string, string> InternalMissionMapping(string profileSubtypeId, string spawnGroupName, List<string> Tags, Vector3D storeblockposition)
        {






            //Scrapmissions
            if (Tags.Contains("SCRAP"))
            {
                return ScrapMapping(storeblockposition);
            }

            //Scrapmissions
            if (Tags.Contains("HeavySCRAP"))
            {
                return ScrapMapping(storeblockposition, true);
            }

            if (profileSubtypeId == "SOLCOOP_Mission_RaidXolAsteroidBase")
            {
                return SOLCOOP_Mission_RaidXolAsteroidBase();
            }



            //other
            var startingEncounter = GetStaticEncounter(spawnGroupName, storeblockposition);
            string Faction = FindOutFaction(Tags);
            string EncounterFaction = startingEncounter.FactionTag;
            string NodeFaction = startingEncounter.Node.Faction.Tag;

            ContextMode context = ContextMode.Peace;

            if (Faction == EncounterFaction)
            {
                //UNION in UNION node
                var factionobj = _allfactions.FirstOrDefault(n => n.Tag == Faction);

                if (factionobj.AtWar())
                {
                    if (startingEncounter.Node.FrontLineCount > 0)
                    {
                        context = ContextMode.WarFrontLine;
                    }
                    else
                    {
                        context = ContextMode.WarSupport;
                    }

                }
                else
                {
                    context = ContextMode.Peace;
                }

                

            }
            else
            {
                //Not the same, so SYN in CIV for example.
                if (WarSim.FactionsAtWar(Faction, NodeFaction))
                {
                    //SYN in CIV at war with GC in GC
                    context = ContextMode.HoldingUndermineWarEffort;

                }
                else
                {
                    context = ContextMode.Holding;
                }
            }




            //
            if (Tags.Contains("CIVILIAN"))
            {
                var target = spawnGroupName + "CIVILIAN" + MissionType.Transport.ToString();
                int count = ActiveContracts.Count(contract => contract == target);


                ActiveContracts.Add(spawnGroupName + "CIVILIAN" + MissionType.Transport.ToString());
                return TransportMissionMapping(spawnGroupName, "CIVILIAN", storeblockposition);
            }

            if (Tags.Contains("SYN"))
            {

                int countsmuggle = ActiveContracts.Count(contract => contract == spawnGroupName + "SYN" + MissionType.Smuggle.ToString());
                if (countsmuggle < 1)
                {
                    ActiveContracts.Add(spawnGroupName + "SYN" + MissionType.Smuggle.ToString());
                    AaWSessionCore.counter3 = 0;
                    return SmuggleMissionMapping(spawnGroupName, "SYN", storeblockposition);
                }


                int countbounty = ActiveContracts.Count(contract => contract == spawnGroupName + "SYN" + MissionType.Bounty.ToString());
                if (countbounty < 2)
                {
                    ActiveContracts.Add(spawnGroupName + "SYN" + MissionType.Bounty.ToString());
                    AaWSessionCore.counter3 = 0;
                    return BountyMissionMapping("SYN", storeblockposition);
                }


                return null;
            }

            if (Tags.Contains("ITC"))
            {
                return TransportMissionMapping(spawnGroupName, "ITC", storeblockposition);
            }





            return null;
        }

        internal static string FindOutFaction(List<string> Tags)
        {
            if (Tags.Contains("CIVILIAN"))
                return "CIVILIAN";

            if (Tags.Contains("SYN"))
                return "SYN";

            if (Tags.Contains("UNION"))
                return "UNION";

            if (Tags.Contains("GC"))
                return "GC";

            

            return "CIVILIAN";
        }


        public static Dictionary<string, string> ScrapMapping(Vector3D storeblockposition, bool heavy=false)
        {
            var mapping = new Dictionary<string, string>();


            var targetnodde = GetClosestNodeWithFeel(storeblockposition, Feel.Barren);

            if (targetnodde == null)
                return null;

            int minDistance = 35000;

            int maxDistance = 60000;

            if (heavy)
            {
                minDistance = 100000;
                maxDistance = 300000;
            }



            var targetlocation = targetnodde.GetRandomPoint(storeblockposition, minDistance, maxDistance);

            if (targetlocation == null)
                return null;

            var distance = Vector3D.Distance(storeblockposition, targetlocation);

            if (heavy)
            {
                mapping.Add("{EventSpawner}", $"AaW_Spawner_SpaceHeavyScrap");
                mapping.Add("{Collateral}", $"1500000");
                mapping.Add("{Description}", $"Pay 1,500,000 credits to receive the exact GPS coordinates of valuable scrap. Distance: {distance / 1000:F2} km.");

            }
            else
            {
                mapping.Add("{EventSpawner}", $"AaW_Spawner_SpaceScrap");
                mapping.Add("{Collateral}", $"100000");
                mapping.Add("{Description}", $"Pay 100,000 credits to receive the exact GPS coordinates of valuable scrap. Distance: {distance / 1000:F2} km.");
            }


            mapping.Add("{InstanceEventGroupId}", $"MissionGroup_SpaceScrap");
            mapping.Add("{EventCoords}", $"{{X:{targetlocation.X} Y:{targetlocation.Y} Z:{targetlocation.Z}}}");
            mapping.Add("{EventName}", "Buyable Scrap Location");

            mapping.Add("{Reward}", $"0");
            mapping.Add("{ReputationReward}", $"0");
            mapping.Add("{FailReputationPrice}", $"0");

            return mapping;

        }




        public static Dictionary<string, string> BountyMissionMapping(string Faction, Vector3D storeblockposition)
        {
            var mapping = new Dictionary<string, string>();
            mapping.Add("{InstanceEventGroupId}", $"MissionGroup_BountyHunting");

            var targetnodde = GetClosestNodeWithFeel(storeblockposition, Feel.Barren);

            if (targetnodde == null)
                return null;


            var missiondataTags = new List<string>();

            missiondataTags.Add(targetnodde.Feel.ToString());


            var targetlocation = targetnodde.GetRandomPoint(storeblockposition);

            if (targetlocation == null)
                return null;


            var _missionType = GetRandomMissionData(MissionType.Bounty, missiondataTags);

            var distance = Vector3D.Distance(storeblockposition, targetlocation);

            
            var reward = CalculateReward(_missionType.BaseReward, distance, _missionType.Difficulty);
            int Collateral = (int)(reward * 0.05);


            mapping.Add("{EventCoords}", $"{{X:{targetlocation.X} Y:{targetlocation.Y} Z:{targetlocation.Z}}}");
            mapping.Add("{EventName}", _missionType.Name);
            mapping.Add("{Description}", $"{_missionType.Description}. Distance: {distance/1000:F2} km.");
            mapping.Add("{Reward}", $"{reward}");
            mapping.Add("{Collateral}", $"{Collateral}");
            mapping.Add("{ReputationReward}", $"{_missionType.ReputationReward}");
            mapping.Add("{FailReputationPrice}", $"{_missionType.FailReputationPrice}");

            return mapping;

           
        }
        public static Dictionary<string, string> TransportMissionMapping(string spawnGroupName, string Faction, Vector3D storeblockposition)
        {
            var mapping = new Dictionary<string, string>();

            mapping.Add("{InstanceEventGroupId}", $"MissionGroup_Transport");

            var destinationEncounter = TransportGetDestination(spawnGroupName, Faction);

            var missiondataTags = new List<string>();
            
            if(destinationEncounter.Node != null)
            {
                missiondataTags.Add(destinationEncounter.Node.Feel.ToString());
            }

            missiondataTags.AddList(destinationEncounter.Tags);


            if (destinationEncounter == null)
                return null;


            var _missionType = GetRandomMissionData(MissionType.Transport, missiondataTags);

            var eventname = _missionType.Name.Replace("{destinationEncounter}", destinationEncounter.Spawngroupname);
            var description = _missionType.Description.Replace("{destinationEncounter}", destinationEncounter.Spawngroupname);
            var distance = Vector3D.Distance(storeblockposition, destinationEncounter.Coords);
            var reward = CalculateReward(_missionType.BaseReward, distance, _missionType.Difficulty);
            // Calculate duration based on speed (5 meters per second)
            const double speedMetersPerSecond = 5.0;
            var durationMinutes = (distance / speedMetersPerSecond) / 60.0; // Convert seconds to minutes

            int Collateral = (int)(reward * _missionType.CollateralPercentage);

            mapping.Add("{DestinationLocation}", $"{{X:{destinationEncounter.Coords.X} Y:{destinationEncounter.Coords.Y} Z:{destinationEncounter.Coords.Z}}}");
            mapping.Add("{EventName}", $"{eventname}");
            mapping.Add("{Description}", $"{description} Distance: {distance / 1000:F2} km.");
            mapping.Add("{Reward}", $"{reward}");
            mapping.Add("{Collateral}", $"{Collateral}");
            mapping.Add("{ReputationReward}", $"{_missionType.ReputationReward}");
            mapping.Add("{FailReputationPrice}", $"{_missionType.FailReputationPrice}");
            mapping.Add("{Duration}", $"0");
            mapping.Add("{destinationEncounter}", $"{destinationEncounter.Spawngroupname}");

            return mapping;
        }

        public static Dictionary<string, string> SmuggleMissionMapping(string spawnGroupName, string Faction, Vector3D storeblockposition)
        {
            var mapping = new Dictionary<string, string>();
            mapping.Add("{InstanceEventGroupId}", $"MissionGroup_Transport");

            var destinationEncounter = TransportGetDestination(spawnGroupName, Faction);

            var missiondataTags = new List<string>();

            if (destinationEncounter.Node != null)
            {
                missiondataTags.Add(destinationEncounter.Node.Feel.ToString());
            }

            missiondataTags.AddList(destinationEncounter.Tags);


            if (destinationEncounter == null)
                return null;


            var _missionType = GetRandomMissionData(MissionType.Smuggle, missiondataTags);

            var eventname = _missionType.Name.Replace("{destinationEncounter}", destinationEncounter.Spawngroupname);
            var description = _missionType.Description.Replace("{destinationEncounter}", destinationEncounter.Spawngroupname);
            var distance = Vector3D.Distance(storeblockposition, destinationEncounter.Coords);
            var reward = CalculateReward(_missionType.BaseReward, distance, _missionType.Difficulty);
            // Calculate duration based on speed (5 meters per second)
            const double speedMetersPerSecond = 5.0;
            var durationMinutes = (distance / speedMetersPerSecond) / 60.0; // Convert seconds to minutes

            int Collateral = (int)(reward * _missionType.CollateralPercentage);

            mapping.Add("{DestinationLocation}", $"{{X:{destinationEncounter.Coords.X} Y:{destinationEncounter.Coords.Y} Z:{destinationEncounter.Coords.Z}}}");
            mapping.Add("{EventName}", $"{eventname}");
            mapping.Add("{Description}", $"{description} Distance: {distance:F2} km.");
            mapping.Add("{Reward}", $"{reward}");
            mapping.Add("{Collateral}", $"{Collateral}");
            mapping.Add("{ReputationReward}", $"{_missionType.ReputationReward}");
            mapping.Add("{FailReputationPrice}", $"{_missionType.FailReputationPrice}");
            mapping.Add("{Duration}", $"0");

            return mapping;
        }
        private static int CalculateReward(int baseReward, double distance, int difficulty)
        {
            // Example reward calculation
            int distanceBonus = (int)(distance / 100) * 1000; // Example: 1000 units per 100 km
            int difficultyMultiplier = difficulty; // Reward increases with difficulty
            return baseReward + distanceBonus * difficultyMultiplier;
        }

        public static StaticEncounter TransportGetDestination(string currentLocationString, string Faction)
        {
            List<StaticEncounter> possibleDestinations = new List<StaticEncounter>();
            List<int> destinationWeights = new List<int>();
            int totalWeight = 0;

            var currentLocation = _staticEncounters.FirstOrDefault(n => n.Spawngroupname == currentLocationString);


            foreach (var tradeRoute in _TradeRoutes)
            {
                if(tradeRoute.FactionTags != null)
                {
                    if (!tradeRoute.FactionTags.Contains(Faction))
                        continue;
                }

                if (tradeRoute.Locations.Contains(currentLocation))
                {
                    foreach (var location in tradeRoute.Locations)
                    {
                        if (location.Spawngroupname == currentLocation.Spawngroupname) continue;

                        if (ActiveDestinations.Contains(currentLocation.Spawngroupname + location.Spawngroupname)) continue;

                        if (!location.ReadyToReceiveTransportFromFaction(Faction, currentLocation)) continue;


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
                    ActiveDestinations.Add(currentLocation.Spawngroupname + possibleDestinations[i].Spawngroupname);
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


        public static StaticEncounter GetStaticEncounter(string spawnGroupName)
        {
            foreach (var staticEncounter in _staticEncounters)
            {
                if (staticEncounter.Spawngroupname == spawnGroupName)
                {
                    return staticEncounter;
                }
            }
            MyVisualScriptLogicProvider.SendChatMessage($"Couldn't find {spawnGroupName}, trying to get the closest", "AaW-WarSim");
            return null;
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


        public static Node GetClosestNodeWithFeel(Vector3D position, Feel feel)
        {
            Node closestNode = null;
            double closestDistance = double.MaxValue;

            foreach (var node in _nodes)
            {
                if (node.Feel != feel)
                    continue;

                double distance = Vector3D.Distance(node.Location, position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNode = node;
                }
            }

            return closestNode;
        }





    }






}
