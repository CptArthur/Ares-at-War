using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using System.Text;
using VRage.Utils;
using VRageMath;
using VRage.Game.ModAPI;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using Sandbox.Game.Entities.Planet;
using VRage.ModAPI;

namespace RespawnSystem
{
    public struct RespawnData
    {
        public long shipEntityId;
        public long playerId;
        public string RespawnShipPrefabName;
        public long TLL;
    }

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class RespawnSystem : MySessionComponentBase
    {
        private static Random _rnd = new Random();

        int count = 0;

        public static List<RespawnData> _RespawnData = new List<RespawnData>();

        public static RespawnSystem Instance;
        //public MyVoxelBase entity;

        public static MatrixD TargetMatrix = new MatrixD();
        //public static MatrixD FinalMatrix = new MatrixD();


        public static Vector3D PlanetCenter = new Vector3D(0, 0, 0);
        public static Vector3D AgarisCenter = new Vector3D(-1129033.5, 126871.5, 1293873.5);
        public static Vector3D Carcosa = new Vector3D(-1174736.16, 106319.66, 1325749.58);
        public static Vector3D Azuris = new Vector3D(-1080910.05, 135510.47, 1258013.98);


        public static Vector3D LezunoCoords = new Vector3D(-1266529.78125, -314814.913085938, -1422814.98828125);

        public static Vector3D LezunoNorth = new Vector3D(-1312209.11, -264051.53, -1470507.23);
        private static string PlanetName = "Planet Lezuno";


        public override void LoadData()
        {
            MyVisualScriptLogicProvider.SetCustomLoadingScreenText("Ares at War");
            MyVisualScriptLogicProvider.SetCustomLoadingScreenImage("Data/Screens/thumb.png");



        }

        public override void BeforeStart()
        {
            if (MyAPIGateway.Multiplayer.IsServer)
            {
                MyVisualScriptLogicProvider.RespawnShipSpawned += RespawnShipSpawned;
            }



        }

        public static void RespawnShipSpawned(long shipEntityId, long playerId, string RespawnShipPrefabName)
        {
            var respawndata = new RespawnData();
            respawndata.shipEntityId = shipEntityId;
            respawndata.playerId = playerId;
            respawndata.RespawnShipPrefabName = RespawnShipPrefabName;
            respawndata.TLL = MyAPIGateway.Session.GameplayFrameCounter + 120;
            _RespawnData.Add(respawndata);

        }


        public static void ProssesRespawnShip(long shipEntityId, long playerId, string RespawnShipPrefabName)
        {
            MyCubeGrid grid = MyEntities.GetEntityById(shipEntityId) as MyCubeGrid;
            IMyPlayer player = GetPlayer(playerId);


            var block = grid.GetFirstBlockOfType<MyCockpit>();

            if (RespawnShipPrefabName == "ITC Mallard (Crashed)")
            {
                var HeightOffSet = 2f;

                var planet = MyGamePruningStructure.GetClosestPlanet(LezunoCoords);


                Vector3D normal = ((MyUtils.GetRandomVector3Normalized()* 15000 + LezunoNorth)- LezunoCoords).Normalized();


                Vector3D position = planet.GetClosestSurfacePointGlobal(LezunoCoords + (normal * planet.AverageRadius));
                
                Vector3 vector = MyAPIGateway.GravityProviderSystem.CalculateNaturalGravityInPoint(position);
                Vector3D up = -Vector3D.Normalize(vector);
                position += (HeightOffSet * up);

                //MyVisualScriptLogicProvider.SetWeatherP("/Weather SandStormHeavy", "SandStormHeavy", 4000, position);
                Vector3D forward = MyUtils.GetRandomPerpendicularVector(up);
                MatrixD matrix = MatrixD.CreateWorld(position, forward, up);

                var gridWorldMatrix = MatrixD.Invert(block.PositionComp.LocalMatrixRef) * matrix;

                
                MatrixD pitch = MatrixD.CreateFromAxisAngle(gridWorldMatrix.Right, MathHelper.ToRadians(2));
                MatrixD roll = MatrixD.CreateFromAxisAngle(gridWorldMatrix.Forward, MathHelper.ToRadians(-10));

                MatrixD finalMatrix = pitch * roll * gridWorldMatrix;



                MyVoxelMaterialDefinition vox = new MyVoxelMaterialDefinition();
                if (MyDefinitionManager.Static.TryGetVoxelMaterialDefinition("DustyRocks", out vox))
                {
                    //planet.PerformCutOutSphereFast(position, 40,true);

                    
                    //MyAPIGateway.Session.VoxelMaps.RequestVoxelCutoutSphere();
                    //planet.RequestVoxelCutoutSphere(position, 5, false, false);
                    //planet.RootVoxel.CreateVoxelMeteorCrater(position, 20, -up, vox); 
                }
                
                grid.Teleport(finalMatrix);
                grid.PositionComp.SetWorldMatrix(ref finalMatrix, skipTeleportCheck: true);
                grid.ConvertToStatic();
                return;
            }

            if(RespawnShipPrefabName == "Ares-Campaign")
            {

                block.RemovePilot();

                Vector3 up = new Vector3D(-1, 0, 0);
                Vector3 forward = new Vector3D(0, -1, 0);
                Vector3D position = new Vector3D(2730886, -410779, 583541);
                MatrixD matrix = MatrixD.CreateWorld(position, forward, up);

                player.Character.Teleport(matrix);
            }


            if (RespawnShipPrefabName == "CarcosaRespawnRover")
            {
                Vector3 up = -MyAPIGateway.GravityProviderSystem.CalculateNaturalGravityInPoint(Carcosa).Normalized();
                Vector3 forward = MyUtils.GetRandomPerpendicularVector(up);
                Vector3D position = Carcosa + forward * _rnd.Next(50, 6000);
                MatrixD matrix = MatrixD.CreateWorld(position, forward, up);
                var gridWorldMatrix = MatrixD.Invert(block.PositionComp.LocalMatrixRef) * matrix;
                grid.Teleport(gridWorldMatrix);
                grid.PositionComp.SetWorldMatrix(ref gridWorldMatrix, skipTeleportCheck: true);
            }

            if (RespawnShipPrefabName == "AzurisRespawnRover")
            {
                Vector3 up = -MyAPIGateway.GravityProviderSystem.CalculateNaturalGravityInPoint(Azuris).Normalized();
                Vector3 forward = MyUtils.GetRandomPerpendicularVector(up);
                Vector3D position = Azuris + forward * _rnd.Next(50, 1000);
                MatrixD matrix = MatrixD.CreateWorld(position, forward, up);
                var gridWorldMatrix = MatrixD.Invert(block.PositionComp.LocalMatrixRef) * matrix;
                grid.Teleport(gridWorldMatrix);
                grid.PositionComp.SetWorldMatrix(ref gridWorldMatrix, skipTeleportCheck: true);
            }


        }


        public static void DrawOrigin(MatrixD matrix)
        {
            const float lineLen = 5;
            const float lineThick = 0.05f;
            MyStringId material = MyStringId.GetOrCompute("Square");

            MyTransparentGeometry.AddLineBillboard(material, Color.Red, matrix.Translation, (Vector3)matrix.Right, lineLen, lineThick);
            MyTransparentGeometry.AddLineBillboard(material, Color.Lime, matrix.Translation, (Vector3)matrix.Up, lineLen, lineThick);
            MyTransparentGeometry.AddLineBillboard(material, Color.Blue, matrix.Translation, (Vector3)matrix.Backward, lineLen, lineThick);
        }


        public override void UpdateAfterSimulation()
        {
            //DrawOrigin(TargetMatrix);
            //DrawOrigin(FinalMatrix);

            for (int i = _RespawnData.Count - 1; i >= 0; i--)
            {
                var data = _RespawnData[i];
                if (data.TLL> MyAPIGateway.Session.GameplayFrameCounter)
                {
                    ProssesRespawnShip(data.shipEntityId, data.playerId, data.RespawnShipPrefabName);
                    _RespawnData.RemoveAtFast(i);
                }
            }




        }


        public static IMyPlayer GetPlayer(long playerId)
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


        protected override void UnloadData()
        {
            if (MyAPIGateway.Multiplayer.IsServer)
            {
                MyVisualScriptLogicProvider.RespawnShipSpawned -= RespawnShipSpawned;
            }
        }


    }
}
