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
using VRage.Game.ObjectBuilders.Definitions;

namespace IndustrialOreRemap
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class OreRemapping : MySessionComponentBase
	{
		public struct ComponentBlueprintManager
		{
			public string ClassName;
			public List<string> VanillaSubtypeIds;
			public List<string> BlackListSubtypeIds;
			public List<string> AddSubtypeIds;

			public ComponentBlueprintManager(string className, List<string> allSubtypeIds, List<string> filteredSubtypeIds, List<string> additionalSubtypeIds)
			{
				ClassName = className;
				VanillaSubtypeIds = allSubtypeIds;
				BlackListSubtypeIds = filteredSubtypeIds;
				AddSubtypeIds = additionalSubtypeIds;
			}

			public void ProcessBlueprints()
			{
				var componentBlueprints = MyDefinitionManager.Static.GetBlueprintClass(ClassName);
				var enumerator = componentBlueprints.GetEnumerator();

				List<string> moddedSubtypeIds = new List<string>();

				while (enumerator.MoveNext())
				{
					var component = enumerator.Current;
					if (VanillaSubtypeIds.Contains(component.Id.SubtypeName))
						continue;

					if (BlackListSubtypeIds.Contains(component.Id.SubtypeName))
						continue;

					moddedSubtypeIds.Add(component.Id.SubtypeName);
				}
				componentBlueprints.ClearBlueprints();

				foreach (var subtypeId in VanillaSubtypeIds)
				{
					if (BlackListSubtypeIds.Contains(subtypeId))
						continue;
					componentBlueprints.AddBlueprint(MyDefinitionManager.Static.GetBlueprintDefinition(new MyDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), subtypeId)));
				}

				foreach (var subtypeId in moddedSubtypeIds)
				{
					componentBlueprints.AddBlueprint(MyDefinitionManager.Static.GetBlueprintDefinition(new MyDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), subtypeId)));
				}

				foreach (var subtypeId in AddSubtypeIds)
				{
					componentBlueprints.AddBlueprint(MyDefinitionManager.Static.GetBlueprintDefinition(new MyDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), subtypeId)));
				}
			}
		}

		public override void LoadData()
		{
			// Initialize for BasicComponents
			var basicComponentsManager = new ComponentBlueprintManager(
				"BasicComponents",
				new List<string>
				{
					"Display", "SolarCell", "SteelPlate", "InteriorPlate", "MotorComponent",
					"GirderComponent", "ComputerComponent", "ConstructionComponent"
				},
				new List<string>
				{
					// Filtered out components. So compared to vanilla. Which should be removed
					//"SolarCell",
					//"ComputerComponent",

					//Scrapyard does this:
					//SolarCell
					//ComputerComponent
					//PowerCell
				},
				new List<string>
				{
				// So compared to vanilla. What is missing from vanilla. But should be added
				"ExplosivesComponent",

				//Scrapyard does this:
				//ExplosivesComponent
				}
			);

			basicComponentsManager.ProcessBlueprints();


			var simpleComponentsManager = new ComponentBlueprintManager(
			"SimpleComponents",
			new List<string>
			{
				//Default Vanilla
				"BulletproofGlass",
				"ComputerComponent",
				"ConstructionComponent",
				"DetectorComponent",
				"Display",
				"GirderComponent",
				"InteriorPlate",
				"LargeTube",
				"MedicalComponent",
				"MetalGrid",
				"MotorComponent",
				"PowerCell",
				"RadioCommunicationComponent",
				"SmallTube",
				"SolarCell",
				"SteelPlate"
			},
			new List<string>
			{
				// Filtered out components. So compared to vanilla. Which should be removed
				//"PowerCell",
				//"SolarCell",
				//"ComputerComponent",
			},
			new List<string>
			{
				// So compared to vanilla. What is missing from vanilla. But should be added
				"ExplosivesComponent",
				//Scrapyard does this:
				//ExplosivesComponent
			}
			);
			simpleComponentsManager.ProcessBlueprints();

			//Assembler
			var componentsManager = new ComponentBlueprintManager(
				"Components",
				new List<string>
				{
					"ConstructionComponent", "GirderComponent", "MetalGrid", "InteriorPlate", "SteelPlate",
					"SmallTube", "LargeTube", "MotorComponent", "Display", "BulletproofGlass",
					"ComputerComponent", "ReactorComponent", "ThrustComponent", "GravityGeneratorComponent",
					"MedicalComponent", "RadioCommunicationComponent", "DetectorComponent", "ExplosivesComponent",
					"SolarCell", "PowerCell", "Superconductor"
				},
				new List<string>
				{
					// Filtered out components. So compared to vanilla. Which should be removed
					"GravityGeneratorComponent",
					"ThrustComponent",
				},
				new List<string>
				{
					// Add any additional components here
				}
			);

			componentsManager.ProcessBlueprints();

			var eliteToolsManager = new ComponentBlueprintManager(
				"EliteTools",
				new List<string>
				{
					"Position0010_AngleGrinder", "Position0020_AngleGrinder2", "Position0030_AngleGrinder3", "Position0040_AngleGrinder4",
					"Position0050_HandDrill", "Position0060_HandDrill2", "Position0070_HandDrill3", "Position0080_HandDrill4",
					"Position0090_Welder", "Position0100_Welder2", "Position0110_Welder3", "Position0120_Welder4"
				},
				new List<string>
				{
					// Add any filtered components here
					"Position0120_Welder4",
					"Position0080_HandDrill4",
					"Position0040_AngleGrinder4",
				},
				new List<string>
				{
					// Add any additional components here
				}
			);

			eliteToolsManager.ProcessBlueprints();













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
