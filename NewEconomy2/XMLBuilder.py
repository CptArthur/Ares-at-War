"""
XML Builder
Generates the store items XML files.
"""


def generate_xml_item_list(items: list) -> str:
    """Generate ItemSubtypeIds XML section for a list of items"""
    if not items:
        return ""
    
    xml = "\n      <ItemSubtypeIds> \n"
    for item in items:
        xml += f"        <ItemSubtypeId>{item}</ItemSubtypeId> \n"
    xml += "      </ItemSubtypeIds>"
    
    return xml


def create_store_item(
    item_id: str,
    item_type: str,
    items: list = None,
    min_price_mul: int = None,
    max_price_mul: int = None,
    min_amount: int = None,
    max_amount: int = None,
    offer_min_price: int = None,
    offer_max_price: int = None,
    offer_min_amount: int = None,
    offer_max_amount: int = None,
    order_min_price: int = None,
    order_max_price: int = None,
    order_min_amount: int = None,
    order_max_amount: int = None,
) -> str:
    """Generate a single StoreItem XML block"""
    
    xml = f"""    <StoreItem>
      <StoreItemId>{item_id}</StoreItemId>
      <ItemType>{item_type}</ItemType>"""
    
    # Add items if provided
    if items:
        xml += generate_xml_item_list(items)
    
    # Add offer section if specified
    if offer_min_price is not None or min_price_mul is not None:
        xml += "\n      <Offer>"
        if min_price_mul is not None:
            xml += f"\n        <MinPriceMultiplier>{min_price_mul}</MinPriceMultiplier>"
        if max_price_mul is not None:
            xml += f"\n        <MaxPriceMultiplier>{max_price_mul}</MaxPriceMultiplier>"
        if offer_min_price is not None:
            xml += f"\n        <MinPriceMultiplier>{offer_min_price}</MinPriceMultiplier>"
        if offer_max_price is not None:
            xml += f"\n        <MaxPriceMultiplier>{offer_max_price}</MaxPriceMultiplier>"
        if min_amount is not None:
            xml += f"\n        <MinAmount>{min_amount}</MinAmount>"
        if max_amount is not None:
            xml += f"\n        <MaxAmount>{max_amount}</MaxAmount>"
        if offer_min_amount is not None:
            xml += f"\n        <MinAmount>{offer_min_amount}</MinAmount>"
        if offer_max_amount is not None:
            xml += f"\n        <MaxAmount>{offer_max_amount}</MaxAmount>"
        xml += "\n      </Offer>"
    
    # Add order section if specified
    if order_min_price is not None or order_max_price is not None:
        xml += "\n      <Order>"
        if order_min_price is not None:
            xml += f"\n        <MinPriceMultiplier>{order_min_price}</MinPriceMultiplier>"
        if order_max_price is not None:
            xml += f"\n        <MaxPriceMultiplier>{order_max_price}</MaxPriceMultiplier>"
        if order_min_amount is not None:
            xml += f"\n        <MinAmount>{order_min_amount}</MinAmount>"
        if order_max_amount is not None:
            xml += f"\n        <MaxAmount>{order_max_amount}</MaxAmount>"
        xml += "\n      </Order>"
    
    xml += "\n    </StoreItem>"
    
    return xml


