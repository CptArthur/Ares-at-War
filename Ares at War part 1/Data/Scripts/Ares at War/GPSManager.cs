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

namespace AresAtWar.GPSManagers
{
	public class GPSManager
    {
		public static List<GPS> AllActiveGPS = new List<GPS>();
		public static List<GPS> NonActiveGPS = new List<GPS>();

		private static string _AllActiveGPSName = "AaW-ActiveGPS";
		private static string _NonActiveGPSName = "AaW-NonActiveGPS";

		public static void Setup()
        {
			//Get Existing Event Data
			string AllActiveGPSString = "";
			if (MyAPIGateway.Utilities.GetVariable<string>(_AllActiveGPSName, out AllActiveGPSString))
			{
				var GPSListSerialized = Convert.FromBase64String(AllActiveGPSString);
				AllActiveGPS = MyAPIGateway.Utilities.SerializeFromBinary<List<GPS>>(GPSListSerialized);
			}


			//Get Existing Event Data
			string NonActiveGPSActiveGPSString = "";
			if (MyAPIGateway.Utilities.GetVariable<string>(_NonActiveGPSName, out NonActiveGPSActiveGPSString))
			{
				var GPSListSerialized = Convert.FromBase64String(NonActiveGPSActiveGPSString);
				NonActiveGPS = MyAPIGateway.Utilities.SerializeFromBinary<List<GPS>>(GPSListSerialized);
			}

		}


		public static void SaveData()
		{

			//AllActiveGPS
			var AllGPSListSerialized = MyAPIGateway.Utilities.SerializeToBinary<List<GPS>>(AllActiveGPS);
			var AllGPSListString = Convert.ToBase64String(AllGPSListSerialized);
			MyAPIGateway.Utilities.SetVariable<string>(_AllActiveGPSName, AllGPSListString);

			//NonActiveGPS
			var NonGPSListSerialized = MyAPIGateway.Utilities.SerializeToBinary<List<GPS>>(NonActiveGPS);
			var NonGPSListString = Convert.ToBase64String(NonGPSListSerialized);
			MyAPIGateway.Utilities.SetVariable<string>(_NonActiveGPSName, NonGPSListString);
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

			AddGPSObjectiveForAll(name, description, position, color);
			currenttimedisplay = "";


			GPSManager.SaveData();
		}

		public void Update()
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
					if(currenttimedisplay != name + " 1m")
						AddGPSObjectiveForAll(name + " 1m", description, position, color);
					return;
				}
				if (totalmin < 5 )
				{
					if (currenttimedisplay != name + " 5m")
						AddGPSObjectiveForAll(name + " 5m", description, position, color);
					return;
				}
				if (totalmin < 10 )
				{
					if (currenttimedisplay != name + " 10m")
						AddGPSObjectiveForAll(name + " 10m", description, position, color);
					return;
				}

				if (totalmin < 15 )
				{
					if (currenttimedisplay != name + " 15m")
						AddGPSObjectiveForAll(name + " 15m", description, position, color);
					return;
				}
				if (totalmin < 30 )
				{
					if (currenttimedisplay != name + " 30m")
						AddGPSObjectiveForAll(name + " 30m", description, position, color);
					return;
				}
			}
            else
            {
				if (totalmin < 45)
				{
					if (currenttimedisplay != name + " 45m")
						AddGPSObjectiveForAll(name + " 45m", description, position, color);
					return;
				}
				if (totalmin < 60)
				{
					if (currenttimedisplay != name + " 1h")
						AddGPSObjectiveForAll(name + " 1h", description, position, color);
					return;
				}
				if (totalmin < 90)
				{
					if (currenttimedisplay != name + " 1h30m")
						AddGPSObjectiveForAll(name + " 1h30m", description, position, color);
					return;
				}
				if (totalmin < 120)
				{
					if (currenttimedisplay != name + " 2h")
						AddGPSObjectiveForAll(name + " 2h", description, position, color);
					return;
				}
				if (totalmin < 180)
				{
					if (currenttimedisplay != name + " 3h")
						AddGPSObjectiveForAll(name + " 3h", description, position, color);
					return;
				}
				if (totalmin < 240)
				{
					if (currenttimedisplay != name + " 4h")
						AddGPSObjectiveForAll(name + " 4h", description, position, color);
					return;
				}
			}
			return;
		}

		public void AddGPSObjectiveForAll(string name, string description, Vector3D position, Color color)
        {
			currenttimedisplay = name;
			List<string> times =  new List<string>{"" ," 1m", " 5m", " 10m", " 15m", " 30m", " 45m", " 1h", " 1h30m", " 2h", " 3h", " 4h" };
			foreach (string time in times)
            {
				MyVisualScriptLogicProvider.RemoveGPSForAll(this.name + time);
            }
			MyVisualScriptLogicProvider.AddGPSObjectiveForAll(name, description, position, color);
			GPSManager.SaveData();
		}

		public void RemovefromActive()
        {
			List<string> times = new List<string> { "", " 1m", " 5m", " 10m", " 15m", " 30m", " 45m", " 1h", " 1h30m", " 2h", " 3h", " 4h" };
			foreach (string time in times)
			{
				MyVisualScriptLogicProvider.RemoveGPSForAll(this.name + time);
			}

			GPSManager.AllActiveGPS.Remove(this);
			GPSManager.NonActiveGPS.Add(this);
			GPSManager.SaveData();
		}
		public void RemoveGPSForAll()
		{
			List<string> times = new List<string> { "", " 1m", " 5m", " 10m", " 15m", " 30m", " 45m", " 1h", " 1h30m", " 2h", " 3h", " 4h" };
			foreach (string time in times)
			{
				MyVisualScriptLogicProvider.RemoveGPSForAll(this.name + time);
			}
			GPSManager.SaveData();
		}

	}



}

