Hi there,

AaW progression uses the Research System Framework by jTurp.

You can familiarize yourself with that mod here:
https://steamcommunity.com/sharedfiles/filedetails/?id=2307665159

For reference, the progression framework saves its data here:

AppData\Roaming\SpaceEngineers\Saves\...\savename\Storage\2307665159.sbm_ProgressionFramework


Tip: BlockInfo.xml is very handy!

Tip: When switching between different progression mods, it’s recommended to delete this specific folder to reset the progression properly.


<========================================>
What are Schematics?

Schematics are special items (Components in game) that represent research unlocks in the Progression Framework.

When a player obtains a schematic, it unlocks one or more blocks in the tech tree for that player (or optionally for their entire faction, if faction sharing is enabled).

Each schematic is tied to a research group, and that group defines which item acts as the schematic and which blocks will be unlocked by it.

In other words, schematics unlocks specific blocks.

In AaW you can only buy schematics at the University of Sunset City. 

<========================================>
What are Placeholder Blocks?

Placeholder blocks are blocks that serve only as stand-ins. 

They are not meant to be used directly by players. Instead, they will be automatically replaced with their proper, non-placeholder counterparts through the ReplacementBlock option in the MES Shipyard.

When creating placeholders, make sure you are using the same size and center mount points as the original block. 

This can usually be done by starting from the original block or placing the placeholder next to it for alignment. For placeholders, I like to use the "building state" model as their appearance.

Additionally, I use icons to denote different categories:

  <Icon>Icons\gold.dds</Icon>   = SOLCOOP  
  <Icon>Icons\blue.dds</Icon>   = ITC  
  <Icon>Icons\red.dds</Icon>    = ZENOVA  
  <Icon>Icons\white.dds</Icon>  = Unlockable (via schematics)  
  <Icon>Icons\green.dds</Icon>  = Blocks that can be constructed at major settlements



<========================================>
Why is there a ModAdjuster folder?

Please see: https://steamcommunity.com/sharedfiles/filedetails/?id=3017795356

Adjust to your preference :)



<========================================>
Checklist adding new block that can be unlocked by via schematics.

() create Schematic Component (maybe adjust icon and price)
() create Schematic Blueprint
() add blocks to ReserachFrameWork.sbc 
() Add compontent id to Data\Stores&Shipyard\Univeristy_StoreProfile.sbc
() Add compontent id to Data\StoresItems\AaW_StoreItems_Schematics.xml


<========================================>
Checklist adding new block replacement shipyard stuff

() Add placeholder Block (maybe add custom color icon, and add block to _BlockCategories.sbc / G menu)
() Add placeholder, and original to Data\Stores&Shipyard\BlockReplacementProfiles.sbc


<========================================>
Checklist adding new block that can be unlocked by via schematics and shipyard.
See above checklist