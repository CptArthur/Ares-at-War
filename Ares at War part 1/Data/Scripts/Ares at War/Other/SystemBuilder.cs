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

namespace AresAtWar.SystemBuilder
{



	public class SystemBuilder 
	{

		public static Vector3D BylenCenter = new Vector3D(0, 0, 0);
		public static Vector3D Planet_AgarisCenter = new Vector3D(-1131656, 124249, 1291251);


		public static Vector3D MoonCenter = new Vector3D(-1091306, 297482, 1657451);

		public static Vector3D Planet_CraitCenter = new Vector3D(1449653, -622596, -2854164);
		public static Vector3D Planet_LezunoCenter = new Vector3D(-1309682, -327914, -1469100);
		public static Vector3D Planet_LorusCenter = new Vector3D(-1255638, -527925, -1810308);
		public static Vector3D Planet_Thora4Center = new Vector3D(1391341, -96601, -122962);

		public static void RemoveAllPlanets()
        {
			List<VRage.ModAPI.IMyVoxelBase> outInstances = new List<VRage.ModAPI.IMyVoxelBase>();


			MyAPIGateway.Session.VoxelMaps.GetInstances(outInstances);

			foreach (var entity in outInstances)
			{
				if (entity is MyPlanet)
				{
					entity.Close();
				
				}
			}

		}
		public static void SpawnPlanets()
        {
			MyAPIGateway.Session.VoxelMaps.SpawnPlanet("Blackhole (BalCorp)", 850000, 0, CenterLocation(BylenCenter, 1000000));
			MyAPIGateway.Session.VoxelMaps.SpawnPlanet("Planet Agaris - Lava", 120000, 0, CenterLocation(Planet_AgarisCenter, 120000));
			MyAPIGateway.Session.VoxelMaps.SpawnPlanet("Agaris II - Lava", 50000, 0, CenterLocation(MoonCenter, 50000));

			MyAPIGateway.Session.VoxelMaps.SpawnPlanet("Agaris II - Lava", 62928, 0, CenterLocation(Planet_CraitCenter, 62928));
			MyAPIGateway.Session.VoxelMaps.SpawnPlanet("Agaris II - Lava", 120000, 0, CenterLocation(Planet_LezunoCenter, 120000));
			MyAPIGateway.Session.VoxelMaps.SpawnPlanet("Agaris II - Lava", 50000, 0, CenterLocation(Planet_LorusCenter, 50000));
			MyAPIGateway.Session.VoxelMaps.SpawnPlanet("Agaris II - Lava", 100000, 0, CenterLocation(Planet_Thora4Center, 100000));
		}







		public static Vector3D CenterLocation(Vector3D PlanetCenter, int Radius)
		{
			var Offset = -0.524289 * Radius;
			var KeenCenter = new Vector3D((int)Offset, (int)Offset, (int)Offset);
			return PlanetCenter + KeenCenter;

		}

	}

}

