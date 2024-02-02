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
	
	public static class BlocksConfig{
		
		public static WeaponConfig GetWeaponConfig(string subtypeName){
			
			////////////////////////////////////////////////////////
			//////////////// DO NOT EDIT ABOVE HERE ////////////////
			////////////////////////////////////////////////////////
			
			////////////// --- Copy From Here --- //////////////////
			
			if(subtypeName == "LargeFixedAmplifiedLaser"){
				
				//Do Not Edit This Line
				var Settings = new WeaponConfig();
				
				//Weapon Subtype
				Settings.WeaponSubtypeId = "LargeFixedAmplifiedLaser"; //Change this to the same weapon subtype as used above in the script setup
				Settings.UseScriptedFire = true; //Change this to false if you only want to use the ammo generating feature of the weapon.
				//Incomplete Feature: Settings.RegisterDamageHandlerForTracers = false; //Set to true if you plan to use non-beam vanilla tracers with this weapon.
				
				//Regenerative Ammo Settings
				Settings.UseRegenerativeAmmo = true; //If set to 'true', then this weapon will consume grid energy and generate ammo automatically.
				Settings.AmmoMagazineSubtypeId = "PDLaserCharge"; //The AmmoMagazine SubtypeId this weapon uses.
				Settings.AmmoRegenerationMaxPowerDraw = 60; //Maximum amount of power the weapon should draw to generate ammo.
				Settings.AmmoRegenerationMedPowerDraw = 60; //If Maximum amount of power draw is unavailable, then this amount is drawn instead.
				Settings.AmmoRegenerationTime = 60; //Time until ammo is generated (at rate of 1MW per second).
				Settings.AmmoAmountToAdd = 20; //Number of ammo magazines added when a charge is complete.
				Settings.MaxAmmoInInventory = 20; //If ammo in we0apon meets or exceeds this number, ammo regeneration will stop.
				Settings.AmmoRegenerationFreeForNPC = true; //If true and the block is owned by a valid NPC identity, the weapon will not draw energy to generate ammo, but will still create the ammo as if charging at AmmoRegenerationMaxPowerDraw rate.
				Settings.MustBeWorkingAtMaxDraw = false; //If true, weapon must be on, undamaged, and charging at max. If any of the consitions are not met, charge is reset to 0 and ammo is removed.
				
				//Pre-Fire Settings
				Settings.UsePreFireDelay = false; //If true, the weapon will have a delayed fire when the weapon is shot.
				Settings.PreHitTimerLimit = 20; //How long (in game ticks) the pre-fire phase should last
				Settings.PreFireSoundId = ""; //Here you can specify a sound that will play during the pre-fire
				Settings.PreFireEmissiveCharge = false; //If true, the Firing Emissive will fade from EmissiveFiringOffColor to EmissiveFiringColor before the weapon fires
				
				//Damage / Hit Timer Settings
				Settings.TickTimerLimit = 5; //Total Time (in game ticks) the beam is active
				Settings.DamageTimerLimit = 1; //Damage is applied at this game tick interval.
				
				//Distance Settings
				Settings.WeaponDistance = 4000; //Beam Distance
				Settings.SafeRange = 10; //If Beam Hits Own Grid, If Distance From Barrel to Hit is less than this value, it will be ignored.
				
				//Emissives - Off/Disabled/Damaged
			Settings.EmissiveInactiveName = "Emissive"; //Emissive Material Name
			Settings.EmissiveInactiveAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveInactiveColor = new Color(0,0,0,0); //RGBA value of Emissive
			
			//Emissives - Idle
			Settings.EmissiveIdleName = "Emissive"; //Emissive Material Name
			Settings.EmissiveIdleAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveIdleColor = new Color(0,20,255,175); //RGBA value of Emissive
			
			//Emissives - Idle
			Settings.EmissiveIdleName = "Emissive2"; //Emissive Material Name
			Settings.EmissiveIdleAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveIdleColor = new Color(255,1,0,255); //RGBA value of Emissive
			
			//Emissives - Charging
			Settings.EmissiveChargingName = "Emissive"; //Emissive Material Name
			Settings.EmissiveChargingAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveChargingColor = new Color(0,20,255,255); //RGBA value of Emissive
			
			//Emissives - Charging
			Settings.EmissiveChargingName = "Emissive2"; //Emissive Material Name
			Settings.EmissiveChargingAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveChargingColor = new Color(255,1,0,255); //RGBA value of Emissive
			
			//Emissives - Firing
			Settings.EmissiveFiringName = "Emissive"; //Emissive Material Name
			Settings.EmissiveFiringAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveFiringColor = new Color(0,20,255,255); //RGBA value of Emissive when Firing
			Settings.EmissiveFiringOffColor = new Color(0,20,255,255); //RGBA value of Emissive when Not Firing
			
			//Multibeam Settings
			Settings.BarrelSubpartOffsets.Add(new Vector3D(0,0,-4)); //Copy This Line and Provide the XYZ offset of any additional barrels that will fire beams on your weapons. Default offset of 0,0,0 can be changed if needed.
			
			//Upgrade Valid Names
				/*
				Please note that your upgrade definitions attached to your upgrade blocks should only
				ever use <ModifierType>Additive</ModifierType>
				
				Upgrades do not set a new level for the modifier its affecting, but increases or decreases by the value you've provided.
				*/
				Settings.AllowUpgrades = false; //If true, this block will be able to accept upgrade modules.
				Settings.UpgradeDamageName = "ChangeToValidUpgradeName"; //The upgrade name for Damage and Explosion Damage. Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradePowerName = "ChangeToValidUpgradeName"; //The upgrade name for Power Draw (assuming ammo regeneration is enabled). Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradePowerStoreName = "ChangeToValidUpgradeName"; //The upgrade name for the Charged Power trigger that generates a round of ammo. Increase/decrease by a regular number (eg: 50, 100, -25, etc)
				Settings.UpgradeRangeName = "ChangeToValidUpgradeName"; //The upgrade name for Weapon Range. Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradeTeslaEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Tesla Effect. Increase/decrease by a regular number (eg: 1, 2, -1, etc)
				Settings.UpgradeJumpEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Jump Drive Inhibitor Effect. Increase/decrease by a floating point number (eg: 0.1, 0.2, -0.1). Amount reduced is in MW.
				Settings.UpgradeHackEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Hacking Effect. Increase/decrease by a regular number (eg: 1, 2, -1, etc)
				Settings.UpgradeShieldEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Shield Buster Effect. Set to 1 to Enable on Attached Weapon.
				
				//Settings.UpgradeTractorEffectName = "ChangeToValidUpgradeName"; This isn't a thing yet ;)
				
				//Base Damage
				Settings.UseBaseDamage = true; //Specifies if beam should deal regular damage.
				Settings.BaseDamageAmount = 100; //Damage amount per step (steps defined by DamageTimerLimit setting above)
				Settings.UsePenetrativeDamage = false; //If true, the beam will damage multiple blocks within a grid per step.
				Settings.PenetrativeDistance = 5; //Distance the penetrative damage can reach if enabled.
				Settings.RelaxedMissileIntercept = true; //If true, shots fired at lasers that come close to hitting will still register as a hit.
				
				//Explosive Damage
				Settings.UseExplosionDamage = false; //If true, beam will create an explosion each step
				Settings.ExplosionDamage = 400; //Explosion damage
				Settings.ExplosionRadius = 1; //Explosion radius from where beam hits
				
				//Voxel Damage
				Settings.UseVoxelDamage = false; //If true, the beam will cut out voxels at hit position each step.
				Settings.VoxelDamageRadius = 3; //Radius of voxels that are removed at beam hit position.
				
				//Voxel Paint
				Settings.UseVoxelPaint = false; //If true, the beam will paint voxels at hit position each step.
				Settings.VoxelPaintMaterial = "Ice_01"; //Material affected voxels will be replaced with.
				Settings.VoxelPaintRadius = 8; //Radius of voxels that are painted at beam hit position.
				
				//Voxel Add - Feature Not Complete
				Settings.UseVoxelAdd = false; //If true, the beam will add voxels at hit position each step.
				Settings.VoxelAddMaterial = "Ice_01"; //Material added voxels will use.
				Settings.VoxelAddRadius = 3; //Radius of voxels that are added at beam hit position.
				
				//Tesla Damage
				Settings.UseTeslaEffect = false; //If true, a beam hit on a grid will shut off a selection of random blocks.
				Settings.TeslaMaxBlocksAffected = 1; //maximum blocks affected by tesla effect
				Settings.TeslaOnlyAffectBlockAtHitLocation = false; //If true, only the block hit directly by the beam will be affected.
				Settings.TeslaRegisterWeaponForProjectileUse = false; //if true, and if Settings.RegisterDamageHandlerForTracers is true, this weapon will apply tesla damage via projectile/tracer hits.
				
				//Jump Drive Damage
				Settings.UseJumpDriveInhibitor = false; //If true, a beam hit on a grid will drain stored energy on Jump Drives
				Settings.AmountToReduceDrives = 0.3f; //Amount of energy to reduce from Jump Drives (in MW).
				Settings.SplitAcrossEachDrive = true; //If true, the amount to reduce is evenly split across all jump drives on the grid, otherwise the amount is reduced per drive.
				
				//Shield Damage
				Settings.UseShieldBuster = false; //if true, a beam hit on a grid will immediately shutdown and damage all shielding blocks.
				
				//Hacking Damage
				Settings.UseHackingDamage = false; //if true, a beam hit on a grid will cause a random selection of blocks to be converted to the beam owners
				Settings.HackingMinBlocksAffected = 1; //minimum blocks affected by hacking effect
				Settings.HackingMaxBlocksAffected = 2; //maximum blocks affected by hacking effect
				
				//Painter Damage
				Settings.UsePainterDamage = false; //If true, a beam hit on a grid will recolor the block it makes contact with using the color of the laser block.
				Settings.RandomPaintColor = false; //If true, and if UsePainterDamage is true, a random color will be used on target blocks instead.
				
				//DefenseShieldMod Options
				Settings.BypassBubble = false; //If true, the beam will ignore the physical bubble of the Defense Shield mod (shield damage modifier may still apply).
				
				//Sound Settings
				Settings.FiringSoundId = "HeavyLaserFire"; //You can specify an AudioDefinition subtype ID that will play when the weapon is fired.
				
				//Beam Effect
				Settings.UseRegularBeam = true; //if true, a straight laser beam will be drawn from the weapon barrel, 
				Settings.UseBeamFlicker = true; //If true, the beam will not use BeamRadius, but rather random values between BeamMinimumRadius and BeamMaximumRadius
				Settings.BeamRadius = 0.5f; //The beam radius if UseBeamFlicker is false
				Settings.BeamMinimumRadius = 1.5f; //Minimum Random Beam Radius if UseBeamFlicker is true
				Settings.BeamMaximumRadius = 1.75f; //Maximum Random Beam Radius if UseBeamFlicker is true
				Settings.BeamColors.Add(Color.Indigo);   //The color of the bolt. Copy this line to use other colors in the bolt.
				Settings.FadeThroughColors = false; //If true, beam color will not be randomized. The color will fade from one color to the next in the list you provide (requires at least 2 colors to use)
				
				//Tesla Effect
				Settings.UseTeslaBeam = false; //If true, an electric bolt effect will be fired from the barrels of the weapon.
				Settings.UseTeslaBeamFlicker = false; //If true, the beam will not use TeslaBeamRadius, but rather random values between TeslaBeamMinimumRadius and TeslaBeamMaximumRadius
				Settings.TeslaBeamMaxLateral = 0.3f; //The max lateral distance of the bolt effect
				Settings.TeslaBeamMinStep = 3; //Minimum distance of bolt arc forward
				Settings.TeslaBeamMaxStep = 7; //Maximum distance of bolt arc forward
				Settings.TeslaBeamRadius = 0.2f; //Radius of bolt beam if UseTeslaBeamFlicker is false
				Settings.TeslaBeamMinimumRadius = 0.3f; //Minimum Random Beam Radius if UseTeslaBeamFlicker is true
				Settings.TeslaBeamMaximumRadius = 0.6f; //Maximum Random Beam Radius if UseTeslaBeamFlicker is true
				Settings.TeslaBeamColors.Add(Color.Cyan); //The color of the bolt. Copy this line to use other colors in the bolt.
				
				//Particle Barrel Settings
				Settings.UseBarrelParticles = false; //If true, a particle is created at the barrel position when fired.
				Settings.BarrelParticleName = "Warp"; //ID of the barrel particle ID
				Settings.BarrelParticleScale = 1f; //Size multiplier of the barrel particle
				Settings.BarrelParticleColor = new Vector4(0,1,1,1); //Color of the barrel particle.
				Settings.LoopBarrelAfterTicks = 20; //After this many ticks, the barrel particle animation resets
				
				//Particle Hit Settings
				Settings.UseHitParticles = false; //if true, a particle will be created when the beam hits a target.
				Settings.UseParticleAfterRayCount = 5; //Particle is created after this many raycasts - this helps reduce particle spam and increases performance.
				Settings.ParticleName = "Grid_Destruction"; //SubtypeId of the particle you want to display
				Settings.ParticleColor = new Vector4(0,1,1,1); //RBGA to change the particle color. Range from 0-1 (if using a floating point value, add f as suffix - eg: 0.5f). Use 0,0,0,0 for default
				Settings.ParticleScale = 1; //Size multiplier of particles created.
				Settings.UseHitParticleMaxDuration = false; //If true, particle will only play up until time specified below.
				Settings.HitParticleMaxDuration = 0.3f; //Time until particle stops playing (in seconds // 1 is 1 second)
				
				//DefenseShieldMod Options
				Settings.BypassBubble = false; //If true, the beam will ignore the physical bubble of the Defense Shield mod (shield damage modifier may still apply).
				
				//Do Not Edit This Line
				return Settings;
				
			}
			
			if(subtypeName == "SmallFixedAmplifiedLaser"){
				
				//Do Not Edit This Line
				var Settings = new WeaponConfig();
				
				//Weapon Subtype
				Settings.WeaponSubtypeId = "SmallFixedAmplifiedLaser"; //Change this to the same weapon subtype as used above in the script setup
				Settings.UseScriptedFire = true; //Change this to false if you only want to use the ammo generating feature of the weapon.
				//Incomplete Feature: Settings.RegisterDamageHandlerForTracers = false; //Set to true if you plan to use non-beam vanilla tracers with this weapon.
				
				//Regenerative Ammo Settings
				Settings.UseRegenerativeAmmo = true; //If set to 'true', then this weapon will consume grid energy and generate ammo automatically.
				Settings.AmmoMagazineSubtypeId = "PDLaserCharge"; //The AmmoMagazine SubtypeId this weapon uses.
				Settings.AmmoRegenerationMaxPowerDraw = 10; //Maximum amount of power the weapon should draw to generate ammo.
				Settings.AmmoRegenerationMedPowerDraw = 10; //If Maximum amount of power draw is unavailable, then this amount is drawn instead.
				Settings.AmmoRegenerationTime = 10; //Time until ammo is generated (at rate of 1MW per second).
				Settings.AmmoAmountToAdd = 20; //Number of ammo magazines added when a charge is complete.
				Settings.MaxAmmoInInventory = 20; //If ammo in weapon meets or exceeds this number, ammo regeneration will stop.
				Settings.AmmoRegenerationFreeForNPC = true; //If true and the block is owned by a valid NPC identity, the weapon will not draw energy to generate ammo, but will still create the ammo as if charging at AmmoRegenerationMaxPowerDraw rate.
				Settings.MustBeWorkingAtMaxDraw = false; //If true, weapon must be on, undamaged, and charging at max. If any of the consitions are not met, charge is reset to 0 and ammo is removed.
				
				//Pre-Fire Settings
				Settings.UsePreFireDelay = false; //If true, the weapon will have a delayed fire when the weapon is shot.
				Settings.PreHitTimerLimit = 20; //How long (in game ticks) the pre-fire phase should last
				Settings.PreFireSoundId = ""; //Here you can specify a sound that will play during the pre-fire
				Settings.PreFireEmissiveCharge = false; //If true, the Firing Emissive will fade from EmissiveFiringOffColor to EmissiveFiringColor before the weapon fires
				
				//Damage / Hit Timer Settings
				Settings.TickTimerLimit = 5; //Total Time (in game ticks) the beam is active
				Settings.DamageTimerLimit = 1; //Damage is applied at this game tick interval.
				
				//Distance Settings
				Settings.WeaponDistance = 2000; //Beam Distance
				Settings.SafeRange = 10; //If Beam Hits Own Grid, If Distance From Barrel to Hit is less than this value, it will be ignored.
				
				//Emissives - Off/Disabled/Damaged
			Settings.EmissiveInactiveName = "Emissive"; //Emissive Material Name
			Settings.EmissiveInactiveAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveInactiveColor = new Color(0,0,0,0); //RGBA value of Emissive
			
			//Emissives - Idle
			Settings.EmissiveIdleName = "Emissive"; //Emissive Material Name
			Settings.EmissiveIdleAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveIdleColor = new Color(0,20,255,175); //RGBA value of Emissive
			
			//Emissives - Idle
			Settings.EmissiveIdleName = "Emissive2"; //Emissive Material Name
			Settings.EmissiveIdleAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveIdleColor = new Color(255,1,0,255); //RGBA value of Emissive
			
			//Emissives - Charging
			Settings.EmissiveChargingName = "Emissive"; //Emissive Material Name
			Settings.EmissiveChargingAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveChargingColor = new Color(0,20,255,255); //RGBA value of Emissive
			
			//Emissives - Charging
			Settings.EmissiveChargingName = "Emissive2"; //Emissive Material Name
			Settings.EmissiveChargingAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveChargingColor = new Color(255,1,0,255); //RGBA value of Emissive
			
			//Emissives - Firing
			Settings.EmissiveFiringName = "Emissive"; //Emissive Material Name
			Settings.EmissiveFiringAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveFiringColor = new Color(0,20,255,255); //RGBA value of Emissive when Firing
			Settings.EmissiveFiringOffColor = new Color(0,20,255,255); //RGBA value of Emissive when Not Firing
			
			//Multibeam Settings
			Settings.BarrelSubpartOffsets.Add(new Vector3D(0,0,-1.5f)); //Copy This Line and Provide the XYZ offset of any additional barrels that will fire beams on your weapons. Default offset of 0,0,0 can be changed if needed.
			
			//Upgrade Valid Names
				/*
				Please note that your upgrade definitions attached to your upgrade blocks should only
				ever use <ModifierType>Additive</ModifierType>
				
				Upgrades do not set a new level for the modifier its affecting, but increases or decreases by the value you've provided.
				*/
				Settings.AllowUpgrades = false; //If true, this block will be able to accept upgrade modules.
				Settings.UpgradeDamageName = "ChangeToValidUpgradeName"; //The upgrade name for Damage and Explosion Damage. Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradePowerName = "ChangeToValidUpgradeName"; //The upgrade name for Power Draw (assuming ammo regeneration is enabled). Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradePowerStoreName = "ChangeToValidUpgradeName"; //The upgrade name for the Charged Power trigger that generates a round of ammo. Increase/decrease by a regular number (eg: 50, 100, -25, etc)
				Settings.UpgradeRangeName = "ChangeToValidUpgradeName"; //The upgrade name for Weapon Range. Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradeTeslaEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Tesla Effect. Increase/decrease by a regular number (eg: 1, 2, -1, etc)
				Settings.UpgradeJumpEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Jump Drive Inhibitor Effect. Increase/decrease by a floating point number (eg: 0.1, 0.2, -0.1). Amount reduced is in MW.
				Settings.UpgradeHackEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Hacking Effect. Increase/decrease by a regular number (eg: 1, 2, -1, etc)
				Settings.UpgradeShieldEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Shield Buster Effect. Set to 1 to Enable on Attached Weapon.
				
				//Settings.UpgradeTractorEffectName = "ChangeToValidUpgradeName"; This isn't a thing yet ;)
				
				//Base Damage
				Settings.UseBaseDamage = true; //Specifies if beam should deal regular damage.
				Settings.BaseDamageAmount = 20; //Damage amount per step (steps defined by DamageTimerLimit setting above)
				Settings.UsePenetrativeDamage = false; //If true, the beam will damage multiple blocks within a grid per step.
				Settings.PenetrativeDistance = 5; //Distance the penetrative damage can reach if enabled.
				Settings.RelaxedMissileIntercept = true; //If true, shots fired at lasers that come close to hitting will still register as a hit.
				
				//Explosive Damage
				Settings.UseExplosionDamage = false; //If true, beam will create an explosion each step
				Settings.ExplosionDamage = 400; //Explosion damage
				Settings.ExplosionRadius = 1; //Explosion radius from where beam hits
				
				//Voxel Damage
				Settings.UseVoxelDamage = false; //If true, the beam will cut out voxels at hit position each step.
				Settings.VoxelDamageRadius = 3; //Radius of voxels that are removed at beam hit position.
				
				//Voxel Paint
				Settings.UseVoxelPaint = false; //If true, the beam will paint voxels at hit position each step.
				Settings.VoxelPaintMaterial = "Ice_01"; //Material affected voxels will be replaced with.
				Settings.VoxelPaintRadius = 8; //Radius of voxels that are painted at beam hit position.
				
				//Voxel Add - Feature Not Complete
				Settings.UseVoxelAdd = false; //If true, the beam will add voxels at hit position each step.
				Settings.VoxelAddMaterial = "Ice_01"; //Material added voxels will use.
				Settings.VoxelAddRadius = 3; //Radius of voxels that are added at beam hit position.
				
				//Tesla Damage
				Settings.UseTeslaEffect = false; //If true, a beam hit on a grid will shut off a selection of random blocks.
				Settings.TeslaMaxBlocksAffected = 1; //maximum blocks affected by tesla effect
				Settings.TeslaOnlyAffectBlockAtHitLocation = false; //If true, only the block hit directly by the beam will be affected.
				Settings.TeslaRegisterWeaponForProjectileUse = false; //if true, and if Settings.RegisterDamageHandlerForTracers is true, this weapon will apply tesla damage via projectile/tracer hits.
				
				//Jump Drive Damage
				Settings.UseJumpDriveInhibitor = false; //If true, a beam hit on a grid will drain stored energy on Jump Drives
				Settings.AmountToReduceDrives = 0.3f; //Amount of energy to reduce from Jump Drives (in MW).
				Settings.SplitAcrossEachDrive = true; //If true, the amount to reduce is evenly split across all jump drives on the grid, otherwise the amount is reduced per drive.
				
				//Shield Damage
				Settings.UseShieldBuster = false; //if true, a beam hit on a grid will immediately shutdown and damage all shielding blocks.
				
				//Hacking Damage
				Settings.UseHackingDamage = false; //if true, a beam hit on a grid will cause a random selection of blocks to be converted to the beam owners
				Settings.HackingMinBlocksAffected = 1; //minimum blocks affected by hacking effect
				Settings.HackingMaxBlocksAffected = 2; //maximum blocks affected by hacking effect
				
				//Painter Damage
				Settings.UsePainterDamage = false; //If true, a beam hit on a grid will recolor the block it makes contact with using the color of the laser block.
				Settings.RandomPaintColor = false; //If true, and if UsePainterDamage is true, a random color will be used on target blocks instead.
				
				//DefenseShieldMod Options
				Settings.BypassBubble = false; //If true, the beam will ignore the physical bubble of the Defense Shield mod (shield damage modifier may still apply).
				
				//Sound Settings
				Settings.FiringSoundId = "HeavyLaserFire"; //You can specify an AudioDefinition subtype ID that will play when the weapon is fired.
				
				//Beam Effect
				Settings.UseRegularBeam = true; //if true, a straight laser beam will be drawn from the weapon barrel, 
				Settings.UseBeamFlicker = true; //If true, the beam will not use BeamRadius, but rather random values between BeamMinimumRadius and BeamMaximumRadius
				Settings.BeamRadius = 0.5f; //The beam radius if UseBeamFlicker is false
				Settings.BeamMinimumRadius = 0.25f; //Minimum Random Beam Radius if UseBeamFlicker is true
				Settings.BeamMaximumRadius = 0.35f; //Maximum Random Beam Radius if UseBeamFlicker is true
				Settings.BeamColors.Add(Color.Indigo);   //The color of the bolt. Copy this line to use other colors in the bolt.
				Settings.FadeThroughColors = false; //If true, beam color will not be randomized. The color will fade from one color to the next in the list you provide (requires at least 2 colors to use)
				
				//Tesla Effect
				Settings.UseTeslaBeam = false; //If true, an electric bolt effect will be fired from the barrels of the weapon.
				Settings.UseTeslaBeamFlicker = false; //If true, the beam will not use TeslaBeamRadius, but rather random values between TeslaBeamMinimumRadius and TeslaBeamMaximumRadius
				Settings.TeslaBeamMaxLateral = 0.3f; //The max lateral distance of the bolt effect
				Settings.TeslaBeamMinStep = 3; //Minimum distance of bolt arc forward
				Settings.TeslaBeamMaxStep = 7; //Maximum distance of bolt arc forward
				Settings.TeslaBeamRadius = 0.2f; //Radius of bolt beam if UseTeslaBeamFlicker is false
				Settings.TeslaBeamMinimumRadius = 0.3f; //Minimum Random Beam Radius if UseTeslaBeamFlicker is true
				Settings.TeslaBeamMaximumRadius = 0.6f; //Maximum Random Beam Radius if UseTeslaBeamFlicker is true
				Settings.TeslaBeamColors.Add(Color.Cyan); //The color of the bolt. Copy this line to use other colors in the bolt.
				
				//Particle Barrel Settings
				Settings.UseBarrelParticles = false; //If true, a particle is created at the barrel position when fired.
				Settings.BarrelParticleName = "Warp"; //ID of the barrel particle ID
				Settings.BarrelParticleScale = 1f; //Size multiplier of the barrel particle
				Settings.BarrelParticleColor = new Vector4(0,1,1,1); //Color of the barrel particle.
				Settings.LoopBarrelAfterTicks = 20; //After this many ticks, the barrel particle animation resets
				
				//Particle Hit Settings
				Settings.UseHitParticles = false; //if true, a particle will be created when the beam hits a target.
				Settings.UseParticleAfterRayCount = 5; //Particle is created after this many raycasts - this helps reduce particle spam and increases performance.
				Settings.ParticleName = "Grid_Destruction"; //SubtypeId of the particle you want to display
				Settings.ParticleColor = new Vector4(0,1,1,1); //RBGA to change the particle color. Range from 0-1 (if using a floating point value, add f as suffix - eg: 0.5f). Use 0,0,0,0 for default
				Settings.ParticleScale = 1; //Size multiplier of particles created.
				Settings.UseHitParticleMaxDuration = false; //If true, particle will only play up until time specified below.
				Settings.HitParticleMaxDuration = 0.3f; //Time until particle stops playing (in seconds // 1 is 1 second)
				
				//DefenseShieldMod Options
				Settings.BypassBubble = false; //If true, the beam will ignore the physical bubble of the Defense Shield mod (shield damage modifier may still apply).
				
				//Do Not Edit This Line
				return Settings;
				
			}
			
			if(subtypeName == "LargeAmplifiedLaserTurret"){
				
				//Do Not Edit This Line
				var Settings = new WeaponConfig();
				
				//Weapon Subtype
				Settings.WeaponSubtypeId = "LargeAmplifiedLaserTurret"; //Change this to the same weapon subtype as used above in the script setup
				Settings.UseScriptedFire = true; //Change this to false if you only want to use the ammo generating feature of the weapon.
				//Incomplete Feature: Settings.RegisterDamageHandlerForTracers = false; //Set to true if you plan to use non-beam vanilla tracers with this weapon.
				
				//Regenerative Ammo Settings
				Settings.UseRegenerativeAmmo = true; //If set to 'true', then this weapon will consume grid energy and generate ammo automatically.
				Settings.AmmoMagazineSubtypeId = "PDLaserCharge"; //The AmmoMagazine SubtypeId this weapon uses.
				Settings.AmmoRegenerationMaxPowerDraw = 60; //Maximum amount of power the weapon should draw to generate ammo.
				Settings.AmmoRegenerationMedPowerDraw = 60; //If Maximum amount of power draw is unavailable, then this amount is drawn instead.
				Settings.AmmoRegenerationTime = 60; //Time until ammo is generated (at rate of 1MW per second).
				Settings.AmmoAmountToAdd = 20; //Number of ammo magazines added when a charge is complete.
				Settings.MaxAmmoInInventory = 20; //If ammo in weapon meets or exceeds this number, ammo regeneration will stop.
				Settings.AmmoRegenerationFreeForNPC = true; //If true and the block is owned by a valid NPC identity, the weapon will not draw energy to generate ammo, but will still create the ammo as if charging at AmmoRegenerationMaxPowerDraw rate.
				Settings.MustBeWorkingAtMaxDraw = false; //If true, weapon must be on, undamaged, and charging at max. If any of the consitions are not met, charge is reset to 0 and ammo is removed.
				
				//Pre-Fire Settings
				Settings.UsePreFireDelay = false; //If true, the weapon will have a delayed fire when the weapon is shot.
				Settings.PreHitTimerLimit = 20; //How long (in game ticks) the pre-fire phase should last
				Settings.PreFireSoundId = ""; //Here you can specify a sound that will play during the pre-fire
				Settings.PreFireEmissiveCharge = false; //If true, the Firing Emissive will fade from EmissiveFiringOffColor to EmissiveFiringColor before the weapon fires
				
				//Damage / Hit Timer Settings
				Settings.TickTimerLimit = 5; //Total Time (in game ticks) the beam is active
				Settings.DamageTimerLimit = 1; //Damage is applied at this game tick interval.
				
				//Distance Settings
				Settings.WeaponDistance = 3500; //Beam Distance
				Settings.SafeRange = 10; //If Beam Hits Own Grid, If Distance From Barrel to Hit is less than this value, it will be ignored.
				
				//Emissives - Off/Disabled/Damaged
			Settings.EmissiveInactiveName = "Emissive"; //Emissive Material Name
			Settings.EmissiveInactiveAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveInactiveColor = new Color(0,0,0,0); //RGBA value of Emissive
			
			//Emissives - Idle
			Settings.EmissiveIdleName = "Emissive"; //Emissive Material Name
			Settings.EmissiveIdleAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveIdleColor = new Color(0,20,255,175); //RGBA value of Emissive
			
			//Emissives - Idle
			Settings.EmissiveIdleName = "Emissive2"; //Emissive Material Name
			Settings.EmissiveIdleAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveIdleColor = new Color(255,1,0,255); //RGBA value of Emissive
			
			//Emissives - Charging
			Settings.EmissiveChargingName = "Emissive"; //Emissive Material Name
			Settings.EmissiveChargingAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveChargingColor = new Color(0,20,255,255); //RGBA value of Emissive
			
			//Emissives - Charging
			Settings.EmissiveChargingName = "Emissive2"; //Emissive Material Name
			Settings.EmissiveChargingAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveChargingColor = new Color(255,1,0,255); //RGBA value of Emissive
			
			//Emissives - Firing
			Settings.EmissiveFiringName = "Emissive"; //Emissive Material Name
			Settings.EmissiveFiringAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveFiringColor = new Color(0,20,255,255); //RGBA value of Emissive when Firing
			Settings.EmissiveFiringOffColor = new Color(0,20,255,255); //RGBA value of Emissive when Not Firing
			
			//Multibeam Settings
			Settings.BarrelSubpartOffsets.Add(new Vector3D(2.37,0,-8.5f));
			Settings.BarrelSubpartOffsets.Add(new Vector3D(-2.37,0,-8.5f)); //Copy This Line and Provide the XYZ offset of any additional barrels that will fire beams on your weapons. Default offset of 0,0,0 can be changed if needed.
			
			//Upgrade Valid Names
				/*
				Please note that your upgrade definitions attached to your upgrade blocks should only
				ever use <ModifierType>Additive</ModifierType>
				
				Upgrades do not set a new level for the modifier its affecting, but increases or decreases by the value you've provided.
				*/
				Settings.AllowUpgrades = false; //If true, this block will be able to accept upgrade modules.
				Settings.UpgradeDamageName = "ChangeToValidUpgradeName"; //The upgrade name for Damage and Explosion Damage. Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradePowerName = "ChangeToValidUpgradeName"; //The upgrade name for Power Draw (assuming ammo regeneration is enabled). Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradePowerStoreName = "ChangeToValidUpgradeName"; //The upgrade name for the Charged Power trigger that generates a round of ammo. Increase/decrease by a regular number (eg: 50, 100, -25, etc)
				Settings.UpgradeRangeName = "ChangeToValidUpgradeName"; //The upgrade name for Weapon Range. Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradeTeslaEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Tesla Effect. Increase/decrease by a regular number (eg: 1, 2, -1, etc)
				Settings.UpgradeJumpEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Jump Drive Inhibitor Effect. Increase/decrease by a floating point number (eg: 0.1, 0.2, -0.1). Amount reduced is in MW.
				Settings.UpgradeHackEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Hacking Effect. Increase/decrease by a regular number (eg: 1, 2, -1, etc)
				Settings.UpgradeShieldEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Shield Buster Effect. Set to 1 to Enable on Attached Weapon.
				
				//Settings.UpgradeTractorEffectName = "ChangeToValidUpgradeName"; This isn't a thing yet ;)
				
				//Base Damage
				Settings.UseBaseDamage = true; //Specifies if beam should deal regular damage.
				Settings.BaseDamageAmount = 45; //Damage amount per step (steps defined by DamageTimerLimit setting above)
				Settings.UsePenetrativeDamage = false; //If true, the beam will damage multiple blocks within a grid per step.
				Settings.PenetrativeDistance = 5; //Distance the penetrative damage can reach if enabled.
				Settings.RelaxedMissileIntercept = true; //If true, shots fired at lasers that come close to hitting will still register as a hit.
				
				//Explosive Damage
				Settings.UseExplosionDamage = false; //If true, beam will create an explosion each step
				Settings.ExplosionDamage = 400; //Explosion damage
				Settings.ExplosionRadius = 1; //Explosion radius from where beam hits
				
				//Voxel Damage
				Settings.UseVoxelDamage = false; //If true, the beam will cut out voxels at hit position each step.
				Settings.VoxelDamageRadius = 3; //Radius of voxels that are removed at beam hit position.
				
				//Voxel Paint
				Settings.UseVoxelPaint = false; //If true, the beam will paint voxels at hit position each step.
				Settings.VoxelPaintMaterial = "Ice_01"; //Material affected voxels will be replaced with.
				Settings.VoxelPaintRadius = 8; //Radius of voxels that are painted at beam hit position.
				
				//Voxel Add - Feature Not Complete
				Settings.UseVoxelAdd = false; //If true, the beam will add voxels at hit position each step.
				Settings.VoxelAddMaterial = "Ice_01"; //Material added voxels will use.
				Settings.VoxelAddRadius = 3; //Radius of voxels that are added at beam hit position.
				
				//Tesla Damage
				Settings.UseTeslaEffect = false; //If true, a beam hit on a grid will shut off a selection of random blocks.
				Settings.TeslaMaxBlocksAffected = 1; //maximum blocks affected by tesla effect
				Settings.TeslaOnlyAffectBlockAtHitLocation = false; //If true, only the block hit directly by the beam will be affected.
				Settings.TeslaRegisterWeaponForProjectileUse = false; //if true, and if Settings.RegisterDamageHandlerForTracers is true, this weapon will apply tesla damage via projectile/tracer hits.
				
				//Jump Drive Damage
				Settings.UseJumpDriveInhibitor = false; //If true, a beam hit on a grid will drain stored energy on Jump Drives
				Settings.AmountToReduceDrives = 0.3f; //Amount of energy to reduce from Jump Drives (in MW).
				Settings.SplitAcrossEachDrive = true; //If true, the amount to reduce is evenly split across all jump drives on the grid, otherwise the amount is reduced per drive.
				
				//Shield Damage
				Settings.UseShieldBuster = false; //if true, a beam hit on a grid will immediately shutdown and damage all shielding blocks.
				
				//Hacking Damage
				Settings.UseHackingDamage = false; //if true, a beam hit on a grid will cause a random selection of blocks to be converted to the beam owners
				Settings.HackingMinBlocksAffected = 1; //minimum blocks affected by hacking effect
				Settings.HackingMaxBlocksAffected = 2; //maximum blocks affected by hacking effect
				
				//Painter Damage
				Settings.UsePainterDamage = false; //If true, a beam hit on a grid will recolor the block it makes contact with using the color of the laser block.
				Settings.RandomPaintColor = false; //If true, and if UsePainterDamage is true, a random color will be used on target blocks instead.
				
				//DefenseShieldMod Options
				Settings.BypassBubble = false; //If true, the beam will ignore the physical bubble of the Defense Shield mod (shield damage modifier may still apply).
				
				//Sound Settings
				Settings.FiringSoundId = "HeavyLaserFire"; //You can specify an AudioDefinition subtype ID that will play when the weapon is fired.
				
				//Beam Effect
				Settings.UseRegularBeam = true; //if true, a straight laser beam will be drawn from the weapon barrel, 
				Settings.UseBeamFlicker = true; //If true, the beam will not use BeamRadius, but rather random values between BeamMinimumRadius and BeamMaximumRadius
				Settings.BeamRadius = 0.5f; //The beam radius if UseBeamFlicker is false
				Settings.BeamMinimumRadius = 1f; //Minimum Random Beam Radius if UseBeamFlicker is true
				Settings.BeamMaximumRadius = 1.2f; //Maximum Random Beam Radius if UseBeamFlicker is true
				Settings.BeamColors.Add(Color.Indigo);   //The color of the bolt. Copy this line to use other colors in the bolt.
				Settings.FadeThroughColors = false; //If true, beam color will not be randomized. The color will fade from one color to the next in the list you provide (requires at least 2 colors to use)
				
				//Tesla Effect
				Settings.UseTeslaBeam = false; //If true, an electric bolt effect will be fired from the barrels of the weapon.
				Settings.UseTeslaBeamFlicker = false; //If true, the beam will not use TeslaBeamRadius, but rather random values between TeslaBeamMinimumRadius and TeslaBeamMaximumRadius
				Settings.TeslaBeamMaxLateral = 0.3f; //The max lateral distance of the bolt effect
				Settings.TeslaBeamMinStep = 3; //Minimum distance of bolt arc forward
				Settings.TeslaBeamMaxStep = 7; //Maximum distance of bolt arc forward
				Settings.TeslaBeamRadius = 0.2f; //Radius of bolt beam if UseTeslaBeamFlicker is false
				Settings.TeslaBeamMinimumRadius = 0.3f; //Minimum Random Beam Radius if UseTeslaBeamFlicker is true
				Settings.TeslaBeamMaximumRadius = 0.6f; //Maximum Random Beam Radius if UseTeslaBeamFlicker is true
				Settings.TeslaBeamColors.Add(Color.Cyan); //The color of the bolt. Copy this line to use other colors in the bolt.
				
				//Particle Barrel Settings
				Settings.UseBarrelParticles = false; //If true, a particle is created at the barrel position when fired.
				Settings.BarrelParticleName = "Warp"; //ID of the barrel particle ID
				Settings.BarrelParticleScale = 1f; //Size multiplier of the barrel particle
				Settings.BarrelParticleColor = new Vector4(0,1,1,1); //Color of the barrel particle.
				Settings.LoopBarrelAfterTicks = 20; //After this many ticks, the barrel particle animation resets
				
				//Particle Hit Settings
				Settings.UseHitParticles = false; //if true, a particle will be created when the beam hits a target.
				Settings.UseParticleAfterRayCount = 5; //Particle is created after this many raycasts - this helps reduce particle spam and increases performance.
				Settings.ParticleName = "Grid_Destruction"; //SubtypeId of the particle you want to display
				Settings.ParticleColor = new Vector4(0,1,1,1); //RBGA to change the particle color. Range from 0-1 (if using a floating point value, add f as suffix - eg: 0.5f). Use 0,0,0,0 for default
				Settings.ParticleScale = 1; //Size multiplier of particles created.
				Settings.UseHitParticleMaxDuration = false; //If true, particle will only play up until time specified below.
				Settings.HitParticleMaxDuration = 0.3f; //Time until particle stops playing (in seconds // 1 is 1 second)
				
				//DefenseShieldMod Options
				Settings.BypassBubble = false; //If true, the beam will ignore the physical bubble of the Defense Shield mod (shield damage modifier may still apply).
				
				//Do Not Edit This Line
				return Settings;
				
			}
			
			if(subtypeName == "SmallAmplifiedLaserTurret"){
				
				//Do Not Edit This Line
				var Settings = new WeaponConfig();
				
				//Weapon Subtype
				Settings.WeaponSubtypeId = "SmallAmplifiedLaserTurret"; //Change this to the same weapon subtype as used above in the script setup
				Settings.UseScriptedFire = true; //Change this to false if you only want to use the ammo generating feature of the weapon.
				//Incomplete Feature: Settings.RegisterDamageHandlerForTracers = false; //Set to true if you plan to use non-beam vanilla tracers with this weapon.
				
				//Regenerative Ammo Settings
				Settings.UseRegenerativeAmmo = true; //If set to 'true', then this weapon will consume grid energy and generate ammo automatically.
				Settings.AmmoMagazineSubtypeId = "PDLaserCharge"; //The AmmoMagazine SubtypeId this weapon uses.
				Settings.AmmoRegenerationMaxPowerDraw = 10; //Maximum amount of power the weapon should draw to generate ammo.
				Settings.AmmoRegenerationMedPowerDraw = 10; //If Maximum amount of power draw is unavailable, then this amount is drawn instead.
				Settings.AmmoRegenerationTime = 10; //Time until ammo is generated (at rate of 1MW per second).
				Settings.AmmoAmountToAdd = 20; //Number of ammo magazines added when a charge is complete.
				Settings.MaxAmmoInInventory = 20; //If ammo in weapon meets or exceeds this number, ammo regeneration will stop.
				Settings.AmmoRegenerationFreeForNPC = true; //If true and the block is owned by a valid NPC identity, the weapon will not draw energy to generate ammo, but will still create the ammo as if charging at AmmoRegenerationMaxPowerDraw rate.
				Settings.MustBeWorkingAtMaxDraw = false; //If true, weapon must be on, undamaged, and charging at max. If any of the consitions are not met, charge is reset to 0 and ammo is removed.
				
				//Pre-Fire Settings
				Settings.UsePreFireDelay = false; //If true, the weapon will have a delayed fire when the weapon is shot.
				Settings.PreHitTimerLimit = 20; //How long (in game ticks) the pre-fire phase should last
				Settings.PreFireSoundId = ""; //Here you can specify a sound that will play during the pre-fire
				Settings.PreFireEmissiveCharge = false; //If true, the Firing Emissive will fade from EmissiveFiringOffColor to EmissiveFiringColor before the weapon fires
				
				//Damage / Hit Timer Settings
				Settings.TickTimerLimit = 5; //Total Time (in game ticks) the beam is active
				Settings.DamageTimerLimit = 1; //Damage is applied at this game tick interval.
				
				//Distance Settings
				Settings.WeaponDistance = 1500; //Beam Distance
				Settings.SafeRange = 10; //If Beam Hits Own Grid, If Distance From Barrel to Hit is less than this value, it will be ignored.
				
				//Emissives - Off/Disabled/Damaged
			Settings.EmissiveInactiveName = "Emissive"; //Emissive Material Name
			Settings.EmissiveInactiveAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveInactiveColor = new Color(0,0,0,0); //RGBA value of Emissive
			
			//Emissives - Idle
			Settings.EmissiveIdleName = "Emissive"; //Emissive Material Name
			Settings.EmissiveIdleAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveIdleColor = new Color(0,20,255,175); //RGBA value of Emissive
			
			//Emissives - Idle
			Settings.EmissiveIdleName = "Emissive2"; //Emissive Material Name
			Settings.EmissiveIdleAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveIdleColor = new Color(255,1,0,255); //RGBA value of Emissive
			
			//Emissives - Charging
			Settings.EmissiveChargingName = "Emissive"; //Emissive Material Name
			Settings.EmissiveChargingAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveChargingColor = new Color(0,20,255,255); //RGBA value of Emissive
			
			//Emissives - Charging
			Settings.EmissiveChargingName = "Emissive2"; //Emissive Material Name
			Settings.EmissiveChargingAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveChargingColor = new Color(255,1,0,255); //RGBA value of Emissive
			
			//Emissives - Firing
			Settings.EmissiveFiringName = "Emissive"; //Emissive Material Name
			Settings.EmissiveFiringAmount = 1; //Emissive Amount - can be between 0-1 (non integers should be formatted with 'f' at the end. eg: 0.5f)
			Settings.EmissiveFiringColor = new Color(0,20,255,255); //RGBA value of Emissive when Firing
			Settings.EmissiveFiringOffColor = new Color(0,20,255,255); //RGBA value of Emissive when Not Firing
			
			//Multibeam Settings
			Settings.BarrelSubpartOffsets.Add(new Vector3D(0.474f,0,-1.5f)); 
			Settings.BarrelSubpartOffsets.Add(new Vector3D(-0.474f,0,-1.5f));//Copy This Line and Provide the XYZ offset of any additional barrels that will fire beams on your weapons. Default offset of 0,0,0 can be changed if needed.
			
			//Upgrade Valid Names
				/*
				Please note that your upgrade definitions attached to your upgrade blocks should only
				ever use <ModifierType>Additive</ModifierType>
				
				Upgrades do not set a new level for the modifier its affecting, but increases or decreases by the value you've provided.
				*/
				Settings.AllowUpgrades = false; //If true, this block will be able to accept upgrade modules.
				Settings.UpgradeDamageName = "ChangeToValidUpgradeName"; //The upgrade name for Damage and Explosion Damage. Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradePowerName = "ChangeToValidUpgradeName"; //The upgrade name for Power Draw (assuming ammo regeneration is enabled). Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradePowerStoreName = "ChangeToValidUpgradeName"; //The upgrade name for the Charged Power trigger that generates a round of ammo. Increase/decrease by a regular number (eg: 50, 100, -25, etc)
				Settings.UpgradeRangeName = "ChangeToValidUpgradeName"; //The upgrade name for Weapon Range. Increase/decrease by a percentage (eg: 25% would be 0.25 or -0.25)
				Settings.UpgradeTeslaEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Tesla Effect. Increase/decrease by a regular number (eg: 1, 2, -1, etc)
				Settings.UpgradeJumpEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Jump Drive Inhibitor Effect. Increase/decrease by a floating point number (eg: 0.1, 0.2, -0.1). Amount reduced is in MW.
				Settings.UpgradeHackEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Hacking Effect. Increase/decrease by a regular number (eg: 1, 2, -1, etc)
				Settings.UpgradeShieldEffectName = "ChangeToValidUpgradeName"; //The upgrade name for the Shield Buster Effect. Set to 1 to Enable on Attached Weapon.
				
				//Settings.UpgradeTractorEffectName = "ChangeToValidUpgradeName"; This isn't a thing yet ;)
				
				//Base Damage
				Settings.UseBaseDamage = true; //Specifies if beam should deal regular damage.
				Settings.BaseDamageAmount = 8; //Damage amount per step (steps defined by DamageTimerLimit setting above)
				Settings.UsePenetrativeDamage = false; //If true, the beam will damage multiple blocks within a grid per step.
				Settings.PenetrativeDistance = 5; //Distance the penetrative damage can reach if enabled.
				Settings.RelaxedMissileIntercept = true; //If true, shots fired at lasers that come close to hitting will still register as a hit.
				
				//Explosive Damage
				Settings.UseExplosionDamage = false; //If true, beam will create an explosion each step
				Settings.ExplosionDamage = 400; //Explosion damage
				Settings.ExplosionRadius = 1; //Explosion radius from where beam hits
				
				//Voxel Damage
				Settings.UseVoxelDamage = false; //If true, the beam will cut out voxels at hit position each step.
				Settings.VoxelDamageRadius = 3; //Radius of voxels that are removed at beam hit position.
				
				//Voxel Paint
				Settings.UseVoxelPaint = false; //If true, the beam will paint voxels at hit position each step.
				Settings.VoxelPaintMaterial = "Ice_01"; //Material affected voxels will be replaced with.
				Settings.VoxelPaintRadius = 8; //Radius of voxels that are painted at beam hit position.
				
				//Voxel Add - Feature Not Complete
				Settings.UseVoxelAdd = false; //If true, the beam will add voxels at hit position each step.
				Settings.VoxelAddMaterial = "Ice_01"; //Material added voxels will use.
				Settings.VoxelAddRadius = 3; //Radius of voxels that are added at beam hit position.
				
				//Tesla Damage
				Settings.UseTeslaEffect = false; //If true, a beam hit on a grid will shut off a selection of random blocks.
				Settings.TeslaMaxBlocksAffected = 1; //maximum blocks affected by tesla effect
				Settings.TeslaOnlyAffectBlockAtHitLocation = false; //If true, only the block hit directly by the beam will be affected.
				Settings.TeslaRegisterWeaponForProjectileUse = false; //if true, and if Settings.RegisterDamageHandlerForTracers is true, this weapon will apply tesla damage via projectile/tracer hits.
				
				//Jump Drive Damage
				Settings.UseJumpDriveInhibitor = false; //If true, a beam hit on a grid will drain stored energy on Jump Drives
				Settings.AmountToReduceDrives = 0.3f; //Amount of energy to reduce from Jump Drives (in MW).
				Settings.SplitAcrossEachDrive = true; //If true, the amount to reduce is evenly split across all jump drives on the grid, otherwise the amount is reduced per drive.
				
				//Shield Damage
				Settings.UseShieldBuster = false; //if true, a beam hit on a grid will immediately shutdown and damage all shielding blocks.
				
				//Hacking Damage
				Settings.UseHackingDamage = false; //if true, a beam hit on a grid will cause a random selection of blocks to be converted to the beam owners
				Settings.HackingMinBlocksAffected = 1; //minimum blocks affected by hacking effect
				Settings.HackingMaxBlocksAffected = 2; //maximum blocks affected by hacking effect
				
				//Painter Damage
				Settings.UsePainterDamage = false; //If true, a beam hit on a grid will recolor the block it makes contact with using the color of the laser block.
				Settings.RandomPaintColor = false; //If true, and if UsePainterDamage is true, a random color will be used on target blocks instead.
				
				//DefenseShieldMod Options
				Settings.BypassBubble = false; //If true, the beam will ignore the physical bubble of the Defense Shield mod (shield damage modifier may still apply).
				
				//Sound Settings
				Settings.FiringSoundId = "HeavyLaserFire"; //You can specify an AudioDefinition subtype ID that will play when the weapon is fired.
				
				//Beam Effect
				Settings.UseRegularBeam = true; //if true, a straight laser beam will be drawn from the weapon barrel, 
				Settings.UseBeamFlicker = true; //If true, the beam will not use BeamRadius, but rather random values between BeamMinimumRadius and BeamMaximumRadius
				Settings.BeamRadius = 0.5f; //The beam radius if UseBeamFlicker is false
				Settings.BeamMinimumRadius = 0.1f; //Minimum Random Beam Radius if UseBeamFlicker is true
				Settings.BeamMaximumRadius = 0.2f; //Maximum Random Beam Radius if UseBeamFlicker is true
				Settings.BeamColors.Add(Color.Indigo);   //The color of the bolt. Copy this line to use other colors in the bolt.
				Settings.FadeThroughColors = false; //If true, beam color will not be randomized. The color will fade from one color to the next in the list you provide (requires at least 2 colors to use)
				
				//Tesla Effect
				Settings.UseTeslaBeam = false; //If true, an electric bolt effect will be fired from the barrels of the weapon.
				Settings.UseTeslaBeamFlicker = false; //If true, the beam will not use TeslaBeamRadius, but rather random values between TeslaBeamMinimumRadius and TeslaBeamMaximumRadius
				Settings.TeslaBeamMaxLateral = 0.3f; //The max lateral distance of the bolt effect
				Settings.TeslaBeamMinStep = 3; //Minimum distance of bolt arc forward
				Settings.TeslaBeamMaxStep = 7; //Maximum distance of bolt arc forward
				Settings.TeslaBeamRadius = 0.2f; //Radius of bolt beam if UseTeslaBeamFlicker is false
				Settings.TeslaBeamMinimumRadius = 0.3f; //Minimum Random Beam Radius if UseTeslaBeamFlicker is true
				Settings.TeslaBeamMaximumRadius = 0.6f; //Maximum Random Beam Radius if UseTeslaBeamFlicker is true
				Settings.TeslaBeamColors.Add(Color.Cyan); //The color of the bolt. Copy this line to use other colors in the bolt.
				
				//Particle Barrel Settings
				Settings.UseBarrelParticles = false; //If true, a particle is created at the barrel position when fired.
				Settings.BarrelParticleName = "Warp"; //ID of the barrel particle ID
				Settings.BarrelParticleScale = 1f; //Size multiplier of the barrel particle
				Settings.BarrelParticleColor = new Vector4(0,1,1,1); //Color of the barrel particle.
				Settings.LoopBarrelAfterTicks = 20; //After this many ticks, the barrel particle animation resets
				
				//Particle Hit Settings
				Settings.UseHitParticles = false; //if true, a particle will be created when the beam hits a target.
				Settings.UseParticleAfterRayCount = 5; //Particle is created after this many raycasts - this helps reduce particle spam and increases performance.
				Settings.ParticleName = "Grid_Destruction"; //SubtypeId of the particle you want to display
				Settings.ParticleColor = new Vector4(0,1,1,1); //RBGA to change the particle color. Range from 0-1 (if using a floating point value, add f as suffix - eg: 0.5f). Use 0,0,0,0 for default
				Settings.ParticleScale = 1; //Size multiplier of particles created.
				Settings.UseHitParticleMaxDuration = false; //If true, particle will only play up until time specified below.
				Settings.HitParticleMaxDuration = 0.3f; //Time until particle stops playing (in seconds // 1 is 1 second)
				
				//DefenseShieldMod Options
				Settings.BypassBubble = false; //If true, the beam will ignore the physical bubble of the Defense Shield mod (shield damage modifier may still apply).
				
				//Do Not Edit This Line
				return Settings;
				
			}
			////////////////////////////////////////////////////////
			//////////////// DO NOT EDIT BELOW HERE ////////////////
			////////////////////////////////////////////////////////
			
			return new WeaponConfig();
			
		}
		
	}
	
}