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
    public class AaWFactions
    {

        public class FactionData
        {
            public string Tag;
            public string ActiveName;

            public List<AaWHoldings.HoldingData> HoldingsList = new List<AaWHoldings.HoldingData>();

            public string Strength_CounterName;
            public string Aggression_CounterName;


            //Modes
            public List<string> Modes = new List<string>();

            //From Holdings
            public int Strength_Cap;
            public int TotalProduction;



            public FactionData(string TagName)
            {
                Tag = TagName;
                ActiveName = this.Tag + "Active";
                Strength_CounterName = this.Tag + "_Strength_Counter";
                Aggression_CounterName = this.Tag + "_Aggression_Counter";
                Modes.Add(this.Tag + "Mode");

                bool boolplaceholder;
                if (!MyAPIGateway.Utilities.GetVariable<bool>(ActiveName, out boolplaceholder))
                    MyAPIGateway.Utilities.SetVariable<bool>(ActiveName, true);

                int intplaceholder;
                if (!MyAPIGateway.Utilities.GetVariable<int>(Aggression_CounterName, out intplaceholder))
                    MyAPIGateway.Utilities.SetVariable<int>(Aggression_CounterName, 0);

                if (!MyAPIGateway.Utilities.GetVariable<int>(Strength_CounterName, out intplaceholder))
                    MyAPIGateway.Utilities.SetVariable<int>(Strength_CounterName, 50);

                MyAPIGateway.Utilities.SetVariable<bool>(this.Tag + "ReadyForExpansion", false);

            }



            public bool IsFactionActive()
            {
                bool ActivePlaceHolder = false;

                if (MyAPIGateway.Utilities.GetVariable<bool>(this.ActiveName, out ActivePlaceHolder))
                {
                    if (ActivePlaceHolder)
                        return true;
                }

                return false;

            }

            public void Holders()
            {
                int placeholderint_Cap = 0;
                int placeholderint_Production = 0;

                for (int i = 0; i < this.HoldingsList.Count; i++)
                {
                    var holding = this.HoldingsList[i];
                    if (holding.Active())
                    {
                        placeholderint_Cap += this.HoldingsList[i].Cap;
                        placeholderint_Production += this.HoldingsList[i].Production;
                    }
                }

                this.Strength_Cap = placeholderint_Cap;
                this.TotalProduction = placeholderint_Production;

                int Strength_Counter; 
                MyAPIGateway.Utilities.GetVariable<int>(Strength_CounterName, out Strength_Counter);


                if (Strength_Counter > 50)
                    MyAPIGateway.Utilities.SetVariable<bool>(this.Tag + "ReadyForExpansion", true);
                else
                    MyAPIGateway.Utilities.SetVariable<bool>(this.Tag + "ReadyForExpansion", false);

            }

            public int GetStrength()
            {
                int FACplaceholderForStoredVariable = 0;
                MyAPIGateway.Utilities.GetVariable<int>(this.Strength_CounterName, out FACplaceholderForStoredVariable);
                return FACplaceholderForStoredVariable;
            }
            public void StrengthCalculator()
            {
                //Aggression Counter


                int FACplaceholder2 = 0;
                MyAPIGateway.Utilities.GetVariable<int>(this.Aggression_CounterName, out FACplaceholder2);
                if(FACplaceholder2 > 100)
                {
                    FACplaceholder2 = 100;
                }
                if(FACplaceholder2< 0)
                {
                    FACplaceholder2 = 0;
                    MyAPIGateway.Utilities.SetVariable<int>(this.Aggression_CounterName, FACplaceholder2);
                }



                int FACplaceholderForStoredVariable = 0;
                MyAPIGateway.Utilities.GetVariable<int>(this.Strength_CounterName, out FACplaceholderForStoredVariable);

                if (FACplaceholderForStoredVariable < 0)
                {
                    FACplaceholderForStoredVariable = 0;
                    MyAPIGateway.Utilities.SetVariable<int>(this.Strength_CounterName, FACplaceholderForStoredVariable);
                }
  
            }


            public void AddProductiontoCounter()
            {
                var placeholder = 0;


                int Strength_Counter;
                MyAPIGateway.Utilities.GetVariable<int>(Strength_CounterName, out Strength_Counter);

                if (Strength_Counter < this.Strength_Cap)
                {
                    var diff = this.Strength_Cap - Strength_Counter;
                    if (diff <= this.TotalProduction)
                    {
                        placeholder = Strength_Counter += diff;
                    }
                    else
                    {
                        placeholder = Strength_Counter += this.TotalProduction;
                    }
                }

                MyAPIGateway.Utilities.SetVariable<int>(this.Strength_CounterName, placeholder);
            }

        }








        




    }

}

