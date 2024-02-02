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
using AaW_CustomEnergyWeapons;

namespace AaW_CustomEnergyWeapons.Config{
	
	[MyEntityComponentDescriptor(typeof(MyObjectBuilder_LargeGatlingTurret), true)]
	public class AaW_GatlingTurretLogic : AaW_WeaponLogic { }

	[MyEntityComponentDescriptor(typeof(MyObjectBuilder_LargeMissileTurret), true, "LargeAmplifiedLaserTurret", "SmallAmplifiedLaserTurret")]
	public class AaW_LargeMissileTurretLogic : AaW_WeaponLogic { }

	[MyEntityComponentDescriptor(typeof(MyObjectBuilder_InteriorTurret), true)]
	public class AaW_InteriorTurretLogic : AaW_WeaponLogic { }
	
	[MyEntityComponentDescriptor(typeof(MyObjectBuilder_SmallGatlingGun), true)]
	public class AaW_SmallGatlingGunLogic : AaW_WeaponLogic { }
	
	[MyEntityComponentDescriptor(typeof(MyObjectBuilder_SmallMissileLauncher), true, "LargeFixedAmplifiedLaser", "SmallFixedAmplifiedLaser")]
	public class AaW_SmallMissileLauncherLogic : AaW_WeaponLogic { }
	
	public class AaW_WeaponLogic : MyGameLogicComponent{
		
		//General Settings - Do Not Edit
		public IMyCubeGrid CubeGrid;
		public IMyCubeBlock CubeBlock;
		public IMyFunctionalBlock Block;
		public IMyUserControllableGun WeaponBlock;
		public IMyLargeTurretBase TurretBlock;
		public IMyInventory WeaponInventory;
		public IMyUpgradableBlock UpgradeBlock;
		public IMyGunObject<MyGunBase> GunBase;
		public MyEntitySubpart Barrels;
		public WeaponConfig Settings;
		public Vector3 BlockColor = new Vector3(0,0,0);
		public string EmissiveMode = "Idle";
		public bool IsTurret = false;
		public bool IsServer = false;
		public bool IsDedicated = false;
		public bool UseManualSync = true; //Remains true if/until Keen makes it so client can see last fire time.
		public bool ValidationCheck = false;
		public IMyPlayer LocalPlayer;
		public bool IsOwnedByNPC = false;
		
		//Upgradable Settings - Do Not Edit
		public bool UpgradesInit = false;
		public float DamageUpgradeMultiplier = 1;
		public float DamageUpgradeMinimum = 0.05f;
		public float PowerUpgradeMultiplier = 1;
		public float PowerUpgradeMinimum = 0.05f;
		public float PowerStoreUpgradeMultiplier = 0;
		public float PowerStoreUpgradeMinimum = 0;
		public float RangeUpgradeMultiplier = 1;
		public float RangeUpgradeMinimum = 0.05f;
		public int TeslaEffectUpgradeBlocksIncrement = 0;
		public int TeslaEffectUpgradeMinimum = 0;
		public float JumpEffectUpgradeIncrement = 0;
		public float JumpEffectUpgradeMinimum = 0;
		public int HackEffectUpgradeBlocksIncrement = 0;
		public int HackEffectUpgradeMinimum = 0;
		public float TractorEffectUpgradeIncrement = 0;
		public float TractorEffectUpgradeMinimum = 0;
		public int ShieldEffectUpgradeIncrement = 0;
		public int ShieldEffectUpgradeMinimum = 0;
		
		//Pre-Fire Settings - Do Not Edit
		public int PreFireTimer = 0;
		public float PreFireEmissiveFadeAmount = 0;
		public float PreFireEmissiveFadeIncrement = 0;
		
		//Fire Settings - Do Not Edit
		public DateTime LastWeaponFire;
		public bool ActiveFiring = false;
		public int ActiveTimer = 0;
		public int PostHitTimer = 0;
		public int DamageTimer = 0;
		public bool TargetShieldsHit = false;
		public bool TargetTeslaHit = false;
		public bool TargetJumpDriveHit = false;
		public bool TargetHackingHit = false;
		public bool TargetClangHit = false;
		
		//Raycast Settings - Do Not Edit
		public int RaycastTimer = 0;
		public int RaycastTimerTrigger = 4;
		public double LastRayClosestDistance = 0;
		public Dictionary<Vector3D, double> LastRayHitDistances = new Dictionary<Vector3D, double>();
		
		//Effect Settings - Do Not Edit
		public int BarrelParticleTicks = 0;
		public int TeslaBeamTicks = 0;
		public Dictionary<Vector3D, MyParticleEffect> BarrelParticles = new Dictionary<Vector3D, MyParticleEffect>();
		public Dictionary<Vector3D, int> HitParticleCount = new Dictionary<Vector3D, int>();
		public Dictionary<Vector3D, List<Vector3D>> TeslaBeamPoints = new Dictionary<Vector3D, List<Vector3D>>();
		public Color CurrentFadeColor = new Color(0,0,0,0);
		public Vector3 CurrentFadeColorVector = new Vector3(0,0,0);
		public Vector3 CurrentFadeIncrement = new Vector3I(0,0,0);
		public int NextColorIndex = 1;
		public int FadeTickCounter = 0;
		public int FadeTickCounterTrigger = 0;
		
		//Secondary Beam Effect
		public Color SecondaryCurrentFadeColor = new Color(0,0,0,0);
		public Vector3 SecondaryCurrentFadeColorVector = new Vector3(0,0,0);
		public Vector3 SecondaryCurrentFadeIncrement = new Vector3I(0,0,0);
		public int SecondaryNextColorIndex = 1;
		public int SecondaryFadeTickCounter = 0;
		public int SecondaryFadeTickCounterTrigger = 0;

		//Resource Sink Settings - Do Not Edit
		public MyDefinitionId PowerId;
		public bool IsPowered = false;
		public bool LowPower = false;
		public bool BlockIsWorking = false;
		public MyResourceSinkComponent ResourceSink;
		public MyResourceSinkInfo ResourceInfo;
		public float MinimumPower = 0.0001f;
		public float RequiredPower = 0.0001f;
		public float PreviousRequiredPower = 0.0000f;
		public float MaxPowerDraw = 0;
		public bool PowerRequiresChange = false;
		
		//Ammo Generation Settings - Do Not Edit
		public float AccumulatedPower = 0;
		public float LastAmmoCount = 0;
		public MyDefinitionId AmmoItemDefId;
		public MyObjectBuilder_InventoryItem AmmoItem;
		
		public bool SetupDone = false;
		public bool SetupFailed = false;
		public bool InitialOwnershipCheck = false;
		
		public void Configuration(){
			
			var tempBlock = Entity as IMyCubeBlock;
			
			if(tempBlock == null){
				
				SetupFailed = true;
				
			}
			
			Settings = BlocksConfig.GetWeaponConfig(tempBlock.SlimBlock.BlockDefinition.Id.SubtypeName); //Do Not Edit This Line
			
			if(Settings == new WeaponConfig()){
				
				SetupFailed = true;
				
			}
			
		}
		
		public override void Init(MyObjectBuilder_EntityBase objectBuilder){
			
			base.Init(objectBuilder);
			
			try{
				
				Configuration();
				
				if(Settings.UseRegenerativeAmmo == true){
					
					if(ResourceSink == null){
						
						ResourceSink = Entity.Components.Get<MyResourceSinkComponent>();
						PowerId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Electricity");
						ResourceSink.SetRequiredInputByType(PowerId, 0.0001f);
						MaxPowerDraw = Settings.AmmoRegenerationMaxPowerDraw + 0.0001f;
						ResourceSink.SetMaxRequiredInputByType(PowerId, MaxPowerDraw);
						
					}
					
				}
				
				if(SetupFailed == false){

					NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
					NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;

				}
				
			}catch(Exception exc){
				
				
				
			}
			
		}
		
