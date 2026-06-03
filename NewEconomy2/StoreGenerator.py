"""
Unified Store Generator
Generates store profiles for all store types.
Uses configuration from StoreConfig.py
"""

from StoreConfig import get_store_config, get_faction_config


def generate_profile_ingot(ingots: list, trade_ingots: str) -> str:
    """Generate orders and offers for ingots based on score"""
    string = "\n"
    # Orders (required to buy)
    for ingot in ingots:
        if ingot.Name not in trade_ingots:
            continue
        string += f"        [Orders:Ingot/{ingot}] \n"
    
    string += "\n"
    
    # Offers (what they sell) - only low score ores
    for ingot in ingots:
        if ingot.scoreint > 2:
            continue
        if ingot.Name not in trade_ingots:
            continue
        string += f"        [Offers:Ingot/{ingot}] \n"
    
    return string


def generate_profile_ingot_ore(ingots: list, trade_ingots: str, check_scores: bool = True) -> str:
    """Generate orders and offers for ore-to-ingot trades"""
    string = "\n"
    
    # Orders (required to buy ore)
    for ingot in ingots:
        if check_scores and ingot.scoreint > 2:
            continue
        if ingot.Name not in trade_ingots:
            continue
        string += f"        [Orders:Ore/{ingot}] \n"
    
    string += "\n"
    
    # Offers (what they sell as ingots)
    for ingot in ingots:
        if check_scores and ingot.scoreint > 2:
            continue
        if ingot.Name not in trade_ingots:
            continue
        string += f"        [Offers:Ingot/{ingot}] \n"
    
    return string


def generate_mission_ids(mission_ids: list) -> str:
    """Generate mission ID entries"""
    if len(mission_ids) == 0:
        return ""
    
    string = "\n"
    for mission_id in mission_ids:
        string += f"        [MissionIds:AaW_Mission_{mission_id}] \n"
    return string


def build_store_profile_xml(
    xml_name: str,
    faction: str,
    store_name: str,
    store_type: str,
    ingots: list,
    trade_ingots: str,
    is_dynamic: bool = False,
    is_io: bool = False,
) -> str:
    """
    Build a store profile XML component.
    
    Args:
        xml_name: Name for XML file reference
        faction: Faction name
        store_name: Store name/encounter ID
        store_type: Type of store (Settlement, Military, etc.)
        ingots: List of available ingots
        trade_ingots: String of tradeable ingots
        is_dynamic: Whether this is a dynamic store
        is_io: Whether this is an IO variant
    """
    
    config = get_store_config(store_type)
    if not config:
        return ""
    
    profile_id = f"{faction}_StoreProfile_{store_name}_{store_type}"
    
    # Get item counts
    min_offer = config.get("min_offer_items", 5)
    max_offer = config.get("max_offer_items", 20)
    min_order = config.get("min_order_items", 5)
    max_order = config.get("max_order_items", 20)
    
    # Handle IO variants for dynamic stores
    if is_io and is_dynamic:
        min_offer = config.get("min_offer_items_io", min_offer)
        max_offer = config.get("max_offer_items_io", max_offer)
        min_order = config.get("min_order_items_io", min_order)
        max_order = config.get("max_order_items_io", max_order)
    elif is_io:
        max_offer = config.get("max_offer_items_io", max_offer)
        max_order = config.get("max_order_items_io", max_order)
    
    # Build the XML
    xml = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{profile_id}</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{xml_name}.xml]
        
        [MinOfferItems:{min_offer}]
        [MaxOfferItems:{max_offer}]
        [MinOrderItems:{min_order}]
        [MaxOrderItems:{max_order}]

        [ItemsRequireInventory:false]
"""
    
    # Add required orders
    if "required_orders" in config:
        for order in config["required_orders"]:
            xml += f"        [RequiredOrders:{order}] \n"
    
    # Add ore orders if needed
    if config.get("requires_ore_orders"):
        xml += generate_profile_ingot_ore(ingots, trade_ingots)
    
    # Add regular orders
    if "orders" in config:
        for order in config["orders"]:
            xml += f"        [Orders:{order}] \n"
    
    # Add required offers
    if "required_offers" in config:
        for offer in config["required_offers"]:
            xml += f"        [RequiredOffers:{offer}] \n"
    
    # Add ore offers if needed
    if config.get("requires_ore_offers"):
        xml += generate_profile_ingot_ore(ingots, trade_ingots)
    
    # Add ingot orders/offers if needed
    if config.get("requires_ingot_orders"):
        xml += generate_profile_ingot(ingots, trade_ingots)
    if config.get("requires_ingot_offers"):
        xml += generate_profile_ingot(ingots, trade_ingots)
    
    # Add regular offers
    if "offers" in config:
        for offer in config["offers"]:
            xml += f"        [Offers:{offer}] \n"
    
    # Add IO-specific offers
    if is_io and "io_extra_offers" in config:
        for offer in config["io_extra_offers"]:
            xml += f"        [Offers:{offer}] \n"

    xml += """
      </Description>

    </EntityComponent>"""
    
    return xml


def build_faction_store_xml(
    xml_name: str,
    faction: str,
    store_name: str,
    faction_faction: str,
    is_io: bool = False,
) -> str:
    """Build a faction-specific store profile"""
    
    config = get_faction_config(faction_faction)
    if not config:
        return ""
    
    profile_id = f"{faction}_StoreProfile_{store_name}_{faction_faction}"
    
    # Get item counts
    min_offer = config.get("min_offer_items", 5)
    max_offer = config.get("max_offer_items", 20)
    min_order = config.get("min_order_items", 5)
    max_order = config.get("max_order_items", 20)
    
    # Handle IO variants
    if is_io:
        min_offer = config.get("min_offer_items_io", min_offer)
        max_offer = config.get("max_offer_items_io", max_offer)
        min_order = config.get("min_order_items_io", min_order)
        max_order = config.get("max_order_items_io", max_order)
    
    xml = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{profile_id}</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{xml_name}.xml]
        
        [MinOfferItems:{min_offer}]
        [MaxOfferItems:{max_offer}]
        [MinOrderItems:{min_order}]
        [MaxOrderItems:{max_order}]

        [ItemsRequireInventory:false]
"""
    
    # Add orders
    if "orders" in config:
        for order in config["orders"]:
            xml += f"        [Orders:{order}] \n"
    
    # Add offers
    if "offers" in config:
        for offer in config["offers"]:
            xml += f"        [Offers:{offer}] \n"
    
    xml += """
      </Description>

    </EntityComponent>"""
    
    return xml


def get_contract_faction(faction: str, store_name: str, contract_faction: str, mission_ids: list) -> str:
    """Generate a contract block for a faction"""
    
    xml = f"""
    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{faction}_ContractBlockProfile_{store_name}_{contract_faction}</SubtypeId>
      </Id>
      <Description>
        [MES Contract Block]

        [StoreProfileId:{faction}_StoreProfile_{store_name}_Ingot]
        
        [MinContracts:5]
        [MaxContracts:15]
        {generate_mission_ids(mission_ids)}
        
      </Description>
    </EntityComponent>	"""
    
    return xml
