"""
Main Orchestrator
Reads CSV data and generates XML files.
"""

from enum import Enum
import ProfileBuilder as PB
import XMLBuilder as XB
import pandas as pd


# ============================================================================
# DATA STRUCTURES
# ============================================================================

class Score(Enum):
    ONE = 1
    TWO = 2
    THREE = 3
    FOUR = 4
    FIVE = 5


class ItemEnum(Enum):
    SUFFICE = 1
    SHORTAGE = 2
    BIG_SHORTAGE = 3


class ItemTypeEnum(Enum):
    COMPONENT = 1
    AMMO = 2
    TOOL = 3
    CONSUMABLE = 4


class Ore:
    """Represents an ore/resource"""
    
    def __init__(self, name: str, yield_rate: float):
        self.Name = name
        self.Yield = yield_rate
        self.Score = None
        self.scoreint = 0
    
    def add_score(self, score: int) -> "Ore":
        """Add score rating and register in appropriate lists"""
        score_map = {
            1: Score.ONE,
            2: Score.TWO,
            3: Score.THREE,
            4: Score.FOUR,
            5: Score.FIVE,
        }
        self.Score = score_map.get(score)
        self.scoreint = score
        return self
    
    def __str__(self) -> str:
        return self.Name


class Requirement:
    """Represents an ore requirement for an item"""
    
    def __init__(self, ore: Ore, amount: int):
        self.Ore = ore
        self.Amount = amount


class Item:
    """Represents a craftable item"""
    
    def __init__(self, item_type: ItemTypeEnum, name: str, requirements: list):
        self.type = item_type
        self.Name = name
        self.Requirements = requirements if requirements else []
        self.Class = self.classify()
    
    def classify(self) -> ItemEnum:
        """Classify item based on ore availability"""
        if not self.Requirements:
            return ItemEnum.SUFFICE
        
        total = 0
        total_missing = 0
        
        for req in self.Requirements:
            amount = round(req.Amount / req.Ore.Yield, 1)
            total += amount
            
            # Score difficulty factors
            score_factors = {
                Score.ONE: 0.0,
                Score.TWO: 0.1,
                Score.THREE: 0.3,
                Score.FOUR: 0.6,
                Score.FIVE: 0.8,
            }
            
            factor = score_factors.get(req.Ore.Score, 0)
            total_missing += factor * amount
        
        if total == 0:
            return ItemEnum.SUFFICE
        
        shortage_ratio = total_missing / total
        
        if shortage_ratio < 0.1:
            return ItemEnum.SUFFICE
        elif shortage_ratio < 0.5:
            return ItemEnum.SHORTAGE
        else:
            return ItemEnum.BIG_SHORTAGE
    
    def __str__(self) -> str:
        return self.Name


# ============================================================================
# HELPER FUNCTIONS
# ============================================================================

def create_vanilla_ores(row) -> dict:
    """Create vanilla game ores"""
    return {
        "Iron": Ore("Iron", 0.7).add_score(row['Iron']),
        "Nickel": Ore("Nickel", 4).add_score(row['Nickel']),
        "Silicon": Ore("Silicon", 0.7).add_score(row['Silicon']),
        "Cobalt": Ore("Cobalt", 0.3).add_score(row['Cobalt']),
        "Magnesium": Ore("Magnesium", 0.007).add_score(row['Magnesium']),
        "Silver": Ore("Silver", 0.1).add_score(row['Silver']),
        "Gold": Ore("Gold", 0.01).add_score(row['Gold']),
        "Platinum": Ore("Platinum", 0.005).add_score(row['Platinum']),
        "Uranium": Ore("Uranium", 0.01).add_score(row['Uranium']),
        "Lithium": Ore("Lithium", 0.2).add_score(row['Lithium']),
        "Sulfur": Ore("Sulfur", 0.3).add_score(row['Sulfur']),
        "CrystalPrism": Ore("CrystalPrism", 1).add_score(row['CrystalPrism']),
        "DoriumIngot": Ore("DoriumIngot", 1).add_score(row['DoriumIngot']),
        "StabilizedCronyx": Ore("StabilizedCronyx", 1).add_score(row['StabilizedCronyx']),
        "PurifiedSpice": Ore("PurifiedSpice", 1).add_score(row['PurifiedSpice']),
    }


