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
using ProtoBuf;


namespace AresAtWar.Managers
{
	public class GPSManager
    {
		public static List<GPS> AllActiveGPS = new List<GPS>();

		private static string _AllActiveGPSName = "AaW-ActiveGPS";




		public static List<string> times = new List<string> {" 1m", " 5m", " 10m", " 15m", " 30m", " 45m", " 1h", " 1h30m", " 2h", " 3h", " 4h" };



		public static void Setup()
        {
			//Get Existing Event Data
			string AllActiveGPSString = "";
			if (MyAPIGateway.Utilities.GetVariable<string>(_AllActiveGPSName, out AllActiveGPSString))
			{
				var GPSListSerialized = Convert.FromBase64String(AllActiveGPSString);
				AllActiveGPS = MyAPIGateway.Utilities.SerializeFromBinary<List<GPS>>(GPSListSerialized);
			}


		}


		public static void SaveData()
		{

			//AllActiveGPS
			var AllGPSListSerialized = MyAPIGateway.Utilities.SerializeToBinary<List<GPS>>(AllActiveGPS);
			var AllGPSListString = Convert.ToBase64String(AllGPSListSerialized);
			MyAPIGateway.Utilities.SetVariable<string>(_AllActiveGPSName, AllGPSListString);
		}



	}
	[ProtoContract]
	public class GPS
	{
		[ProtoMember(1)] public DateTime StartingDate;
		[ProtoMember(2)] public DateTime EndDate;

		[ProtoMember(3)] public string currenttimedisplay;

		[ProtoMember(4)] public string name;
		[ProtoMember(5)] public string description;
		[ProtoMember(6)] public Vector3D position;
		[ProtoMember(7)] public Color color;
		[ProtoMember(8)] public bool IgnoreCooldown;

		public GPS()
		{
		}

		public GPS(int minute, string name, string description, Vector3D position, Color color)
		{
			StartingDate = MyAPIGateway.Session.GameDateTime;
			EndDate = MyAPIGateway.Session.GameDateTime.AddMinutes(minute);

			this.name = name;
			this.description = description;
			this.position = position;
			this.color = color;
			currenttimedisplay = "";

			_addGPSObjectiveForAll(name, description, position, color);


		}

		public GPS(string name, string description, Vector3D position, Color color)
		{
			StartingDate = MyAPIGateway.Session.GameDateTime;
			EndDate = MyAPIGateway.Session.GameDateTime.AddMinutes(1);

			this.name = name;
			this.description = description;
			this.position = position;
			this.color = color;
			this.IgnoreCooldown = true;
			currenttimedisplay = "";
			_addGPSObjectiveForAll(name, description, position, color);



		}


		public void Update()
        {

            if (!IgnoreCooldown)
            {
				var difference = EndDate - MyAPIGateway.Session.GameDateTime;

				var totalmin = difference.TotalMinutes;
				if (totalmin < 0)
				{
					RemovefromActive();
					return;
				}

				if (totalmin < 30)
				{
					if (totalmin < 1)
					{
						if (currenttimedisplay != name + " 1m")
							_addGPSObjectiveForAll(name + " 1m", description, position, color);
						return;
					}
					if (totalmin < 5)
					{
						if (currenttimedisplay != name + " 5m")
							_addGPSObjectiveForAll(name + " 5m", description, position, color);
						return;
					}
					if (totalmin < 10)
					{
						if (currenttimedisplay != name + " 10m")
							_addGPSObjectiveForAll(name + " 10m", description, position, color);
						return;
					}

					if (totalmin < 15)
					{
						if (currenttimedisplay != name + " 15m")
							_addGPSObjectiveForAll(name + " 15m", description, position, color);
						return;
					}
					if (totalmin < 30)
					{
						if (currenttimedisplay != name + " 30m")
							_addGPSObjectiveForAll(name + " 30m", description, position, color);
						return;
					}
				}
				else
				{
					if (totalmin < 45)
					{
						if (currenttimedisplay != name + " 45m")
							_addGPSObjectiveForAll(name + " 45m", description, position, color);
						return;
					}
					if (totalmin < 60)
					{
						if (currenttimedisplay != name + " 1h")
							_addGPSObjectiveForAll(name + " 1h", description, position, color);
						return;
					}
					if (totalmin < 90)
					{
						if (currenttimedisplay != name + " 1h30m")
							_addGPSObjectiveForAll(name + " 1h30m", description, position, color);
						return;
					}
					if (totalmin < 120)
					{
						if (currenttimedisplay != name + " 2h")
							_addGPSObjectiveForAll(name + " 2h", description, position, color);
						return;
					}
					if (totalmin < 180)
					{
						if (currenttimedisplay != name + " 3h")
							_addGPSObjectiveForAll(name + " 3h", description, position, color);
						return;
					}
					if (totalmin < 240)
					{
						if (currenttimedisplay != name + " 4h")
							_addGPSObjectiveForAll(name + " 4h", description, position, color);
						return;
					}
				}
				return;
			}

		}

		public void _addGPSObjectiveForAll(string name, string description, Vector3D position, Color color)
        {
			currenttimedisplay = name;

            if (!this.IgnoreCooldown)
            {
				foreach (string time in GPSManager.times)
				{
					MyVisualScriptLogicProvider.RemoveGPSForAll(this.name + time);
				}
			}
			MyVisualScriptLogicProvider.RemoveGPSForAll(this.name);

			MyVisualScriptLogicProvider.AddGPSObjectiveForAll(name, description, position, color);
		}

		public void RemovefromActive()
        {
			if (!this.IgnoreCooldown)
			{
				foreach (string time in GPSManager.times)
				{
					MyVisualScriptLogicProvider.RemoveGPSForAll(this.name + time);
				}
			}
			MyVisualScriptLogicProvider.RemoveGPSForAll(this.name);
			GPSManager.AllActiveGPS.Remove(this);

		}

	}



}

