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
	
	public class WeaponConfig{
		
		//Weapon Subtype
		public string WeaponSubtypeId {get; set;}
		public bool UseScriptedFire {get; set;}
		public bool RegisterDamageHandlerForTracers {get; set;}
		
		//Regenerative Ammo Settings
		public bool UseRegenerativeAmmo {get; set;}
		public string AmmoMagazineSubtypeId {get; set;}
		public float AmmoRegenerationMaxPowerDraw {get; set;}
		public float AmmoRegenerationMedPowerDraw {get; set;}
		public float AmmoRegenerationLowPowerDraw {get; set;}
		public float AmmoRegenerationTime {get; set;}
		public float AmmoAmountToAdd {get; set;}
		public float MaxAmmoInInventory {get; set;}
		public bool AmmoRegenerationFreeForNPC {get; set;}
		public bool MustBeWorkingAtMaxDraw {get; set;}
		
		//Pre-Fire Options
		public bool UsePreFireDelay {get; set;}
		public int PreHitTimerLimit {get; set;}
		public string PreFireSoundId {get; set;}
		public bool PreFireEmissiveCharge {get; set;}
		
		//Timer and Distance Settings
		public int TickTimerLimit {get; set;}
		public int PostHitTimerLimit {get; set;}
		public int DamageTimerLimit {get; set;}
		
		//Distance Settings
		public double WeaponDistance {get; set;}
		public double SafeRange {get; set;}
				
		//Emissives - Off/Disabled/Damaged
		public string EmissiveInactiveName {get; set;}
		public float EmissiveInactiveAmount {get; set;}
		public Color EmissiveInactiveColor {get; set;}
		
		//Emissives - Idle
		public string EmissiveIdleName {get; set;}
		public float EmissiveIdleAmount {get; set;}
		public Color EmissiveIdleColor {get; set;}
		
		//Emissives - Charging
		public string EmissiveChargingName {get; set;}
		public float EmissiveChargingAmount {get; set;}
		public Color EmissiveChargingColor {get; set;}
		
		//Emissives - Firing
		public string EmissiveFiringName {get; set;}
		public float EmissiveFiringAmount {get; set;}
		public Color EmissiveFiringColor {get; set;}
		public Color EmissiveFiringOffColor {get; set;}
		
		//Multibeam Settings
		public string TurretBarrelsSubpartName {get; set;}
		public List<Vector3D> BarrelSubpartOffsets {get; set;}
		
		//Upgrade Settings
		public bool AllowUpgrades {get; set;}
		public string UpgradeDamageName {get; set;}
		public string UpgradePowerName {get; set;}
		public string UpgradePowerStoreName {get; set;}
		public string UpgradeRangeName {get; set;}
		public string UpgradeTeslaEffectName {get; set;}
		public string UpgradeJumpEffectName {get; set;}
		public string UpgradeHackEffectName {get; set;}
		public string UpgradeTractorEffectName {get; set;}
		public string UpgradeShieldEffectName {get; set;}
		
		//Base Damage
		public bool UseBaseDamage {get; set;}
		public float BaseDamageAmount {get; set;}
		public bool UsePenetrativeDamage {get; set;}
		public double PenetrativeDistance {get; set;}
		public bool RelaxedMissileIntercept {get; set;}
		
		//Explosive Damage
		public bool UseExplosionDamage {get; set;}
		public int ExplosionDamage {get; set;}
		public float ExplosionRadius {get; set;}
		public double ExplosionForwardOffset {get; set;}

		//Multi-Charge Shot
		public bool UseMultiChargeShot { get; set; }

		//Voxel Damage
		public bool UseVoxelDamage {get; set;}
		public float VoxelDamageRadius {get; set;}
		
		//Voxel Paint
		public bool UseVoxelPaint {get; set;}
		public string VoxelPaintMaterial {get; set;}
		public float VoxelPaintRadius {get; set;}
		
		//Voxel Add
		public bool UseVoxelAdd {get; set;}
		public string VoxelAddMaterial {get; set;}
		public float VoxelAddRadius {get; set;}
			
		//Tesla Damage
		public bool UseTeslaEffect {get; set;}
		public int TeslaMinBlocksAffected {get; set;}
		public int TeslaMaxBlocksAffected {get; set;}
		public bool TeslaOnlyAffectBlockAtHitLocation {get; set;}
		public bool TeslaRegisterWeaponForProjectileUse {get; set;}
		
		//Jump Drive Damage
		public bool UseJumpDriveInhibitor {get; set;}
		public float AmountToReduceDrives {get; set;}
		public bool SplitAcrossEachDrive {get; set;}
		
		//Shield Damage
		public bool UseShieldBuster {get; set;}
		public float ShieldDamagePercentage {get; set;}
		
		//Hacking Damage
		public bool UseHackingDamage {get; set;}
		public int HackingMinBlocksAffected {get; set;}
		public int HackingMaxBlocksAffected {get; set;}
		
		//Painter Damage
		public bool UsePainterDamage {get; set;}
		public bool RandomPaintColor {get; set;}
		
		//Tractor Beam Effect
		public bool UseTractorBeamEffect {get; set;}
		public float TractorBeamForce {get; set;}
		
		//Physics Push
		public bool UsePhysicsPush {get; set;}
		public float PhysicsPushForce {get; set;}
		public bool ApplyToCenterOfMass {get; set;}
		public double ReverseForceWithinDistance {get; set;}
		
		//Speed Reduction
		public bool UseSpeedReduction {get; set;}
		public float SpeedReductionForce {get; set;}
		public float MinimumTargetSpeed {get; set;}
		
		//Clang Cannon
		public bool UseClangCannonEffect {get; set;}
		
		//Defense Shield Specific Settings
		public bool BypassBubble {get; set;}
		
		//Inventory Item
		public MyDefinitionId InventoryItemId {get; set;}
		public MyObjectBuilder_InventoryItem InventoryItem {get; set;}
		
		//Sound Settings {get; set;}
		public string FiringSoundId {get; set;}
		public string PostFiringSoundId {get; set;}
		
		//Beam Effect
		public bool UseRegularBeam {get; set;}
		public bool UseBeamFlicker {get; set;}
		public bool FadeBeamEndPoint {get; set;}
		public bool ShrinkBeamEndPoint {get; set;}
		public float BeamRadius {get; set;}
		public float BeamMinimumRadius {get; set;}
		public float BeamMaximumRadius {get; set;}
		public List<Color> BeamColors {get; set;}
		public bool FadeThroughColors {get; set;}
		
		//Tesla Effect
		public bool UseTeslaBeam {get; set;}
		public bool UseTeslaBeamFlicker {get; set;}
		public double TeslaBeamMaxLateral {get; set;}
		public double TeslaBeamMinStep {get; set;}
		public double TeslaBeamMaxStep {get; set;}
		public float TeslaBeamRadius {get; set;}
		public float TeslaBeamMinimumRadius {get; set;}
		public float TeslaBeamMaximumRadius {get; set;}
		public List<Color> TeslaBeamColors {get; set;}
		
		//Secondary Beam
		public bool UseSecondaryBeam {get; set;}
		public List<Vector3D> SecondaryBeamOffsets {get; set;}
		public bool UseSecondaryBeamFlicker {get; set;}
		public float SecondaryBeamRadius {get; set;}
		public float SecondaryBeamMinimumRadius {get; set;}
		public float SecondaryBeamMaximumRadius {get; set;}
		public List<Color> SecondaryBeamColors {get; set;}
		public bool SecondaryFadeThroughColors {get; set;}
		
		//Particle Barrel Settings
		public bool UseBarrelParticles {get; set;}
		public string BarrelParticleName {get; set;}
		public float BarrelParticleScale {get; set;}
		public Vector4 BarrelParticleColor {get; set;}
		public bool LoopBarrelParticle {get; set;}
		public int LoopBarrelAfterTicks {get; set;}
		
		public bool UseBarrelLights {get; set;}
		public Color BarrelLightColor {get; set;}
		public float BarrelLightIntensity {get; set;}
		public float BarrelLightRange {get; set;}
		public bool BarrelLightUseGlare {get; set;}
		public float BarrelLightGlareIntensity {get; set;}
		public float BarrelLightGlareMaxDistance {get; set;}
		
		//Hit Particle Effect
		public bool UseHitParticles {get; set;}
		public int UseParticleAfterRayCount {get; set;}
		public string ParticleName {get; set;}
		public Vector4 ParticleColor {get; set;}
		public float ParticleScale {get; set;}
		public bool UseHitParticleMaxDuration {get; set;}
		public float HitParticleMaxDuration {get; set;}
		
		public WeaponConfig(){
			
			WeaponSubtypeId = "YourWeaponSubtypeId";
			UseScriptedFire = true;
			RegisterDamageHandlerForTracers = false;
			
			UseRegenerativeAmmo = false;
			AmmoMagazineSubtypeId = "";
			AmmoRegenerationMaxPowerDraw = 10;
			AmmoRegenerationMedPowerDraw = 5;
			AmmoRegenerationLowPowerDraw = 1;
			AmmoRegenerationTime = 12;
			AmmoAmountToAdd = 1;
			MaxAmmoInInventory = 5;
			AmmoRegenerationFreeForNPC = true;
			MustBeWorkingAtMaxDraw = false;
			
			UsePreFireDelay = false;
			PreHitTimerLimit = 0;
			PreFireSoundId = "";
			PreFireEmissiveCharge = false;
			
			TickTimerLimit = 0;
			PostHitTimerLimit = 0;
			DamageTimerLimit = 0;
			
			WeaponDistance = 800;
			SafeRange = 2;
			
			EmissiveInactiveName = "";
			EmissiveInactiveAmount = 1;
			EmissiveInactiveColor = new Color(0,0,0,255);
			
			EmissiveIdleName = "";
			EmissiveIdleAmount = 1;
			EmissiveIdleColor = new Color(0,0,0,255);
			
			EmissiveChargingName = "";
			EmissiveChargingAmount = 1;
			EmissiveChargingColor = new Color(0,0,0,255);
			
			EmissiveFiringName = "";
			EmissiveFiringAmount = 1;
			EmissiveFiringColor = new Color(0,0,0,255);
			EmissiveFiringOffColor = new Color(0,0,0,255);

			TurretBarrelsSubpartName = "";
			BarrelSubpartOffsets = new List<Vector3D>();
			
			AllowUpgrades = false;
			UpgradeDamageName = "ChangeToValidName";
			UpgradePowerName = "ChangeToValidName";
			UpgradePowerStoreName = "ChangeToValidName";
			UpgradeRangeName = "ChangeToValidName";
			UpgradeTeslaEffectName = "ChangeToValidName";
			UpgradeJumpEffectName = "ChangeToValidName";
			UpgradeHackEffectName = "ChangeToValidName";
			UpgradeTractorEffectName = "ChangeToValidName";
			UpgradeShieldEffectName = "ChangeToValidName";
			
			UseBaseDamage = false;
			BaseDamageAmount = 0;
			UsePenetrativeDamage = false;
			PenetrativeDistance = 0;
			RelaxedMissileIntercept = false;
			
			UseExplosionDamage = false;
			ExplosionDamage = 0;
			ExplosionRadius = 0;
			ExplosionForwardOffset = 0;

			UseMultiChargeShot = false;

			UseVoxelDamage = false;
			VoxelDamageRadius = 0;
			
			UseTeslaEffect = false;
			TeslaMinBlocksAffected = 0;
			TeslaMaxBlocksAffected = 0;
			TeslaOnlyAffectBlockAtHitLocation = false;
			TeslaRegisterWeaponForProjectileUse = false;
			
			UseJumpDriveInhibitor = false;
			AmountToReduceDrives = 0;
			SplitAcrossEachDrive = false;
			
			UsePainterDamage = false;
			RandomPaintColor = false;
			
			UseShieldBuster = false;
			ShieldDamagePercentage = 25;
			
			UseHackingDamage = false;
			HackingMinBlocksAffected = 1;
			HackingMaxBlocksAffected = 2;
			
			UseTractorBeamEffect = false;
			TractorBeamForce = 100;
			
			UsePhysicsPush = false;
			PhysicsPushForce = 100;
			ApplyToCenterOfMass = false;
			ReverseForceWithinDistance = -1;
			
			UseSpeedReduction = false;
			SpeedReductionForce = 100;
			MinimumTargetSpeed = 5;
			
			UseClangCannonEffect = false;
			
			BypassBubble = false;
			
			InventoryItemId = new MyDefinitionId();
			InventoryItem = null;
			
			FiringSoundId = "";
			PostFiringSoundId = "";
			
			UseRegularBeam = false;
			UseBeamFlicker = false;
			FadeBeamEndPoint = false;
			ShrinkBeamEndPoint = false;
			BeamRadius = 0;
			BeamMinimumRadius = 0;
			BeamMaximumRadius = 0;
			BeamColors = new List<Color>();
			FadeThroughColors = false;
			
			UseTeslaBeam = false;
			UseTeslaBeamFlicker = false;
			TeslaBeamMaxLateral = 0;
			TeslaBeamMinStep = 0;
			TeslaBeamMaxStep = 0;
			TeslaBeamRadius = 0;
			TeslaBeamMinimumRadius = 0;
			TeslaBeamMaximumRadius = 0;
			TeslaBeamColors = new List<Color>();
			
			UseSecondaryBeam = false;
			SecondaryBeamOffsets = new List<Vector3D>();
			UseSecondaryBeamFlicker = false;
			SecondaryBeamRadius = 1;
			SecondaryBeamMinimumRadius = 0.5f;
			SecondaryBeamMaximumRadius = 1;
			SecondaryBeamColors = new List<Color>();
			SecondaryFadeThroughColors = false;
			
			UseBarrelParticles = false;
			BarrelParticleName = "";
			BarrelParticleScale = 1;
			BarrelParticleColor = Vector4.Zero;
			LoopBarrelParticle = false;
			LoopBarrelAfterTicks = 4;
			
			UseBarrelLights = true;
			BarrelLightColor = Color.Cyan;
			BarrelLightIntensity = 10;
			BarrelLightRange = 15;
			BarrelLightUseGlare = true;
			BarrelLightGlareIntensity = 10;
			BarrelLightGlareMaxDistance = 40;
			
			UseHitParticles = false;
			UseParticleAfterRayCount = 3;
			ParticleName = "";
			ParticleColor = new Vector4(0,0,0,0);
			ParticleScale = 1;
			UseHitParticleMaxDuration = false;
			HitParticleMaxDuration = 0;
			
		}
		
	}
	
}