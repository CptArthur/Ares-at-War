using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using System.Text;
using VRageMath;
using AresAtWar.Factions;
using AresAtWar.SessionCore;

namespace AresAtWar.Command
{
    public class AaWCommand
    {
        public static StringBuilder SendAllInfo(StringBuilder text, List<AaWFactions.FactionData>  AllFactionList)
        {
            text.Append("Ares at War").AppendLine();
            text.Append($"ModVersion: {AaWSession.ModVersion}");



            //Holdings check 
            for (int i = 0; i < AllFactionList.Count; i++)
            {

                int Strength_Counter;
                MyAPIGateway.Utilities.GetVariable<int>(AllFactionList[i].Strength_CounterName, out Strength_Counter);
                bool ReadyForExpansion;
                MyAPIGateway.Utilities.GetVariable<bool>(AllFactionList[i].Tag + "ReadyForExpansion",out ReadyForExpansion);


                text.Append("").AppendLine();
                text.Append("::: Faction: ").Append(AllFactionList[i].Tag.ToString()).Append(" :::").AppendLine();
                text.Append(" - Active:              ").Append(AllFactionList[i].IsFactionActive().ToString()).AppendLine();
                text.Append(" - Strength:            ").Append(Strength_Counter).AppendLine();
                text.Append(" - Strength_Cap:        ").Append(AllFactionList[i].Strength_Cap.ToString()).AppendLine();
                text.Append(" - TotalProduction:     ").Append(AllFactionList[i].TotalProduction.ToString()).AppendLine();
                text.Append(" - ReadyForExpansion:   ").Append(ReadyForExpansion).AppendLine();

                text.Append("::: Modes :::").AppendLine();
                for (int j = 0; j < AllFactionList[i].Modes.Count; j++)
                {
                    int placeholder;
                    MyAPIGateway.Utilities.GetVariable<int>(AllFactionList[i].Modes[j].ToString(), out placeholder);
                    text.Append(" -  ").Append(AllFactionList[i].Modes[j].ToString()).Append(":   ").Append(placeholder.ToString()).AppendLine();
                }
                if(AllFactionList[i].IsFactionActive())
                    HoldingData(ref text, AllFactionList[i]);
            }

            return text;
        }
        
        public static void HoldingData(ref StringBuilder text, AaWFactions.FactionData Faction)
        {
            text.Append("::: Holding :::").AppendLine();
            for (int j = 0; j < Faction.HoldingsList.Count; j++)
            {
                text.Append(" - HoldingName:   ").Append(Faction.HoldingsList[j].Name.ToString()).AppendLine();
                text.Append(" - Active:         ").Append(Faction.HoldingsList[j].Active().ToString()).AppendLine();
                text.Append(" - Cap:            ").Append(Faction.HoldingsList[j].Cap.ToString()).AppendLine();
                text.Append(" - Production:     ").Append(Faction.HoldingsList[j].Production.ToString()).AppendLine().AppendLine();
            }

        }


        
        public static StringBuilder GetItemStuff()
        {

            MyVisualScriptLogicProvider.ShowNotificationToAll("Hello", 5000);
            var allItems = MyDefinitionManager.Static.GetBlueprintDefinitions();

            var sb = new StringBuilder();
            var Components = new StringBuilder();


            //MyProductionBlockDefinition


            foreach (var bp in allItems)
            {
                if (bp != null)
                {
                    var req = new StringBuilder();
                    foreach (var Prerequisit in bp.Prerequisites)
                    {

                        req.Append($"Requirement({Prerequisit.Id},{Prerequisit.Amount}),");
                    }

                    string Line = $"Item({bp.Results[0]}, [{req}])";
                    Components.Append(Line);
                    Components.AppendLine();

                }
            }


            sb.Append("Components").AppendLine();
            sb.Append(Components);

            return sb;
        }





    }

}



               /*
                //Component
                if (item.Id.TypeId == typeof(MyObjectBuilder_Component))
                {





                    MyVisualScriptLogicProvider.ShowNotificationToAll("Component Found", 5000);


                    MyBlueprintDefinitionBase blueprint = MyDefinitionManager.Static.GetBlueprintDefinition(item.Id);

                }




                //Ammo
                if (item.Id.TypeId == typeof(MyObjectBuilder_AmmoMagazine))
                {

                }

                //Tool
                if (item.Id.TypeId == typeof(MyObjectBuilder_PhysicalGunObject))
                {

                }

                //Consumable

                if (item.Id.TypeId == typeof(MyObjectBuilder_ConsumableItem))
                {

                }
                */