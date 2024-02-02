using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Lights;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;


namespace AaW_CustomEnergyWeapons{
	
	public class BarrelData{
		
		public bool Active {get; set;}
		public bool Scanning {get; set;}
		public bool DamageReady {get; set;}
		public bool Damaging {get; set;}
		public bool ResetHits {get; set;}
		
		public bool HitObject {get; set;}
		public bool HitGrid {get; set;}
		public bool HitVoxel {get; set;}
		public bool HitDefenseShield {get; set;}
		
		public double HitDistance {get; set;}
		public IMyCubeGrid CubeGrid {get; set;}
		public IMyTerminalBlock ShieldBlock {get; set;}
		
		public BarrelData(){
			
			Active = false;
			Scanning = false;
			DamageReady = false;
			Damaging = false;
			ResetHits = false;
			
			HitObject = false;
			HitGrid = false;
			HitVoxel = false;
			HitDefenseShield = false;
			
			HitDistance = -1;
			CubeGrid = null;
			ShieldBlock = null;
			
		}
		
	}
	
}