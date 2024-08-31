using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace PlantAndCook
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class Session : MySessionComponentBase
	{
		private static int runCount = 0;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			MyAPIGateway.Entities.OnEntityAdd += ReplaceBlocks;
		}

		public override void UpdateAfterSimulation()
		{
			try
			{
				if (runCount++ < 100)
				{
					return;
				}
				else
				{
					runCount = 0;
				}

				var players = new List<IMyPlayer>();
				MyAPIGateway.Multiplayer.Players.GetPlayers(players, p => p.Character != null && p.Character.ToString().Contains("Astronaut"));

				foreach (IMyPlayer player in players)
				{
					var statComp = player.Character?.Components.Get<MyEntityStatComponent>();
					if (statComp == null)
						continue;

					MyEntityStat sleep = GetPlayerStat(statComp, "Sleep");
					MyEntityStat food = GetPlayerStat(statComp, "Food");

					if (sleep == null || food == null)
						continue;

					var block = player.Controller?.ControlledEntity?.Entity as IMyCubeBlock;
					if (block != null)
					{
						string subtypeId = block.BlockDefinition.SubtypeId.ToLower();
						if (subtypeId.Contains("toilet") || subtypeId.Contains("bathroom"))
						{
							food.Decrease(5f, null);
							if (food.Value > 0)
								player.Character.GetInventory(0).AddItems((MyFixedPoint)0.05f, (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Organic")));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Echo("Plant and Cook exception", ex.ToString());
			}
		}

		private MyEntityStat GetPlayerStat(MyEntityStatComponent statComp, string statName)
		{
			MyEntityStat stat;
			statComp.TryGetStat(MyStringHash.GetOrCompute(statName), out stat);
			return stat;
		}

		private static void Echo(string msg1, string msg2 = "")
		{
			MyLog.Default.WriteLineAndConsole(msg1 + ": " + msg2);
			//MyAPIGateway.Utilities.ShowNotification(msg1 + ": " + msg2, 5000);
		}

		private void ReplaceBlocks(IMyEntity ent)
		{
			if (!(ent is IMyCubeGrid) || ent.Physics == null)
				return;

			var grid = ent as IMyCubeGrid;

			try
			{
				var slimList = new List<IMySlimBlock>();
                List<string> idsToReplace = new List<string>
                {
                    "MyObjectBuilder_Planter/LargeBlockPlanters",
                    "MyObjectBuilder_Kitchen/LargeBlockKitchen",
                    "MyObjectBuilder_CubeBlock/LargeBlockBarCounter",
                    "MyObjectBuilder_CubeBlock/LargeBlockBarCounterCorner"
                };
                grid.GetBlocks(slimList, s => idsToReplace.Contains(s.BlockDefinition.Id.ToString()));

				foreach (var slim in slimList)
				{
                    switch (slim.BlockDefinition.Id.ToString())
                    {
						case "MyObjectBuilder_Planter/LargeBlockPlanters":
							ReplaceBlock(slim, typeof(MyObjectBuilder_Assembler), "Planters");
							break;
						case "MyObjectBuilder_Kitchen/LargeBlockKitchen":
							ReplaceBlock(slim, typeof(MyObjectBuilder_Assembler), "Kitchen");
							break;
						case "MyObjectBuilder_CubeBlock/LargeBlockBarCounter":
							ReplaceBlock(slim, typeof(MyObjectBuilder_CargoContainer), "LargeBlockBarCounter");
							break;
						case "MyObjectBuilder_CubeBlock/LargeBlockBarCounterCorner":
							ReplaceBlock(slim, typeof(MyObjectBuilder_CargoContainer), "LargeBlockBarCounterCorner");
							break;
						default:
                            break;
                    }
				}
			}
			catch (Exception ex)
			{
				Echo("Plant and Cook exception", ex.ToString());
			}
		}

		private static void ReplaceBlock(IMySlimBlock oldBlock, MyObjectBuilderType newType, string newSubtype)
		{
			try
			{
				MyObjectBuilder_CubeBlock oldBuilder = oldBlock.GetObjectBuilder();
				var newDefinitionId = new MyDefinitionId(newType, newSubtype);
				MyObjectBuilder_CubeBlock newBuilder = MyObjectBuilderSerializer
					.CreateNewObject(newDefinitionId) as MyObjectBuilder_CubeBlock;

				newBuilder.BlockOrientation = oldBuilder.BlockOrientation;
				newBuilder.Min = oldBuilder.Min;
				newBuilder.ColorMaskHSV = oldBuilder.ColorMaskHSV;
				newBuilder.SkinSubtypeId = oldBuilder.SkinSubtypeId;
				newBuilder.Owner = oldBuilder.Owner;

				IMyCubeGrid grid = oldBlock.CubeGrid;

				grid.RemoveBlock(oldBlock);
				grid.AddBlock(newBuilder, false);
			}
			catch (Exception ex)
			{
				Echo("Plant and Cook exception", ex.ToString());
			}
		}
	}
}
