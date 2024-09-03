using AresAtWar.Configuration.Editor;
using AresAtWar.SessionCore;
using AresAtWar.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using VRage.Game;

namespace AresAtWar.Configuration {

	//General

	/*
	  
	  Hello Stranger!
	 
	  If you are in here because you want to change settings
	  for how this mod behaves, you are in the wrong place.

	  All the settings in this file, along with the other
	  configuration files, are created as XML files in the
	  \Storage\1521905890.sbm_ModularEncountersSpawner folder
	  of your Save File. This means you do not need to edit
	  the mod files here to tune the settings to your liking.

	  The workshop page for this mod also has a link to a
	  guide that explains what all the configuration options
	  do, along with how to activate them in-game via chat
	  commands if desired.
	  
	  If you plan to edit the values here anyway, I ask that
	  you do not reupload this mod to the Steam Workshop. If
	  this is not respected and I find out about it, I'll
	  exercise my rights as the creator and file a DMCA
	  takedown on any infringing copies. This warning can be
	  found on the workshop page for this mod as well.

	  Thank you.
		 
	*/

	public class ConfigWar{
		
		public string ModVersion {get; set;}

		public bool EnableWarSystem;
		public int MinBattleSimIniativeScore;
		public int MaxBattleSimIniativeScore;

		public int GCStrengthScore;
		public int FAFStrengthScore;
		public int SYNStrengthScore;

		[XmlIgnore]
		public bool ConfigLoaded;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigWar(){
			
			ModVersion = AaWSessionCore.ModVersion;

			EnableWarSystem = false;

			MinBattleSimIniativeScore = 6;
			MaxBattleSimIniativeScore = 16;

			GCStrengthScore = -100;
			FAFStrengthScore = 25;
			SYNStrengthScore = -5;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"EnableWarSystem", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableWarSystem) },
				{"MinBattleSimIniativeScore", (s, o) => EditorTools.SetCommandValueInt(s, ref MinBattleSimIniativeScore) },
				{"MaxBattleSimIniativeScore", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxBattleSimIniativeScore) },

				{"GCStrengthScore", (s, o) => EditorTools.SetCommandValueInt(s, ref GCStrengthScore) },
				{"FAFStrengthScore", (s, o) => EditorTools.SetCommandValueInt(s, ref FAFStrengthScore) },
				{"SYNStrengthScore", (s, o) => EditorTools.SetCommandValueInt(s, ref SYNStrengthScore) },

			};

		}
		
		public ConfigWar LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-Combat.xml", typeof(ConfigWar)) == true){
				
				try{
					
					ConfigWar config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-Combat.xml", typeof(ConfigWar));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigWar>(configcontents);
					config.ConfigLoaded = true;
					Logger.Write("Loaded Existing Settings From Config-Combat.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					Logger.Write("ERROR: Could Not Load Settings From Config-Combat.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigWar();
					return defaultSettings;
					
				}

			} else {

				Logger.Write("Config-Combat.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigWar();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-Combat.xml", typeof(ConfigWar))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigWar>(settings));
				
				}
				
			}catch(Exception exc){
				
				Logger.Write("ERROR: Could Not Create Config-Combat.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public string SaveSettings(){
			
			try{

				//return "Combat Phases Feature Not Fully Implemented. Config Only Saved For This Session.";

				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-Combat.xml", typeof(ConfigWar))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigWar>(this));
				
				}
				
				Logger.Write("Settings In Config-Combat.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				Logger.Write("ERROR: Could Not Save To Config-Combat.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
			}
			
			return "Settings Changed, But Could Not Be Saved To XML. Changes May Be Lost On Session Reload.";
			
		}

		public string EditFields(string receivedCommand) {

			var commandSplit = receivedCommand.Split('.');

			if (commandSplit.Length < 5)
				return "Provided Command Missing Parameters.";

			Func<string, object, bool> referenceMethod = null;

			if (!EditorReference.TryGetValue(commandSplit[3], out referenceMethod))
				return "Provided Field [" + commandSplit[3] + "] Does Not Exist.";


			if (!referenceMethod?.Invoke(receivedCommand, null) ?? false)
				return "Provided Value For [" + commandSplit[3] + "] Could Not Be Parsed.";

			return SaveSettings();

		}

	}
	
}