# Store item templates organized by category
STORE_ITEM_TEMPLATES = {
    "Ingot1": {
        "MinPriceMultiplier": 50, "MaxPriceMultiplier": 60,
        "MinAmount": 1000, "MaxAmount": 10000,
        "OrderMinPrice": 10, "OrderMaxPrice": 20,
    },
    "Ingot2": {
        "MinPriceMultiplier": 80, "MaxPriceMultiplier": 90,
        "MinAmount": 1000, "MaxAmount": 3000,
        "OrderMinPrice": 65, "OrderMaxPrice": 70,
    },
    "Ingot3": {
        "MinPriceMultiplier": 120, "MaxPriceMultiplier": 130,
        "MinAmount": 300, "MaxAmount": 2000,
        "OrderMinPrice": 108, "OrderMaxPrice": 100,
    },
    "Ingot4": {
        "MinPriceMultiplier": 145, "MaxPriceMultiplier": 160,
        "MinAmount": 100, "MaxAmount": 800,
        "OrderMinPrice": 125, "OrderMaxPrice": 135,
    },
    "Ingot5": {
        "MinPriceMultiplier": 190, "MaxPriceMultiplier": 210,
        "MinAmount": 100, "MaxAmount": 900,
        "OrderMinPrice": 170, "OrderMaxPrice": 180,
    },
    "Component1": {
        "MinPriceMultiplier": 60, "MaxPriceMultiplier": 70,
        "MinAmount": 500, "MaxAmount": 15000,
        "OrderMinPrice": 20, "OrderMaxPrice": 40,
    },
    "Component2": {
        "MinPriceMultiplier": 120, "MaxPriceMultiplier": 130,
        "MinAmount": 200, "MaxAmount": 5000,
        "OrderMinPrice": 90, "OrderMaxPrice": 100,
    },
    "Component3": {
        "MinPriceMultiplier": 190, "MaxPriceMultiplier": 210,
        "MinAmount": 50, "MaxAmount": 200,
        "OrderMinPrice": 150, "OrderMaxPrice": 160,
    },
}


