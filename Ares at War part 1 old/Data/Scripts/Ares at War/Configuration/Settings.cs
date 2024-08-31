using AresAtWar.Logging;

using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;

namespace AresAtWar.Configuration {
	public static class Settings {

		public static string UniqueModId = "";

		public static ConfigGeneral General = new ConfigGeneral();
		public static ConfigWar War = new ConfigWar();

		public static SavedInternalData SavedData = null;

		public static void InitSettings(string phase) {

			if (!MyAPIGateway.Multiplayer.IsServer)
				return;

			if (phase == "BeforeStart" && string.IsNullOrWhiteSpace(UniqueModId)) {

				if (!MyAPIGateway.Utilities.GetVariable<string>("MES-SavedDataId", out UniqueModId)) {

					UniqueModId = DateTime.Now.Ticks.ToString();
					MyAPIGateway.Utilities.SetVariable<string>("MES-SavedDataId", UniqueModId);

				}
			
			}

			if(!General.ConfigLoaded)
				General = General.LoadSettings(phase);


			if (MyAPIGateway.Multiplayer.IsServer && SavedData == null && phase == "BeforeStart")
				SavedData = SavedInternalData.LoadSettings(phase);



		}




		public static void SaveAll() {

			if (!MyAPIGateway.Multiplayer.IsServer)
				return;

			try {


				War.SaveSettings();

				General.SaveSettings();

				

			} catch (Exception e) {

				Logger.Write(e.ToString(), SpawnerDebugEnum.Error, true);

			}

		}

	}

}
