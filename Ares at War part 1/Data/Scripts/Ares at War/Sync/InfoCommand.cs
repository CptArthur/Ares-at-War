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
using AresAtWar.SessionCore;
using AresAtWar.War;
namespace AresAtWar.Command
{
    public class AaWCommand
    {
        public static StringBuilder SendAllInfo(StringBuilder text)
        {
            bool StoryEvents = false;

            MyAPIGateway.Utilities.GetVariable<bool>("AaWEventsEnabled", out StoryEvents);

            int PlayerReadyForPurge = 0;

            MyAPIGateway.Utilities.GetVariable<int>("PlayerReadyForPurge", out PlayerReadyForPurge);

            text.Append("Ares at War").AppendLine();
            text.Append($"ModVersion: {AaWSessionCore.ModVersion}").AppendLine();
            text.Append($"StoryEvents Enabled: {StoryEvents} (False by default if the world just loaded (6min to 25 min)").AppendLine();
            text.Append($"Counter: {PlayerReadyForPurge}");

            text.Append($"Current Node Control:").AppendLine();
            foreach (var node in WarSim._nodes)
            {
                text.Append($"{node.Id}: {node.Faction.Tag}").AppendLine();

            }

            return text;
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