def create_xml(
    name: str,
    ingot1_items: list,
    ingot2_items: list,
    ingot3_items: list,
    ingot4_items: list,
    ingot5_items: list,
    component1_items: list,
    component2_items: list,
    component3_items: list,
    ammo1_items: list = None,
    ammo2_items: list = None,
    ammo3_items: list = None,
    tool1_items: list = None,
    tool2_items: list = None,
    tool3_items: list = None,
    consumable1_items: list = None,
    consumable2_items: list = None,
    consumable3_items: list = None,
):
    """Create XML file with standard store items"""
    
    items_sections = []
    
    # Gravel - basic stone
    items_sections.append("""    <StoreItem>
      <StoreItemId>Gravel</StoreItemId>
      <ItemType>Ingot</ItemType>
      <ItemSubtypeIds> 
        <ItemSubtypeId>Stone</ItemSubtypeId>  
      </ItemSubtypeIds> 
      <Offer>
        <CustomPrice>5</CustomPrice>
        <MinAmount>190000</MinAmount>
        <MaxAmount>250000</MaxAmount>
      </Offer>
    </StoreItem>""")
    
    # Ingots 1-5
    for i, items in enumerate([ingot1_items, ingot2_items, ingot3_items, ingot4_items, ingot5_items], 1):
        tier = f"Ingot{i}"
        config = STORE_ITEM_TEMPLATES[tier]
        items_sections.append(
            create_store_item(
                tier, "Ingot", items,
                min_price_mul=config["MinPriceMultiplier"],
                max_price_mul=config["MaxPriceMultiplier"],
                min_amount=config["MinAmount"],
                max_amount=config["MaxAmount"],
                order_min_price=config["OrderMinPrice"],
                order_max_price=config["OrderMaxPrice"],
            )
        )
    
    # Ore equivalents
    for i, items in enumerate([ingot1_items, ingot2_items, ingot3_items, ingot4_items, ingot5_items], 1):
        tier = f"Ore{i}"
        config = STORE_ITEM_TEMPLATES[f"Ingot{i}"]
        items_sections.append(
            create_store_item(
                tier, "Ore", items,
                min_price_mul=config["MinPriceMultiplier"] * 5,  # Ore is more expensive
                max_price_mul=config["MaxPriceMultiplier"] * 5,
            )
        )
    
    # Components
    for i, items in enumerate([component1_items, component2_items, component3_items], 1):
        tier = f"Component{i}"
        config = STORE_ITEM_TEMPLATES[tier]
        items_sections.append(
            create_store_item(
                tier, "Component", items,
                min_price_mul=config["MinPriceMultiplier"],
                max_price_mul=config["MaxPriceMultiplier"],
                min_amount=config["MinAmount"],
                max_amount=config["MaxAmount"],
                order_min_price=config["OrderMinPrice"],
                order_max_price=config["OrderMaxPrice"],
            )
        )
    
    # Ammo (if provided)
    if ammo1_items:
        items_sections.append(
            create_store_item(
                "Ammo1", "Ammo", ammo1_items,
                min_price_mul=50, max_price_mul=60,
                min_amount=500, max_amount=5000,
                order_min_price=20, order_max_price=40,
            )
        )
    
    # Tools (if provided)
    if tool1_items:
        items_sections.append(
            create_store_item(
                "Tool1", "Tool", tool1_items,
                min_price_mul=60, max_price_mul=70,
                min_amount=5, max_amount=10,
                order_min_price=20, order_max_price=40,
            )
        )
    
    # Consumables (if provided)
    if consumable1_items:
        items_sections.append(
            create_store_item(
                "Consumable1", "Consumable", consumable1_items,
                min_price_mul=60, max_price_mul=70,
                min_amount=5, max_amount=50,
                order_min_price=20, order_max_price=40,
            )
        )
    
    # Gas offerings
    items_sections.append("""    <StoreItem>
      <StoreItemId>HydrogenOffer</StoreItemId>
      <ItemType>Hydrogen</ItemType>
      <ItemSubtypeId>Hydrogen</ItemSubtypeId>
      <Offer>
        <MinPriceMultiplier>75</MinPriceMultiplier>
        <MaxPriceMultiplier>125</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
    </StoreItem>

    <StoreItem>
      <StoreItemId>OxygenOffer</StoreItemId>
      <ItemType>Oxygen</ItemType>
      <ItemSubtypeId>Oxygen</ItemSubtypeId>
      <Offer>
        <MinPriceMultiplier>65</MinPriceMultiplier>
        <MaxPriceMultiplier>115</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
    </StoreItem>

    <StoreItem>
      <StoreItemId>IceOffer</StoreItemId>
      <ItemType>Ore</ItemType>
      <ItemSubtypeIds> 
        <ItemSubtypeId>Ice</ItemSubtypeId>  
      </ItemSubtypeIds> 
      <Offer>
        <MinPriceMultiplier>110</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>100000</MinAmount>
        <MaxAmount>200000</MaxAmount>
      </Offer>
    </StoreItem>""")
    
    xml_content = f"""<?xml version="1.0" encoding="utf-16"?>
<StoreItemsContainer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <StoreItems>
{chr(10).join(items_sections)}
  </StoreItems>
</StoreItemsContainer>
"""
    
    output_path = f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/StoreItems/AaW_StoreItems_{name}.xml'
    with open(output_path, 'w') as f:
        f.write(xml_content)


def create_xml_dynamic(
    name: str,
    ingot1_items: list,
    ingot2_items: list,
    ingot3_items: list,
    ingot4_items: list,
    ingot5_items: list,
    component1_items: list,
    component2_items: list,
    component3_items: list,
    ammo1_items: list = None,
    ammo2_items: list = None,
    ammo3_items: list = None,
    tool1_items: list = None,
    tool2_items: list = None,
    tool3_items: list = None,
    consumable1_items: list = None,
    consumable2_items: list = None,
    consumable3_items: list = None,
):
    """Create XML file for dynamic stores (slightly different amounts)"""
    
    # For dynamic stores, we typically use smaller amounts
    # This is a placeholder - extend as needed
    create_xml(name, ingot1_items, ingot2_items, ingot3_items, ingot4_items, ingot5_items,
               component1_items, component2_items, component3_items,
               ammo1_items, ammo2_items, ammo3_items,
               tool1_items, tool2_items, tool3_items,
               consumable1_items, consumable2_items, consumable3_items)
