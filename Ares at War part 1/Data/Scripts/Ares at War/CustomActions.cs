using System;
using System.Collections.Generic;
using AresAtWar.API;
using AresAtWar.GPSManagers;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
namespace AresAtWar.SessionCore
{
    public class CustomActions
    {
        public static Vector3D PlanetCenter = new Vector3D(0, 0, 0);


        public static void GPSBattleforAHEHQ()
        {
            GPS GPSBattleforAHEHQ = new GPS(120, "Battle for AHE-HQ", "AHE HQ has become the epicenter of a violent clash between the GC forces and AHE loyalists.", new Vector3D(-1162242.5,139010.1,1341161.84), Color.LightGreen);
            GPSManager.AllActiveGPS.Add(GPSBattleforAHEHQ);
        }
        public static void GPSBattleforFAFHQ()
        {
            GPS GPSBattleforFAFHQ = new GPS(120, "Battle for FAF-HQ", "", Vector3D.Zero, Color.Red);
            GPSManager.AllActiveGPS.Add(GPSBattleforFAFHQ);
        }


        public static void IamSoSmart()
        {
            MyVisualScriptLogicProvider.SetCustomSkybox(AaWSession.ModLocation + @"\Textures\BackgroundCube\Final\A_Skybox.dds");
            SystemBuilder.SystemBuilder.RemoveAllPlanets();
            SystemBuilder.SystemBuilder.SpawnPlanets();

        }
  





    }


}

