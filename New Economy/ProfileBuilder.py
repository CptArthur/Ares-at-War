import Templates as Temp
import TemplateTriggers as Trigger

import Templates_Dynamic as Temp_Dynamic
def CreateStoreItems(Faction, Name:str,XML_Name: str, StoresProfiles:str,TradeIngots:str,Ingots:list, 
                     MissionIds:list, SECURITY_mission_ids,ITC_mission_ids,SHIVAN_mission_ids,AGURO_mission_ids,ZENOVA_mission_ids,SOLCOOP_mission_ids):
    string = f"""<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    {Temp.GetStoreSettlement(XML_Name,Faction,Name,Ingots,TradeIngots) if "Settlement" in StoresProfiles else ""}
     {Temp.GetStoreTradestation(XML_Name,Faction,Name,Ingots,TradeIngots) if "Tradestation" in StoresProfiles else ""}


     {Temp.GetStoreMilitary(XML_Name,Faction,Name,Ingots,TradeIngots) if "Military" in StoresProfiles else ""}



    {Temp.GetStoreVendingMachine(XML_Name,Faction,Name) if "Vending Machine" in StoresProfiles else ""}
    {Temp.GetStoreIngot(XML_Name,Faction,Name,Ingots,TradeIngots) if "Ingot" in StoresProfiles else ""}
    {Temp.GetStoreCiv(XML_Name,Faction,Name) if "Civ" in StoresProfiles else ""}
    {Temp.GetStoreScrap(XML_Name,Faction,Name) if "Scrap" in StoresProfiles else ""}
    {Temp.GetStoreAmmo(XML_Name,Faction,Name) if "Ammo" in StoresProfiles else ""}


    {Temp.GetStoreSHIVAN(XML_Name,Faction,Name) if "SHIVAN" in StoresProfiles else ""}


    {Temp.GetStoreSECURITY(XML_Name,Faction,Name) if "SECURITY" in StoresProfiles else ""}
    {Temp.GetStoreAGURO(XML_Name,Faction,Name) if "AGURO" in StoresProfiles else ""}
    {Temp.GetStoreZENOVA(XML_Name,Faction,Name) if "ZENOVA" in StoresProfiles else ""}
    {Temp.GetStoreAHE(XML_Name,Faction,Name) if "AHE" in StoresProfiles else ""}


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
        {Temp.GenerateMissionIds(MissionIds)}
        
      </Description>
    </EntityComponent>	    

    {Temp.GetContractFaction(Faction,Name,"SHIVAN",SHIVAN_mission_ids) if "SHIVAN" in StoresProfiles else ""}
    {Temp.GetContractFaction(Faction,Name,"SECURITY",SECURITY_mission_ids) if "SECURITY" in StoresProfiles else ""}
    {Temp.GetContractFaction(Faction,Name,"AGURO",AGURO_mission_ids) if "AGURO" in StoresProfiles else ""}
    {Temp.GetContractFaction(Faction,Name,"ZENOVA",ZENOVA_mission_ids) if "ZENOVA" in StoresProfiles else ""}
    {Temp.GetContractFaction(Faction,Name,"ITC",ITC_mission_ids) if "ITC" in StoresProfiles else ""}
    {Temp.GetContractFaction(Faction,Name,"SOLCOOP",SOLCOOP_mission_ids) if "SOLCOOP" in StoresProfiles else ""} 
    


  </EntityComponents>
</Definitions>
"""
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/Encounters/{Faction}/[{Faction}]-Behaviors/Store/{Faction}-StoreItems-{Name}.sbc', 'w') as f:
        f.write(string)

def CreateStoreItems_Dynamic(Faction, Name:str,XML_Name: str, StoresProfiles:str,TradeIngots:str,Ingots:list, 
                     MissionIds:list, SECURITY_mission_ids,ITC_mission_ids,SHIVAN_mission_ids,AGURO_mission_ids,ZENOVA_mission_ids,SOLCOOP_mission_ids):
    string = f"""<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>
     {Temp_Dynamic.GetStoreMilitary(XML_Name,Faction,Name,Ingots,TradeIngots) if "Military" in StoresProfiles else ""}

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
        {Temp_Dynamic.GenerateMissionIds(MissionIds)}
        
      </Description>
    </EntityComponent>	      

  </EntityComponents>
</Definitions>
"""
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/Encounters/{Faction}/[{Faction}]-Behaviors/Store/{Faction}-StoreItems-{Name}.sbc', 'w') as f:
        f.write(string)




