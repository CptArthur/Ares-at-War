﻿using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
using ProtoBuf;
using AresAtWar.GPSManagers;
using System.Linq;

namespace AresAtWar.War
{
    public partial class WarSim
    {
        public enum InitiativeState
        {
            Neutral = 0,
            Positive = 1,
            Negative = -1
        }

        public class Frontline
        {
            public string Id { get; }

            public string GPSName;

            public string NodeAId { get; }
            public string NodeBId { get; }

            private Node _nodeA;
            private Node _nodeB;





            public Vector3D MiddleCoords;

            private bool _refreshActive;
            private bool _active { get; set; }

            public bool Active
            {
                get
                {
                    if (_refreshActive)
                    {
                        _refreshActive = false;
                        bool temp;
                        if (!MyAPIGateway.Utilities.GetVariable<bool>(Id+"Active", out temp))
                        {
                            //If it does not exist;
                            MyAPIGateway.Utilities.SetVariable<bool>(Id + "Active", false);
                            _active = false;
                            return false;
                        }

                        if (temp)
                        {
                            _active = true;
                            return true;
                        }
                        else
                        {
                            //Bool exist, but is false;
                            _active = false;
                            return false;
                        }
                    }
                    return _active;
                }
                set
                {

                    if (Active && !value)
                    {
                        //Close FrontLine
                        for (int i = 0; i < GPSManager.AllActiveGPS.Count; i++)
                        {
                            var gps = GPSManager.AllActiveGPS[i];

                            if (gps.name.Contains(GPSName))
                                gps.RemovefromActive();
                        }
                    }

                    if (!Active && value)
                    {
                        //Start a new frontline
                        GPS gps = new GPS(GPSName, $"{_nodeA.FancyName} - {_nodeB.FancyName}", MiddleCoords, Color.LightGray);
                        GPSManager.AllActiveGPS.Add(gps);

                        MyAPIGateway.Utilities.ShowMessage("FrontLine Updates", $"A new frontline between the {_nodeA.Faction.Tag} and the {_nodeB.Faction.Tag} has been formed");
                    }






                    _active = value;
                    MyAPIGateway.Utilities.SetVariable<bool>(Id + "Active", value);


                }
            }

            public bool HasSiege
            {
                get
                {
                    bool temp;
                    if (!MyAPIGateway.Utilities.GetVariable<bool>(Id + "HasSiege", out temp))
                    {
                        //If it does not exist;
                        MyAPIGateway.Utilities.SetVariable<bool>(Id + "HasSiege", false);
                        return false;
                    }

                    if (temp)
                    {
                        return true;
                    }
                    else
                    {
                        //Bool exist, but is false;
                        return false;
                    }


                }
                set
                {
                    MyAPIGateway.Utilities.SetVariable<bool>(Id + "HasSiege", value);
                }
            }



            public bool _refreshScore;
            private int _score { get; set; }


            public InitiativeState Initiative
            {
                get
                {
                    int InitiativeInt;
                    if (MyAPIGateway.Utilities.GetVariable<int>(Id + "Initiative", out InitiativeInt))
                    {
                        if (Enum.IsDefined(typeof(InitiativeState), InitiativeInt))
                        {
                            return (InitiativeState)InitiativeInt;
                        }
                        else
                        {
                            return InitiativeState.Neutral;
                        }

                    }
                    else
                    {
                        return InitiativeState.Neutral;
                    }
                }
                set
                {
                    MyAPIGateway.Utilities.SetVariable<int>(Id + "Initiative", (int)value);

                }



            }

