using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using Sandbox.Game;
using AresAtWar.Managers;

namespace AresAtWar.SessionCore
{

    public partial class CustomMissionMapping
    {

        private static Dictionary<string, string> SOLCOOP_Mission_RaidXolAsteroidBase()
        {
            var mapping = new Dictionary<string, string>();

            var xolZoneCenterPoint = new Vector3D(-1921803, -1017815, -2321340);

            var targetlocation = Helper.GeneratePointInBylenBelt(xolZoneCenterPoint, 10000, 25000).GetValueOrDefault();

            if (targetlocation == null)
            {
                return null;
            }

            mapping.Add("{StartingGPSCoords}", $"{{X:{targetlocation.X} Y:{targetlocation.Y} Z:{targetlocation.Z}}}");
            mapping.Add("{PlayerNearCoords}", $"{{X:{targetlocation.X} Y:{targetlocation.Y} Z:{targetlocation.Z}}}");
            mapping.Add("{EncounterSpawnCoords}", $"{{X:{targetlocation.X} Y:{targetlocation.Y} Z:{targetlocation.Z}}}");
            mapping.Add("{GPSText}", $"GPS:Backup:{targetlocation.X}:{targetlocation.Y}:{targetlocation.Z}:#FF75C9F1:");

            return mapping;
        }



    }

}