def create_io_ores(row) -> dict:
    """Create IO mod ores (includes additional resources)"""
    ores = create_vanilla_ores(row)
    
    # Add IO-specific ores
    ores.update({
        "Copper": Ore("Copper", 0.6).add_score(row['Copper']),
        "Aluminum": Ore("Aluminum", 0.6).add_score(row['Aluminum']),
        "Titanium": Ore("Titanium", 0.4).add_score(row['Titanium']),
        "Carbon": Ore("Carbon", 0.75).add_score(row['Carbon']),
        "Polymer": Ore("Polymer", 0.3).add_score(row['Polymer']),
        "FuelOil": Ore("FuelOil", 0.45).add_score(row['FuelOil']),
        "Niter": Ore("Niter", 0.007).add_score(row['Niter']),
        "Tantalum": Ore("Tantalum", 0.01).add_score(row['Tantalum']),
    })
    
    return ores


def create_vanilla_items(ores: dict) -> tuple:
    """Create vanilla game items and return categorized lists"""
    items_by_class = {
        ItemEnum.SUFFICE: [],
        ItemEnum.SHORTAGE: [],
        ItemEnum.BIG_SHORTAGE: [],
    }
    
    all_items = []
    
    # Components (easiest)
    for name in ["SteelPlate", "Construction", "Girder", "InteriorPlate", "SmallTube", 
                 "LargeTube", "BulletproofGlass", "Motor", "Display", "Computer"]:
        item = Item(ItemTypeEnum.COMPONENT, name, [])
        items_by_class[item.Class].append(item)
        all_items.append(item)
    
    # More components with requirements
    item_defs = [
        (ItemTypeEnum.COMPONENT, "Explosives", [
            Requirement(ores["Magnesium"], 2),
            Requirement(ores["Silicon"], 0.5),
        ]),
        (ItemTypeEnum.COMPONENT, "MedicalComponent", [
            Requirement(ores["Iron"], 60),
            Requirement(ores["Silver"], 20),
            Requirement(ores["Nickel"], 70),
        ]),
        (ItemTypeEnum.COMPONENT, "MetalGrid", [
            Requirement(ores["Iron"], 12),
            Requirement(ores["Nickel"], 5),
            Requirement(ores["Cobalt"], 3),
        ]),
        (ItemTypeEnum.COMPONENT, "Thrust", [
            Requirement(ores["Iron"], 12),
            Requirement(ores["Platinum"], 0.4),
            Requirement(ores["Gold"], 1),
            Requirement(ores["Cobalt"], 10),
        ]),
        # Ammo
        (ItemTypeEnum.AMMO, "NATO_25x184mm", [
            Requirement(ores["Iron"], 40),
            Requirement(ores["Magnesium"], 3),
            Requirement(ores["Nickel"], 5),
        ]),
        (ItemTypeEnum.AMMO, "missile", [
            Requirement(ores["Iron"], 55),
            Requirement(ores["Magnesium"], 0.5),
            Requirement(ores["Platinum"], 0.04),
            Requirement(ores["Nickel"], 7),
            Requirement(ores["Uranium"], 0.1),
        ]),
        # Tools
        (ItemTypeEnum.TOOL, "HandDrill2Item", []),
        (ItemTypeEnum.TOOL, "SemiAutoPistolItem", []),
        # Consumables
        (ItemTypeEnum.CONSUMABLE, "Medkit", []),
        (ItemTypeEnum.CONSUMABLE, "SpiceInhaler", [Requirement(ores["PurifiedSpice"], 100)]),
    ]
    
    for item_type, name, requirements in item_defs:
        item = Item(item_type, name, requirements)
        items_by_class[item.Class].append(item)
        all_items.append(item)
    
    return items_by_class[ItemEnum.SUFFICE], items_by_class[ItemEnum.SHORTAGE], items_by_class[ItemEnum.BIG_SHORTAGE]


