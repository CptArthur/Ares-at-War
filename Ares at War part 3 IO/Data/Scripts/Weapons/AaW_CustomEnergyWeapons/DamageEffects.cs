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
		
		public static long CorruptFounderId = 0;
		
		public static List<string> ShieldBlockSubtypeNames = new List<string>();
		
		public static bool DefenseShieldModCheck = false;
		public static bool DefenseShieldModActive = false;
		
		public static bool DamageHandlerRegistered = false;
		public static Dictionary<long, WeaponConfig> TeslaProjectileWeapons = new Dictionary<long, WeaponConfig>();
		
		public static void AddVoxels(Vector3D position, float radius, string subtypeName){
			
			try{
				
				var voxelMaterialDef = MyDefinitionManager.Static.GetVoxelMaterialDefinition(subtypeName);
				
				if(voxelMaterialDef == null){
					
					return;
					
				}
				
				var voxelShape = MyAPIGateway.Session.VoxelMaps.GetSphereVoxelHand();
				voxelShape.Center = position;
				voxelShape.Radius = radius;
				voxelShape.Transform = MatrixD.CreateWorld(position, Vector3D.Forward, Vector3D.Up);
				var sphere = new BoundingSphereD(position, 500);
				var mapList = new List<IMyVoxelBase>();
				MyAPIGateway.Session.VoxelMaps.GetInstances(mapList);
				
				//Debug:
				//var gps = MyAPIGateway.Session.GPS.Create("Hit", "", position, true);
				//MyAPIGateway.Session.GPS.AddLocalGps(gps);
				
				for(int i = mapList.Count - 1; i >= 0; i--){
					
					if(mapList[i].PositionComp.WorldAABB.Intersects(sphere) == false){
						
						mapList.RemoveAt(i);
						continue;
						
					}
					
					MyAPIGateway.Session.VoxelMaps.FillInShape(mapList[i], voxelShape, voxelMaterialDef.Index);
					
				}
				
			}catch(Exception exc){
				
				//MyVisualScriptLogicProvider.ShowNotificationToAll("Cut Voxel Fail", 1000);
				
			}
			
		}
		
		public static void ApplyPenetrativeDamage(IMyCubeGrid cubeGrid, Vector3D startCoords, Vector3D endCoords, float damageAmount, long damageOwner, double penetrateDistance){
			
			var newStartCoords = Vector3D.Normalize(startCoords - endCoords) * 50 + endCoords;
			var newEndCoords = Vector3D.Normalize(endCoords - startCoords) * penetrateDistance + endCoords;
			var cellList = new List<Vector3I>();
			cubeGrid.RayCastCells(newStartCoords, newEndCoords, cellList);
			
			var blockList = new List<IMySlimBlock>();
			
			foreach(var cell in cellList){
				
				var block = cubeGrid.GetCubeBlock(cell);
				
				if(block == null){
					
					continue;
					
				}
				
				var blockPosition = Vector3D.Zero;
				block.ComputeWorldCenter(out blockPosition);
				var thisBlockDist = Vector3D.Distance(blockPosition, endCoords);
				
				if(thisBlockDist < penetrateDistance){
					
					blockList.Add(block);
					
				}
				
			}
			
			foreach(var targetBlock in blockList){
				
				if(targetBlock == null){
					
					continue;
					
				}
				
				targetBlock.DoDamage(damageAmount, MyStringHash.GetOrCompute("Laser"), true, null, damageOwner);
				
			}
			
		}
		
		public static void ApplyRegularDamage(IMyCubeGrid cubeGrid, Vector3D startCoords, Vector3D endCoords, float damageAmount, long damageOwner){
			
			var newStartCoords = Vector3D.Normalize(startCoords - endCoords) * 50 + endCoords;
			var newEndCoords = Vector3D.Normalize(endCoords - startCoords) * 50 + endCoords;
			var cellList = new List<Vector3I>();
			cubeGrid.RayCastCells(newStartCoords, newEndCoords, cellList);
			
			IMySlimBlock closestBlock = null;
			double closestBlockDist = 0;
			
			foreach(var cell in cellList){
				
				var block = cubeGrid.GetCubeBlock(cell);
				
				if(block == null){
					
					continue;
					
				}
				
				var blockPosition = Vector3D.Zero;
				block.ComputeWorldCenter(out blockPosition);
				var thisBlockDist = Vector3D.Distance(blockPosition, endCoords);
				
				if(closestBlock == null){
					
					closestBlock = block;
					closestBlockDist = thisBlockDist;
					
				}
				
				if(thisBlockDist < closestBlockDist){
					
					closestBlock = block;
					closestBlockDist = thisBlockDist;
					
				}
				
			}
			
			if(closestBlock == null){
				
				return;
				
			}
			
			closestBlock.DoDamage(damageAmount, MyStringHash.GetOrCompute("Laser"), true, null, damageOwner);
			
		}
		
		public static void ApplyPhysicsPush(IMyCubeGrid cubeGrid, Vector3D hitPosition, Vector3 forceVector){
			
			if(cubeGrid.Physics == null){
				
				return;
				
			}
			
			if(cubeGrid.IsStatic == true){
				
				return;
				
			}
			
			cubeGrid.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, forceVector, hitPosition, null);
			
		}
		
		public static void ApplySpeedReduction(IMyCubeGrid cubeGrid, float impulseApplied, float minimumSpeed){
			
			if(cubeGrid.Physics == null){
				
				return;
				
			}
			
			if(cubeGrid.IsStatic == true){
				
				return;
				
			}
			
			if(cubeGrid.Physics.LinearVelocity.Length() < minimumSpeed){
				
				return;
				
			}
			
			var gridSpeedDirection = Vector3.Normalize(cubeGrid.Physics.LinearVelocity);
			var impulseAppliedVector = -gridSpeedDirection * impulseApplied;
			cubeGrid.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, impulseAppliedVector, cubeGrid.Physics.CenterOfMassWorld, null);
			var newVelocityDirection = Vector3.Normalize(cubeGrid.Physics.LinearVelocity);

			if(Vector3.Distance(gridSpeedDirection * 10, newVelocityDirection * 10) > 10){
				
				cubeGrid.Physics.LinearVelocity = minimumSpeed * gridSpeedDirection;
				
			}

		}
		
		public static void CreatePhantomExplosion(Vector3D position, float radius, float damage, long blockEntityId = 0, bool showParticles = false, bool damageVoxels = false, bool createForceImpulse = true){
			
			MyExplosionTypeEnum explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_50;
			
			if (radius < 2f){
				
				explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_02;
				
			}else if (radius < 15f){
				
				explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_15;
				
			}else if (radius < 30f){
				
				explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_30;
				
			}
			
			MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
			myExplosionInfo.PlayerDamage = 0f;
			myExplosionInfo.OriginEntity = blockEntityId;
			myExplosionInfo.Damage = damage;
			myExplosionInfo.ExplosionType = explosionType;
			myExplosionInfo.ExplosionSphere = new BoundingSphereD(position, radius);
			myExplosionInfo.LifespanMiliseconds = 700;
			myExplosionInfo.ParticleScale = 1f;
			myExplosionInfo.Direction = Vector3.Down;
			myExplosionInfo.VoxelExplosionCenter = position;
			myExplosionInfo.ExplosionFlags = (MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION);
			myExplosionInfo.VoxelCutoutScale = 1f;
			myExplosionInfo.PlaySound = true;
			myExplosionInfo.ApplyForceAndDamage = true;
			
			
			
			myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
			myExplosionInfo.CreateParticleEffect = showParticles;
			myExplosionInfo.AffectVoxels = damageVoxels;
			MyExplosionInfo explosionInfo = myExplosionInfo;
			MyExplosions.AddExplosion(ref explosionInfo);
			
		}
		
		public static void CutVoxels(Vector3D position, float radius){
			
			try{
				
				var voxelShape = MyAPIGateway.Session.VoxelMaps.GetSphereVoxelHand();
				voxelShape.Center = position;
				voxelShape.Radius = radius;
				voxelShape.Transform = MatrixD.CreateWorld(position, Vector3D.Forward, Vector3D.Up);
				var sphere = new BoundingSphereD(position, 500);
				var mapList = new List<IMyVoxelBase>();
				MyAPIGateway.Session.VoxelMaps.GetInstances(mapList);
				
				//Debug:
				//var gps = MyAPIGateway.Session.GPS.Create("Hit", "", position, true);
				//MyAPIGateway.Session.GPS.AddLocalGps(gps);
				
				for(int i = mapList.Count - 1; i >= 0; i--){
					
					if(mapList[i].PositionComp.WorldAABB.Intersects(sphere) == false){
						
						mapList.RemoveAt(i);
						continue;
						
					}
					
					MyAPIGateway.Session.VoxelMaps.CutOutShape(mapList[i], voxelShape);
					
				}
				
			}catch(Exception exc){
				
				MyVisualScriptLogicProvider.ShowNotificationToAll("Cut Voxel Fail", 1000);
				
			}
			
		}
		
		public static void DamageHandler(object target, ref MyDamageInformation info){
			
			if(info.Type.ToString() == "ShieldBusterCython" && info.Amount > 0){
				
				info.Amount = 0;
				
			}
			
			/*
			if(TeslaProjectileWeapons.ContainsKey(info.AttackerId) == true){
				
				var weaponInfo = TeslaProjectileWeapons[info.AttackerId];
				//TODO: Tesla Stuff
				var slimBlock = target as IMySlimBlock;
				
				if(slimBlock != null){
					
					var targetGrid = slimBlock.CubeGrid;
					int blocksToAffect = 0;
					
					if(weaponInfo.TeslaOnlyAffectBlockAtHitLocation == false){
						
						slimBlock = null;
						
						if(weaponInfo.TeslaMaxBlocksAffected > 1){
							
							blocksToAffect = Rnd.Next(1, weaponInfo.TeslaMaxBlocksAffected + 1);
							
						}else if(weaponInfo.TeslaMaxBlocksAffected < 0){
							
							blocksToAffect = 0;
							
						}else{
							
							blocksToAffect = weaponInfo.TeslaMaxBlocksAffected;
							
						}
						
					}
					
					DisableGridRandomBlocks(targetGrid, blocksToAffect, slimBlock);
					
				}
				
			}
			*/
			
		}
		
		public static void DamageShielding(IMyCubeGrid cubeGrid, float percentageAffected, IMyTerminalBlock dsShield, Vector3D hitPos, long weaponBlockId){
			
			RegisterDamageHandler();
			
			if(ShieldBlockSubtypeNames.Count == 0){
				
				//Cython's Energy Shields
				ShieldBlockSubtypeNames.Add("LargeShipSmallShieldGeneratorBase");
				ShieldBlockSubtypeNames.Add("LargeShipLargeShieldGeneratorBase");
				ShieldBlockSubtypeNames.Add("SmallShipSmallShieldGeneratorBase");
				ShieldBlockSubtypeNames.Add("SmallShipMicroShieldGeneratorBase");
				
				var corruptFaction = MyAPIGateway.Session.Factions.TryGetFactionByTag("CORRUPT");
				
				if(corruptFaction != null){
					
					CorruptFounderId = corruptFaction.FounderId;
					
				}
				
			}
			
			//DarkStar
			if(dsShield != null && ShieldApiLoaded){
				
				var maxShieldHP = ShieldAPI.GetMaxHpCap(dsShield);
				
				if(maxShieldHP > 0 && ShieldAPI.IsShieldUp(dsShield) == true){
					
					bool sheildBreaks = false;
					
					if(ShieldAPI.GetShieldPercent(dsShield) <= percentageAffected || percentageAffected == 100){
						
						sheildBreaks = true;
						ShieldAPI.OverLoadShield(dsShield);
						
					}else{
						
						var amountToDamage = (maxShieldHP * 10000) * (percentageAffected / 100);
						ShieldAPI.PointAttackShield(dsShield, hitPos, weaponBlockId, amountToDamage, false, true, false);
						
					}
					
					
					//MyVisualScriptLogicProvider.ShowNotificationToAll("Shield Damage: " + amountToDamage.ToString() + " / " + maxShieldHP.ToString() + " - " + api.GetShieldPercent(dsShield).ToString() + " - " + api.HpToChargeRatio(dsShield).ToString(), 3000);
					
					if(sheildBreaks == false){
						
						return;
						
					}
					
				}

			}
			
			if(cubeGrid == null){
				
				return;
				
			}
			
			//Cython
			try{
				
				//TODO: Use Proper Interface
				var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(cubeGrid);
				var blockList = new List<IMyRefinery>();
				gts.GetBlocksOfType<IMyRefinery>(blockList);
				
				float totalCurrentShield = 0;
				float totalMaxShield = 0;
				IMySlimBlock shieldBlock = null;
				
				foreach(var block in blockList){
					
					if(block.IsFunctional == false || block.IsWorking == false){
						
						continue;
						
					}
					
					if(ShieldBlockSubtypeNames.Contains(block.SlimBlock.BlockDefinition.Id.SubtypeName) == false){
						
						continue;
						
					}
					
					var thisCurrentShield = GetShieldPointsFromEnergyShield(block, 0);
					var thisMaxShield = GetShieldPointsFromEnergyShield(block, 1);
					var amountToDamage = thisMaxShield * (percentageAffected / 100);
					
					if(amountToDamage > thisCurrentShield){
						
						amountToDamage = thisCurrentShield;
						
					}
					
					block.SlimBlock.DoDamage(amountToDamage, MyStringHash.GetOrCompute("ShieldBusterCython"), true, null, weaponBlockId);

				}
				
			}catch(Exception exc){
				
				
				
			}
			
			//Corruption
			
		}
		
		public static void DetachTargetBlock(IMyCubeGrid cubeGrid, Vector3D startCoords, Vector3D endCoords){
			
			var newStartCoords = Vector3D.Normalize(startCoords - endCoords) * 50 + endCoords;
			var newEndCoords = Vector3D.Normalize(endCoords - startCoords) * 50 + endCoords;
			var cellList = new List<Vector3I>();
			cubeGrid.RayCastCells(newStartCoords, newEndCoords, cellList);
			
			IMySlimBlock closestBlock = null;
			double closestBlockDist = 0;
			
			foreach(var cell in cellList){
				
				var block = cubeGrid.GetCubeBlock(cell);
				
				if(block == null){
					
					continue;
					
				}
				
				var blockPosition = Vector3D.Zero;
				block.ComputeWorldCenter(out blockPosition);
				var thisBlockDist = Vector3D.Distance(blockPosition, endCoords);
				
				if(closestBlock == null){
					
					closestBlock = block;
					closestBlockDist = thisBlockDist;
					
				}
				
				if(thisBlockDist < closestBlockDist){
					
					closestBlock = block;
					closestBlockDist = thisBlockDist;
					
				}
				
			}
			
			if(closestBlock == null){
				
				return;
				
			}
			
			var blockList = new List<IMySlimBlock>();
			blockList.Add(closestBlock);
			var result = cubeGrid.Split(blockList, true);
			
		}
		
		public static void DisableGridRandomBlocks(IMyCubeGrid cubeGrid, int blocksTotal, IMySlimBlock specificBlock = null){
			
			if(blocksTotal <= 0){
				
				return;
				
			}
			
			if(specificBlock != null){
				
				if(specificBlock.FatBlock == null){
					
					return;
					
				}
				
				var funcBlock = specificBlock.FatBlock as IMyFunctionalBlock;
				
				if(funcBlock == null){
					
					return;
					
				}
				
				funcBlock.Enabled = false;
				return;
				
			}
			
			try{
				
				var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(cubeGrid);
				var blockList = new List<IMyFunctionalBlock>();
				gts.GetBlocksOfType<IMyFunctionalBlock>(blockList);
				
				var totalBlocksAffected = 0;
				
				while(totalBlocksAffected < blocksTotal){
					
					if(blockList.Count == 0){
						
						break;
						
					}
					
					var randIndex = Utilities.Rnd.Next(0, blockList.Count);
					
					if(blockList[randIndex].IsFunctional == false || blockList[randIndex].IsWorking == false){
						
						blockList.RemoveAt(randIndex);
						continue;
						
					}
					
					blockList[randIndex].Enabled = false;
					totalBlocksAffected++;
					blockList.RemoveAt(randIndex);

				}
				
			}catch(Exception exc){
				
				
				
			}
			
		}
		
		public static float GetExplosiveDamageForDefenseShield(float damage, float range){
			
			return damage * range;
			
		}
		
		public static float GetShieldPointsFromEnergyShield(IMyRefinery block, int toggleIndex = 0){
			
			var splitA = block.CustomInfo.Split('\n');
			string shipPoints = "";
			
			foreach(var item in splitA){
			
				if(item.StartsWith("Local Shield: ")){
					
					var splitB = item.Split('/');
					
					if(splitB.Length == 2){
						
						shipPoints = splitB[toggleIndex];
						shipPoints = shipPoints.Replace("Local Shield: ", "");
						
					}
					
					break;
				
				}
			
			}
			
			float actualShipPoints = 0;
			var splitC = shipPoints.Split(' ');
			
			if(splitC.Length == 2){
				
				float.TryParse(splitC[0], out actualShipPoints);
				
				if(splitC[1].ToUpper() == "KPT"){
					
					actualShipPoints *= 1000;
					
				}
				
				if(splitC[1].ToUpper() == "MPT"){
					
					actualShipPoints *= 1000000;
					
				}
				
				if(splitC[1].ToUpper() == "GPT"){
					
					actualShipPoints *= 1000000000;
					
				}
				
			}
			
			return actualShipPoints;

		}
		
		public static void HackTargetBlocks(IMyCubeGrid cubeGrid, int numberOfBlocks, long newOwner){
			
			try{
				
				var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(cubeGrid);
				var blockList = new List<IMyTerminalBlock>();
				gts.GetBlocksOfType<IMyTerminalBlock>(blockList);
				int blocksConverted = 0;
				
				while(blockList.Count > 0){
					
					var randIndex = Rnd.Next(0, blockList.Count);
					var block = blockList[randIndex];
					blockList.RemoveAt(randIndex);
					
					if(block.OwnerId != 0 && block.OwnerId != newOwner){
						
						var blockEntity = block as IMyEntity;
						var cubeBlock = blockEntity as MyCubeBlock;
						cubeBlock.ChangeBlockOwnerRequest(newOwner, MyOwnershipShareModeEnum.Faction);
						blocksConverted++;
						
					}
					
					if(blocksConverted >= numberOfBlocks){
						
						break;
						
					}
					
				}
				
			}catch(Exception exc){
				
				
				
			}
			
		}
		
		public static void InhibitJumpDrives(IMyCubeGrid cubeGrid, float amountToInhibit, bool splitEvenlyAcrossDrives = true){
			
			try{
				
				var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(cubeGrid);
				var blockList = new List<IMyJumpDrive>();
				gts.GetBlocksOfType<IMyJumpDrive>(blockList);
				
				if(blockList.Count == 0){
					
					return;
					
				}
				
				for(int i = blockList.Count - 1; i >= 0; i--){
					
					if(blockList[i].IsWorking == false || blockList[i].IsFunctional == false){
						
						blockList.RemoveAt(i);
						continue;
						
					}
					
					if(blockList[i].CurrentStoredPower == 0){
						
						blockList.RemoveAt(i);
						continue;
						
					}
					
				}
				
				if(blockList.Count == 0){
					
					return;
					
				}
				
				float inhibitReducer = amountToInhibit;
				
				if(splitEvenlyAcrossDrives == true){
					
					inhibitReducer = amountToInhibit / (float)blockList.Count;
					
				}
				
				foreach(var block in blockList){
					
					//Test if needs sync...
					block.CurrentStoredPower -= inhibitReducer;
					
					if(block.CurrentStoredPower < 0){
						
						block.CurrentStoredPower = 0;
						
					}
					
				}
				
			}catch(Exception exc){
				
				
				
			}
			
		}
		
		public static void PaintVoxels(Vector3D position, float radius, string subtypeName){
			
			try{
				
				var voxelMaterialDef = MyDefinitionManager.Static.GetVoxelMaterialDefinition(subtypeName);
				
				if(voxelMaterialDef == null){
					
					return;
					
				}
				
				var voxelShape = MyAPIGateway.Session.VoxelMaps.GetSphereVoxelHand();
				voxelShape.Center = position;
				voxelShape.Radius = radius;
				voxelShape.Transform = MatrixD.CreateWorld(position, Vector3D.Forward, Vector3D.Up);
				var sphere = new BoundingSphereD(position, 500);
				var mapList = new List<IMyVoxelBase>();
				MyAPIGateway.Session.VoxelMaps.GetInstances(mapList);
				
				//Debug:
				//var gps = MyAPIGateway.Session.GPS.Create("Hit", "", position, true);
				//MyAPIGateway.Session.GPS.AddLocalGps(gps);
				
				for(int i = mapList.Count - 1; i >= 0; i--){
					
					if(mapList[i].PositionComp.WorldAABB.Intersects(sphere) == false){
						
						mapList.RemoveAt(i);
						continue;
						
					}
					
					MyAPIGateway.Session.VoxelMaps.PaintInShape(mapList[i], voxelShape, voxelMaterialDef.Index);
					
				}
				
			}catch(Exception exc){
				
				//MyVisualScriptLogicProvider.ShowNotificationToAll("Cut Voxel Fail", 1000);
				
			}
			
		}
		
		public static void RegisterDamageHandler(){
			
			if(DamageHandlerRegistered == false){
				
				DamageHandlerRegistered = true;
				MyAPIGateway.Session.DamageSystem.RegisterBeforeDamageHandler(5, DamageHandler);
				
			}
			
		}
		
		public static void RegisterProjectileWeapon(long entityId, WeaponConfig config){

			if(config.TeslaRegisterWeaponForProjectileUse == true){
				
				if(TeslaProjectileWeapons.ContainsKey(entityId) == false){
					
					TeslaProjectileWeapons.Add(entityId, config);
					
				}
				
			}
			
		}
		
		public static void RepaintTargetBlockstargetGrid(IMyCubeGrid cubeGrid, Vector3D startCoords, Vector3D endCoords, Vector3 blockColor, bool randomColor){
			
			var newStartCoords = Vector3D.Normalize(startCoords - endCoords) * 50 + endCoords;
			var newEndCoords = Vector3D.Normalize(endCoords - startCoords) * 50 + endCoords;
			var cellList = new List<Vector3I>();
			cubeGrid.RayCastCells(newStartCoords, newEndCoords, cellList);
			
			IMySlimBlock closestBlock = null;
			double closestBlockDist = 0;
			
			foreach(var cell in cellList){
				
				var block = cubeGrid.GetCubeBlock(cell);
				
				if(block == null){
					
					continue;
					
				}
				
				var blockPosition = Vector3D.Zero;
				block.ComputeWorldCenter(out blockPosition);
				var thisBlockDist = Vector3D.Distance(blockPosition, endCoords);
				
				if(closestBlock == null){
					
					closestBlock = block;
					closestBlockDist = thisBlockDist;
					
				}
				
				if(thisBlockDist < closestBlockDist){
					
					closestBlock = block;
					closestBlockDist = thisBlockDist;
					
				}
				
			}
			
			if(closestBlock == null){
				
				return;
				
			}
			
			var newColor = blockColor;
			
			if(randomColor == true){
				
				var randHue = (float)Rnd.Next(0, 360) / 360f;
				var randSat = (float)Rnd.Next(-200, 200) / 100f;
				var randVal = (float)Rnd.Next(-250, 550) / 100f;
				newColor = new Vector3(randHue, randSat, randVal);
				
			}
			
			cubeGrid.ColorBlocks(closestBlock.Min + new Vector3I(-1,-1,-1), closestBlock.Min + new Vector3I(1,1,1), newColor);
			
		}
		
		
	}
	
}