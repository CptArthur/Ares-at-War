using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Game.Components;

namespace DrinkWater
{
	public struct Block
	{
		public string SubtypeId;
		public bool RequiresPressure;
	}

	public struct StatsConfig
	{
		public float WATER_USAGE;
		public float FOOD_USAGE;
		public float SLEEP_USAGE;
		public float WATER_DAMAGE;
		public float FOOD_DAMAGE;
		public float SLEEP_DAMAGE;
		public float SLEEP_GAIN_SITTING;
		public float SLEEP_GAIN_SLEEPING;
		public float RESPAWN_WATER;
		public float RESPAWN_FOOD;
		public float RESPAWN_SLEEP;
	}

	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class Config : MySessionComponentBase
	{
		public static StatsConfig statsConfig = new StatsConfig()
		{
			WATER_USAGE = 0.003f, //Usued to be 0.03f
            FOOD_USAGE = 0.0015f,//0.015f
            SLEEP_USAGE = 0.001f, //0.01f
            WATER_DAMAGE = 3f,
			FOOD_DAMAGE = 1.5f,
			SLEEP_DAMAGE = 1f,
			SLEEP_GAIN_SITTING = 0.5f,
			SLEEP_GAIN_SLEEPING = 34f,
			RESPAWN_WATER = 10f,
			RESPAWN_FOOD = 10f,
			RESPAWN_SLEEP = 10f,
		};

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			if (!MyAPIGateway.Multiplayer.IsServer)
			{
				return;
			}
			try
			{
				string configFileName = "config_2_14.xml";
				if (MyAPIGateway.Utilities.FileExistsInWorldStorage(configFileName, typeof(StatsConfig)))
				{
					var textReader = MyAPIGateway.Utilities.ReadFileInWorldStorage(configFileName, typeof(StatsConfig));
					var configXml = textReader.ReadToEnd();
					textReader.Close();
					statsConfig = MyAPIGateway.Utilities.SerializeFromXML<StatsConfig>(configXml);
				}
				else
				{
					var textWriter = MyAPIGateway.Utilities.WriteFileInWorldStorage(configFileName, typeof(StatsConfig));
					textWriter.Write(MyAPIGateway.Utilities.SerializeToXML(statsConfig));
					textWriter.Flush();
					textWriter.Close();
				}
			}
			catch (Exception e)
			{
				MyAPIGateway.Utilities.ShowMessage("EDSR", "Exception: " + e);
			}
		}
	}
}