def parse_mission_ids(value) -> list:
    """Parse comma-separated mission IDs from CSV"""
    if isinstance(value, str):
        return value.split(',')
    return []


# ============================================================================
# MAIN PROCESSING
# ============================================================================

def process_zone(zone_row, encounters_df):
    """Process a single zone and its encounters"""
    
    xml_id = zone_row['ZoneName']
    is_io = zone_row['IO']
    xml_name = f"{xml_id}IO" if is_io else xml_id
    is_static = zone_row["Static"]
    
    print(f"\nProcessing zone: {xml_id} (IO={is_io}, Static={is_static})")
    
    # Create ores based on game type
    ores = create_io_ores(zone_row) if is_io else create_vanilla_ores(zone_row)
    ore_list = list(ores.values())
    
    # Create items and categorize
    suffix_items, short_items, big_short_items = create_vanilla_items(ores)
    
    # Generate XML for this zone
    if is_static:
        XB.CreateXml(xml_name, suffix_items, short_items, big_short_items)
    else:
        XB.CreateXml_Dynamic(xml_name, suffix_items, short_items, big_short_items)
    
    # Process encounters for this zone
    matching_encounters = encounters_df[
        (encounters_df["FileName"] == xml_id) & (encounters_df["IO"] == is_io)
    ]
    
    for enc_index, enc_row in matching_encounters.iterrows():
        encounter_id = enc_row['Name']
        if is_io:
            encounter_id = f"{encounter_id}IO"
        
        faction = enc_row['Faction']
        
        # Parse mission IDs
        mission_ids = parse_mission_ids(enc_row['MissionIds'])
        security_ids = parse_mission_ids(enc_row['SECURITY'])
        itc_ids = parse_mission_ids(enc_row['ITC'])
        shivan_ids = parse_mission_ids(enc_row['SHIVAN'])
        aguro_ids = parse_mission_ids(enc_row['AGURO'])
        zenova_ids = parse_mission_ids(enc_row['ZENOVA'])
        solcoop_ids = parse_mission_ids(enc_row['SOLCOOP'])
        mayor_ids = parse_mission_ids(enc_row['MAYOR'])
        
        # Create store items
        if is_static:
            PB.create_store_items(
                faction, encounter_id, xml_name,
                enc_row['StoreProfiles'], enc_row['TradeIngots'],
                ore_list, mission_ids, security_ids, itc_ids,
                shivan_ids, aguro_ids, zenova_ids, solcoop_ids, mayor_ids
            )
        else:
            PB.create_store_items_dynamic(
                faction, encounter_id, xml_name,
                enc_row['StoreProfiles'], enc_row['TradeIngots'],
                ore_list, mission_ids, security_ids, itc_ids,
                shivan_ids, aguro_ids, zenova_ids, solcoop_ids, mayor_ids
            )
        
        # Create triggers
        PB.create_triggers(faction, encounter_id, is_io, enc_row['StoreProfiles'])


def main():
    """Main entry point"""
    
    # Read CSV files
    zones_df = pd.read_csv('XML.csv', header=0)
    encounters_df = pd.read_csv('Encounters.csv', header=0)
    
    print("="*70)
    print("Starting store generation...")
    print("="*70)
    
    # Process each zone
    for zone_index, zone_row in zones_df.iterrows():
        try:
            process_zone(zone_row, encounters_df)
        except Exception as e:
            print(f"ERROR processing zone {zone_row['ZoneName']}: {e}")
            raise
    
    print("\n" + "="*70)
    print("Store generation complete!")
    print("="*70)


if __name__ == "__main__":
    main()
    input("Done - Press Enter to exit")
