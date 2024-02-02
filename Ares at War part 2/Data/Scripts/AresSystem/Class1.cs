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

namespace BylenBelt
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class Example_Session : MySessionComponentBase
    {
        public static Example_Session Instance;
        public MyVoxelBase entity;

        public static Vector3D PlanetCenter = new Vector3D(0, 0, 0);
        public static Vector3D AgarisCenter = new Vector3D(-1129033.5, 126871.5, 1293873.5);
        public override void LoadData()
        {

        }

        public override void BeforeStart()
        {

            //MyAPIGateway.Session.SessionSettings.ProceduralDensity = 0.75F;



        }


        public override void UpdateAfterSimulation()
        {

                List<VRage.ModAPI.IMyVoxelBase> outInstances = new List<VRage.ModAPI.IMyVoxelBase>();


                MyAPIGateway.Session.VoxelMaps.GetInstances(outInstances);

                foreach (var entity in outInstances)
                {
                    if (entity is MyVoxelMap && !(entity is MyPlanet))
                    {
                        var tja = entity.GetPosition();

                        var agarisdisctance = Vector3.Distance(AgarisCenter, tja);
                        var distance = Vector3D.Distance(PlanetCenter, tja);
                        var ydistance = Math.Abs((tja.Y - PlanetCenter.Y));

                        if ((((650000 <= distance && distance <= 740000) || (780000 <= distance && distance <= 985000) || (1025000 <= distance && distance <= 1100000)) && ydistance <= 6000) ^ agarisdisctance<= 70000)
                        {
                            continue;
                        }
                        else
                        {
                            //MyVisualScriptLogicProvider.ShowNotificationToAll("Removing Entity", 1000);
                            entity.Close();
                        }
                    }
                }




        }
        public override void Draw()
        {

        }

        public override void SaveData()
        {

        }


    }
}