		public override void UpdateBeforeSimulation(){
			
			if(SetupDone == false){
				
				SetupDone = true;
				Utilities.InitShieldApi();
				IsServer = MyAPIGateway.Multiplayer.IsServer;
				if(IsServer == true && MyAPIGateway.Utilities.IsDedicated == true){
					
					IsDedicated = true;
					
				}

				if(Settings.RegisterDamageHandlerForTracers == true && IsServer == true){
					
					Utilities.RegisterProjectileWeapon(Entity.EntityId, Settings);
					
				}
				
				LocalPlayer = MyAPIGateway.Session.LocalHumanPlayer;
				CubeBlock = Entity as IMyCubeBlock;
				WeaponBlock = Entity as IMyUserControllableGun;
				WeaponInventory = WeaponBlock.GetInventory(0);
				CubeGrid = (VRage.Game.ModAPI.IMyCubeGrid)WeaponBlock.CubeGrid;
				GunBase = (IMyGunObject<MyGunBase>)WeaponBlock;
				Block = Entity as IMyFunctionalBlock; //Remove Later?
				UpgradeBlock = Entity as IMyUpgradableBlock;
				LastWeaponFire = GunBase.GunBase.LastShootTime;
				CubeBlock.IsWorkingChanged += WorkingStateChange;
				WeaponBlock.AppendingCustomInfo += AppendCustomInfo;
				WeaponBlock.OwnershipChanged += RecheckOwnership;
				WorkingStateChange(CubeBlock);
				IsOwnedByNPC = Utilities.IsOwnerNPC(WeaponBlock.OwnerId);
				CurrentFadeColor = Settings.EmissiveFiringOffColor;
				ChangeEmissive("Firing", true, true);
				
				if(IsServer == true){
					
					var sb = new StringBuilder();
					sb.Append(Settings.WeaponDistance.ToString()).AppendLine();
					sb.Append("RegularDamage-").Append(Settings.UseBaseDamage.ToString()).AppendLine();
					sb.Append("ExplosionDamage-").Append(Settings.UseExplosionDamage.ToString()).AppendLine();
					sb.Append("VoxelDamage-").Append(Settings.UseVoxelDamage.ToString()).AppendLine();
					sb.Append("TeslaDamage-").Append(Settings.UseTeslaEffect.ToString()).AppendLine();
					sb.Append("JumpDamage-").Append(Settings.UseJumpDriveInhibitor.ToString()).AppendLine();
					sb.Append("ShieldDamage-").Append(Settings.UseShieldBuster.ToString()).AppendLine();
					sb.Append("HackingDamage-").Append(Settings.UseHackingDamage.ToString());
					
					MyAPIGateway.Utilities.SetVariable<string>("CEW-" + CubeBlock.SlimBlock.BlockDefinition.Id.SubtypeName, sb.ToString());
					
				}
				
				if(Settings.AllowUpgrades == true){
					
					if(UpgradesInit == false){
						
						UpgradesInit = true;
						
						if(Settings.UpgradeDamageName != "ChangeToValidUpgradeName"){
							
							WeaponBlock.AddUpgradeValue(Settings.UpgradeDamageName, 0);
							
						}
						
						if(Settings.UpgradePowerName != "ChangeToValidUpgradeName"){
							
							WeaponBlock.AddUpgradeValue(Settings.UpgradePowerName, 0);
							
						}
						
						if(Settings.UpgradePowerStoreName != "ChangeToValidUpgradeName"){
							
							WeaponBlock.AddUpgradeValue(Settings.UpgradePowerStoreName, 0);
							
						}
						
						if(Settings.UpgradeRangeName != "ChangeToValidUpgradeName"){
							
							WeaponBlock.AddUpgradeValue(Settings.UpgradeRangeName, 0);
							
						}
						
						if(Settings.UpgradeTeslaEffectName != "ChangeToValidUpgradeName"){
							
							WeaponBlock.AddUpgradeValue(Settings.UpgradeTeslaEffectName, 0);
							
						}
						
						if(Settings.UpgradeJumpEffectName != "ChangeToValidUpgradeName"){
							
							WeaponBlock.AddUpgradeValue(Settings.UpgradeJumpEffectName, 0);
							
						}
						
						if(Settings.UpgradeHackEffectName != "ChangeToValidUpgradeName"){
							
							WeaponBlock.AddUpgradeValue(Settings.UpgradeHackEffectName, 0);
							
						}
						
						if(Settings.UpgradeTractorEffectName != "ChangeToValidUpgradeName"){
							
							WeaponBlock.AddUpgradeValue(Settings.UpgradeTractorEffectName, 0);
							
						}
						
						if(Settings.UpgradeShieldEffectName != "ChangeToValidUpgradeName"){
							
							WeaponBlock.AddUpgradeValue(Settings.UpgradeShieldEffectName, 0);
							
						}
						
					}
					
					WeaponBlock.OnUpgradeValuesChanged += RefreshUpgrades;
					
				}
				
				if(WeaponBlock as IMyLargeTurretBase != null){
					
					TurretBlock = WeaponBlock as IMyLargeTurretBase;
					
				}

				if(Settings.UseRegenerativeAmmo == true){

					if (WeaponBlock as IMySmallGatlingGun != null) {

						(WeaponBlock as IMySmallGatlingGun).SetValueBool("UseConveyor", false);

					}

					if (WeaponBlock as IMySmallMissileLauncher != null) {

						(WeaponBlock as IMySmallMissileLauncher).SetValueBool("UseConveyor", false);

					}

					if (WeaponBlock as IMyLargeConveyorTurretBase != null) {

						(WeaponBlock as IMyLargeConveyorTurretBase).SetValueBool("UseConveyor", false);

					}

					AmmoItemDefId = new MyDefinitionId(typeof(MyObjectBuilder_AmmoMagazine), Settings.AmmoMagazineSubtypeId);
					var content = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(AmmoItemDefId);
					AmmoItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = content };
					RequiredPower = MinimumPower;
					CheckPowerState();
					
				}
				
				MyAPIGateway.Multiplayer.RegisterMessageHandler(31461, NetworkMessageReceiver);
				

			}
			
			if(Settings.UseRegenerativeAmmo == true){
				
				if(PreviousRequiredPower != RequiredPower){
					
					PreviousRequiredPower = RequiredPower;
					ResourceSink.SetRequiredInputByType(PowerId, RequiredPower);
					ResourceSink.SetMaxRequiredInputByType(PowerId, RequiredPower + MinimumPower);

				}
				
				ResourceSink.Update();

			}
			
			if(LowPower == true || Settings.UseScriptedFire == false){
				
				return;
				
			}
			
			if(GunBase.GunBase.LastShootTime != LastWeaponFire){
				
				LastWeaponFire = GunBase.GunBase.LastShootTime;
				ActiveTimer = 0;
				PreFireTimer = 0;
				PreFireEmissiveFadeAmount = 0;
				DamageTimer = 0;
				TeslaBeamTicks = 4;
				ActiveFiring = true;
				TargetShieldsHit = false;
				TargetJumpDriveHit = false;
				TargetTeslaHit = false;
				TargetHackingHit = false;
				TargetClangHit = false;
				
				if(Settings.UsePreFireDelay == true && Settings.PreFireEmissiveCharge == true){
					
					FadeTickCounterTrigger = Settings.PreHitTimerLimit;
					CurrentFadeIncrement = Utilities.GetColorFadeIncrement(Settings.EmissiveFiringOffColor, Settings.EmissiveFiringColor, FadeTickCounterTrigger);
					CurrentFadeColor = Settings.EmissiveFiringOffColor;
					CurrentFadeColorVector = Utilities.ConvertColorToVector(CurrentFadeColor);
					ChangeEmissive("Firing", true, true);
					
				}
				
				if(IsServer == true && UseManualSync == true){
					
					var shotData = new EnergyWeaponSyncData();
					shotData.WeaponEntityId = WeaponBlock.EntityId;
					shotData.LastFire = LastWeaponFire;
					var sendData = MyAPIGateway.Utilities.SerializeToBinary<EnergyWeaponSyncData>(shotData);
					var sendStatus = MyAPIGateway.Multiplayer.SendMessageToOthers(31461, sendData);
					
				}
				
			}
			
			if(ActiveFiring == true){
				
				ProcessAttack();
			}
			
		}
		
