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
using Sandbox.ModAPI.Interfaces.Terminal;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;

namespace HeavyDrillYield{													// Add/replace your drill subtype IDs here
	
	[MyEntityComponentDescriptor(typeof(MyObjectBuilder_Drill), false, "ReinforcedDrill")]
	 
	public class HeavyDrillYield : MyGameLogicComponent{
		
		public IMyShipDrill Drill;
		public IMyInventory Inventory;
		public bool SetupDone = false;
		public bool FirstRun = true;
		
		public float LastStoneLevel = 0;
		public float LastSulfurLevel = 0;
		public float LastIronLevel = 0;
		public float LastNickelLevel = 0;
		public float LastCobaltLevel = 0;
		public float LastSiliconLevel = 0;
		public float LastSilverLevel = 0;
		public float LastGoldLevel = 0;
		public float LastPlatinumLevel = 0;
		public float LastUraniumLevel = 0;
		public float LastCopperLevel = 0;
		public float LastLithiumLevel = 0;
		public float LastBauxiteLevel = 0;
		public float LastTitaniumLevel = 0;
		public float LastTantalumLevel = 0;
		public float LastNiterLevel = 0;
		public float LastOilSandLevel = 0;
		public float LastCoalLevel = 0;
		public float LastIceLevel = 0;
		
		public float CurrentStoneLevel = 0;
		public float CurrentSulfurLevel = 0;
		public float CurrentIronLevel = 0;
		public float CurrentNickelLevel = 0;
		public float CurrentCobaltLevel = 0;
		public float CurrentSiliconLevel = 0;
		public float CurrentSilverLevel = 0;
		public float CurrentGoldLevel = 0;
		public float CurrentPlatinumLevel = 0;
		public float CurrentUraniumLevel = 0;
		public float CurrentCopperLevel = 0;
		public float CurrentLithiumLevel = 0;
		public float CurrentBauxiteLevel = 0;
		public float CurrentTitaniumLevel = 0;
		public float CurrentTantalumLevel = 0;
		public float CurrentNiterLevel = 0;
		public float CurrentOilSandLevel = 0;
		public float CurrentCoalLevel = 0;
		public float CurrentIceLevel = 0;
		
		public float StoneMined = 0;
		public float SulfurMined = 0;
		public float IronMined = 0;
		public float NickelMined = 0;
		public float CobaltMined = 0;
		public float SiliconMined = 0;
		public float SilverMined = 0;
		public float GoldMined = 0;
		public float PlatinumMined = 0;
		public float UraniumMined = 0;
		public float CopperMined = 0;
		public float LithiumMined = 0;
		public float BauxiteMined = 0;
		public float TitaniumMined = 0;
		public float TantalumMined = 0;
		public float NiterMined = 0;
		public float OilSandMined = 0;
		public float CoalMined = 0;
		public float IceMined = 0;
		
		public float YieldMultiplier = 2f;
		
		public MyDefinitionId StoneDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Stone");
		public MyDefinitionId SulfurDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Sulfur");
		public MyDefinitionId IronDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Iron");
		public MyDefinitionId NickelDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Nickel");
		public MyDefinitionId CobaltDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Cobalt");
		public MyDefinitionId SiliconDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Silicon");
		public MyDefinitionId SilverDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Silver");
		public MyDefinitionId GoldDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Gold");
		public MyDefinitionId PlatinumDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Platinum");
		public MyDefinitionId UraniumDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Uranium");
		public MyDefinitionId CopperDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Copper");
		public MyDefinitionId LithiumDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Lithium");
		public MyDefinitionId BauxiteDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Bauxite");
		public MyDefinitionId TitaniumDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Titanium");
		public MyDefinitionId TantalumDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Tantalum");
		public MyDefinitionId NiterDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Niter");
		public MyDefinitionId OilSandDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Oil Sand");
		public MyDefinitionId CoalDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Coal");
		public MyDefinitionId IceDefId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Ice");
		
