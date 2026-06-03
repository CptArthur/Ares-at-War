"""
Store Configuration System
Define all store types and their properties here. Easy to extend!
"""

# Define all available store types
STORE_TYPES = {
    "Settlement": {
        "min_offer_items": 99,
        "max_offer_items": 99,
        "min_order_items": 99,
        "max_order_items": 99,
        "orders": ["Item/Grain", "Consumable/Mushrooms", "Consumable/Vegetables", "Consumable/Fruit", "Consumable/Fish"],
        "offers": ["Ingot/Stone", "Component/SteelPlate", "Component/Construction", "Component/Computer", "Component/Motor", "Component/SolarCell", "Component/AlkalinePowerCell", "Component/Girder", "Component/InteriorPlate"],
        "requires_ore_orders": True,
        "requires_ore_offers": True,
    },
    "Tradestation": {
        "min_offer_items": 99,
        "max_offer_items": 99,
        "min_order_items": 99,
        "max_order_items": 99,
        "requires_ore_orders": True,
        "requires_ore_offers": True,
    },
    "Military": {
        "min_offer_items": 7,
        "max_offer_items": 10,
        "min_order_items": 12,
        "max_order_items": 16,
        "required_orders": ["Component/SteelPlate", "Component/MetalGrid"],
        "orders": ["Ammo/NATO_25x184mm", "Ammo/missile", "Ammo/MediumCalibreAmmo", "Ammo/LargeCalibreAmmo", "Ammo/LargeRailgunAmmo", "Ammo/SmallRailgunAmmo", "Ammo/AutocannonClip"],
        "required_offers": ["Tool/AutomaticRifleItem", "Tool/PreciseAutomaticRifleItem", "Tool/RapidFireAutomaticRifleItem", "Ammo/PreciseAutomaticRifleGun_Mag_5rd", "Ammo/RapidFireAutomaticRifleGun_Mag_50rd", "Ammo/AutomaticRifleGun_Mag_20rd", "Ammo/UltimateAutomaticRifleGun_Mag_30rd"],
        "offers": ["Component/Construction", "Component/Computer", "Component/Motor", "Component/SolarCell", "Component/AlkalinePowerCell", "Component/Girder", "Component/InteriorPlate"],
        "requires_ore_orders": True,
        "requires_ore_offers": True,
    },
    "FishingBoat": {
        "min_offer_items": 2,
        "max_offer_items": 5,
        "min_order_items": 7,
        "max_order_items": 11,
        "required_orders": ["Component/SteelPlate"],
        "required_offers": ["Consumable/Fish"],
    },
    "University": {
        "min_offer_items": 99,
        "max_offer_items": 99,
        "min_order_items": 99,
        "max_order_items": 99,
        "offers": ["Component/AaW_Schematic_CeramicsFurnace", "Component/AaW_Schematic_MicroelectronicsFactory", "Component/AaW_Schematic_WireDrawer", "Component/AaW_Schematic_AutoLoom", "Component/AaW_Schematic_PlateStamp", "Component/AaW_Schematic_Extruder", "Component/AaW_Schematic_MunitionsFactory", "Component/AaW_Schematic_Fabricator", "Component/AaW_Schematic_Reactor", "Component/AaW_Schematic_JumpDrive"],
    },
    "Ingot": {
        "min_offer_items": 20,
        "max_offer_items": 20,
        "min_order_items": 20,
        "max_order_items": 20,
        "requires_ingot_orders": True,
        "requires_ingot_offers": True,
    },
    "Civ": {
        "min_offer_items": 5,
        "max_offer_items": 20,
        "min_order_items": 5,
        "max_order_items": 20,
        "offers": ["Tool/HandDrill2Item", "Tool/AngleGrinder2Item", "Tool/Welder2Item", "Consumable/Medkit", "Consumable/Powerkit", "Component/SteelPlate", "Component/Motor", "Component/Girder", "Component/InteriorPlate", "Component/Construction", "Component/Display", "Component/MedicalComponent", "Component/MetalGrid"],
        "io_extra_offers": ["Component/Cupper", "Component/Lightbulb", "Component/AcidPowerCell"],
    },
    "Scrap": {
        "min_offer_items": 5,
        "max_offer_items_normal": 20,
        "max_offer_items_io": 5,
        "min_order_items": 5,
        "max_order_items_normal": 5,
        "max_order_items_io": 5,
        "offers": ["Component/SteelPlate", "Component/Construction", "Component/Girder", "Component/InteriorPlate", "Component/SmallTube", "Component/LargeTube", "Component/BulletproofGlass", "Component/Motor", "Component/Display", "Component/Computer", "Component/RadioCommunicationComponent", "Component/Detector", "Component/Canvas", "Component/SolarCell", "Component/PowerCell", "Component/Explosives", "Component/MedicalComponent", "Component/ReactorComponent", "Component/MetalGrid", "Component/Thrust", "Component/GravityGenerator", "Component/Superconductor", "Component/ReinforcedDrillbit", "Component/ReinforcedPlate", "Component/LaserAmplifier"],
        "io_extra_offers": ["Component/FSSolarCell", "Component/ElectronMatrix", "Component/Capacitor", "Component/CompositeArmor", "Component/TokamakBlanket", "Component/SuperMagnet", "Component/Cryocooler", "Component/Thermocouple", "Component/ArmorGlass", "Component/Cupper", "Component/GoldWire", "Component/Rubber", "Component/Plastic", "Component/AdvancedComputer", "Component/QuantumComputer", "Component/Concrete", "Component/TitaniumPlate", "Component/ArmoredPlate", "Component/Electromagnet", "Component/Lightbulb", "Component/AcidPowerCell", "Component/AlkalinePowerCell", "Component/Ceramic", "Component/LaserEmitter", "Component/HeatingElement"],
    },
    "Ammo": {
        "min_offer_items": 5,
        "max_offer_items": 20,
        "min_order_items": 5,
        "max_order_items": 20,
        "offers": ["Ammo/NATO_25x184mm", "Ammo/missile", "Ammo/MediumCalibreAmmo", "Ammo/LargeCalibreAmmo", "Ammo/LargeRailgunAmmo", "Ammo/SmallRailgunAmmo", "Ammo/AutocannonClip"],
    },
    "Vending Machine": {
        "min_offer_items": 99,
        "max_offer_items": 99,
        "min_order_items": 99,
        "max_order_items": 99,
        "offers": ["Tool/HandDrill2Item", "Tool/HandDrill3Item", "Tool/AngleGrinder2Item", "Tool/AngleGrinder3Item", "Tool/Welder2Item", "Tool/Welder3Item", "Consumable/Medkit", "Consumable/Powerkit"],
    },
    "Outpost": {
        "min_offer_items": 5,
        "max_offer_items": 20,
        "min_order_items": 20,
        "max_order_items": 20,
        "orders": ["Component/CivilianProductI"],
        "offers": ["Tool/HandDrill2Item", "Tool/AngleGrinder2Item", "Tool/Welder2Item", "Consumable/Medkit", "Consumable/Powerkit", "HydrogenOffer", "Component/SteelPlate", "Component/Motor", "Component/Girder", "Component/InteriorPlate", "Component/Construction", "Component/Display"],
        "io_extra_offers": ["Component/Cupper", "Component/Lightbulb", "Component/AcidPowerCell"],
    },
}

