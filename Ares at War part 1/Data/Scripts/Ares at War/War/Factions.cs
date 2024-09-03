using System;
using System.Collections.Generic;
using AresAtWar.Configuration;
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

        public class Faction
        {
            public string Tag;
            public string ActiveName;
            public Color Color { get; }
            public int Strength
            {
                get
                {
                    if (Tag == "FAF")
                        return Settings.War.FAFStrengthScore;

                    if (Tag == "GC")
                        return Settings.War.GCStrengthScore;

                    if (Tag == "SYN")
                        return Settings.War.SYNStrengthScore;

                    else return 0;
                }
            }

            public bool _refreshmodifier;

            private int _modifier;

            public int Modifier
            {
                get
                {
                    if (_refreshmodifier)
                    {
                        _refreshmodifier = false;

                        int intplaceholder = 0;
                        if (!MyAPIGateway.Utilities.GetVariable<int>(Tag + "_Modifier", out intplaceholder))
                            MyAPIGateway.Utilities.SetVariable<int>(Tag + "_Modifier", 0);

                        _modifier = intplaceholder;
                        return intplaceholder;
                    }
                    else
                        return _modifier;

                }

            }

            public Faction(string name, Color color, bool MinorFaction = false, bool EventFaction = false)
            {
                Tag = name;
                Color = color;
                Refresh();
            }

            public void Refresh()
            {
                _refreshmodifier = true;
            }


        }

    }
}