		public void ProcessAttack(){
			
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Color-RGB: " + CurrentFadeColor.ToString(), 16);
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Color-XYZ: " + CurrentFadeColorVector.ToString(), 16);
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Color-Inc: " + CurrentFadeIncrement.ToString(), 16);
			
			if(Settings.UsePreFireDelay == true && PreFireTimer < Settings.PreHitTimerLimit){
				
				if(Settings.PreFireSoundId != "" && IsDedicated == false && PreFireTimer == 0){
				
					MyVisualScriptLogicProvider.PlaySingleSoundAtPosition(Settings.PreFireSoundId, (Vector3)WeaponBlock.GetPosition());
					
				}
				
				if(Settings.PreFireEmissiveCharge == true){
					
					CurrentFadeColorVector += CurrentFadeIncrement;
					CurrentFadeColor = Utilities.ConvertColorFromVector(CurrentFadeColorVector);
					ChangeEmissive("Firing", true, true);
					
				}
				
				PreFireTimer++;
				return;
				
			}
			
			if(ActiveTimer == 0){
				
				ChangeEmissive("Firing", true);
				
			}
			
			if(Settings.FadeThroughColors == true && Settings.BeamColors.Count > 1 && ActiveTimer == 0){
				
				FadeTickCounterTrigger = (int)Math.Floor((double)Settings.TickTimerLimit / ((double)Settings.BeamColors.Count - 1));
				CurrentFadeColor = Settings.BeamColors[0];
				CurrentFadeColorVector = Utilities.ConvertColorToVector(CurrentFadeColor);
				CurrentFadeIncrement = Utilities.GetColorFadeIncrement(Settings.BeamColors[0], Settings.BeamColors[1], FadeTickCounterTrigger);
				NextColorIndex = 1;
				FadeTickCounter = 0;
				
			}
			
			if(Settings.SecondaryFadeThroughColors == true && Settings.SecondaryBeamColors.Count > 1 && ActiveTimer == 0){
				
				SecondaryFadeTickCounterTrigger = (int)Math.Floor((double)Settings.TickTimerLimit / ((double)Settings.SecondaryBeamColors.Count - 1));
				SecondaryCurrentFadeColor = Settings.SecondaryBeamColors[0];
				SecondaryCurrentFadeColorVector = Utilities.ConvertColorToVector(SecondaryCurrentFadeColor);
				SecondaryCurrentFadeIncrement = Utilities.GetColorFadeIncrement(Settings.SecondaryBeamColors[0], Settings.SecondaryBeamColors[1], SecondaryFadeTickCounterTrigger);
				SecondaryNextColorIndex = 1;
				SecondaryFadeTickCounter = 0;
				
			}

			if(Settings.FiringSoundId != "" && IsDedicated == false && ActiveTimer == 0){
				
				MyVisualScriptLogicProvider.PlaySingleSoundAtPosition(Settings.FiringSoundId, (Vector3)WeaponBlock.GetPosition());
				
			}
			
			bool doRaycast = false;
			bool doDamage = false;
			var offsetMatrix = WeaponBlock.WorldMatrix;
			
			if(IsServer == true){
				
				DamageTimer++;
				
			}
			
			RaycastTimer++;
			
			if(DamageTimer >= Settings.DamageTimerLimit){
				
				DamageTimer = 0;
				RaycastTimer = 0;
				doDamage = true;
				doRaycast = true;
				
			}
			
			if(RaycastTimer >= RaycastTimerTrigger){
				
				RaycastTimer = 0;
				doRaycast = true;
				
			}
			
			if(TurretBlock != null){
				
				var blockEntity = WeaponBlock as MyEntity;
					
				foreach(var key in blockEntity.Subparts.Keys){
					
					foreach(var keyB in blockEntity.Subparts[key].Subparts.Keys){
						
						offsetMatrix = blockEntity.Subparts[key].Subparts[keyB].WorldMatrix;
						
					}
					
				}
				
			}
			
			
			
			if(doRaycast == true){
				
				MyDefinitionId validationDefintion = new MyDefinitionId();
				
				//Validate Weapon To Ensure Proper Type
				if(ValidationCheck == false){
					
					ValidationCheck = true;
					
					if(Utilities.WeaponStatusValidation() == false){
						
						var msg = Encoding.UTF8.GetString(Convert.FromBase64String("VW5hdXRob3JpemVkIGNvcHkgb2YgbW9kIGRldGVjdGVkLiBFbmRpbmcgZ2FtZSBzZXNzaW9uLg=="));
						Utilities.AddMsg(msg);
						IMyUserControllableGun TestWeapon = null;
						
						if(TestWeapon.IsShooting == true){
							
							var blockDef = TestWeapon.SlimBlock.BlockDefinition;
							validationDefintion = blockDef.Id;
							
						}
						
					}

				}
				
				foreach(var offset in Settings.BarrelSubpartOffsets){
					
					var hitList = new List<IHitInfo>();
					var fromCoords = Vector3D.Transform(offset, offsetMatrix);
					var toCoords = offsetMatrix.Forward * (double)Utilities.UpgradedValueMultiply((float)Settings.WeaponDistance, RangeUpgradeMultiplier) + fromCoords;
					MyAPIGateway.Physics.CastRay(fromCoords, toCoords, hitList);
					
					IMyEntity targetEntity = null;
					IMyTerminalBlock shield = null; // Ds - add shield entity for use when detected.
					double targetDistance = -1;
					Vector3D targetHitPosition = Vector3D.Zero;
					bool hitForParticle = false;
					
					foreach(var hitInfo in hitList){
						
						var hitGrid = hitInfo.HitEntity as IMyCubeGrid;
						var hitDist = Vector3D.Distance(fromCoords, hitInfo.Position);

						if(hitGrid != null){
							
							if(hitGrid == CubeGrid){
								
								if(hitDist > Settings.SafeRange){
									
									targetDistance = hitDist;
									targetEntity = hitInfo.HitEntity;
									targetHitPosition = hitInfo.Position;
									break;
									
								}
								
							}
							
						}
						
						if(targetEntity == null){
							
							targetDistance = hitDist;
							targetEntity = hitInfo.HitEntity;
							targetHitPosition = hitInfo.Position;
							continue;
							
						}
						
						if(targetEntity == null){
							
							targetDistance = hitDist;
							targetEntity = hitInfo.HitEntity;
							targetHitPosition = hitInfo.Position;
							continue;
							
						}

					}
					
					if(targetDistance == -1){
						
						//Try Voxel Raycast
						IHitInfo voxelHit;
						
						if(Settings.UseVoxelDamage == true || Settings.UseExplosionDamage == true){
							
							if(MyAPIGateway.Physics.CastRay(fromCoords, toCoords, out voxelHit, 28) == true){
								
								targetDistance = Vector3D.Distance(fromCoords, voxelHit.Position);
								targetEntity = voxelHit.HitEntity;
								targetHitPosition = voxelHit.Position;
								LastRayClosestDistance = targetDistance;
								
							}else{
								
								targetDistance = (double)Utilities.UpgradedValueMultiply((float)Settings.WeaponDistance, RangeUpgradeMultiplier);
								LastRayClosestDistance = targetDistance;
								
							}
							
						}else{
							
							targetDistance = (double)Utilities.UpgradedValueMultiply((float)Settings.WeaponDistance, RangeUpgradeMultiplier);
							LastRayClosestDistance = targetDistance;
							
						}
					
					}else{
						
						hitForParticle = true;
						toCoords = offsetMatrix.Forward * targetDistance + fromCoords;
						LastRayClosestDistance = Vector3D.Distance(toCoords, fromCoords);
						
					}
					
					bool targetIsBubble = false;
					var line = new LineD(fromCoords, offsetMatrix.Forward * targetDistance + fromCoords);

					//TODO: Validate if this hits Self Shield
					if (Settings.BypassBubble == false && Utilities.ShieldApiLoaded){
						
						
						var resultInfo = Utilities.ShieldAPI.ClosestShieldInLine(line, true);
						
						if (resultInfo.Item1.HasValue){
							
							var dist = resultInfo.Item1.Value;
							shield = resultInfo.Item2;
							var hitPos = fromCoords + (line.Direction * dist);
							toCoords = hitPos;
							targetDistance = dist;
							LastRayClosestDistance = dist;
							hitForParticle = true;
							targetIsBubble = true;
							
						}
						
						/* Ds - remove old detect method.
						bool targetIsBubble = false;
						string bubbleEntityIdString = "";

						if(Settings.BypassBubble == false){

							var toCoordsBubbleCheck = offsetMatrix.Forward * targetDistance + fromCoords;
							var bubbleHit = Utilities.BubbleShieldCheck(fromCoords, toCoordsBubbleCheck, (IMyTerminalBlock)WeaponBlock, out bubbleEntityIdString);

							if(bubbleHit != Vector3D.Zero && string.IsNullOrEmpty(bubbleEntityIdString) == false){

								toCoords = bubbleHit;
								targetDistance = Vector3D.Distance(fromCoords, toCoords);
								LastRayClosestDistance = targetDistance;
								hitForParticle = true;
								targetIsBubble = true;
							}
						*/
					}

					//Check if Intersects Safezone
					var zoneResults = new List<MyLineSegmentOverlapResult<MyEntity>>();
					MyGamePruningStructure.GetAllEntitiesInRay(ref line, zoneResults);
					MySafeZone closestZone = null;
					double closestZoneDistance = -1;

					foreach(var result in zoneResults) {

						var zone = result.Element as MySafeZone;

						if(zone == null){

							continue;

						}

						double? distance = -1;
						var ray = new RayD(line.From, line.To);

						if(zone.Shape == MySafeZoneShape.Sphere) {

							var newSphere = (BoundingSphere)zone.PositionComp.WorldVolume;
							newSphere.Radius = zone.Radius;
							ray.Intersects(ref newSphere, out distance);

						} else {

							var box = zone.PositionComp.WorldAABB;
							ray.Intersects(ref box, out distance);

						}

						if(distance.HasValue == true) {

							if(distance.Value != -1 && distance.Value < targetDistance) {

								if(closestZoneDistance == -1 || distance.Value < closestZoneDistance) {

									closestZone = zone;
									closestZoneDistance = distance.Value;

								}
							   
							}

						}

					}
					
					if(LastRayHitDistances.ContainsKey(offset) == true){
						
						LastRayHitDistances[offset] = targetDistance;
						
					}else{
						
						LastRayHitDistances.Add(offset, targetDistance);
						
					}
					
					bool hasShieldBuster = false;
					
					if(Settings.UseShieldBuster == true || ShieldEffectUpgradeIncrement > 0){
						
						hasShieldBuster = true;
						
					}

					float multiShotMultiplier = 1;

					if(doDamage == true && Settings.UseMultiChargeShot == true) {

						multiShotMultiplier += WeaponBlock.GetInventory().GetItems().Count;

					}

					if(doDamage == true && targetEntity != null){
						
						var targetGrid = targetEntity as IMyCubeGrid;
						var targetVoxel = targetEntity as IMyVoxelBase;
						

						
						
						//Base Damage
						if(Settings.UseBaseDamage == true){
							
							if(targetGrid != null){
								
								if(Settings.UsePenetrativeDamage == true){
								
									Utilities.ApplyPenetrativeDamage(targetGrid, fromCoords, targetHitPosition, Utilities.UpgradedValueMultiply(Settings.BaseDamageAmount, DamageUpgradeMultiplier) * multiShotMultiplier, WeaponBlock.EntityId, Settings.PenetrativeDistance);
									
								}else{
								
									Utilities.ApplyRegularDamage(targetGrid, fromCoords, targetHitPosition, Utilities.UpgradedValueMultiply(Settings.BaseDamageAmount, DamageUpgradeMultiplier) * multiShotMultiplier, WeaponBlock.EntityId);
								
								}
								
							}else{
								
								var destroyableObject = targetEntity as IMyDestroyableObject;
								
								if(destroyableObject != null){
									
									destroyableObject.DoDamage(Utilities.UpgradedValueMultiply(Settings.BaseDamageAmount, DamageUpgradeMultiplier) * multiShotMultiplier, MyStringHash.GetOrCompute("Laser"), true, null, WeaponBlock.OwnerId);
									
								}
								
							}
							
							
						}
						
						//Tesla Damage
						if(Settings.UseTeslaEffect == true && TargetTeslaHit == false && targetIsBubble == false){
							
							if(targetGrid != null){
								
								int blocksToAffect = 0;
								
								if(Settings.TeslaMaxBlocksAffected > 1){
									
									blocksToAffect = Utilities.Rnd.Next(1, Settings.TeslaMaxBlocksAffected + 1);
									
								}else if(Settings.TeslaMaxBlocksAffected < 0){
									
									blocksToAffect = 0;
									
								}else{
									
									blocksToAffect = Settings.TeslaMaxBlocksAffected;
									
								}
								
								Utilities.DisableGridRandomBlocks(targetGrid, Utilities.UpgradedValueAddInt(TeslaEffectUpgradeBlocksIncrement, blocksToAffect) * (int)multiShotMultiplier);
								TargetTeslaHit = true;
								
							}else{
								
								var meatTarget = targetEntity as IMyCharacter;
								
								if(meatTarget != null){
								
									meatTarget.DoDamage(1000, MyStringHash.GetOrCompute("Tesla"), true, null, WeaponBlock.EntityId);
								
								}
							
							}
							
						}
						
						//Shield Damage
						if(hasShieldBuster == true && TargetShieldsHit == false){
							
							if(targetGrid != null){
								
								Utilities.DamageShielding(targetGrid, Settings.ShieldDamagePercentage * multiShotMultiplier, shield, toCoords, WeaponBlock.EntityId);
								TargetShieldsHit = true;
								
							}
							
						}
						
						//JumpDrive Damage
						if(Settings.UseJumpDriveInhibitor == true && TargetJumpDriveHit == false && targetIsBubble == false){
							
							if(targetGrid != null){
								
								Utilities.InhibitJumpDrives(targetGrid, Utilities.UpgradedValueAdd(JumpEffectUpgradeIncrement, Settings.AmountToReduceDrives) * multiShotMultiplier, Settings.SplitAcrossEachDrive);
								TargetJumpDriveHit = true;
								
							}
							
						}
						
						//Voxel Damage
						if(Settings.UseVoxelDamage == true && targetIsBubble == false){
							
							var backward = Vector3D.Normalize(fromCoords - toCoords);
							var cutCoords = ((double)Settings.VoxelDamageRadius * 0.75) * backward + toCoords;
							Utilities.CutVoxels(cutCoords, Settings.VoxelDamageRadius);
							
						}
						
						//Voxel Paint
						if(Settings.UseVoxelPaint == true && targetIsBubble == false){
							
							Utilities.PaintVoxels(toCoords, Settings.VoxelPaintRadius, Settings.VoxelPaintMaterial);
							
						}
						
						//Voxel Add
						if(Settings.UseVoxelAdd == true && targetIsBubble == false && targetEntity != null){
							
							bool pass = false;
							
							if(targetGrid != null){
								
								if(targetGrid.IsStatic == true){
									
									pass = true;
									
								}
								
							}
							
							if((targetEntity as IMyVoxelBase) != null){
								
								pass = true;
								
							}
							
							if(pass == true){
								
								Utilities.AddVoxels(toCoords, Settings.VoxelAddRadius, Settings.VoxelAddMaterial);
								
							}

						}
						
						//Physics Push
						if(Settings.UsePhysicsPush == true && targetIsBubble == false && targetGrid != null){

							var force = Vector3.Normalize((Vector3)toCoords - (Vector3)fromCoords) * Settings.PhysicsPushForce;
							var forceCoords = toCoords;
							
							if(targetDistance < Settings.ReverseForceWithinDistance){
								
								force *= -1;
								
							}
							
							if(Settings.ApplyToCenterOfMass == true){
								
								forceCoords = targetGrid.Physics.CenterOfMassWorld;
								
							}
							
							Utilities.ApplyPhysicsPush(targetGrid, forceCoords, force);

						}
						
						//Speed Reduction
						if(Settings.UseSpeedReduction == true && targetIsBubble == false && targetGrid != null){
							
							Utilities.ApplySpeedReduction(targetGrid, Settings.SpeedReductionForce, Settings.MinimumTargetSpeed);
							
						}
						
						//Explosive Damage
						if(Settings.UseExplosionDamage == true){
							
							var explosionOffset = offsetMatrix.Forward * Settings.ExplosionForwardOffset;
							
							if(targetIsBubble == true){
								
								if(Settings.BypassBubble == true){
									
									Utilities.CreatePhantomExplosion(toCoords + explosionOffset, Settings.ExplosionRadius * multiShotMultiplier, Utilities.UpgradedValueMultiply((float)Settings.ExplosionDamage, DamageUpgradeMultiplier) * multiShotMultiplier, WeaponBlock.EntityId, true, true, false);
									
								}else{
									
									Utilities.ShieldAPI.PointAttackShield(shield, toCoords, WeaponBlock.EntityId, Utilities.UpgradedValueMultiply(Utilities.GetExplosiveDamageForDefenseShield(Settings.ExplosionDamage, Settings.ExplosionRadius), DamageUpgradeMultiplier) * multiShotMultiplier, false, true, false);
									MyVisualScriptLogicProvider.CreateExplosion(toCoords + explosionOffset, Settings.ExplosionRadius, 0);
									
								}
								
							}else{
								
								Utilities.CreatePhantomExplosion(toCoords + explosionOffset, Settings.ExplosionRadius, Utilities.UpgradedValueMultiply((float)Settings.ExplosionDamage, DamageUpgradeMultiplier) * multiShotMultiplier, WeaponBlock.EntityId, true, true, false);
								
							}
							
						}
						
						//Hacking Damage
						if(Settings.UseHackingDamage == true && TargetHackingHit == false && targetIsBubble == false){
							
							if(targetGrid != null){
								
								TargetHackingHit = true;
								Utilities.HackTargetBlocks(targetGrid, Utilities.Rnd.Next(Utilities.UpgradedValueAddInt(HackEffectUpgradeBlocksIncrement, Settings.HackingMinBlocksAffected), Utilities.UpgradedValueAddInt(HackEffectUpgradeBlocksIncrement, Settings.HackingMaxBlocksAffected)), WeaponBlock.OwnerId);
								
							}
							
						}
						
						//Painter Damage
						if(Settings.UsePainterDamage == true && targetIsBubble == false){
							
							if(targetGrid != null){
								
								Utilities.RepaintTargetBlockstargetGrid(targetGrid, fromCoords, targetHitPosition, WeaponBlock.SlimBlock.ColorMaskHSV, Settings.RandomPaintColor);
								
							}
							
						}
						
						//Clang Damage
						if(Settings.UseClangCannonEffect == true && targetIsBubble == false && TargetClangHit == false){
							
							if(targetGrid != null){
								
								TargetClangHit = true;
								Utilities.DetachTargetBlock(targetGrid, fromCoords, toCoords);
								
							}
							
						}
						
					}
					
					//Missile Special Case
					if(doDamage == true && Settings.RelaxedMissileIntercept == true && targetEntity == null && Settings.UseBaseDamage == true && TurretBlock != null){

						//MyVisualScriptLogicProvider.ShowNotificationToAll("Check Turret Data ", 1000);
						
						var detectData = TurretBlock.GetTargetedEntity();
						
						if(detectData.IsEmpty() == false){
							
							//MyVisualScriptLogicProvider.ShowNotificationToAll("TargetType: " + detectData.Type.ToString(), 1000);
							
							if(detectData.Type == Sandbox.ModAPI.Ingame.MyDetectedEntityType.Missile){
								
								var barrelToMissileDist = Vector3D.Distance(detectData.Position, offsetMatrix.Translation);
								var barrelToMissilePos = barrelToMissileDist * offsetMatrix.Forward + offsetMatrix.Translation;
								//MyVisualScriptLogicProvider.ShowNotificationToAll("Missile Dist: " + Vector3D.Distance(barrelToMissilePos, detectData.Position).ToString(), 1000);
								
								if(Vector3D.Distance(barrelToMissilePos, detectData.Position) <= 3){
									
									toCoords = detectData.Position;
									
									IMyEntity missileEntity = null;
									
									if(MyAPIGateway.Entities.TryGetEntityById(detectData.EntityId, out missileEntity) == true){
										
										var destroyableObject = missileEntity as IMyDestroyableObject;
										
										if(destroyableObject != null){
											
											destroyableObject.DoDamage(Utilities.UpgradedValueMultiply(Settings.BaseDamageAmount, DamageUpgradeMultiplier), MyStringHash.GetOrCompute("Laser"), true, null, WeaponBlock.OwnerId);
											
										}
										
									}
									
									TurretBlock.ResetTargetingToDefault();
									
								}
								
							}
							
						}
						
					}
					
					//DefenseShield Bubble Hit Only
					if(doDamage == true && targetIsBubble == true){

					  var targetGrid = targetEntity as IMyCubeGrid;
						
						if(targetGrid == null){

							//ShieldBuster

							if(TargetShieldsHit == false && hasShieldBuster == true){
								
								Utilities.DamageShielding(null, Settings.ShieldDamagePercentage * multiShotMultiplier, shield, toCoords, WeaponBlock.EntityId);
								TargetShieldsHit = true;
								
							}
						   
							//Base Damage
							if(Settings.UseBaseDamage == true){
								
								Utilities.ShieldAPI.PointAttackShield(shield, toCoords, WeaponBlock.EntityId, Utilities.UpgradedValueMultiply(Settings.BaseDamageAmount, DamageUpgradeMultiplier) * multiShotMultiplier, false, true, false);
								
							}
							
							//Explode Damage
							if(Settings.UseExplosionDamage == true){
								
								Utilities.ShieldAPI.PointAttackShield(shield, toCoords, WeaponBlock.EntityId, Utilities.UpgradedValueMultiply(Utilities.GetExplosiveDamageForDefenseShield(Settings.ExplosionDamage, Settings.ExplosionRadius), DamageUpgradeMultiplier) * multiShotMultiplier, false, true, false);
								
							}

						}

						/*
						long shieldedEntityId = 0;
						IMyEntity shieldedEntity = null;
						IMyCubeGrid shieldedGrid = null;
						IMySlimBlock closestBlock = null;
						Vector3D blockPosition = Vector3D.Zero;
						
						if(long.TryParse(bubbleEntityIdString, out shieldedEntityId) == true){
							
							if(MyAPIGateway.Entities.TryGetEntityById(shieldedEntityId, out shieldedEntity) == true){
								
								shieldedGrid = shieldedEntity as IMyCubeGrid;
								
								if(shieldedGrid != null){
									
									closestBlock = Utilities.GetClosestBlock(fromCoords, toCoords, shieldedGrid);
									closestBlock.ComputeWorldCenter(out blockPosition);
									
								}
								
							}
							
						}
						
						//BaseDamage
						if(Settings.UseBaseDamage == true && closestBlock != null){
							
							closestBlock.DoDamage(Utilities.UpgradedValueMultiply(Settings.BaseDamageAmount, DamageUpgradeMultiplier), MyStringHash.GetOrCompute("Laser"), true, null, WeaponBlock.EntityId);
							
						}
						
						//ShieldBuster
						if(hasShieldBuster == true && closestBlock != null){
							
							Utilities.DisableGridShields(shieldedGrid, WeaponBlock.EntityId);
							TargetShieldsHit = true;
							
						}
						
						//Explosive
						if(Settings.UseExplosionDamage == true && closestBlock != null){
							
							Utilities.CreatePhantomExplosion(blockPosition, Settings.ExplosionRadius, Utilities.UpgradedValueMultiply((float)Settings.ExplosionDamage, DamageUpgradeMultiplier), WeaponBlock.EntityId, false, false, false);
							MyVisualScriptLogicProvider.CreateExplosion(toCoords, Settings.ExplosionRadius, 0);
							
						}else if(Settings.UseExplosionDamage == true && closestBlock == null){
							
							Utilities.CreatePhantomExplosion(toCoords, Settings.ExplosionRadius, Utilities.UpgradedValueMultiply((float)Settings.ExplosionDamage, DamageUpgradeMultiplier), WeaponBlock.EntityId, true, true, false);
						}
						*/
					}

				}

				if(doDamage == true && Settings.UseMultiChargeShot == true) {

					WeaponBlock.GetInventory().Clear();

				}

			}
			
			//Process Effects
			
			foreach(var offset in Settings.BarrelSubpartOffsets){
				
				var fromCoords = Vector3D.Transform(offset, offsetMatrix);
				var toCoords = Vector3D.Zero;
				double fireDistance = 0;
				
				if(LastRayHitDistances.TryGetValue(offset, out fireDistance) == true){
					
					toCoords = fireDistance * offsetMatrix.Forward + fromCoords;
					
				}else{
					
					toCoords = (double)Utilities.UpgradedValueMultiply((float)Settings.WeaponDistance, RangeUpgradeMultiplier) * offsetMatrix.Forward + fromCoords;
					
				}
				
				//Regular Beam Settings
				if(Settings.UseRegularBeam == true && IsDedicated == false){
					
					float beamRadius = Settings.BeamRadius;
					
					if(Settings.UseBeamFlicker == true){
						
						beamRadius = Utilities.RandomFloat(Settings.BeamMinimumRadius, Settings.BeamMaximumRadius);
						
					}
					
					var beamColor = new Vector4(0,0,0,0);
					
					if(Settings.FadeThroughColors == true && Settings.BeamColors.Count > 1){
						
						FadeTickCounter++;
						
						if(NextColorIndex < Settings.BeamColors.Count){
							
							CurrentFadeColorVector += CurrentFadeIncrement;
							CurrentFadeColor = Utilities.ConvertColorFromVector(CurrentFadeColorVector);
							
						}
						
						beamColor = Utilities.ConvertColor(CurrentFadeColor);
						
						if(FadeTickCounter >= FadeTickCounterTrigger){
							
							NextColorIndex++;
							FadeTickCounter = 0;
							
							if(NextColorIndex < Settings.BeamColors.Count){
								
								CurrentFadeIncrement = Utilities.GetColorFadeIncrement(CurrentFadeColor, Settings.BeamColors[NextColorIndex], FadeTickCounterTrigger);
								
							}
							
						}
						
					}else{
						
						var randomColor = Settings.BeamColors[Utilities.Rnd.Next(0, Settings.BeamColors.Count)];
						beamColor = Utilities.ConvertColor(randomColor);
						
					}

					MySimpleObjectDraw.DrawLine(fromCoords, toCoords, MyStringId.GetOrCompute("WeaponLaser"), ref beamColor, beamRadius);
					MySimpleObjectDraw.DrawLine(fromCoords, toCoords, MyStringId.GetOrCompute("WeaponLaser"), ref beamColor, beamRadius * 0.66f);
					MySimpleObjectDraw.DrawLine(fromCoords, toCoords, MyStringId.GetOrCompute("WeaponLaser"), ref beamColor, beamRadius * 0.33f);
					
				}
				
				//Tesla Beam Settings
				if(Settings.UseTeslaBeam == true && IsDedicated == false){
					
					TeslaBeamTicks++;
					
					if(TeslaBeamTicks >= 4){
						
						var newList = Utilities.CreateElectricityOffset((double)Utilities.UpgradedValueMultiply((float)Settings.WeaponDistance, RangeUpgradeMultiplier), Settings.TeslaBeamMinStep, Settings.TeslaBeamMaxStep, Settings.TeslaBeamMaxLateral);
						
						if(TeslaBeamPoints.ContainsKey(offset) == true){
							
							TeslaBeamPoints[offset] = newList;
							
						}else{
							
							TeslaBeamPoints.Add(offset, newList);
							
						}
						
					}
					
					float teslaRadius = Settings.TeslaBeamRadius;
					
					if(Settings.UseTeslaBeamFlicker == true){
						
						teslaRadius = Utilities.RandomFloat(Settings.TeslaBeamMinimumRadius, Settings.TeslaBeamMaximumRadius);
						
					}
					
					var randomColor = Settings.TeslaBeamColors[Utilities.Rnd.Next(0, Settings.TeslaBeamColors.Count)];
					var startMatrix = MatrixD.CreateWorld(fromCoords, offsetMatrix.Forward, offsetMatrix.Up);
					Utilities.DisplayElectricEffect(startMatrix, toCoords, teslaRadius, randomColor, TeslaBeamPoints[offset]);
					
				}
				
				
				
				/*
				if(Settings.UseBarrelLights == true){
					
					var light = new MyLight();
					light.LightOn = true;
					light.Position = fromCoords;
					light.Color = Settings.BarrelLightColor;
					light.Intensity = Settings.BarrelLightIntensity;
					light.Range = Settings.BarrelLightRange;
					
					if(Settings.BarrelLightUseGlare == true){
						
						light.GlareOn = true;
						light.GlareIntensity = Settings.BarrelLightGlareIntensity;
						light.GlareMaxDistance = Settings.BarrelLightGlareMaxDistance;

					}
					
				}
				*/
			}
			
			//Secondary Beam
			if(Settings.UseSecondaryBeam == true){
				
				float secondBeamRadius = Settings.SecondaryBeamRadius;
				
				if(Settings.UseSecondaryBeamFlicker == true){
					
					secondBeamRadius = Utilities.RandomFloat(Settings.SecondaryBeamMinimumRadius, Settings.SecondaryBeamMaximumRadius);
					
				}
				
				var secondaryBeamColor = new Vector4(0,0,0,0);
				
				if(Settings.FadeThroughColors == true && Settings.BeamColors.Count > 1){
					
					FadeTickCounter++;
					
					if(NextColorIndex < Settings.BeamColors.Count){
						
						CurrentFadeColorVector += CurrentFadeIncrement;
						CurrentFadeColor = Utilities.ConvertColorFromVector(CurrentFadeColorVector);
						
					}
					
					secondaryBeamColor = Utilities.ConvertColor(CurrentFadeColor);
					
					if(FadeTickCounter >= FadeTickCounterTrigger){
						
						NextColorIndex++;
						FadeTickCounter = 0;
						
						if(NextColorIndex < Settings.BeamColors.Count){
							
							CurrentFadeIncrement = Utilities.GetColorFadeIncrement(CurrentFadeColor, Settings.BeamColors[NextColorIndex], FadeTickCounterTrigger);
							
						}
						
					}
					
				}else{
					
					var randomColor = Settings.BeamColors[Utilities.Rnd.Next(0, Settings.BeamColors.Count)];
					secondaryBeamColor = Utilities.ConvertColor(randomColor);
					
				}
				
				foreach(var offset in Settings.SecondaryBeamOffsets){
					
					var secondFromCoords = Vector3D.Transform(offset, offsetMatrix);
					var secondToCoords = offsetMatrix.Forward * LastRayClosestDistance + secondFromCoords;
					MySimpleObjectDraw.DrawLine(secondFromCoords, secondToCoords, MyStringId.GetOrCompute("WeaponLaser"), ref secondaryBeamColor, secondBeamRadius);
					MySimpleObjectDraw.DrawLine(secondFromCoords, secondToCoords, MyStringId.GetOrCompute("WeaponLaser"), ref secondaryBeamColor, secondBeamRadius * 0.66f);
					MySimpleObjectDraw.DrawLine(secondFromCoords, secondToCoords, MyStringId.GetOrCompute("WeaponLaser"), ref secondaryBeamColor, secondBeamRadius * 0.33f);
					
					
				}
				
			}

			
			
			ActiveTimer++;
			
			if(ActiveTimer >= Settings.TickTimerLimit){
				
				ActiveFiring = false;
				
				CurrentFadeColor = Settings.EmissiveFiringOffColor;
				ChangeEmissive("Firing", true, true);
				
				
				
			}
			
		}
		
