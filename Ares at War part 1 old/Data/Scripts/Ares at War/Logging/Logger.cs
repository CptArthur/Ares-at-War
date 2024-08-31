using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Utils;

namespace AresAtWar.Logging {

	public struct LoggerQueue
	{

		public string message;
		public SpawnerDebugEnum type;
		public bool forceSpawn;

		public LoggerQueue(string msg, SpawnerDebugEnum spawntype, bool force = false)
		{

			message = msg;
			type = spawntype;
			forceSpawn = force;

		}

	}


	public enum DebugEnum {

		None,
		TempDebug,
		Terminal,

	}

	[Flags]
	public enum SpawnerDebugEnum {

		None = 0,
		Startup = 1,
		Error = 1 << 1,
		Settings = 1 << 2,
		War = 1 << 3,
		GameLog= 1 << 4,


	}

	public static class Logger {

		public static bool LoggerDebugMode = false;
		public static bool DebugToGameLog = false;

		//Spawner Debugs
		public static StringBuilder War = new StringBuilder();
		public static StringBuilder Error = new StringBuilder();
		public static StringBuilder Settings = new StringBuilder();
		public static StringBuilder Startup = new StringBuilder();

		public static List<LoggerQueue> QueuedItems = new List<LoggerQueue>();
		public static Dictionary<string, int> Reason = null;

		//Behavior Debugs

		public static SpawnerDebugEnum ActiveDebug = SpawnerDebugEnum.None;

		public static void Setup() {

			int activeInt = 0;

			if (MyAPIGateway.Utilities.GetVariable<int>("MES-ActiveSpawnerDebug", out activeInt)) {

				ActiveDebug = (SpawnerDebugEnum)activeInt;
				Write("Active Debug Loaded: " + ActiveDebug.ToString(), SpawnerDebugEnum.Startup, true);

			} else {

				Write("Active Debug Not Found", SpawnerDebugEnum.Startup, true);

			}

		
		}

		public static void AppendDateAndTime(StringBuilder sb)
		{

			DateTime time = DateTime.Now;
			sb.Append(time.ToString("yyyy-MM-dd hh-mm-ss-fff")).Append(": ");

		}

		public static void Queue(string msg, SpawnerDebugEnum type, bool force = false, bool addToReason = false) {

			QueuedItems.Add(new LoggerQueue(msg, type, force));

			if (Reason == null || !addToReason)
				return;

			string trimmedReason = msg.Trim();
			int output = 0;

			if (!Reason.TryGetValue(trimmedReason, out output))
				Reason.Add(trimmedReason, 0);

			Reason[trimmedReason]++;

		}

		public static void ProcessQueue() {

			lock (QueuedItems) {

				for (int i = 0; i < QueuedItems.Count; i++) {

					var item = QueuedItems[i];
					Write(item.message, item.type, item.forceSpawn);

				}

				QueuedItems.Clear();
			
			}
		
		}

		public static bool SetActiveDebugFlag(SpawnerDebugEnum type, bool mode) {

			bool updated = false;

			if (mode && !ActiveDebug.HasFlag(type)) {

				ActiveDebug |= type;
				updated = true;

			} else if (!mode && ActiveDebug.HasFlag(type)) {

				ActiveDebug &= ~type;
				updated = true;

			}

			if (updated) {

				var intVal = (int)ActiveDebug;
				Write(type.ToString() + " / " + intVal, SpawnerDebugEnum.Settings, true);
				MyAPIGateway.Utilities.SetVariable<int>("MES-ActiveSpawnerDebug", intVal);

			}

			return updated;
		
		}

		public static void Write(string msg, SpawnerDebugEnum type, bool forceGameLog = false) {

			if (type == SpawnerDebugEnum.War)
				WriteToBuilder(msg, type, War, forceGameLog);

			if (type == SpawnerDebugEnum.Error)
				WriteToBuilder(msg, type, Error, forceGameLog);

			if (type == SpawnerDebugEnum.Settings)
				WriteToBuilder(msg, type, Settings, forceGameLog);

			if (type == SpawnerDebugEnum.Startup)
				WriteToBuilder(msg, type, Startup, forceGameLog);


		}

		public static void WriteToBuilder(string msg, SpawnerDebugEnum type, StringBuilder sb, bool forceGameLog) {

		//	if (!ActiveDebug.HasFlag(type) && !forceGameLog)
		//		return;

			AppendDateAndTime(sb);
			sb.Append("MES / ").Append(type.ToString()).Append(": ").Append(msg).AppendLine();
			WriteToGameLog(msg, type, forceGameLog);

		}

		public static void WriteToGameLog(string msg, SpawnerDebugEnum type, bool forceGameLog) {

			if (!ActiveDebug.HasFlag(SpawnerDebugEnum.GameLog) && !forceGameLog)
				return;

			MyLog.Default.WriteLineAndConsole("MES Spawner / " + type.ToString() + ": " + msg);
		
		}

		

	}

}
