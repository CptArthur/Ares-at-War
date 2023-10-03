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
using AresAtWar.Factions;
using AresAtWar.Command;
using AresAtWar.Init;

using AaWSyncManager;
using VRage.Game.ModAPI;
using Sandbox.Game.Entities.Planet;

namespace AresAtWar.SessionCore
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class AaWSession : MySessionComponentBase
    {
        public static string ModVersion = "0.4.9.8";

        public static MESApi MESApi;
        public int counter = 0;
        public int counter2 = 0;
        public bool restart = false;
        public bool hasrun = false;
        public static string ModLocation;


        public override void LoadData()
        {
            MESApi = new MESApi();
            ModLocation= ModContext.ModPath;

            MyAPIGateway.Utilities.SetVariable<bool>("AaWWorldStartUp", false);


            MyAPIGateway.Utilities.SetVariable<bool>("ResetCapturableController", false);

            AaWMain.CheckValues();
            AaWMain.init();
            SyncManager.Setup();

            MyVisualScriptLogicProvider.RespawnShipSpawned += RespawnShipSpawned;
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


                hasrun = true;
            }

        }


        public override void UpdateAfterSimulation()
        {

            //Every 10 seconds
            if (counter >= 600)
            {

                //Faction check 
                for (int i = 0; i < AaWMain.listOfFactions.Count; i++)
                {
                    var fac = AaWMain.listOfFactions[i];
                    fac.StrengthCalculator();
                    fac.Holders();
                }
                counter = 0;
            }


            //20m 
            if (counter2 >= 72000)
            {
                MyVisualScriptLogicProvider.ShowNotificationToAll("Updated Faction Strength", 5000);
                for (int i = 0; i < AaWMain.listOfFactions.Count; i++)
                {
                    var fac = AaWMain.listOfFactions[i];
                    fac.AddProductiontoCounter();
                }
                counter2 = 0;
            }

            counter++;
            counter2++; 
        }

        protected override void UnloadData()
        {

            SyncManager.Close();

            MyVisualScriptLogicProvider.RespawnShipSpawned -= RespawnShipSpawned;

            
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


        public static void RespawnShipSpawned(long shipEntityId, long playerId, string RespawnShipPrefabName) {

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