		public MyObjectBuilder_InventoryItem StoneItem;
		public MyObjectBuilder_InventoryItem SulfurItem;
		public MyObjectBuilder_InventoryItem IronItem;
		public MyObjectBuilder_InventoryItem NickelItem;
		public MyObjectBuilder_InventoryItem CobaltItem;
		public MyObjectBuilder_InventoryItem SiliconItem;
		public MyObjectBuilder_InventoryItem SilverItem;
		public MyObjectBuilder_InventoryItem GoldItem;
		public MyObjectBuilder_InventoryItem PlatinumItem;
		public MyObjectBuilder_InventoryItem UraniumItem;
		public MyObjectBuilder_InventoryItem CopperItem;
		public MyObjectBuilder_InventoryItem LithiumItem;
		public MyObjectBuilder_InventoryItem BauxiteItem;
		public MyObjectBuilder_InventoryItem TitaniumItem;
		public MyObjectBuilder_InventoryItem TantalumItem;
		public MyObjectBuilder_InventoryItem NiterItem;
		public MyObjectBuilder_InventoryItem OilSandItem;
		public MyObjectBuilder_InventoryItem CoalItem;
		public MyObjectBuilder_InventoryItem IceItem;
		
		
		public float StoneToAdd = 0;
		public float SulfurToAdd = 0;
		public float IronToAdd = 0;
		public float NickelToAdd = 0;
		public float CobaltToAdd = 0;
		public float SiliconToAdd = 0;
		public float SilverToAdd = 0;
		public float GoldToAdd = 0;
		public float PlatinumToAdd = 0;
		public float UraniumToAdd = 0;
		public float CopperToAdd = 0;
		public float LithiumToAdd = 0;
		public float BauxiteToAdd = 0;
		public float TitaniumToAdd = 0;
		public float TantalumToAdd = 0;
		public float NiterToAdd = 0;
		public float OilSandToAdd = 0;
		public float CoalToAdd = 0;
		public float IceToAdd = 0;
		
		
		public override void Init(MyObjectBuilder_EntityBase objectBuilder){
			
			base.Init(objectBuilder);
			
			try{
				
				NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				
			}catch(Exception exc){
				
				
				
			}
			
		}
		
		public override void UpdateBeforeSimulation(){
			
		}
		
