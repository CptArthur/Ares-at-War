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
    public class CustomSpawnCodtions
    {
        public static Vector3D PlanetCenter = new Vector3D(0, 0, 0);

        public static bool AaW(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            return true; 
    
        }



        public static bool BylenRing(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {

            var distance = Math.Abs(Vector3D.Distance(PlanetCenter, location));
            var ydistance = Math.Abs((location.Y - PlanetCenter.Y));

            if (((650000 <= distance && distance <= 740000) || (780000 <= distance && distance <= 985000) || (1025000 <= distance && distance <= 1100000)) && ydistance <= 6000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AgarisDeepOcean(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            Vector3D AgarisCenter = new Vector3D(-1129033.5, 126871.5, 1293873.5);
            var distancefromlocation = (location - AgarisCenter).Length();

            if (distancefromlocation > 65000.0)
            {
                return false;
            }

            MyPlanet Agaris = MyGamePruningStructure.GetClosestPlanet(location);

            if (Agaris == null)
                return false;

            if(!Agaris.Name.Contains("Planet Agaris"))
                return false;


            //58.90km waterlevel

            var SurfacePoint = Agaris.GetClosestSurfacePointGlobal(location);
            var distancefromsurface = (SurfacePoint - AgarisCenter).Length();

            //300 meter
            if(distancefromsurface > 58600)
            {
                return false;
            }
            return true;
        }

        public static bool AgarisLand(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            Vector3D AgarisCenter = new Vector3D(-1129033.5, 126871.5, 1293873.5);
            var distancefromlocation = (location - AgarisCenter).Length();

            if (distancefromlocation > 65000.0)
            {
                return false;
            }

            MyPlanet Agaris = MyGamePruningStructure.GetClosestPlanet(location);

            if (Agaris == null)
                return false;

            if (!Agaris.Name.Contains("Planet Agaris"))
                return false;


            var SurfacePoint = Agaris.GetClosestSurfacePointGlobal(location);
            var distancefromsurface = (SurfacePoint - AgarisCenter).Length();

            //AboveWaterLevel
            if (distancefromsurface < 58880)
            {
                return false;
            }
            return true;
        }



        public static void CustomSpawnInit(MESApi MESApi)
        {

            MESApi.ToggleSpawnGroupEnabled("Reaver - Group - Test - A", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Aggressor", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Berserker", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Butcher", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Carver", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Charger", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Cockroach", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Crimson", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - CrimsonTormentor", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Devourer", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - GroupEncounter - Hard - A", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - GroupEncounter - Hard - B", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - GroupEncounter - Medium - A", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - GroupEncounter - Medium - B", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Havelock", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Impaler", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Interloper", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Invader - Space", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Mutilator", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Nikkadudu", false);

            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Piercer", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Scourge", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Slayer", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Thrasher", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Tormentor", false);


        }




    }


}

