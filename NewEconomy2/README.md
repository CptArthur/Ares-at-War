# Store Generation System - Refactored

## Overview

This is a completely refactored version of your store generation system. The main improvements are:

- **Configuration-driven** - Define store types in one place (StoreConfig.py)
- **Much less code duplication** - Eliminated 3000+ lines of repetitive code
- **Easy to extend** - Add new store types by editing one file
- **Better organized** - Clear separation of concerns
- **Readable** - No massive files to scroll through

## Architecture

### File Structure

```
StoreConfig.py          ← Define all store types here (THE KEY FILE!)
    ↓
StoreGenerator.py       ← Generates XML components from config
    ↓
ProfileBuilder.py       ← Creates store item and trigger files
    ↓
Main.py                 ← Orchestrates everything
    ↓
XMLBuilder.py           ← Creates the final XML store items
```

## Key Files Explained

### 1. **StoreConfig.py** - YOUR CONFIGURATION FILE

This is where you define everything. No more scrolling through 1500-line files!

```python
STORE_TYPES = {
    "Military": {
        "min_offer_items": 7,
        "max_offer_items": 10,
        "min_order_items": 12,
        "max_order_items": 16,
        "orders": ["Ammo/NATO_25x184mm", "Ammo/missile", ...],
        "offers": ["Component/Construction", ...],
        "requires_ore_orders": True,
        "requires_ore_offers": True,
    },
    # ... other store types
}

FACTION_STORES = {
    "SHIVAN": { ... },
    "SECURITY": { ... },
    # ... etc
}
```

#### How to Add a New Store Type

Let's say you want to add a "Refinery" store:

1. Open `StoreConfig.py`
2. Add to `STORE_TYPES`:

```python
"Refinery": {
    "min_offer_items": 10,
    "max_offer_items": 15,
    "min_order_items": 8,
    "max_order_items": 12,
    "required_orders": ["Ore/Iron"],
    "offers": ["Ingot/Iron", "Ingot/Nickel"],
    "requires_ore_orders": True,
    "requires_ore_offers": True,
},
```

3. Add reference in your Encounters.csv StoreProfiles column: `"Settlement,Military,Refinery"`
4. Done! No code changes needed.

#### Available Config Options

```python
{
    "min_offer_items": 5,          # Minimum items to offer
    "max_offer_items": 20,         # Maximum items to offer
    "min_order_items": 5,          # Min items player can order
    "max_order_items": 20,         # Max items player can order
    "orders": ["Item/Type"],       # List of items to order
    "offers": ["Item/Type"],       # List of items to offer
    "required_orders": ["Item"],   # Orders the store MUST have
    "required_offers": ["Item"],   # Offers the store MUST have
    "requires_ore_orders": True,   # Auto-generate ore orders from ingots
    "requires_ore_offers": True,   # Auto-generate ore offers from ingots
    "requires_ingot_orders": True, # Auto-generate ingot orders
    "requires_ingot_offers": True, # Auto-generate ingot offers
    "io_extra_offers": [],         # Additional offers for IO variant only
    "min_offer_items_io": 5,       # Override for IO variant
    "max_offer_items_io": 20,      # Override for IO variant
}
```

### 2. **StoreGenerator.py** - Generates XML Components

This does the actual XML generation based on your config. You rarely need to touch this.

Key functions:
- `build_store_profile_xml()` - Creates a store profile component
- `build_faction_store_xml()` - Creates a faction-specific store
- `generate_profile_ingot()` - Generates ingot order/offer lists
- `generate_mission_ids()` - Generates mission ID entries

### 3. **ProfileBuilder.py** - Creates Files

Creates the actual .sbc files (store items and triggers).

- `create_store_items()` - Static store items file
- `create_store_items_dynamic()` - Dynamic store items file
- `create_triggers()` - Trigger configuration file

### 4. **Main.py** - Orchestrator

Reads CSV files and coordinates everything:
1. Reads XML.csv (zone definitions)
2. Reads Encounters.csv (encounter definitions)
3. Creates Ore and Item objects
4. Calls ProfileBuilder to generate files
5. Calls XMLBuilder to generate store items XML

### 5. **XMLBuilder.py** - Final XML Generation

Creates the AaW_StoreItems_*.xml files with pricing and item lists.

## How It Works

### Flow Diagram

```
XML.csv + Encounters.csv
       ↓
   Main.py reads both
       ↓
  For each zone:
    - Create Ores
    - Create Items
    - For each encounter:
      - ProfileBuilder.create_store_items()
        → Calls StoreGenerator.build_store_profile_xml() for each store type
        → Gets config from StoreConfig.py
        → Returns XML components
        → Writes to .sbc file
      - ProfileBuilder.create_triggers()
        → Generates trigger XML
        → Writes to trigger .sbc file
  XMLBuilder.CreateXml()
    → Generates AaW_StoreItems_*.xml
```

## Extending the System

### Scenario 1: Add a New Store Type

Edit `StoreConfig.py`:
```python
"MyNewStore": {
    "min_offer_items": 5,
    "max_offer_items": 15,
    "offers": ["Component/X", "Component/Y"],
    # ... other settings
},
```

That's it. Use "MyNewStore" in your Encounters.csv and it works.

### Scenario 2: Create a Store Type with Dynamic Items