            public int Score
            {
                get 
                {
                    if (_refreshScore)
                    {
                        _refreshScore = false;

                        int score;
                        if (MyAPIGateway.Utilities.GetVariable<int>(Id + "Score", out score))
                        {
                            _score = score;
                        }
                        else
                        {
                            _score = 0;
                        }
                        return _score;
                    }
                    else
                        return _score;

                }

                set
                {


                    _score = value;
                    MyAPIGateway.Utilities.SetVariable<int>(Id + "Score", _score);

                    if (!Active)
                        return;
                    switch (Initiative)
                    {
                        case InitiativeState.Neutral:
                            // If initiative is Neutral and score is greater than 30, set initiative to Positive
                            if (_score > 33)
                            {
                                Initiative = InitiativeState.Positive;
                                GPS gps2 = new GPS(GPSName, $"{_nodeB.Faction.Tag} has the initiative and is advancing on {_nodeA.Faction.Tag} {_nodeA.FancyName} ", _nodeA.Location, _nodeB.Faction.Color);
                                GPSManager.AllActiveGPS.Add(gps2);
                                MyAPIGateway.Utilities.ShowMessage("FrontLine Updates", $"{_nodeB.Faction.Tag} has the initiative and is advancing on {_nodeA.Faction.Tag} {_nodeA.FancyName}");
                            }
                            // If initiative is Neutral and score is less than -30, set initiative to Negative
                            else if (_score < -33)
                            {
                                GPS gps3 = new GPS(GPSName, $"{_nodeA.Faction.Tag} has the initiative and is advancing on {_nodeB.Faction.Tag} {_nodeB.FancyName} ", _nodeB.Location, _nodeA.Faction.Color);
                                GPSManager.AllActiveGPS.Add(gps3);
                                MyAPIGateway.Utilities.ShowMessage("FrontLine Updates", $"{_nodeA.Faction.Tag} has the initiative and is advancing on {_nodeB.Faction.Tag} {_nodeB.FancyName}");

                                Initiative = InitiativeState.Negative;
                            }
                            break;

                        case InitiativeState.Positive:
                            // If initiative is Positive and score drops below 0, reset initiative to Neutral
                            if (_score < 0)
                            {
                                Initiative = InitiativeState.Neutral;

                                GPS gps = new GPS(GPSName, $"{_nodeA.FancyName} - {_nodeB.FancyName}", MiddleCoords, Color.LightGray);
                                GPSManager.AllActiveGPS.Add(gps);

                                MyAPIGateway.Utilities.ShowMessage("FrontLine Updates", $"{_nodeB.Faction.Tag} has lost the initiative can the {_nodeA.Faction.Tag} continue with this momentum?");
                            }
                            break;

                        case InitiativeState.Negative:
                            // If initiative is Negative and score rises above 0, reset initiative to Neutral
                            if (_score > 0)
                            {
                                Initiative = InitiativeState.Neutral;

                                GPS gps = new GPS(GPSName, $"{_nodeA.FancyName} - {_nodeB.FancyName}", MiddleCoords, Color.LightGray);
                                GPSManager.AllActiveGPS.Add(gps);
                                MyAPIGateway.Utilities.ShowMessage("FrontLine Updates", $"{_nodeA.Faction.Tag} has lost the initiative can the {_nodeB.Faction.Tag} continue with this momentum?");
                            }
                            break;

                        // Optional: default case for completeness, though not strictly necessary here
                        default:
                            break;
                    }

                    // Example of setting a variable using the enum value
                    MyAPIGateway.Utilities.SetVariable<int>(Id + "Initiative", (int)Initiative);

                }
            }

            public Frontline(string nodeAId, string nodeBId, Vector3D middleCoords)
            {
                Id = "FrontLine" + nodeAId + nodeBId;
                NodeAId = nodeAId;
                NodeBId = nodeBId;
                MiddleCoords = middleCoords;

                _nodeA = _nodes.FirstOrDefault(n => n.Id == NodeAId);
                _nodeB = _nodes.FirstOrDefault(n => n.Id == NodeBId);
                GPSName = "FrontLine: " + _nodeA.Faction.Tag + " - " + _nodeB.Faction.Tag;

                Refresh();




            }

            public void Deactivate(bool changeActivity = true)
            {
                if (changeActivity)
                    Active = false;

                Score = 0;
                Initiative = InitiativeState.Neutral;

            }

            public void UpdateScore(int score, bool changeActivity=true)
            {
                Score = score;

                if (changeActivity)
                {
                    if (!Active)
                    {


                        Active = true;
                    }
                }

            }

            public void Refresh()
            {
                _refreshActive = true;
                _refreshScore = true;

                _nodeA = _nodes.FirstOrDefault(n => n.Id == NodeAId);
                _nodeB = _nodes.FirstOrDefault(n => n.Id == NodeBId);
                GPSName = "FrontLine: " + _nodeA.Faction.Tag + " - " + _nodeB.Faction.Tag;
            }






        }



    }
}