		public override void UpdateBeforeSimulation10(){
			
			if(SetupDone == false){
				
				Drill = Entity as IMyShipDrill;
				Inventory = (VRage.Game.ModAPI.IMyInventory)Drill.GetInventory();
				
				StoneItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(StoneDefId)) };
				SulfurItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(SulfurDefId)) };
				IronItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(IronDefId)) };
				NickelItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(NickelDefId)) };
				CobaltItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(CobaltDefId)) };
				SiliconItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(SiliconDefId)) };
				SilverItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(SilverDefId)) };
				GoldItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(GoldDefId)) };
				PlatinumItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(PlatinumDefId)) };
				UraniumItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(UraniumDefId)) };
				CopperItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(CopperDefId)) };
				LithiumItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(LithiumDefId)) };
				BauxiteItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(BauxiteDefId)) };
				TitaniumItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(TitaniumDefId)) };
				TantalumItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(TantalumDefId)) };
				NiterItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(NiterDefId)) };
				OilSandItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(OilSandDefId)) };
				CoalItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(CoalDefId)) };
				IceItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = ((MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(IceDefId)) };
				
				SetupDone = true;
			
			}
			
			if(FirstRun == true){
				
				LastStoneLevel = (float)Inventory.GetItemAmount(StoneDefId); // Initializes the script by checking starting ore levels, and waits for the next run
				LastSulfurLevel = (float)Inventory.GetItemAmount(SulfurDefId);
				LastIronLevel = (float)Inventory.GetItemAmount(IronDefId);
				LastNickelLevel = (float)Inventory.GetItemAmount(NickelDefId);
				LastCobaltLevel = (float)Inventory.GetItemAmount(CobaltDefId);
				LastSiliconLevel = (float)Inventory.GetItemAmount(SiliconDefId);
				LastSilverLevel = (float)Inventory.GetItemAmount(SilverDefId);
				LastGoldLevel = (float)Inventory.GetItemAmount(GoldDefId);
				LastPlatinumLevel = (float)Inventory.GetItemAmount(PlatinumDefId);
				LastUraniumLevel = (float)Inventory.GetItemAmount(UraniumDefId);
				LastCopperLevel = (float)Inventory.GetItemAmount(CopperDefId);
				LastLithiumLevel = (float)Inventory.GetItemAmount(LithiumDefId);
				LastBauxiteLevel = (float)Inventory.GetItemAmount(BauxiteDefId);
				LastTitaniumLevel = (float)Inventory.GetItemAmount(TitaniumDefId);
				LastTantalumLevel = (float)Inventory.GetItemAmount(TantalumDefId);
				LastNiterLevel = (float)Inventory.GetItemAmount(NiterDefId);
				LastOilSandLevel = (float)Inventory.GetItemAmount(OilSandDefId);
				LastCoalLevel = (float)Inventory.GetItemAmount(CoalDefId);
				LastIceLevel = (float)Inventory.GetItemAmount(CoalDefId);
			
			    FirstRun = false;
				
				return;
				
			}else{
				
				CurrentStoneLevel = (float)Inventory.GetItemAmount(StoneDefId); // Checks current level of one type of ore
			
				if(CurrentStoneLevel > LastStoneLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						StoneToAdd = (CurrentStoneLevel - LastStoneLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)StoneToAdd, StoneItem.Content);
						
					}
				
				}
				
				CurrentSulfurLevel = (float)Inventory.GetItemAmount(SulfurDefId); // Checks current level of one type of ore
			
				if(CurrentSulfurLevel > LastSulfurLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						SulfurToAdd = (CurrentSulfurLevel - LastSulfurLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)SulfurToAdd, SulfurItem.Content);
						
					}
				
				}
				
				CurrentIronLevel = (float)Inventory.GetItemAmount(IronDefId); // Checks current level of one type of ore
			
				if(CurrentIronLevel > LastIronLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						IronToAdd = (CurrentIronLevel - LastIronLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)IronToAdd, IronItem.Content);
						
					}
				
				}
				
				CurrentNickelLevel = (float)Inventory.GetItemAmount(NickelDefId); // Checks current level of one type of ore
			
				if(CurrentNickelLevel > LastNickelLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						NickelToAdd = (CurrentNickelLevel - LastNickelLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)NickelToAdd, NickelItem.Content);
						
					}
				
				}
				
				CurrentCobaltLevel = (float)Inventory.GetItemAmount(CobaltDefId); // Checks current level of one type of ore
			
				if(CurrentCobaltLevel > LastCobaltLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						CobaltToAdd = (CurrentCobaltLevel - LastCobaltLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)CobaltToAdd, CobaltItem.Content);
						
					}
				
				}
				
				CurrentSiliconLevel = (float)Inventory.GetItemAmount(SiliconDefId); // Checks current level of one type of ore
			
				if(CurrentSiliconLevel > LastSiliconLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						SiliconToAdd = (CurrentSiliconLevel - LastSiliconLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)SiliconToAdd, SiliconItem.Content);
						
					}
				
				}
				
				CurrentSilverLevel = (float)Inventory.GetItemAmount(SilverDefId); // Checks current level of one type of ore
			
				if(CurrentSilverLevel > LastSilverLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						SilverToAdd = (CurrentSilverLevel - LastSilverLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)SilverToAdd, SilverItem.Content);
						
					}
				
				}
				
				CurrentGoldLevel = (float)Inventory.GetItemAmount(GoldDefId); // Checks current level of one type of ore
			
				if(CurrentGoldLevel > LastGoldLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						GoldToAdd = (CurrentGoldLevel - LastGoldLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)GoldToAdd, GoldItem.Content);
						
					}
				
				}
				
				CurrentPlatinumLevel = (float)Inventory.GetItemAmount(PlatinumDefId); // Checks current level of one type of ore
			
				if(CurrentPlatinumLevel > LastPlatinumLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						PlatinumToAdd = (CurrentPlatinumLevel - LastPlatinumLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)PlatinumToAdd, PlatinumItem.Content);
						
					}
				
				}
				
				CurrentUraniumLevel = (float)Inventory.GetItemAmount(UraniumDefId); // Checks current level of one type of ore
			
				if(CurrentUraniumLevel > LastUraniumLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						UraniumToAdd = (CurrentUraniumLevel - LastUraniumLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)UraniumToAdd, UraniumItem.Content);
						
					}
				
				}
				
				CurrentCopperLevel = (float)Inventory.GetItemAmount(CopperDefId); // Checks current level of one type of ore
			
				if(CurrentCopperLevel > LastCopperLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						CopperToAdd = (CurrentCopperLevel - LastCopperLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)CopperToAdd, CopperItem.Content);
						
					}
				
				}
				
				CurrentLithiumLevel = (float)Inventory.GetItemAmount(LithiumDefId); // Checks current level of one type of ore
			
				if(CurrentLithiumLevel > LastLithiumLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						LithiumToAdd = (CurrentLithiumLevel - LastLithiumLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)LithiumToAdd, LithiumItem.Content);
						
					}
				
				}
				
				CurrentBauxiteLevel = (float)Inventory.GetItemAmount(BauxiteDefId); // Checks current level of one type of ore
			
				if(CurrentBauxiteLevel > LastBauxiteLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						BauxiteToAdd = (CurrentBauxiteLevel - LastBauxiteLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)BauxiteToAdd, BauxiteItem.Content);
						
					}
				
				}
				
				CurrentTitaniumLevel = (float)Inventory.GetItemAmount(TitaniumDefId); // Checks current level of one type of ore
			
				if(CurrentTitaniumLevel > LastTitaniumLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						TitaniumToAdd = (CurrentTitaniumLevel - LastTitaniumLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)TitaniumToAdd, TitaniumItem.Content);
						
					}
				
				}
				
				CurrentTantalumLevel = (float)Inventory.GetItemAmount(TantalumDefId); // Checks current level of one type of ore
			
				if(CurrentTantalumLevel > LastTantalumLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						TantalumToAdd = (CurrentTantalumLevel - LastTantalumLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)TantalumToAdd, TantalumItem.Content);
						
					}
				
				}
				
				CurrentNiterLevel = (float)Inventory.GetItemAmount(NiterDefId); // Checks current level of one type of ore
			
				if(CurrentNiterLevel > LastNiterLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						NiterToAdd = (CurrentNiterLevel - LastNiterLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)NiterToAdd, NiterItem.Content);
						
					}
				
				}
				
				CurrentOilSandLevel = (float)Inventory.GetItemAmount(OilSandDefId); // Checks current level of one type of ore
			
				if(CurrentOilSandLevel > LastOilSandLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						OilSandToAdd = (CurrentOilSandLevel - LastOilSandLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)OilSandToAdd, OilSandItem.Content);
						
					}
				
				}
				
				CurrentCoalLevel = (float)Inventory.GetItemAmount(CoalDefId); // Checks current level of one type of ore
			
				if(CurrentCoalLevel > LastCoalLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						CoalToAdd = (CurrentCoalLevel - LastCoalLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)CoalToAdd, CoalItem.Content);
						
					}
				
				}
				
				CurrentIceLevel = (float)Inventory.GetItemAmount(IceDefId); // Checks current level of one type of ore
			
				if(CurrentIceLevel > LastIceLevel){ // If ore level has increased, adds additional ore of that type, based on YieldMultiplier
				
					if(Drill.Enabled == true){
					
						IceToAdd = (CurrentIceLevel - LastIceLevel) * (YieldMultiplier - 1);
					
						Inventory.AddItems((MyFixedPoint)IceToAdd, IceItem.Content);
						
					}
				
				}
				
				LastStoneLevel = (float)Inventory.GetItemAmount(StoneDefId); // Updates last known ore levels for the next run
				LastSulfurLevel = (float)Inventory.GetItemAmount(SulfurDefId);
				LastIronLevel = (float)Inventory.GetItemAmount(IronDefId);
				LastNickelLevel = (float)Inventory.GetItemAmount(NickelDefId);
				LastCobaltLevel = (float)Inventory.GetItemAmount(CobaltDefId);
				LastSiliconLevel = (float)Inventory.GetItemAmount(SiliconDefId);
				LastSilverLevel = (float)Inventory.GetItemAmount(SilverDefId);
				LastGoldLevel = (float)Inventory.GetItemAmount(GoldDefId);
				LastPlatinumLevel = (float)Inventory.GetItemAmount(PlatinumDefId);
				LastUraniumLevel = (float)Inventory.GetItemAmount(UraniumDefId);
				LastCopperLevel = (float)Inventory.GetItemAmount(CopperDefId);
				LastLithiumLevel = (float)Inventory.GetItemAmount(LithiumDefId);
				LastBauxiteLevel = (float)Inventory.GetItemAmount(BauxiteDefId);
				LastTitaniumLevel = (float)Inventory.GetItemAmount(TitaniumDefId);
				LastTantalumLevel = (float)Inventory.GetItemAmount(TantalumDefId);
				LastNiterLevel = (float)Inventory.GetItemAmount(NiterDefId);
				LastOilSandLevel = (float)Inventory.GetItemAmount(OilSandDefId);
				LastCoalLevel = (float)Inventory.GetItemAmount(CoalDefId);
				LastIceLevel = (float)Inventory.GetItemAmount(IceDefId);
				
				
			}
			
			
		}
		
		public override void OnRemovedFromScene(){
			
			base.OnRemovedFromScene();
			
			var Block = Entity as IMyShipDrill;
			
			if(Block == null){
				
				return;
				
			}
			
		}
		
		public override void OnBeforeRemovedFromContainer(){
			
			base.OnBeforeRemovedFromContainer();
			
			if(Entity.InScene == true){
				
				OnRemovedFromScene();
				
			}
			
		}
		
	}
	
}