using System;
using System.Collections.Generic;
using AresAtWar.API;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using System.Text;
using VRage.Utils;
using VRageMath;

using AresAtWar.Command;
using AresAtWar.Init;
using AresAtWar.War;
using AaWSyncManager;
using VRage.Game.ModAPI;
using Sandbox.Game.Entities.Planet;
using VRage.ModAPI;
using AresAtWar.Logging;
using AresAtWar.GPSManagers;
namespace AresAtWar.SessionCore
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class AaWSessionCore : MySessionComponentBase
    {
        public static string ModVersion = "0.5.2";

        public static MESApi MESApi;
        public static int counter = 0;
        public static int counter2 = 0;
        public bool restart = false;
        public bool hasrun = false;
        public static string ModLocation;

        public static Dictionary<string, MyDefinitionId> ConsumableItems;
        //public MyObjectBuilder_ConsumableItem RescueRover = new MyObjectBuilder_ConsumableItem() { SubtypeName = "RescueRover" };

        public override void LoadData()
        {
            MESApi = new MESApi();
            ModLocation= ModContext.ModPath;

            MyAPIGateway.Utilities.SetVariable<bool>("AaWWorldStartUp", false);


            MyAPIGateway.Utilities.SetVariable<bool>("ResetCapturableController", false);

            AaWMain.CheckValues();

            SyncManager.Setup();

            MyVisualScriptLogicProvider.RespawnShipSpawned += RespawnShipSpawned;
            MyAPIGateway.Players.ItemConsumed += Players_ItemConsumed;


            ConsumableItems = new Dictionary<string, MyDefinitionId>();
            ConsumableItems.Add("FAFSquadron", MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(typeof(MyObjectBuilder_ConsumableItem), "FAFSquadron")).Id);



        }

        

        public override void BeforeStart()
        {

            //Check for update
            string ActivePlaceHolder;
            if (MyAPIGateway.Utilities.GetVariable<string>("AaW_Version", out ActivePlaceHolder))
            {
                if (ActivePlaceHolder != ModVersion)
                {
                    MyVisualScriptLogicProvider.ShowNotificationToAll("Ares at War version is outdated", 1000000, "Red");
                    MyVisualScriptLogicProvider.ShowNotificationToAll("Save and reload the game", 1000000, "Red");
                    MESApi.ChatCommand("/MES.Debug.ClearStaticEncounters", MatrixD.Identity, 0, 0);
                    MyAPIGateway.Utilities.SetVariable<string>("AaW_Version", ModVersion);
                }


            }
            //AaW mod not detected. Setting default values.
            else
            {
                MyAPIGateway.Utilities.SetVariable<string>("AaW_Version", ModVersion);
                AaWMain.SetStartingValues();
                MyVisualScriptLogicProvider.ShowNotificationToAll("Ares at War setup complete", 5000, "Green");
            }

        }

        public override void UpdateBeforeSimulation()
        {
            if (!hasrun)
            {
                if (!MESApi.MESApiReady)
                {
                    MyVisualScriptLogicProvider.ShowNotificationToAll("Ares at War FAILED - MES API is not ready", 1000000, "Red");
                    return;
                }



                MESApi.RegisterCustomSpawnCondition(true, "BylenRing", CustomSpawnCodtions.BylenRing);
                MESApi.RegisterCustomSpawnCondition(true, "AgarisDeepOcean", CustomSpawnCodtions.AgarisDeepOcean);
                MESApi.RegisterCustomSpawnCondition(true, "AgarisLand", CustomSpawnCodtions.AgarisLand);

                MESApi.RegisterCustomAction(true, "FireSuperWeapon", CustomActions.FireSuperWeapon);
                MESApi.RegisterCustomAction(true, "DestroyTheSystem", CustomActions.DestroyTheSystem);
                MESApi.RegisterCustomAction(true, "PurgeArrivalEffect", CustomActions.PurgeArrivalEffect);
                MESApi.RegisterCustomAction(true, "AaW-WarNextRound", WarSim.SimulateWarRound);

                MESApi.RegisterCustomMissionMapping(true, "AaW-MissionMapping", CustomMissionMapping.InternalMissionMapping);

                WarSim.Init();


                hasrun = true;
            }

        }


        public override void UpdateAfterSimulation()
        {


            if (counter > 60)
            {
                Logger.ProcessQueue();
                counter = 0;
            }
            counter++;


            //Every 9 seconds
            if (counter2 > 540)
            {
                //GPS check
                for (int i = 0; i < GPSManager.AllActiveGPS.Count; i++)
                {
                    GPSManager.AllActiveGPS[i].Update();
                    counter2 = 0;
                    return;
                }
                counter2 = 0;
            }
            counter2++;


        }

        protected override void UnloadData()
        {

            SyncManager.Close();

            MyVisualScriptLogicProvider.RespawnShipSpawned -= RespawnShipSpawned;
            MyAPIGateway.Players.ItemConsumed -= Players_ItemConsumed;

        }



        public static IMyPlayer GetPlayerfromId(long playerId)
        {
            IMyPlayer return_player = null;

            List<IMyPlayer> all_players = new List<IMyPlayer>();
            MyAPIGateway.Multiplayer.Players.GetPlayers(all_players);

            foreach (var p in all_players)
            {
                if (p.IdentityId == playerId)
                {
                    return_player = p;
                    break;
                }
            }
            return return_player;
        }


        private void Players_ItemConsumed(IMyCharacter character, MyDefinitionId consumed)
        {
            


            if (character == null || !character.IsPlayer) //how?!?
                return;


            long playerId = character.ControllerInfo.ControllingIdentityId;





            if (!MESApi.MESApiReady)
            {
                MyLog.Default.WriteLineAndConsole("MES not ready");
                return;
            }



            switch (consumed.SubtypeName)
            {
                case "FAFSquadron":
                    if ((character.GetPosition() - new Vector3D(-1129033.5, 126871.5, 1293873.5)).Length() < 60000)
                    {
                        if(MESApi.ApiSpawnRequest(character.GetPosition(), "AaW", "PlanetaryCargoShip", true, true, new List<string> { "FAF-SpawnGroup-PlayerReinforcement-Squadron" }))
                        {
                            return;
                        }
                    }

                    MyVisualScriptLogicProvider.SendChatMessage($"Sorry {character.DisplayName}, we are unable to reach your location", "FAF", playerId, "Green");
                    MyVisualScriptLogicProvider.AddToPlayersInventory(playerId, ConsumableItems["FAFSquadron"]); // if it couldn't spawn the ship, refund the item



                    break;
            }
        }




        private void RespawnShipSpawned(long shipEntityId, long playerId, string RespawnShipPrefabName) {

            var player = GetPlayerfromId(playerId);
            if (player == null)
            {
                return;
            }

            MyVisualScriptLogicProvider.ShowNotification($"Warning Reset {player.DisplayName} Progress", 4000, "Red", playerId);


            var FAF = MyAPIGateway.Session.Factions.TryGetFactionByTag("FAF");
            if (FAF == null)
                return;

            var AHE = MyAPIGateway.Session.Factions.TryGetFactionByTag("AHE");
            if (AHE == null)
                return;
            var ITC = MyAPIGateway.Session.Factions.TryGetFactionByTag("ITC");
            if (ITC == null)
                return;
            var GC = MyAPIGateway.Session.Factions.TryGetFactionByTag("GC");
            if (GC == null)
                return;

            var ROS = MyAPIGateway.Session.Factions.TryGetFactionByTag("ROS");
            if (ROS == null)
                return;

            var COL = MyAPIGateway.Session.Factions.TryGetFactionByTag("COL-A");
            if (COL == null)
                return;
            var COLT4 = MyAPIGateway.Session.Factions.TryGetFactionByTag("COL-T4");
            if (COLT4 == null)
                return;

            var COLB = MyAPIGateway.Session.Factions.TryGetFactionByTag("COL-B");
            if (COLB == null)
                return;


            var SYN = MyAPIGateway.Session.Factions.TryGetFactionByTag("SYN");
            if (SYN == null)
                return;


            MyVisualScriptLogicProvider.SetRelationBetweenPlayerAndFaction(playerId, "FAF", 0);
            MyVisualScriptLogicProvider.SetRelationBetweenPlayerAndFaction(playerId, "AHE", 0);
            MyVisualScriptLogicProvider.SetRelationBetweenPlayerAndFaction(playerId, "GC", 0);
            MyVisualScriptLogicProvider.SetRelationBetweenPlayerAndFaction(playerId, "ROS", 0);
            MyVisualScriptLogicProvider.SetRelationBetweenPlayerAndFaction(playerId, "COL-A", 0);
            MyVisualScriptLogicProvider.SetRelationBetweenPlayerAndFaction(playerId, "COL-T4", 0);
            MyVisualScriptLogicProvider.SetRelationBetweenPlayerAndFaction(playerId, "COL-B", 0);
            MyVisualScriptLogicProvider.SetRelationBetweenPlayerAndFaction(playerId, "SYN", 0);
            MyVisualScriptLogicProvider.SetRelationBetweenPlayerAndFaction(playerId, "ITC", 0);




        }
    }
}

