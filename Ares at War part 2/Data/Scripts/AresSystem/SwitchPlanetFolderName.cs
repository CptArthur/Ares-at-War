using Sandbox.Definitions;
using Sandbox.ModAPI;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Utils;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders;

namespace IndustrialOreRemap
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class OreRemapping : MySessionComponentBase
	{

		public override void LoadData()
		{

			var allVoxelMaterials = MyDefinitionManager.Static.GetVoxelMaterialDefinitions();
			var allPlanets = MyDefinitionManager.Static.GetPlanetsGeneratorsDefinitions();

			var ioDetected = false;
			var Planets = new List<string>() { "Planet Agaris", "Planet Crait", "Planet Lorus", "Planet Lezuno", "Planet Thora 4"};


			foreach (var mod in MyAPIGateway.Session.Mods)
			{
				if (mod.PublishedFileId == 2344068716)
				{
					ioDetected = true;
					break;

				}

			}


            if (ioDetected)
            {
				foreach (var def in allPlanets)
				{
					var planet = def as MyPlanetGeneratorDefinition;

					if (Planets.Contains(planet.Id.SubtypeName))
					{
						planet.FolderName = planet.Id.SubtypeName + " - IO";
						var mapProvider = planet.MapProvider as MyObjectBuilder_PlanetTextureMapProvider;
						if (mapProvider != null)
							mapProvider.Path = planet.FolderName;
						continue;
					}
				}

				foreach (var voxelmat in allVoxelMaterials)
				{
					var voxel = voxelmat as MyVoxelMaterialDefinition;
					if (voxel.Id.SubtypeName == "Coal")
					{
						voxel.SpawnsInAsteroids = false;
						return;
					}

				}

				return;
			}

			//Vanilla
			foreach (var voxelmat in allVoxelMaterials)
			{
				var voxel = voxelmat as MyVoxelMaterialDefinition;
				if(voxel.Id.SubtypeName == "Magnesium_01")
                {
					voxel.SpawnsInAsteroids = false;
					return;
				}
			}

		}


	}
}