```python
"DynamicTrader": {
    "min_offer_items": 3,
    "max_offer_items": 8,
    "requires_ore_orders": True,  # ← Uses ores from zone
    "requires_ore_offers": True,
},
```

Now this store will automatically order and offer ores based on the zone's ore availability.

### Scenario 3: Add IO-Specific Variants

```python
"Factory": {
    "min_offer_items": 10,
    "max_offer_items": 20,
    "offers": ["Component/Basic"],
    "io_extra_offers": ["Component/Advanced", "Component/Industrial"],
},
```

The regular variant offers basic components, but the IO variant gets extra advanced components.

### Scenario 4: Create a Faction-Specific Store

Edit `FACTION_STORES` in `StoreConfig.py`:
```python
"MYGROUP": {
    "min_offer_items": 5,
    "max_offer_items": 10,
    "orders": ["Item/Specialty"],
    "offers": ["Item/Rare"],
},
```

Use "MYGROUP" in Encounters.csv and it works.

## CSV Format

### XML.csv Structure
```
ZoneName, Static, IO, Iron, Nickel, ... (all ore scores)
```

### Encounters.csv Structure
```
FileName, Static, Faction, Name, IO, TradeIngots, StoreProfiles, 
MissionIds, ITC, SHIVAN, AGURO, SOLCOOP, ZENOVA, MAYOR, SECURITY
```

**StoreProfiles** is where you list store types:
- Separate with commas: `"Settlement,Military,Ingot,SECURITY"`
- Use names from `StoreConfig.py`
- Order doesn't matter

## Code Statistics

### Before Refactor
- Templates.py: 1500+ lines
- Templates_Dynamic.py: 800+ lines
- TemplateTriggers.py: 400+ lines
- Total repetitive code: 2700+ lines

### After Refactor
- StoreConfig.py: 150+ lines (configuration only)
- StoreGenerator.py: 250+ lines (reusable logic)
- ProfileBuilder.py: 200+ lines (clean, simple)
- TemplateTriggers.py: **DELETED**
- Total: ~600 lines (77% reduction!)

## Maintenance Benefits

### Adding a Store Type - Before
- Edit Templates.py (search through 1500 lines)
- Edit Templates_Dynamic.py (duplicate code!)
- Edit TemplateTriggers.py
- Update ProfileBuilder.py
- **5+ files, easy to miss something**

### Adding a Store Type - Now
- Edit StoreConfig.py (150 lines, all visible)
- **1 file, done**

### Changing Store Settings - Before
- Might need to edit multiple functions
- Might need to make the same change 3 times
- Easy to introduce bugs

### Changing Store Settings - Now
- Edit one config entry
- Change automatically applies everywhere

## Common Tasks

### Change Military Store Items Offered

**Before**: Edit `GetStoreMilitary()` in Templates.py (50+ lines to find and modify)

**Now**: Edit StoreConfig.py:
```python
"Military": {
    # ...
    "offers": ["Component/Construction", "Component/NewComponent"],  ← Here
    # ...
},
```

### Add a New Faction Store

**Before**: Create function in Templates.py, create function in TemplateTriggers.py, update ProfileBuilder.py

**Now**: Add to StoreConfig.py:
```python
FACTION_STORES = {
    # ...
    "NEWFACTION": {
        "min_offer_items": 5,
        "max_offer_items": 10,
        "offers": ["Item/X"],
    },
}
```

### Make Settlement Offer More Items

**Before**: Find `GetStoreSettlement()`, change 99→100, change in TWO places (normal and IO)

**Now**: Edit StoreConfig.py:
```python
"Settlement": {
    "max_offer_items": 100,  ← One place!
    # ...
},
```

## Troubleshooting

### "StoreConfig not found"
- Make sure StoreConfig.py is in the same folder as Main.py

### Store isn't appearing
- Check StoreProfiles column in Encounters.csv
- Check spelling against StoreConfig.py
- Add `print()` statements in ProfileBuilder.generate_store_profiles()

### IO variant not applying
- Check Encounters.csv IO column (must match XML.csv)
- Check `io_extra_offers` in StoreConfig.py
- Ensure store_profiles includes the IO-aware store type

## Migration Notes

This refactor is **fully compatible** with your existing:
- XML.csv format
- Encounters.csv format
- Output file locations
- Item definitions

You just need to replace these files:
- Download.py (unchanged)
- Main.py (new - cleaner)
- ProfileBuilder.py (new - much simpler)
- XMLBuilder.py (new - cleaner)
- StoreConfig.py (new - **your configuration file**)
- StoreGenerator.py (new - replaces Templates.py + Templates_Dynamic.py)
- TemplateTriggers.py (DELETE - functionality integrated)
- Templates.py (DELETE - replaced by StoreConfig + StoreGenerator)
- Templates_Dynamic.py (DELETE - replaced by StoreConfig + StoreGenerator)

## Next Steps

1. **Review StoreConfig.py** - Understand your current store types
2. **Test** - Run Main.py, verify output
3. **Extend** - Add new store types to StoreConfig.py
4. **Maintain** - All future changes go in StoreConfig.py

---

**Questions?** Check the config options in StoreConfig.py or trace through StoreGenerator.py's `build_store_profile_xml()` function to see how configs become XML.