# Faction-specific stores
FACTION_STORES = {
    "SHIVAN": {
        "min_offer_items": 5,
        "max_offer_items": 20,
        "min_order_items": 5,
        "max_order_items": 20,
        "min_offer_items_io": 20,
        "max_offer_items_io": 20,
        "min_order_items_io": 20,
        "max_order_items_io": 20,
        "offers": ["Consumable/SpiceInhaler"],
        "orders": ["Consumable/SpiceInhaler"],
    },
    "SECURITY": {
        "min_offer_items": 5,
        "max_offer_items": 20,
        "min_order_items": 5,
        "max_order_items": 20,
        "offers": ["Ammo/NATO_25x184mm", "Ammo/missile", "Ammo/MediumCalibreAmmo", "Ammo/LargeCalibreAmmo", "Ammo/LargeRailgunAmmo", "Ammo/SmallRailgunAmmo", "Ammo/AutocannonClip", "Tool/SemiAutoPistolItem", "Tool/FullAutoPistolItem", "Tool/MediumCalibreAmmo", "Tool/AutomaticRifleItem", "Tool/PreciseAutomaticRifleItem", "Tool/RapidFireAutomaticRifleItem"],
    },
    "AGURO": {
        "min_offer_items": 20,
        "max_offer_items": 20,
        "min_order_items": 20,
        "max_order_items": 20,
        "orders": ["Ingot/PurifiedSpice"],
    },
    "ITC": {
        "min_offer_items": 20,
        "max_offer_items": 20,
        "min_order_items": 20,
        "max_order_items": 20,
        "orders": ["Ingot/CrystalPrism", "Ingot/StabilizedCronyx", "Ingot/PurifiedSpice"],
        "offers": ["Ingot/DoriumIngot", "Component/ReinforcedDrillbit", "Component/ReinforcedPlate"],
    },
    "UNION": {
        "min_offer_items": 20,
        "max_offer_items": 20,
        "min_order_items": 20,
        "max_order_items": 20,
        "orders": ["Ammo/NATO_25x184mm", "Ammo/missile", "Ammo/MediumCalibreAmmo", "Ammo/LargeCalibreAmmo", "Ammo/LargeRailgunAmmo", "Ammo/SmallRailgunAmmo", "Ammo/AutocannonClip"],
    },
    "SOLCOOP": {
        "min_offer_items": 20,
        "max_offer_items": 20,
        "min_order_items": 20,
        "max_order_items": 20,
        "offers": ["Component/Thrust", "Component/GravityGenerator", "Component/Superconductor", "Component/JumpCore"],
    },
    "ZENOVA": {
        "min_offer_items": 5,
        "max_offer_items": 20,
        "min_order_items": 5,
        "max_order_items": 20,
        "offers": ["Component/Thrust", "Component/GravityGenerator", "Component/Superconductor", "Component/JumpCore"],
    },
    "AHE": {
        "min_offer_items": 5,
        "max_offer_items": 20,
        "min_order_items": 5,
        "max_order_items": 20,
        "offers": ["Component/Thrust", "Component/GravityGenerator", "Component/Superconductor", "Component/JumpCore", "Component/PowerCell", "Component/MedicalComponent", "Component/ReactorComponent"],
    },
    "MAYOR": {
        # Define MAYOR config if needed
    },
}

def get_store_config(store_type: str) -> dict:
    """Get configuration for a store type"""
    return STORE_TYPES.get(store_type, {})

def get_faction_config(faction: str) -> dict:
    """Get configuration for a faction store"""
    return FACTION_STORES.get(faction, {})