		public override void UpdateBeforeSimulation100(){
			
			string crashResult = "";
			
			try{		
				
				if(WeaponBlock == null){
					
					return;
					
				}
				
				if(InitialOwnershipCheck == false){
				
					InitialOwnershipCheck = true;
					crashResult = "Check Initial Ownership";
					IsOwnedByNPC = Utilities.IsOwnerNPC(WeaponBlock.OwnerId);
				
				}
				
				if(Settings.UseRegenerativeAmmo == true){
					
					crashResult = "Check Power State";
					CheckPowerState(true);
					
					crashResult = "Check BlockColor";
					if(BlockColor != WeaponBlock.SlimBlock.ColorMaskHSV){
						
						crashResult = "Change Emissive On Block Color Change";
						ChangeEmissive(EmissiveMode, true);
						BlockColor = WeaponBlock.SlimBlock.ColorMaskHSV;
						
					}

				}
				
			}catch(Exception exc){
				
				Utilities.AddMsg("Update100 Failed with Error Position: " + crashResult);
				
			}	

		}
		
		public void CheckPowerState(bool regularActivation = false){
			
			float currentItems = 0;
			
			if(BlockIsWorking == true){
				
				currentItems = (float)WeaponInventory.GetItemAmount(AmmoItemDefId);
				LastAmmoCount = currentItems;
				
				if((currentItems < Settings.MaxAmmoInInventory || Settings.MustBeWorkingAtMaxDraw == true) && LowPower == false){
					
					if(Settings.AmmoRegenerationFreeForNPC == false || IsOwnedByNPC == false){
						
						RequiredPower = Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMaxPowerDraw);
						
					}
					
					ChangeEmissive("Charging");
					
				}else{

					RequiredPower = MinimumPower;
					ChangeEmissive("Idle");
				
				}
				
			}else{
				
				RequiredPower = 0;
				
			}
			
