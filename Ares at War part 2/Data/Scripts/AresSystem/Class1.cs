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


        public static Vector3D BylenCenter = new Vector3D(-2070540.14510301, -1017587.98462825, -3367463.76531797);


        public static Vector3D MilaCenter = new Vector3D(2299140.85489699, 1166634.01537175, 2935888.23468203);

        public static Vector3D AgarisCenter = new Vector3D(-1129033.5, 126871.5, 1293873.5);

        public static Vector3D NebulaCenter = new Vector3D(-17115, -4610, -8171);


        public override void LoadData()
        {
            var allFactions = MyDefinitionManager.Static.GetFactionsFromDefinition();

            foreach (var faction in allFactions)
            {
                if(faction.Id.SubtypeName == "SpacePirates")
                {
                    faction.Enabled = false;
                    faction.IsDefault = false;
                }

                if (faction.Id.SubtypeName == "Factorum")
                {
                    faction.Enabled = false;
                    faction.IsDefault = false;
                }

            }
        }

        public override void BeforeStart()
        {




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
                    var Nebuladisctance = Vector3.Distance(NebulaCenter, tja);
                    var Bylendistance = Vector3D.Distance(BylenCenter, tja);
                    var Bylenydistance = Math.Abs((tja.Y - BylenCenter.Y));

                    var Miladistance = Vector3D.Distance(MilaCenter, tja);
                    var Milaydistance = Math.Abs((tja.Y - MilaCenter.Y));

                    if (agarisdisctance <= 70000)
                    {
                        continue;
                    }

                    if (((650000 <= Bylendistance && Bylendistance <= 740000) || (780000 <= Bylendistance && Bylendistance <= 985000) || (1025000 <= Bylendistance && Bylendistance <= 1100000)) && Bylenydistance <= 6000)
                    {
                        continue;
                    }


                    if (((610000 <= Miladistance && Miladistance <= 700000) || (800000 <= Miladistance && Miladistance <= 835000) || (850000 <= Miladistance && Miladistance <= 880000)) && Milaydistance <= 5500)
                    {
                        continue;
                    }

                    if (Nebuladisctance <= 200000)
                    {
                        continue;
                    }

                    

                    //MyVisualScriptLogicProvider.ShowNotificationToAll("Removing Entity", 1000);
                    entity.Close();

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
