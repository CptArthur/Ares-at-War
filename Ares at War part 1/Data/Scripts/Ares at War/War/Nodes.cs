using System;
using System.Collections.Generic;
using AresAtWar.SessionCore;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
namespace AresAtWar.War
{
    public partial class WarSim
    {
        public enum MarcoLocation
        {
            Bylen,
            Mila,
            Rak
        }

        public enum Planet
        {
            Space,
            Agaris,
            Thora4,
            Lezuno,
            Lorus,
            Moon,
            Crait,
            Europa,
        }

        public enum Feel
        {
            Military,
            Urban,
            Rural,
            Barren
        }



        public class StaticEncounter
        {
            public string NodeId;
            public string Spawngroupname;
            public string FactionTag;
            public Vector3D Coords;
            public bool StarterFort;
            public List<string> Tags;
            private Node _node;

            public Node Node
            {
                get
                {
                    if(_node == null)
                    {
                        foreach (var node in _nodes)
                        {
                            if(node.Id == NodeId)
                            {
                                _node = node;
                                return _node;
                            }

                        }

                        _node = null;
                        return _node;
                    }
                    else
                    {
                        return _node;
                    }

                }
            }

            public StaticEncounter(string nodeId, string spawngroupname, string factionTag, Vector3D coords,bool starterFort, List<string> Tags = null)
            {
                this.NodeId = nodeId;
                this.Spawngroupname = spawngroupname;
                this.FactionTag = factionTag;
                this.Coords = coords;
                this.StarterFort = starterFort;
                this.Tags = Tags ?? new List<string>();

                _staticEncounters.Add(this);
            }


            public bool ReadyToReceiveTransportFromFaction(string sendingFaction, StaticEncounter sendingEncounter)
            {
                if (this.Node.FrontLineCount > 0)
                    return false;

                //ITC at Station27, to AHE HQ (ITC-AHE WAR)
                if (WarSim.FactionsAtWar(sendingFaction, this.FactionTag))
                    return false;

                //ITC at Station27, to Carcosa (ITC-AHE War) 
                if (WarSim.FactionsAtWar(sendingFaction, this.Node.Faction.Tag))
                    return false;


                //No smuggle 
                if (WarSim.FactionsAtWar(sendingEncounter.Node.Faction.Tag, this.Node.Faction.Tag))
                    return false;





                return true;
            }


        }



        public class Node
        {
            public string Id;
            public string FancyName;

            public bool StartNode;

            public int Radius;

            public Vector3D Location;

            public bool Inactive
            {
                get
                {
                    if (StartNode)
                    {
                        bool temp;
                        if (!MyAPIGateway.Utilities.GetVariable<bool>(Id, out temp))
                        {
                            //If it does not exist;
                            MyAPIGateway.Utilities.SetVariable<bool>(Id, true);
                            return false;
                        }

                        if (temp)
                        {
                            return false;

                        }
                        else
                        {
                            //Bool exist, but is false;
                            return true;
                        }
                    }
                    return false;
                }
            }


            private Faction startingFaction;
            private bool _refreshFaction;
            private Faction _faction;

            public Faction Faction {
                get 
                {
                    if (_refreshFaction)
                    {
                        _refreshFaction = false;

                        if (StartNode)
                        {
                            _faction = startingFaction;
                            return _faction;

                        }
                        else
                        {
                            bool foundFaction = false;
                            foreach (var faction in _allfactions)
                            {

                                bool temp;
                                if (!MyAPIGateway.Utilities.GetVariable<bool>(faction.Tag + Id, out temp))
                                {
                                    continue;
                                }

                                if (temp)
                                {
                                    foundFaction = true;
                                    _faction = faction;
                                    break;
                                }
                            }

                            if (!foundFaction)
                            {
                                _faction = startingFaction;

                                MyAPIGateway.Utilities.SetVariable<bool>(startingFaction.Tag + Id, true);
                            }
                                

                            return _faction;
                        }

                    }
                    else
                        return _faction;
                }
                set
                {

                    if (StartNode)
                    {
                        if(value.Tag == startingFaction.Tag)
                        {
                            MyAPIGateway.Utilities.SetVariable<bool>(Id, true);
                        }
                        else
                        {
                            MyAPIGateway.Utilities.SetVariable<bool>(Id, false);
                        }
                    }

                    else
                    {
                        _faction = value;

                        MyAPIGateway.Utilities.SetVariable<bool>(value.Tag + Id, true);

                        // Set the other factions' tags to false
                        foreach (var faction in _allfactions)
                        {
                            if (faction.Tag != value.Tag)
                            {
                                MyAPIGateway.Utilities.SetVariable<bool>(faction.Tag + Id, false);
                            }
                        }
                    }
                }
            }