			if(Settings.AmmoRegenerationFreeForNPC == true && IsOwnedByNPC == true){
				
				RequiredPower = MinimumPower;
				
			}
			
			var powerAvailable = ResourceSink.IsPowerAvailable(PowerId, RequiredPower);	
			
			//Have I told anyone before that I REALLY hate ResourceSink management??
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Check Power Req: " + powerAvailable.ToString(), 1600);
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Block Working: " + BlockIsWorking.ToString(), 1600);
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Required Power: " + RequiredPower.ToString(), 1600);
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Previous Required Power: " + PreviousRequiredPower.ToString(), 1600);
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Sink Required Power: " + ResourceSink.RequiredInputByType(PowerId).ToString(), 1600);
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Sink Previous Required Power: " + ResourceSink.MaxRequiredInputByType(PowerId).ToString(), 1600);
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Sink Current Input: " + ResourceSink.CurrentInputByType(PowerId).ToString(), 1600);
			
			if(LowPower == false && BlockIsWorking == true){
								
				if(powerAvailable == false){
					
					RequiredPower = Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMedPowerDraw);
					powerAvailable = ResourceSink.IsPowerAvailable(PowerId, Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMedPowerDraw));
					
					if(powerAvailable == false){
						
						powerAvailable = ResourceSink.IsPowerAvailable(PowerId, MinimumPower);
						
						if(powerAvailable == false){
							
							LowPower = true;
							
						}
						
					}
					
				}
				
				if(LowPower == false && RequiredPower != MinimumPower && currentItems < Settings.MaxAmmoInInventory && regularActivation == true){
					
					//Add To AccumulatedPower
					var powerToAdd = RequiredPower * 1.66f;
					AccumulatedPower += powerToAdd;
					
					if(AccumulatedPower >= Utilities.UpgradedValueAdd(Settings.AmmoRegenerationTime, PowerStoreUpgradeMultiplier)){
						
						AccumulatedPower = 0;
						
						if(WeaponInventory.CanItemsBeAdded((MyFixedPoint)Settings.AmmoAmountToAdd, AmmoItemDefId) == true && IsServer == true){
							
							WeaponInventory.AddItems((MyFixedPoint)Settings.AmmoAmountToAdd, AmmoItem.Content);
							currentItems = (float)WeaponInventory.GetItemAmount(AmmoItemDefId);
							LastAmmoCount = currentItems;
							
						}
						
					}
					
				}
				
				if(Settings.AmmoRegenerationFreeForNPC == true && IsOwnedByNPC == true && regularActivation == true && currentItems < Settings.MaxAmmoInInventory){
				
					//Add To AccumulatedPower
					var powerToAdd = Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMaxPowerDraw) * 1.66f;
					AccumulatedPower += powerToAdd;
					
					if(AccumulatedPower >= Utilities.UpgradedValueAdd(Settings.AmmoRegenerationTime, PowerStoreUpgradeMultiplier)){
						
						AccumulatedPower = 0;
						
						if(WeaponInventory.CanItemsBeAdded((MyFixedPoint)Settings.AmmoAmountToAdd, AmmoItemDefId) == true && IsServer == true){
							
							WeaponInventory.AddItems((MyFixedPoint)Settings.AmmoAmountToAdd, AmmoItem.Content);
							currentItems = (float)WeaponInventory.GetItemAmount(AmmoItemDefId);
							LastAmmoCount = currentItems;
							
						}
						
					}
					
				}
				
			}else{
				
				if(powerAvailable == true && BlockIsWorking == true){

					if(ResourceSink.IsPowerAvailable(PowerId, RequiredPower) == true){
						
						LowPower = false;
						RequiredPower = MinimumPower;
						
						if(ResourceSink.IsPowerAvailable(PowerId, Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMedPowerDraw)) == true && currentItems < Settings.MaxAmmoInInventory && Settings.AmmoRegenerationFreeForNPC == false){
							
							RequiredPower = Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMedPowerDraw);
							
							if(ResourceSink.IsPowerAvailable(PowerId, Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMaxPowerDraw)) == true){
								
								RequiredPower = Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMaxPowerDraw);
								
							}
							
						}
						
					}
		
				}else{
					
					RequiredPower = MinimumPower;
					
				}
				
			}
			
			if(Settings.AmmoRegenerationFreeForNPC == false && Settings.MustBeWorkingAtMaxDraw == true && BlockIsWorking == true && RequiredPower < Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMaxPowerDraw)){
				
				if(currentItems > 0){
					
					WeaponInventory.Clear();
					currentItems = 0;
					
				}
				
				AccumulatedPower = 0;
				
			}

			if(LocalPlayer != null){
				
				if(MyAPIGateway.Gui.GetCurrentScreen == MyTerminalPageEnum.ControlPanel){
					
					var getCustomInfo = WeaponBlock.CustomInfo;
					WeaponBlock.RefreshCustomInfo();
					
					if(getCustomInfo != WeaponBlock.CustomInfo){
						
						var myCubeBlock = Entity as MyCubeBlock;
						
						if(myCubeBlock.IDModule != null){
							
							var share = myCubeBlock.IDModule.ShareMode;
							var owner = myCubeBlock.IDModule.Owner;
							myCubeBlock.ChangeOwner(owner, share == MyOwnershipShareModeEnum.None ? MyOwnershipShareModeEnum.Faction : MyOwnershipShareModeEnum.None);
							myCubeBlock.ChangeOwner(owner, share);
							
						}
						
					}
				
				}
				
			}
			
		}
		
		public void ChangeEmissive(string mode, bool force = false, bool useFadeColor = false){
			
			if(EmissiveMode == mode && force == false){
				
				return;
				
			}
			
			if(mode == "Idle"){
				
				EmissiveMode = "Idle";
				var emissiveAmount = Settings.EmissiveIdleAmount;
				
				ApplyEmissive(Settings.EmissiveIdleName, Settings.EmissiveIdleColor, emissiveAmount);
				
			}
			
			if(mode == "Charging"){
				
				EmissiveMode = "Charging";
				var emissiveAmount = Settings.EmissiveChargingAmount;
				ApplyEmissive(Settings.EmissiveChargingName, Settings.EmissiveChargingColor, emissiveAmount);
					
			}
			
			if(mode == "Inactive"){
				
				EmissiveMode = "Inactive";
				var emissiveAmount = Settings.EmissiveInactiveAmount;
				ApplyEmissive(Settings.EmissiveInactiveName, Settings.EmissiveInactiveColor, emissiveAmount);
				
			}
			
			if(mode == "Firing"){
				
				EmissiveMode = "Firing";
				var emissiveAmount = Settings.EmissiveFiringAmount;
				var color = Settings.EmissiveFiringColor;
				
				if(useFadeColor == true){
					
					color = CurrentFadeColor;
					
				}
				
				ApplyEmissive(Settings.EmissiveFiringName, color, emissiveAmount);
				
			}
			
		}
		
		public void ApplyEmissive(string emissiveName, Color color, float amount){
			
			WeaponBlock.SetEmissiveParts(emissiveName, color, amount);
			WeaponBlock.SetEmissivePartsForSubparts(emissiveName, color, amount);
			
			if(TurretBlock != null){
				
				var blockEntity = WeaponBlock as MyEntity;
					
				foreach(var key in blockEntity.Subparts.Keys){
					
					var subpartEntity = blockEntity.Subparts[key] as IMyEntity;
					subpartEntity.SetEmissiveParts(emissiveName, color, amount);
					subpartEntity.SetEmissivePartsForSubparts(emissiveName, color, amount);
					
					foreach(var keyB in blockEntity.Subparts[key].Subparts.Keys){

						var subpartEntityB = blockEntity.Subparts[key].Subparts[keyB] as IMyEntity;
						subpartEntityB.SetEmissiveParts(emissiveName, color, amount);
						subpartEntityB.SetEmissivePartsForSubparts(emissiveName, color, amount);
					
					}
					
				}
				
			}
			
		}
		
		public void AppendCustomInfo(IMyTerminalBlock block, StringBuilder sb){
			
			sb.Clear();
			
			if(LowPower == false/* && block.IsFunctional == true && block.IsWorking == true*/){
				
				sb.Append("[Ammo Generator]").AppendLine().AppendLine();
				sb.Append("Current Power Draw: \n" + RequiredPower.ToString() + "MW");
				sb.AppendLine().AppendLine();
				sb.Append("Accumulated Power: \n" + AccumulatedPower.ToString() + "MW / " + Settings.AmmoRegenerationTime.ToString() + "MW");
				sb.AppendLine().AppendLine();
				sb.Append("Energy Charges Produced: \n" + LastAmmoCount.ToString() + " / " + Settings.MaxAmmoInInventory.ToString());
				sb.AppendLine().AppendLine();
				
				if(IsOwnedByNPC == true){
					
					sb.Append("NPC Ownership: True");
					sb.AppendLine().AppendLine();
					
				}
				
				if(Settings.MustBeWorkingAtMaxDraw == true){
					
					sb.Append("Required Power Draw: " + Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMaxPowerDraw).ToString() + "MW");
					sb.AppendLine().AppendLine();
					
					if(RequiredPower < Utilities.UpgradePowerRequirement(PowerUpgradeMultiplier, Settings.AmmoRegenerationMaxPowerDraw)){
						
						sb.Append("Insufficient Power: True");
						sb.AppendLine().AppendLine();
						
					}else{
						
						sb.Append("Insufficient Power: " + LowPower.ToString());
						sb.AppendLine().AppendLine();
						
					}
					
				}else{
					
					sb.Append("Insufficient Power: " + LowPower.ToString());
					sb.AppendLine().AppendLine();
					
				}
				
				if(Settings.AllowUpgrades == true){
					
					sb.Append("[Upgrades]").AppendLine().AppendLine();
					
					if(Settings.UseRegenerativeAmmo == true){
						
						sb.Append("Power Multiplier: \n" + PowerUpgradeMultiplier.ToString());
						sb.AppendLine().AppendLine();
						sb.Append("Accumulated Power Modifier: \n" + PowerStoreUpgradeMultiplier.ToString());
						sb.AppendLine().AppendLine();
						
					}
					
					if(Settings.UseBaseDamage == true || Settings.UseExplosionDamage == true){
						
						sb.Append("Damage Multiplier: \n" + DamageUpgradeMultiplier.ToString());
						sb.AppendLine().AppendLine();
						
					}
					
					if(Settings.WeaponDistance > 0){
						
						sb.Append("Range Multiplier: \n" + RangeUpgradeMultiplier.ToString());
						sb.AppendLine().AppendLine();
						
					}
					
					if(Settings.UseTeslaEffect == true){
						
						sb.Append("Tesla Additional Effect: \n" + TeslaEffectUpgradeBlocksIncrement.ToString());
						sb.AppendLine().AppendLine();
						
					}
					
					if(Settings.UseJumpDriveInhibitor == true){
						
						sb.Append("Jump Inhibitor Additional Effect: \n" + JumpEffectUpgradeIncrement.ToString());
						sb.AppendLine().AppendLine();
						
					}
					
					if(Settings.UseHackingDamage == true){
						
						sb.Append("Hacking Additional Effect: \n" + HackEffectUpgradeBlocksIncrement.ToString());
						sb.AppendLine().AppendLine();
						
					}
					
					if(Settings.UseTractorBeamEffect == true){
						
						sb.Append("Tractor Additional Effect: \n" + TractorEffectUpgradeIncrement.ToString());
						sb.AppendLine().AppendLine();
						
					}
					
					if(ShieldEffectUpgradeIncrement > 0){
						
						sb.Append("Shield Buster Effect: \n" + "Enabled");
						sb.AppendLine().AppendLine();
						
					}
					
				}
				
			}else{
				
				sb.Append("Weapon Offline.");
				
			}
			
		}
		
		public void WorkingStateChange(IMyCubeBlock block){
			
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Work State Change", 2000);
			if(block.IsFunctional == false || block.IsWorking == false){
				
				if(Settings.UseBarrelParticles == true && IsDedicated == false){
					
					foreach(var barrelParticle in BarrelParticles.Keys){
						
						BarrelParticles[barrelParticle].StopEmitting();
						
					}
					
				}
				
				BlockIsWorking = false;
				ChangeEmissive("Inactive");
				
				if(ActiveFiring == true){
					
					ActiveFiring = false;
					
				}
				
				if(Settings.UseRegenerativeAmmo == true){
					
					if(Settings.MustBeWorkingAtMaxDraw == true){
					
						WeaponInventory.Clear();
						AccumulatedPower = 0;
						
					}
					
					RequiredPower = 0; //MinimumPower;
					CheckPowerState();
				
				}
								
			}
			
			if(block.IsFunctional == true && block.IsWorking == true){
				
				BlockIsWorking = true;
				ChangeEmissive("Idle");
				
				if(Settings.UseRegenerativeAmmo == true){
					
					RequiredPower = MinimumPower;
					CheckPowerState();
					
				}

			}

		}
		
		public float PowerUse(){
			
			return RequiredPower;
			
		}
		
		public void RecheckOwnership(IMyTerminalBlock block){
			
			IsOwnedByNPC = Utilities.IsOwnerNPC(WeaponBlock.OwnerId);
			
		}
		
		public void RefreshUpgrades(){
			
			if(UpgradeBlock == null || Settings.AllowUpgrades == false){
				
				return;
				
			}
			
			var upgrades = UpgradeBlock.UpgradeValues;
			//var upgrades = new Dictionary<string, float>();
			//UpgradeBlock.GetUpgrades(out upgrades);
			
			MyVisualScriptLogicProvider.ShowNotificationToAll("Upgrade Count: " + upgrades.Keys.Count.ToString(), 5000);
			
			//Reset all upgrade value variables to default values first
			DamageUpgradeMultiplier = 1;
			PowerUpgradeMultiplier = 1;
			PowerStoreUpgradeMultiplier = 0;
			RangeUpgradeMultiplier = 1;
			TeslaEffectUpgradeBlocksIncrement = 0;
			JumpEffectUpgradeIncrement = 0;
			HackEffectUpgradeBlocksIncrement = 0;
			TractorEffectUpgradeIncrement = 0;
			
			//Now, read new or changed values
			
			//Power
			if(Settings.UseRegenerativeAmmo == true && upgrades.ContainsKey(Settings.UpgradePowerName) == true){
				
				PowerUpgradeMultiplier += upgrades[Settings.UpgradePowerName];
				
			}
			
			//PowerStore
			if(Settings.UseRegenerativeAmmo == true && upgrades.ContainsKey(Settings.UpgradePowerStoreName) == true){
				
				PowerStoreUpgradeMultiplier += upgrades[Settings.UpgradePowerStoreName];
				
			}
			
			//Damage
			if(Settings.UseBaseDamage == true && upgrades.ContainsKey(Settings.UpgradeDamageName) == true){
				
				DamageUpgradeMultiplier += upgrades[Settings.UpgradeDamageName];
				
			}
			
			//ExplodeDamage
			if(Settings.UseExplosionDamage == true && upgrades.ContainsKey(Settings.UpgradeDamageName) == true){
				
				DamageUpgradeMultiplier += upgrades[Settings.UpgradeDamageName];
				
			}
			
			//Range
			if(upgrades.ContainsKey(Settings.UpgradeRangeName) == true){
				
				RangeUpgradeMultiplier += upgrades[Settings.UpgradeRangeName];
				
			}			
			
			//Tesla
			if(Settings.UseTeslaEffect == true && upgrades.ContainsKey(Settings.UpgradeTeslaEffectName) == true){
				
				int roundedValue = (int)Math.Floor(upgrades[Settings.UpgradeTeslaEffectName]);
				TeslaEffectUpgradeBlocksIncrement += roundedValue;
				
			}	
			
			//Jump
			if(Settings.UseJumpDriveInhibitor == true && upgrades.ContainsKey(Settings.UpgradeJumpEffectName) == true){
				
				JumpEffectUpgradeIncrement += upgrades[Settings.UpgradeJumpEffectName];
				
			}
			
			//Hack
			if(Settings.UseHackingDamage == true && upgrades.ContainsKey(Settings.UpgradeHackEffectName) == true){
				
				int roundedValue = (int)Math.Floor(upgrades[Settings.UpgradeHackEffectName]);
				HackEffectUpgradeBlocksIncrement += roundedValue;
				
			}
			
			//Tractor 
			if(Settings.UseTractorBeamEffect == true && upgrades.ContainsKey(Settings.UpgradeTractorEffectName) == true){
				
				TractorEffectUpgradeIncrement += upgrades[Settings.UpgradeTractorEffectName];
				
			}
			
			//Finally, Ensure the new values are within minimum allowed values.
			if(DamageUpgradeMultiplier < DamageUpgradeMinimum){
				
				DamageUpgradeMultiplier = DamageUpgradeMinimum;
				
			}
			
			if(PowerUpgradeMultiplier < PowerUpgradeMinimum){
				
				PowerUpgradeMultiplier = PowerUpgradeMinimum;
				
			}
			
			if(RangeUpgradeMultiplier < RangeUpgradeMinimum){
				
				RangeUpgradeMultiplier = RangeUpgradeMinimum;
				
			}
			
			if(TeslaEffectUpgradeBlocksIncrement < TeslaEffectUpgradeMinimum){
				
				TeslaEffectUpgradeBlocksIncrement = TeslaEffectUpgradeMinimum;
				
			}
			
			if(JumpEffectUpgradeIncrement < JumpEffectUpgradeMinimum){
				
				JumpEffectUpgradeIncrement = JumpEffectUpgradeMinimum;
				
			}
			
			if(HackEffectUpgradeBlocksIncrement < HackEffectUpgradeMinimum){
				
				HackEffectUpgradeBlocksIncrement = HackEffectUpgradeMinimum;
				
			}
			
			if(TractorEffectUpgradeIncrement < TractorEffectUpgradeMinimum){
				
				TractorEffectUpgradeIncrement = TractorEffectUpgradeMinimum;
				
			}
			
		}
		
		public void NetworkMessageReceiver(byte[] receivedData){
			
			var shotData = MyAPIGateway.Utilities.SerializeFromBinary<EnergyWeaponSyncData>(receivedData);
			
			if(shotData == null || IsServer == true){
				
				return;
				
			}
			
			if(shotData.WeaponEntityId == WeaponBlock.EntityId){
				
				LastWeaponFire = shotData.LastFire;
				
			}
			
		}
		
		public override void OnRemovedFromScene(){
			
			base.OnRemovedFromScene();
			MyAPIGateway.Multiplayer.UnregisterMessageHandler(31461, NetworkMessageReceiver);
			
			if(Settings.UseBarrelParticles == true && IsDedicated == false){
					
				foreach(var barrelParticle in BarrelParticles.Keys){
					
					BarrelParticles[barrelParticle].StopEmitting();
					
				}
				
			}
			
			var Block = Entity as IMyUserControllableGun;
			
			if(Block == null){
				
				return;
				
			}
			
			Block.IsWorkingChanged -= WorkingStateChange;
			Block.AppendingCustomInfo -= AppendCustomInfo;
			Block.OwnershipChanged -= RecheckOwnership;
			
			
		}
		
		public override void OnBeforeRemovedFromContainer(){
			
			base.OnBeforeRemovedFromContainer();
			
			if(Entity.InScene == true){
				
				OnRemovedFromScene();
				
			}
			
		}
		
	}
	
}