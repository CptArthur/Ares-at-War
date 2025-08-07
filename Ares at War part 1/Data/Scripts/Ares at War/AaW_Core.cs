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

using AresAtWar.War;
using AaWSyncManager;
using VRage.Game.ModAPI;
using Sandbox.Game.Entities.Planet;
using VRage.ModAPI;
using AresAtWar.Logging;
using AresAtWar.Managers;
using System.Security.Cryptography;

namespace AresAtWar.SessionCore
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class AaWSessionCore : MySessionComponentBase
    {
        public static AaWSessionCore Instance;

        public static string ModVersion = "0.6.1";

        public static MESApi MESApi;
        public static int counter = 0;
        public static int counter2 = 0;
        public static int counter3 = 0;
        public static int counter4 = 0;

        public bool restart = false;
        public bool hasrun = false;
        public static string ModLocation;

        internal static readonly Random _rnd = new Random();

        int timer = 0;
        long wealthMaintain = 1000000000;

        public static Dictionary<string, MyDefinitionId> ConsumableItems;
        //public MyObjectBuilder_ConsumableItem RescueRover = new MyObjectBuilder_ConsumableItem() { SubtypeName = "RescueRover" };

        public override void LoadData()
        {

            MESApi = new MESApi();
            ModLocation= ModContext.ModPath;

            MyAPIGateway.Utilities.SetVariable<bool>("AaWWorldStartUp", false);
            MyAPIGateway.Utilities.SetVariable<bool>("ResetCapturableController", false);

            //AaWMain.CheckValues();

            SyncManager.Setup();
            GPSManager.Setup();
            GravityManager.Setup();


            //Check for update
            bool ActivePlaceHolder = false;
            if (MyAPIGateway.Utilities.GetVariable<bool>("FrostboundDisabled", out ActivePlaceHolder))
            {
                if (ActivePlaceHolder)
                {
                    var spawnoption = MyDefinitionManager.Static.GetRespawnShipDefinition("Thora4-EscapePod");

                    if(spawnoption != null)
                    {
                        spawnoption.Enabled = false;
                        spawnoption.UseForSpace = false;

                    }
                        

                }
            }


            CustomMissionMapping.ActiveDestinations = new List<string>();



            MyVisualScriptLogicProvider.RespawnShipSpawned += RespawnShipSpawned;
            MyAPIGateway.Players.ItemConsumed += Players_ItemConsumed;


            ConsumableItems = new Dictionary<string, MyDefinitionId>();
            ConsumableItems.Add("FAFSquadron", MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(typeof(MyObjectBuilder_ConsumableItem), "FAFSquadron")).Id);

            Instance = this;
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

                    MyAPIGateway.Utilities.SendMessage("Ares at War version is outdated!");
                    MyAPIGateway.Utilities.SendMessage("Save and reload the game!");

                    MESApi.ChatCommand("/MES.Debug.ClearStaticEncounters", MatrixD.Identity, 0, 0);
                    MyAPIGateway.Utilities.SetVariable<string>("AaW_Version", ModVersion);
                }


            }
            //AaW mod not detected. Setting default values.
            else
            {
                MyAPIGateway.Utilities.SetVariable<string>("AaW_Version", ModVersion);
                MyAPIGateway.Utilities.ShowMessage("AaW", "Ares at War setup complete");
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



                MESApi.RegisterCustomSpawnCondition(true, "AaW_Home", CustomSpawnConditions.AaW_Home);
                MESApi.RegisterCustomSpawnCondition(true, "AaW_Outside", CustomSpawnConditions.AaW_Outside);
                MESApi.RegisterCustomSpawnCondition(true, "BylenRing", CustomSpawnConditions.BylenRing);
                MESApi.RegisterCustomSpawnCondition(true, "MilaRing", CustomSpawnConditions.MilaRing);
                MESApi.RegisterCustomSpawnCondition(true, "AgarisDeepOcean", CustomSpawnConditions.AgarisDeepOcean);
                MESApi.RegisterCustomSpawnCondition(true, "AgarisLand", CustomSpawnConditions.AgarisLand);
                MESApi.RegisterCustomSpawnCondition(true, "Thora4DeepOcean", CustomSpawnConditions.Thora4DeepOcean);
                MESApi.RegisterCustomSpawnCondition(true, "Thora4Land", CustomSpawnConditions.Thora4Land);


                MESApi.RegisterCustomAction(true, "FireSuperWeapon", CustomActions.FireSuperWeapon);
                MESApi.RegisterCustomAction(true, "DestroyTheSystem", CustomActions.DestroyTheSystem);
                MESApi.RegisterCustomAction(true, "PurgeArrivalEffect", CustomActions.PurgeArrivalEffect);
                MESApi.RegisterCustomAction(true, "AaW-DisableSpawnOption", CustomActions.DisableSpawnOption);
                MESApi.RegisterCustomAction(true, "PurgeGravityWeapon", CustomActions.PurgeGravityWeapon);
                MESApi.RegisterCustomAction(true, "AaW-WarNextRound", WarSim.SimulateWarRound);

                MESApi.RegisterCustomMissionMapping(true, "AaW-MissionMapping", CustomMissionMapping.InternalMissionMapping);


                MESApi.ToggleSpawnGroupEnabled("(NPC-SY) Cluster Core", false);
                MESApi.ToggleSpawnGroupEnabled("NPC-SY-LG-SpaceCargoShips", false);
                MESApi.ToggleSpawnGroupEnabled("NPC-SY-LG-SpaceRandomEncounters", false);
                MESApi.ToggleSpawnGroupEnabled("NPC-SY-SG-SpaceRandomEncounters", false);

                WarSim.Init();



                hasrun = true;
            }





        }


        public override void UpdateAfterSimulation()
        {


            //Gravity check
            for (int i = GravityManager.AllActiveGravity.Count - 1; i >= 0; i--)
            {
                GravityManager.AllActiveGravity[i].Update();
            }

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

                }
                counter2 = 0;
            }

            counter2++;


            if (counter3 > 600)
            {
                CustomMissionMapping.ActiveDestinations.Clear();
                CustomMissionMapping.ActiveContracts.Clear();

                counter3 = 0;
            }
            counter3++;





            /*
            if (MyAPIGateway.Session.IsServer)
            {
                if (timer == 70)
                {
                    if (MyAPIGateway.Session.Factions.FactionTagExists("CIVILIAN"))
                    {
                        var mainF = MyAPIGateway.Session.Factions.TryGetFactionByTag("CIVILIAN");

                        var playerCollection = MyAPIGateway.Multiplayer.Players;


                        playerCollection.RequestChangeBalance(mainF.FounderId, wealthMaintain);
                        MyVisualScriptLogicProvider.ShowNotificationToAll("I LIKE MONEY!",5000);
                    }
                }
            }
            
            timer += 1;
            */

        }

        protected override void UnloadData()
        {

            SyncManager.Close();





            MyVisualScriptLogicProvider.RespawnShipSpawned -= RespawnShipSpawned;
            MyAPIGateway.Players.ItemConsumed -= Players_ItemConsumed;

            Instance = null; // important for avoiding this object to remain allocated in memory
        }


        public override void SaveData()
        {
            // executed AFTER world was saved

            GPSManager.SaveData();
            GravityManager.SaveData();


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

           // MyVisualScriptLogicProvider.ShowNotification($"Warning Reset {player.DisplayName} Progress", 4000, "Red", playerId);


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

        public static int GetRandomNumber(int minValue, int maxValue)
        {
            return _rnd.Next(minValue, maxValue);
        }
    }


}

