"""
Profile Builder
Creates store item files and trigger files.
Uses StoreGenerator for all profile generation.
"""

import StoreGenerator as SG
from StoreConfig import STORE_TYPES, FACTION_STORES


def generate_store_profiles(
    xml_name: str,
    faction: str,
    store_name: str,
    store_profiles: str,
    trade_ingots: str,
    ingots: list,
    is_dynamic: bool = False,
    is_io: bool = False,
) -> str:
    """Generate all requested store profiles as XML"""
    
    xml_components = []
    
    # Regular store types
    for store_type in STORE_TYPES.keys():
        if store_type in store_profiles:
            profile = SG.build_store_profile_xml(
                xml_name, faction, store_name, store_type,
                ingots, trade_ingots, is_dynamic, is_io
            )
            if profile:
                xml_components.append(profile)
    
    # Faction stores
    for faction_name in FACTION_STORES.keys():
        if faction_name in store_profiles:
            profile = SG.build_faction_store_xml(
                xml_name, faction, store_name, faction_name, is_io
            )
            if profile:
                xml_components.append(profile)
    
    return "\n".join(xml_components)


def create_store_items(
    faction: str,
    name: str,
    xml_name: str,
    store_profiles: str,
    trade_ingots: str,
    ingots: list,
    mission_ids: list,
    security_mission_ids: list,
    itc_mission_ids: list,
    shivan_mission_ids: list,
    aguro_mission_ids: list,
    zenova_mission_ids: list,
    solcoop_mission_ids: list,
    mayor_mission_ids: list,
):
    """Create store items file"""
    
    store_profiles_xml = generate_store_profiles(
        xml_name, faction, name, store_profiles, trade_ingots, ingots, is_dynamic=False
    )
    
    string = f"""<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    {store_profiles_xml}
    
    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{faction}_StoreProfile_{name}_Fuel</SubtypeId>
      </Id>
      <Description>
        [MES Store]

        [FileSource:AaW_StoreItems_{xml_name}.xml]
        
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
        <SubtypeId>{faction}_ContractBlockProfile_{name}</SubtypeId>
      </Id>
      <Description>
        [MES Contract Block]

        [StoreProfileId:{faction}_StoreProfile_{name}_Ingot]
        
        [MinContracts:5]
        [MaxContracts:15]
        {SG.generate_mission_ids(mission_ids)}
        
      </Description>
    </EntityComponent>	    

    {SG.get_contract_faction(faction, name, "SHIVAN", shivan_mission_ids) if "SHIVAN" in store_profiles else ""}
    {SG.get_contract_faction(faction, name, "SECURITY", security_mission_ids) if "SECURITY" in store_profiles else ""}
    {SG.get_contract_faction(faction, name, "AGURO", aguro_mission_ids) if "AGURO" in store_profiles else ""}
    {SG.get_contract_faction(faction, name, "ZENOVA", zenova_mission_ids) if "ZENOVA" in store_profiles else ""}
    {SG.get_contract_faction(faction, name, "ITC", itc_mission_ids) if "ITC" in store_profiles else ""}
    {SG.get_contract_faction(faction, name, "SOLCOOP", solcoop_mission_ids) if "SOLCOOP" in store_profiles else ""} 
    {SG.get_contract_faction(faction, name, "MAYOR", mayor_mission_ids) if "MAYOR" in store_profiles else ""}     

  </EntityComponents>
</Definitions>
"""
    
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/Encounters/{faction}/[{faction}]-Behaviors/Store/{faction}-StoreItems-{name}.sbc', 'w') as f:
        f.write(string)


def create_store_items_dynamic(
    faction: str,
    name: str,
    xml_name: str,
    store_profiles: str,
    trade_ingots: str,
    ingots: list,
    mission_ids: list,
    security_mission_ids: list,
    itc_mission_ids: list,
    shivan_mission_ids: list,
    aguro_mission_ids: list,
    zenova_mission_ids: list,
    solcoop_mission_ids: list,
    mayor_mission_ids: list,
):
    """Create dynamic store items file"""
    
    store_profiles_xml = generate_store_profiles(
        xml_name, faction, name, store_profiles, trade_ingots, ingots, is_dynamic=True
    )
    
    string = f"""<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>
    {store_profiles_xml}
    
    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{faction}_ContractBlockProfile_{name}</SubtypeId>
      </Id>
      <Description>
        [MES Contract Block]

        [StoreProfileId:{faction}_StoreProfile_{name}_Ingot]
        
        [MinContracts:5]
        [MaxContracts:15]
        {SG.generate_mission_ids(mission_ids)}
        
      </Description>
    </EntityComponent>	      

  </EntityComponents>
</Definitions>
"""
    
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/Encounters/{faction}/[{faction}]-Behaviors/Store/{faction}-StoreItems-{name}.sbc', 'w') as f:
        f.write(string)


