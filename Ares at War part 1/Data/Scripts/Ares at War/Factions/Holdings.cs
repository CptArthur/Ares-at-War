using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
namespace AresAtWar.Factions
{
    public class AaWHoldings
    {
        public class HoldingData
        {
            public string Name;
            public int Cap;
            public int Production;

            public bool CheckTrueBooleans = false;
            public List<string> TrueBooleans = new List<string>();
            public bool AllowAnyTrueBoolean = false;
            public bool CheckFalseBooleans = false;
            public List<string> FalseBooleans = new List<string>();
            public bool AllowAnyFalseBoolean = false;

            public HoldingData(string SpawnGroupName)
            {
                Name = SpawnGroupName;
                CheckFalseBooleans = true;
                var falseboolean = SpawnGroupName + "Destroyed";
                FalseBooleans.Add(falseboolean);
            }
            public HoldingData()
            {

            }


            public bool Active()
            {

                int usedConditions = 0;
                int satisfiedConditions = 0;


                //Bool
                if (this.CheckTrueBooleans == true)
                {
                    usedConditions++;
                    bool failedCheck = false;
                    bool placeholder = false;
                    foreach (var boolName in this.TrueBooleans)
                    {
                        MyAPIGateway.Utilities.GetVariable<bool>(boolName, out placeholder);

                        if (!placeholder)
                        {

                            failedCheck = true;

                            if (!this.AllowAnyTrueBoolean)
                            {

                                failedCheck = true;
                                break;

                            }

                        }
                        else if (this.AllowAnyTrueBoolean)
                        {

                            failedCheck = false;
                            break;

                        }

                    }

                    if (!failedCheck)
                        satisfiedConditions++;

                }

                //Bool False
                if (this.CheckFalseBooleans == true)
                {
                    usedConditions++;
                    bool failedCheck = false;
                    bool placeholder = false;
                    foreach (var boolName in this.FalseBooleans)
                    {
                        MyAPIGateway.Utilities.GetVariable<bool>(boolName, out placeholder);


                        if (placeholder)
                        {

                            failedCheck = true;

                            if (!this.AllowAnyFalseBoolean)
                            {

                                failedCheck = true;
                                break;

                            }

                        }
                        else if (this.AllowAnyFalseBoolean)
                        {

                            failedCheck = false;
                            break;

                        }

                    }

                    if (!failedCheck)
                        satisfiedConditions++;

                }


                if (usedConditions == satisfiedConditions)
                    return true;
                else
                    return false;

            }



        }

    }
}

