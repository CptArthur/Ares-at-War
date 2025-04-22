import Templates as Temp

def GenerateMissionIds(MissionIds:list):
    if len(MissionIds) == 0:
        return ""
    string = "\n"
    for id in MissionIds:
        string += f"        [MissionIds:AaW_Mission_{id}] \n"

    return string

def CreateStoreItems(Faction, Name:str,XML_Name: str,StoresProfiles:str,TradeIngots:str,Ingots:list, MissionIds:list):
    string = f"""<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    {Temp.GetStoreSettlement(XML_Name,Faction,Name,Ingots,TradeIngots) if "Settlement" in StoresProfiles else ""}
     {Temp.GetStoreTradestation(XML_Name,Faction,Name,Ingots,TradeIngots) if "Tradestation" in StoresProfiles else ""}


     {Temp.GetStoreSettlement(XML_Name,Faction,Name,Ingots,TradeIngots) if "HQ" in StoresProfiles else ""}


    {Temp.GetStoreVendingMachine(XML_Name,Faction,Name) if "VendingMachine" in StoresProfiles else ""}
    {Temp.GetStoreVendingMachine(XML_Name,Faction,Name) if "Vending Machine" in StoresProfiles else ""}
    {Temp.GetStoreIngot(XML_Name,Faction,Name,Ingots,TradeIngots) if "Ingot" in StoresProfiles else ""}
    {Temp.GetStoreCiv(XML_Name,Faction,Name) if "Civ" in StoresProfiles else ""}
    {Temp.GetStoreScrap(XML_Name,Faction,Name) if "Scrap" in StoresProfiles else ""}
    {Temp.GetStoreAmmo(XML_Name,Faction,Name) if "Ammo" in StoresProfiles else ""}
    {Temp.GetStoreSYN(XML_Name,Faction,Name) if "SYN" in StoresProfiles else ""}
    {Temp.GetStoreITC(XML_Name,Faction,Name) if "ITC" in StoresProfiles else ""}
    {Temp.GetStoreUNION(XML_Name,Faction,Name) if "UNION" in StoresProfiles else ""}
    {Temp.GetStoreSOLCOOP(XML_Name,Faction,Name) if "SOLCOOP" in StoresProfiles else ""}


    {Temp.GetStoreOutpost(XML_Name,Faction,Name) if "Outpost" in StoresProfiles else ""}
    
    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Fuel</SubtypeId>
      </Id>
      <Description>
        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:5]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Offers:HydrogenOffer] 
        [Offers:OxygenOffer] 
        [Offers:Ore/Ice] 
      </Description>
    </EntityComponent>	
	
    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_ContractBlockProfile_{Name}</SubtypeId>
      </Id>
      <Description>
        [MES Contract Block]

        [StoreProfileId:{Faction}_StoreProfile_{Name}_Ingot]
        
        [MinContracts:5]
        [MaxContracts:15]
        {GenerateMissionIds(MissionIds)}
        
      </Description>
    </EntityComponent>	    

 
  </EntityComponents>
</Definitions>
"""
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/Encounters/{Faction}/[{Faction}]-Behaviors/Store/{Faction}-StoreItems-{Name}.sbc', 'w') as f:
        f.write(string)




def CreateTriggers(Faction, Name:str, IO):
    print(f"[Triggers:{Faction}_Trigger_Static_PopulateStores_{Name}]")
    FailCondition = ""
    if IO == False:
      FailCondition = "[UseFailCondition:true]"
    ITC = "{ITC}"
    SYN = "{SYN}"
    UNION = "{UNION}"  
    SOLCOOP = "{SOLCOOP}"  
    string = f"""<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

  <!-- [Triggers:{Faction}_Trigger_Static_PopulateStores_{Name}] -->
    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_Trigger_Static_PopulateStores_{Name}</SubtypeId>
      </Id>
      <Description>

        [RivalAI Trigger]

        [UseTrigger:true]
        [Type:Timer]
        [MinCooldownMs:1200000]
        [MaxCooldownMs:1200001]
        [Conditions:{Faction}_Condition_Static_PopulateStores_{Name}]
        [StartsReady:true]
        [MaxActions:-1]
        [Actions:{Faction}_Action_Static_PopulateStores_{Name}]

      </Description>

    </EntityComponent>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_Condition_Static_PopulateStores_{Name}</SubtypeId>
      </Id>
      <Description>

        [RivalAI Condition]
              
        [UseConditions:true]
        {FailCondition}
        [CheckAllLoadedModIDs:true]
        [AllModIDsToCheck:2344068716]

      </Description>

    </EntityComponent>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_Action_Static_PopulateStores_{Name}</SubtypeId>
      </Id>
      <Description>

        [RivalAI Action]

        [ApplyStoreProfiles:true]
        [ClearStoreContentsFirst:true]

       [StoreBlocks:Store Settlement]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Settlement]		

       [StoreBlocks:Store Tradestation]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Tradestation]		




      [StoreBlocks:Store {ITC}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_ITC]		
		
	    [StoreBlocks:Store {SYN}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_SYN]	

	    [StoreBlocks:Store {UNION}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_UNION]	

      [StoreBlocks:Store {SOLCOOP}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_SOLCOOP]	


      [StoreBlocks:Store Civ]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Civ]
  
      [StoreBlocks:Store Salvage]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Salvage]

      [StoreBlocks:Store Ammo]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Ammo]

      [StoreBlocks:Store Ingot]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Ingot]
  
      [StoreBlocks:Store Tool]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Tool]      

      [StoreBlocks:Store Prefab]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Prefab]      

      
      [StoreBlocks:Store Fuel]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Fuel]
  
      
      [StoreBlocks:Store Outpost]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Outpost]


      [StoreBlocks:Vending Machine]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_VendingMachine]




      [AddCustomDataToBlocks:true]
      [CustomDataBlockNames:EconomySurvival Store Water Vehicles,EconomySurvival Store Land Vehicles,EconomySurvival Store Air Vehicles,EconomySurvival Store Space Vehicles]
      [CustomDataFiles:{Faction}_Water_Vehicles.xml]
      [CustomDataFiles:{Faction}_Land_Vehicles.xml]
      [CustomDataFiles:{Faction}_Air_Vehicles.xml]
      [CustomDataFiles:{Faction}_Space_Vehicles.xml]  

      [ApplyContractProfiles:true]
      [ClearContractContentsFirst:true]
      [ContractBlocks:Contracts]
      [ContractBlockProfiles:{Faction}_ContractBlockProfile_{Name}]

       [ContractBlocks:Contracts {SYN}]
      [ContractBlockProfiles:SHIVAN_ContractBlockProfile_Civilian]

       [ContractBlocks:Contracts {ITC}]
      [ContractBlockProfiles:ITC_ContractBlockProfile_Civilian]
      
       [ContractBlocks:Contracts {UNION}]
      [ContractBlockProfiles:UNION_ContractBlockProfile_Civilian]


       [ContractBlocks:Contracts {SOLCOOP}]
      [ContractBlockProfiles:SOLCOOP_ContractBlockProfile_Civilian]

      </Description>

    </EntityComponent>
  </EntityComponents>
</Definitions>
"""
    
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/Encounters/{Faction}/[{Faction}]-Behaviors/Store/Triggers/{Faction}-{Name}-Triggers-Store.sbc', 'w') as f:
        f.write(string)