def CreateTriggers(Faction, Name:str, IO, StoresProfiles):
    print(f"[Triggers:{Faction}_Trigger_Static_PopulateStores_{Name}]")
    FailCondition = ""
    Tags = "StoreRefresh"
    if IO == False:
      FailCondition = "[UseFailCondition:true]"
      Tags = "StoreRefreshIO"
    ITC = "{ITC}"
    SHIVAN = "{SHIVAN}"
    SECURITY = "{SECURITY}"
    AGURO = "{AGURO}"  	
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
        [Tags:{Tags}]
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

    {Trigger.GetStoreSettlement(Faction,Name) if "Settlement" in StoresProfiles else ""}
    {Trigger.GetStoreTradestation(Faction,Name) if "Tradestation" in StoresProfiles else ""}

    {Trigger.GetStoreUniversity(Faction,Name) if "University" in StoresProfiles else ""}


    {Trigger.GetStoreMilitary(Faction,Name) if "Military" in StoresProfiles else ""}
    {Trigger.GetStoreVendingMachine(Faction,Name) if "Vending Machine" in StoresProfiles else ""}
    {Trigger.GetStoreIngot(Faction,Name) if "Ingot" in StoresProfiles else ""}
    {Trigger.GetStoreCiv(Faction,Name) if "Civ" in StoresProfiles else ""}
    {Trigger.GetStoreScrap(Faction,Name) if "Scrap" in StoresProfiles else ""}
    {Trigger.GetStoreAmmo(Faction,Name) if "Ammo" in StoresProfiles else ""}


    {Trigger.GetStoreSHIVAN(Faction,Name) if "SHIVAN" in StoresProfiles else ""}
    {Trigger.GetStoreSECURITY(Faction,Name) if "SECURITY" in StoresProfiles else ""}
    {Trigger.GetStoreAGURO(Faction,Name) if "AGURO" in StoresProfiles else ""}
    {Trigger.GetStoreITC(Faction,Name) if "ITC" in StoresProfiles else ""}
    {Trigger.GetStoreUNION(Faction,Name) if "UNION" in StoresProfiles else ""}
    {Trigger.GetStoreSOLCOOP(Faction,Name) if "SOLCOOP" in StoresProfiles else ""}
    {Trigger.GetStoreZENOVA(Faction,Name) if "ZENOVA" in StoresProfiles else ""}
    {Trigger.GetStoreAHE(Faction,Name) if "AHE" in StoresProfiles else ""}
    {Trigger.GetStoreOutpost(Faction,Name) if "Outpost" in StoresProfiles else ""}        

      [StoreBlocks:Store Fuel]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Fuel]
  

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
   
    {Trigger.GetContractSHIVAN(Faction,Name) if "SHIVAN" in StoresProfiles else ""}
    {Trigger.GetContractSECURITY(Faction,Name) if "SECURITY" in StoresProfiles else ""}
    {Trigger.GetContractAGURO(Faction,Name) if "AGURO" in StoresProfiles else ""}
    {Trigger.GetContractITC(Faction,Name) if "ITC" in StoresProfiles else ""}
    {Trigger.GetContractUNION(Faction,Name) if "UNION" in StoresProfiles else ""}
    {Trigger.GetContractSOLCOOP(Faction,Name) if "SOLCOOP" in StoresProfiles else ""}
    {Trigger.GetContractSOLCOOP(Faction,Name) if "ZENOVA" in StoresProfiles else ""}

      </Description>
    </EntityComponent>
  </EntityComponents>
</Definitions>
"""
    
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/Encounters/{Faction}/[{Faction}]-Behaviors/Store/Triggers/{Faction}-{Name}-Triggers-Store.sbc', 'w') as f:
        f.write(string)


def CreateTriggers_Dynamic(Faction, Name:str, IO, StoresProfiles):
    print(f"[Triggers:{Faction}_Trigger_Static_PopulateStores_{Name}]")
    FailCondition = ""
    Tags = "StoreRefresh"
    if IO == False:
      FailCondition = "[UseFailCondition:true]"
      Tags = "StoreRefreshIO"
    ITC = "{ITC}"
    SHIVAN = "{SHIVAN}"
    SECURITY = "{SECURITY}"
    AGURO = "{AGURO}"  	
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
        [Tags:{Tags}]
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

    {Trigger.GetStoreSettlement(Faction,Name) if "Settlement" in StoresProfiles else ""}
    {Trigger.GetStoreTradestation(Faction,Name) if "Tradestation" in StoresProfiles else ""}
    {Trigger.GetStoreUniversity(Faction,Name) if "University" in StoresProfiles else ""}
    {Trigger.GetStoreMilitary(Faction,Name) if "Military" in StoresProfiles else ""}
    {Trigger.GetStoreVendingMachine(Faction,Name) if "Vending Machine" in StoresProfiles else ""}
    {Trigger.GetStoreIngot(Faction,Name) if "Ingot" in StoresProfiles else ""}
    {Trigger.GetStoreCiv(Faction,Name) if "Civ" in StoresProfiles else ""}
    {Trigger.GetStoreScrap(Faction,Name) if "Scrap" in StoresProfiles else ""}
    {Trigger.GetStoreAmmo(Faction,Name) if "Ammo" in StoresProfiles else ""}
    {Trigger.GetStoreSHIVAN(Faction,Name) if "SHIVAN" in StoresProfiles else ""}
    {Trigger.GetStoreSECURITY(Faction,Name) if "SECURITY" in StoresProfiles else ""}
    {Trigger.GetStoreAGURO(Faction,Name) if "AGURO" in StoresProfiles else ""}
    {Trigger.GetStoreITC(Faction,Name) if "ITC" in StoresProfiles else ""}
    {Trigger.GetStoreUNION(Faction,Name) if "UNION" in StoresProfiles else ""}
    {Trigger.GetStoreSOLCOOP(Faction,Name) if "SOLCOOP" in StoresProfiles else ""}
    {Trigger.GetStoreZENOVA(Faction,Name) if "ZENOVA" in StoresProfiles else ""}
    {Trigger.GetStoreOutpost(Faction,Name) if "Outpost" in StoresProfiles else ""}        
   
      [ApplyContractProfiles:true]
      [ClearContractContentsFirst:true]
      [ContractBlocks:Contracts]
      [ContractBlockProfiles:{Faction}_ContractBlockProfile_{Name}]
   
      </Description>
    </EntityComponent>
  </EntityComponents>
</Definitions>
"""
    
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/Encounters/{Faction}/[{Faction}]-Behaviors/Store/Triggers/{Faction}-{Name}-Triggers-Store.sbc', 'w') as f:
        f.write(string)