def create_triggers(faction: str, name: str, io: bool, store_profiles: str):
    """Create trigger file"""
    
    print(f"[Triggers:{faction}_Trigger_Static_PopulateStores_{name}]")
    
    fail_condition = ""
    tags = "StoreRefreshIO"
    
    if not io:
        fail_condition = "[UseFailCondition:true]"
        tags = "StoreRefresh"
    
    # Build store block references dynamically
    store_blocks = []
    for store_type in STORE_TYPES.keys():
        if store_type in store_profiles:
            if store_type == "Settlement":
                store_blocks.append(f"[StoreBlocks:Store Settlement]\n      [StoreProfiles:{faction}_StoreProfile_{name}_Settlement]")
            elif store_type == "Tradestation":
                store_blocks.append(f"[StoreBlocks:Store Tradestation]\n      [StoreProfiles:{faction}_StoreProfile_{name}_Tradestation]")
            elif store_type == "Military":
                store_blocks.append(f"[StoreBlocks:Store Military]\n      [StoreProfiles:{faction}_StoreProfile_{name}_Military]")
            elif store_type == "FishingBoat":
                store_blocks.append(f"[StoreBlocks:Store FishingBoat]\n      [StoreProfiles:{faction}_StoreProfile_{name}_FishingBoat]")
            elif store_type == "University":
                store_blocks.append(f"[StoreBlocks:Store University]\n      [StoreProfiles:AaW_StoreProfile_{name}_University]")
            elif store_type == "Vending Machine":
                store_blocks.append(f"[StoreBlocks:Vending Machine]\n      [StoreProfiles:{faction}_StoreProfile_{name}_VendingMachine]")
            elif store_type == "Outpost":
                store_blocks.append(f"[StoreBlocks:Store Outpost]\n      [StoreProfiles:{faction}_StoreProfile_{name}_Outpost]")
    
    for faction_name in FACTION_STORES.keys():
        if faction_name in store_profiles:
            store_blocks.append(f"[StoreBlocks:Store {faction_name}]\n      [StoreProfiles:{faction}_StoreProfile_{name}_{faction_name}]")
    
    store_blocks_str = "\n    ".join(store_blocks)
    
    # Build contract block references
    contract_blocks = []
    for faction_name in FACTION_STORES.keys():
        if faction_name in store_profiles:
            contract_blocks.append(f"[ContractBlocks:Contracts {faction_name}]\n      [ContractBlockProfiles:{faction}_ContractBlockProfile_{name}_{faction_name}]")
    
    contract_blocks_str = "\n    ".join(contract_blocks)
    
    string = f"""<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

  <!-- [Triggers:{faction}_Trigger_Static_PopulateStores_{name}] -->
    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{faction}_Trigger_Static_PopulateStores_{name}</SubtypeId>
      </Id>
      <Description>

        [RivalAI Trigger]
        [Tags:{tags}]
        [UseTrigger:true]
        [Type:Timer]
        [MinCooldownMs:1200000]
        [MaxCooldownMs:1200001]
        [Conditions:{faction}_Condition_Static_PopulateStores_{name}]
        [StartsReady:true]
        [MaxActions:-1]
        [Actions:{faction}_Action_Static_PopulateStores_{name}]

      </Description>

    </EntityComponent>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{faction}_Condition_Static_PopulateStores_{name}</SubtypeId>
      </Id>
      <Description>

        [RivalAI Condition]
              
        [UseConditions:true]
        {fail_condition}
        [CheckAllLoadedModIDs:true]
        [AllModIDsToCheck:2344068716]

      </Description>

    </EntityComponent>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{faction}_Action_Static_PopulateStores_{name}</SubtypeId>
      </Id>
      <Description>
        [RivalAI Action]

        [ApplyStoreProfiles:true]
        [ClearStoreContentsFirst:true]

        {store_blocks_str}

        [StoreBlocks:Store Fuel]
        [StoreProfiles:{faction}_StoreProfile_{name}_Fuel]
  
        [AddCustomDataToBlocks:true]
        [CustomDataBlockNames:EconomySurvival Store Water Vehicles,EconomySurvival Store Land Vehicles,EconomySurvival Store Air Vehicles,EconomySurvival Store Space Vehicles]
        [CustomDataFiles:{faction}_Water_Vehicles.xml]
        [CustomDataFiles:{faction}_Land_Vehicles.xml]
        [CustomDataFiles:{faction}_Air_Vehicles.xml]
        [CustomDataFiles:{faction}_Space_Vehicles.xml]  
  
        [ApplyContractProfiles:true]
        [ClearContractContentsFirst:true]
        [ContractBlocks:Contracts]
        [ContractBlockProfiles:{faction}_ContractBlockProfile_{name}]
   
        {contract_blocks_str}

      </Description>
    </EntityComponent>
  </EntityComponents>
</Definitions>
"""
    
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/Encounters/{faction}/[{faction}]-Behaviors/Store/Triggers/{faction}-{name}-Triggers-Store.sbc', 'w') as f:
        f.write(string)