            private List<StaticEncounter> _starterForts;


            public List<StaticEncounter> StarterForts
            {
                get
                {

                    if (_starterForts == null)
                    {
                        _starterForts = new List<StaticEncounter>();

                        foreach (var staticEncounter in _staticEncounters)
                        {
                            if (staticEncounter.NodeId != null && staticEncounter.NodeId == Id && staticEncounter.StarterFort)
                            {
                                _starterForts.Add(staticEncounter);
                                return _starterForts;
                            }

                        }


                        return _starterForts;
                    }
                    else
                    {
                        return _starterForts;
                    }

                }
            }


            public bool Fort
            {
                get
                {
                    bool tempFort;

                    if(!MyAPIGateway.Utilities.GetVariable<bool>(Id+"Fort", out tempFort))
                    {
                        //Fort variable doesn't exist. So check for starterForts. It would make sense if every Node has at least a static Fort.
                        var currentfactiontag = Faction.Tag;

                        foreach (var entry in StarterForts)
                        {
                            if (entry.FactionTag == currentfactiontag)
                            {
                                string spawngroupname = entry.Spawngroupname; 
                                Vector3D matchingCoords = entry.Coords;


                                bool tempStaticDestroyed = false;


                                //Ex. (FAF-NameDestroyed) check
                                if (!MyAPIGateway.Utilities.GetVariable<bool>(spawngroupname+"Destroyed", out tempStaticDestroyed))
                                {
                                    //variable doesn't exist

                                    MyAPIGateway.Utilities.SetVariable<bool>(Id + "Fort", true);
                                    MyAPIGateway.Utilities.SetVariable<Vector3D>(Id + "FortCoords", matchingCoords);
                                    return true;
                                }
                                else
                                {
                                    //variable does exist
                                    if (!tempStaticDestroyed)
                                    {

                                        MyAPIGateway.Utilities.SetVariable<bool>(Id + "Fort", true);
                                        MyAPIGateway.Utilities.SetVariable<Vector3D>(Id + "FortCoords", matchingCoords);
                                        return true;
                                    }
                                    else
                                    {
                                        //This only make sense for existing worlds that this update is later added on.
                                        continue;
                                    }

                                }




                            }
                        }


                        //No staticforts found so no fort.
                        MyAPIGateway.Utilities.SetVariable<bool>(Id + "Fort", false);
                        return false;

                    }
                    else
                    {
                        // Fort variable does exit


                        if (tempFort)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                        
                    }

                }
                set
                {
                    MyAPIGateway.Utilities.SetVariable<bool>(Id + "Fort", value);
                }

            }

            public Vector3D FortCoords
            {
                get
                {
                    Vector3D matchingCoords;
                    
                    if(!MyAPIGateway.Utilities.GetVariable<Vector3D>(Id + "FortCoords", out matchingCoords))
                    {
                        return new Vector3D(0, 0, 0);
                    }

                    return matchingCoords;
                }
                set
                {
                    MyAPIGateway.Utilities.SetVariable<Vector3D>(Id + "FortCoords", value);
                }
            }


            public bool FortUnderSiege
            {
                get
                {
                    bool tempFort;

                    if (!Fort)
                        return false;


                    if (!MyAPIGateway.Utilities.GetVariable<bool>(Id + "FortUnderSiege", out tempFort))
                    {
                        //FortUnderSiege variable doesn't exist. 

                        //No staticforts found so no fort.
                        MyAPIGateway.Utilities.SetVariable<bool>(Id + "FortUnderSiege", false);
                        return false;

                    }
                    else
                    {
                        // Fort variable does exit
                        if (tempFort)
                        {
                            return true;
                        }
                        else
                        {

                            return false;
                        }

                    }

                }
                set
                {
                    MyAPIGateway.Utilities.SetVariable<bool>(Id + "FortUnderSiege", value);
                }

            }


            public int FrontLineCount;

            public bool SpaceNode;

            public List<string> Tags;

            public MarcoLocation Macro;
            public Planet Planet;
            public Feel Feel;

            public Node(string iD,string fancyName,Faction faction,Vector3D location,int radius, bool startNode = false, List<string> Tags = null, MarcoLocation macro = MarcoLocation.Bylen, Planet planet = Planet.Agaris, Feel feel = Feel.Military)
            {
                this.Id = iD;
                this.FancyName = fancyName;
                this.StartNode = startNode;
                this.startingFaction = faction;
                this.Radius = radius;
                this.Location = location;

                this.Tags = Tags ?? new List<string>();

                this.Macro = macro;
                this.Planet = planet;
                this.Feel = feel;

                _refreshFaction = true;
                var thisissodumbbutIneedtodoit = Faction;
                Refresh();

                _nodes.Add(this);
            }



