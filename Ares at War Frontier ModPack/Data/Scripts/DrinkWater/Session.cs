using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace DrinkWater
{
	public struct CharacterStats
	{
		public long playerId;
		public IMyCharacter character;
		public MyEntityStat water;
		public MyEntityStat food;
		public MyEntityStat sleep;
	}

	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class Session : MySessionComponentBase
	{
		private static int skippedTicks = 0;
		private static List<IMyPlayer> players = new List<IMyPlayer>();
		private static List<long> justSpawned = new List<long>();
		private static List<CharacterStats> charactersStats = new List<CharacterStats>();
		private bool isServer;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			isServer = MyAPIGateway.Multiplayer.IsServer;
			if (!isServer)
			{
				return;
			}
			MyVisualScriptLogicProvider.PlayerSpawned += (playerId) =>
			{
				justSpawned.Add(playerId);
			};
			MyAPIGateway.Session.SessionSettings.AutoHealing = false;
			UpdateAfterSimulation100();
		}

		public override void UpdateAfterSimulation()
		{
			if (!isServer)
			{
				return;
			}
			if (skippedTicks++ <= 100)
			{
				if (skippedTicks % 10 == 0)
				{
					UpdateAfterSimulation10();
				}
			}
			else
			{
				skippedTicks = 0;
				UpdateAfterSimulation100();
			}
		}

		public void UpdateAfterSimulation10()
		{
			foreach (CharacterStats characterStats in charactersStats)
			{
				if (characterStats.character.EnabledHelmet)
				{
					if (characterStats.water.HasAnyEffect())
					{
						characterStats.character.SwitchHelmet();
						MyVisualScriptLogicProvider.ShowNotification("Helmet opened to drink!", 3000, playerId: characterStats.playerId);
					}
					else if (characterStats.food.HasAnyEffect())
					{
						characterStats.character.SwitchHelmet();
						MyVisualScriptLogicProvider.ShowNotification("Helmet opened to eat!", 3000, playerId: characterStats.playerId);
					}
				}
			}
		}

		public void UpdateAfterSimulation100()
		{
			players.Clear();
			charactersStats.Clear();
			MyAPIGateway.Players.GetPlayers(players);

			foreach (IMyPlayer player in players)
			{
				if (player.IsBot == true)
				{
					continue;
				}
				MyEntityStatComponent statComp = player.Character?.Components?.Get<MyEntityStatComponent>();
				if (statComp == null)
				{
					continue;
				}

				MyEntityStat water;
				MyEntityStat food;
				MyEntityStat sleep;
				statComp.TryGetStat(MyStringHash.GetOrCompute("Water"), out water);
				statComp.TryGetStat(MyStringHash.GetOrCompute("Food"), out food);
				statComp.TryGetStat(MyStringHash.GetOrCompute("Sleep"), out sleep);

				charactersStats.Add(new CharacterStats
				{
					playerId = player.IdentityId,
					character = player.Character,
					water = water,
					food = food,
					sleep = sleep
				});

				if (water.Value <= 0)
				{
					player.Character.DoDamage(Config.statsConfig.WATER_DAMAGE, MyDamageType.Unknown, true);
				}

				if (food.Value <= 0)
				{
					player.Character.DoDamage(Config.statsConfig.FOOD_DAMAGE, MyDamageType.Unknown, true);
				}

				if (sleep.Value <= 0)
				{
					player.Character.DoDamage(Config.statsConfig.SLEEP_DAMAGE, MyDamageType.Unknown, true);
				}

				bool inCryoOrBed = false;

				if (player.Character.CurrentMovementState == MyCharacterMovementEnum.Sitting &&
					(player.Controller.ControlledEntity as IMyShipController) != null &&
					!(player.Controller.ControlledEntity as IMyShipController).CanControlShip)
				{
					//Sitting, but not working
					inCryoOrBed = player.Controller.ControlledEntity.ToString().StartsWith("MyCryoChamber");
					float sleepGain = inCryoOrBed ? Config.statsConfig.SLEEP_GAIN_SLEEPING : Config.statsConfig.SLEEP_GAIN_SITTING;
					sleep.Increase(sleepGain, null);
				}
				else
				{
					sleep.Decrease(Config.statsConfig.SLEEP_USAGE, null);
				}

				if (!inCryoOrBed)
				{
					food.Decrease(Config.statsConfig.FOOD_USAGE, null);
					water.Decrease(Config.statsConfig.WATER_USAGE, null);
				}

				if (justSpawned.Contains(player.IdentityId))
				{
					sleep.Value = Config.statsConfig.RESPAWN_SLEEP;
					food.Value = Config.statsConfig.RESPAWN_FOOD;
					water.Value = Config.statsConfig.RESPAWN_WATER;
					justSpawned.Remove(player.IdentityId);
				}
			}
		}
	}
}
