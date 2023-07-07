using System;
using System.Collections.Generic;
using AresAtWar.API;
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
  
        public static void DestroyTheSystem(params object[] arguments)
        {
            MyVisualScriptLogicProvider.SetCustomSkybox(AaWSession.ModLocation + @"\Textures\BackgroundCube\Final\A_Skybox.dds");
            SystemBuilder.SystemBuilder.RemoveAllPlanets();
            SystemBuilder.SystemBuilder.SpawnPlanets();

        }

        public static void FireSuperWeapon(params object[] arguments)
        {
            var position = new Vector3D(-103.97, -297.29, 2578246.63);
            /*
            var foward = new Vector3D(0.35300264116349, 0.823272244011135, 0.444535653882195);
            var up = new Vector3D(0.787532090568454, -0.00492430757373354, -0.616253971605701);
            var matrix = MatrixD.CreateWorld(position, foward.Cross(up), up);
            MyParticleEffect particle = null;
            MyParticlesManager.TryCreateParticleEffect("MonolithAwaken", ref matrix, ref position, uint.MaxValue, out particle);
            if (particle != null)
            {
                particle.UserScale = 1;
            }
            */
            MyVisualScriptLogicProvider.CreateParticleEffectAtPosition("MonolithAwaken", position);
        }

        public static void PurgeArrivalEffect(params object[] arguments)
        {
            var position = new Vector3D(-1725718.78, 1493440.69, -698321.45);
            MyVisualScriptLogicProvider.CreateParticleEffectAtPosition("PurgeArrival", position);
        }




    }


}