            public Vector3D GetRandomPoint(Vector3D currentposition, int minDistance = 25000, int maxDistance = 50000)
            {
                Random random = new Random();

                if(this.Id == "Station27Space")
                {
                    return Helper.GeneratePointInBylenBelt(currentposition, minDistance, maxDistance).GetValueOrDefault();
                }

                if (this.Id == "MilaBelt")
                {
                    return Helper.GeneratePointInMilaBelt(currentposition, minDistance, maxDistance).GetValueOrDefault();
                }


                var planet = MyGamePruningStructure.GetClosestPlanet(Location);
                var planetcenter = planet.PositionComp.GetPosition();
                double randomRadius = random.NextDouble() * Radius;

                if ((planet.PositionComp.GetPosition()-Location).Length() < 80000)
                {
                    //Onplanet
                    var upAtPosition = Vector3D.Normalize(Location - planetcenter);


                    // Generate a random point around the current location within the radius

                    double randomAngle = random.NextDouble() * Math.PI * 2; // Random angle between 0 and 2*PI


                    // Calculate x and y offsets relative to the plane tangent to the surface
                    double offsetX = randomRadius * Math.Cos(randomAngle);
                    double offsetY = randomRadius * Math.Sin(randomAngle);

                    // Create a vector offset in the local tangent plane
                    Vector3D tangentOffset = (upAtPosition.Cross(Vector3D.Right) * offsetX) +
                                             (upAtPosition.Cross(Vector3D.Up) * offsetY);

                    // Return a new position adjusted by the tangent offset

                    var newposition = planet.GetClosestSurfacePointGlobal(Location + tangentOffset);

                    return newposition + (upAtPosition * 150);



                }



                randomRadius = random.NextDouble() * Radius;
                var randomvector =  MyUtils.GetRandomVector3D();

                return Location + (randomRadius * randomvector);


            }




            public void Refresh()
            {
                this.FrontLineCount = 0;
                _refreshFaction = true;
            }


            public void BuildFort()
            {
                if (StartNode)
                    return;


                foreach (var entry in StarterForts)
                {
                    if (entry.FactionTag == Faction.Tag)
                    {
                        string spawngroupname = entry.Spawngroupname;
                        Vector3D matchingCoords = entry.Coords;

                        bool tempStaticDestroyed = false;

                        //Ex. (FAF-NameDestroyed) check
                        if (!MyAPIGateway.Utilities.GetVariable<bool>(spawngroupname + "Destroyed", out tempStaticDestroyed))
                        {
                            //variable doesn't exist

                            MyAPIGateway.Utilities.SetVariable<bool>(Id + "Fort", true);
                            MyAPIGateway.Utilities.SetVariable<Vector3D>(Id + "FortCoords", matchingCoords);
                            Fort = true;
                            return;
                        }
                        else
                        {
                            //variable does exist
                            if (!tempStaticDestroyed)
                            {

                                MyAPIGateway.Utilities.SetVariable<bool>(Id + "Fort", true);
                                MyAPIGateway.Utilities.SetVariable<Vector3D>(Id + "FortCoords", matchingCoords);
                                Fort = true;
                                return;
                            }
                            else
                            {
                                //This only make sense for existing worlds that this update is later added on.
                                continue;
                            }

                        }
                    }
                }

                string spawnGroup = Faction.Tag + Id;
                Vector3D coord = Location;
                List<string> spawnGroups = new List<string>();
                spawnGroups.Add(spawnGroup);

                int i = 0;

                if (SpaceNode)
                {
                    while (!AaWSessionCore.MESApi.ApiSpawnRequest(coord, "AaW", "RandomEncounter", false, true, spawnGroups))
                    {
                        i += 1;

                        if (i > 500)
                        {
                            MyVisualScriptLogicProvider.ShowNotificationToAll($"Error: Can't find a spot to spawn {spawnGroup}", 50000, "Red");
                            break;
                        }
                    }
                    if (i <= 500) // This means the ApiSpawnRequest was successful
                    {
                        Fort = true;
                    }

                }
                else
                {
                    while (!AaWSessionCore.MESApi.ApiSpawnRequest(coord, "AaW", "PlanetaryInstallation", false, true, spawnGroups))
                    {
                        i += 1;

                        if (i > 500)
                        {
                            MyVisualScriptLogicProvider.ShowNotificationToAll($"Error: Can't find a spot to spawn {spawnGroup}", 50000, "Red");
                            break;
                        }
                    }

                    if (i <= 500) // This means the ApiSpawnRequest was successful
                    {
                        Fort = true;
                    }

                }

            }

        }
    }
}
