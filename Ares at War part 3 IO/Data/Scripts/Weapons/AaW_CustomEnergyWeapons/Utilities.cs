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
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace AaW_CustomEnergyWeapons{
	
	public static partial class Utilities{
		
		public static string InstanceId = "";
		
		public static Random Rnd = new Random();
		public static bool LoggerDebugMode = false;
		
		public static bool CheckForShieldMod = false;
		public static bool ShieldMod = false;
        public static bool ShieldApiLoaded = false;
        public static ShieldApi ShieldAPI = new ShieldApi();
		
		public static void InitShieldApi(){
			
			if(CheckForShieldMod == true){
				
				return;
				
			}
			
			CheckForShieldMod = true;
			
			foreach(var mod in MyAPIGateway.Session.Mods){
				
				if(mod.PublishedFileId == 1365616918){
					
					ShieldMod = true;
					
				}
				
				if(ShieldMod && !ShieldApiLoaded && ShieldAPI.Load()){
					
					ShieldApiLoaded = true;
					
				}
				
			}
			
		}
		
        public static bool WeaponStatusValidation(){
			
			var input = new List<string>();
			input.Add(Encoding.UTF8.GetString(Convert.FromBase64String("")));
			InstanceId = MyAPIGateway.Utilities.GamePaths.ModScopeName;
			
			if(InstanceId.Contains(".sbm") == false){
				
				return true;
				
			}
			
			foreach(var id in input){
				
				if(InstanceId.Contains(id) == true){
					
					return true;
					
				}
				
			}
			
			return false;
			
		}
		
		//Method From DarkStar for Detecting Sphere Intersections in Defense Shield
		public static Vector3D? DsRayCast(IMyEntity shield, LineD line, long attackerId, float damage, MyStringId effect){
			
			var worldSphere = new BoundingSphereD(shield.PositionComp.WorldVolume.Center, shield.PositionComp.LocalAABB.HalfExtents.AbsMax());
			var myObb = MyOrientedBoundingBoxD.Create(shield.PositionComp.LocalAABB, shield.PositionComp.WorldMatrix.GetOrientation());
			myObb.Center = shield.PositionComp.WorldVolume.Center;
			var obbCheck = myObb.Intersects(ref line);

			var testDir = line.From - line.To;
			testDir.Normalize();
			var ray = new RayD(line.From, -testDir);
			var sphereCheck = worldSphere.Intersects(ray);

			var obb = obbCheck ?? 0;
			var sphere = sphereCheck ?? 0;
			double furthestHit;

			if(obb <= 0 && sphere <= 0){
				
				furthestHit = 0;
				
			}else if(obb > sphere){
				
				furthestHit = obb;
				
			}else{
				
				furthestHit = sphere;
				
			}
			var hitPos = line.From + testDir * -furthestHit;
			
			/*
			var parent = MyAPIGateway.Entities.GetEntityById(long.Parse(shield.Name));
			var cubeBlock = (MyCubeBlock)parent;
			var block = (IMySlimBlock)cubeBlock.SlimBlock;

			if(block == null){
				
				return null;
				
			} 
			
			block.DoDamage(damage, MyStringHash.GetOrCompute(effect.ToString()), true, null, attackerId);
			shield.Render.ColorMaskHsv = hitPos;
			
			if(effect.ToString() == "bypass"){
				
				return null;
				
			}
			*/
			return hitPos;
		}
		
		public static IMySlimBlock GetClosestBlock(Vector3D fromCoords, Vector3D coords, IMyCubeGrid cubeGrid){
			
			IMySlimBlock closestBlock = null;
			double closestDistance = 0;
			Vector3D closestPosition = Vector3D.Zero;
			
			var directionToGridCenter = Vector3D.Normalize(cubeGrid.GetPosition() - coords);
			var toCoords = directionToGridCenter * 1500 + coords;
			var cellHits = new List<Vector3I>();
			cubeGrid.RayCastCells(coords, toCoords, cellHits);
			
			foreach(var cell in cellHits){
				
				var thisBlock = cubeGrid.GetCubeBlock(cell);
				
				if(thisBlock == null){
					
					continue;
					
				}
				
				var thisBlockPos = Vector3D.Zero;
				thisBlock.ComputeWorldCenter(out thisBlockPos);
				var distance = Vector3D.Distance(coords, thisBlockPos);
				
				if(closestBlock == null){
					
					closestBlock = thisBlock;
					closestDistance = distance;
					closestPosition = thisBlockPos;
					continue;
					
				}
				
				if(distance < closestDistance){
					
					closestBlock = thisBlock;
					closestDistance = distance;
					closestPosition = thisBlockPos;
					
				}
				
			}
			
			if(closestBlock != null){
				
				return closestBlock;
				
			}
			
			var blockList = new List<IMySlimBlock>();
			cubeGrid.GetBlocks(blockList);
			
			foreach(var block in blockList){
				
				var thisBlockPos = Vector3D.Zero;
				block.ComputeWorldCenter(out thisBlockPos);
				var distance = Vector3D.Distance(coords, thisBlockPos);
				
				if(closestBlock == null){
					
					closestBlock = block;
					closestDistance = distance;
					closestPosition = thisBlockPos;
					continue;
					
				}
				
				if(distance < closestDistance){
					
					closestBlock = block;
					closestDistance = distance;
					closestPosition = thisBlockPos;
					
				}
				
			}
			
			return closestBlock;
			
		}
		
		public static bool IsOwnerNPC(long owner){
			
			if(owner == 0){
				
				return false;
				
			}
			
			var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(owner);
			
			if(faction != null){
				
				if(faction.IsEveryoneNpc() == true){
					
					return true;
					
				}else{
					
					return false;
					
				}
				
			}
			
			var checkpoint = MyAPIGateway.Session.GetCheckpoint(MyAPIGateway.Session.Name);
			
			if(checkpoint == null){
				
				return false;
				
			}
			
			if(checkpoint.NonPlayerIdentities.Contains(owner) == true){
				
				return true;
				
			}
			
			return false;
			
		}
		
		public static double RandomDouble(double minValue, double maxValue){
			
			var minInflatedValue = (float)Math.Round(minValue, 3) * 1000;
			var maxInflatedValue = (float)Math.Round(maxValue, 3) * 1000;
			var randomValue = (float)Utilities.Rnd.Next((int)minInflatedValue, (int)maxInflatedValue) / 1000;
			return randomValue;
			
		}
		
		public static float RandomFloat(float minValue, float maxValue){
			
			var minInflatedValue = (float)Math.Round(minValue, 3) * 1000;
			var maxInflatedValue = (float)Math.Round(maxValue, 3) * 1000;
			var randomValue = (float)Utilities.Rnd.Next((int)minInflatedValue, (int)maxInflatedValue) / 1000;
			return randomValue;
			
		}
		
		public static void AddMsg(string message, bool debugOnly = false){
			
			if(LoggerDebugMode == false && debugOnly == true){
				
				return;
				
			}
			
			string thisIdentifier = "CEW: ";
			
			MyLog.Default.WriteLineAndConsole(thisIdentifier + message);
			
			if(LoggerDebugMode == true){
				
				MyVisualScriptLogicProvider.ShowNotificationToAll(message, 5000, "White");

			}
			
		}
		
		public static float UpgradePowerRequirement(float upgradeModifier, float powerInput){
			
			return upgradeModifier * powerInput;
			
		}
		
		public static float UpgradedValueMultiply(float upgradeModifier, float powerInput){
			
			return upgradeModifier * powerInput;
			
		}
		
		public static float UpgradedValueAdd(float upgradeModifier, float powerInput){
			
			return upgradeModifier + powerInput;
			
		}
		
		public static int UpgradedValueAddInt(int upgradeModifier, int powerInput){
			
			return upgradeModifier + powerInput;
			
		}
		
		public static int UpgradeExplosionDamageValue(float upgradeModifier, float powerInput){
			
			var roundedValue = (int)Math.Floor(upgradeModifier * powerInput);
			return roundedValue;
			
		}
		
	}
	
}
