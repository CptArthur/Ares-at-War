using System.Collections.Generic;
using static ModAdjuster.DefinitionStructure;
using static ModAdjuster.DefinitionStructure.BlockDef;
using static ModAdjuster.DefinitionStructure.BlockDef.BlockAction.BlockMod;

namespace ModAdjuster
{
    public class BlockDefinitions
    {
        public List<BlockDef> Definitions = new List<BlockDef>()
        {
            // List of blocks to modify. Can be as many or few as desired
           
			new BlockDef()
            {
                BlockName = "MyObjectBuilder_UpgradeModule/FSDriveSmall", // Name of the block to modify. Format is "MyObjectBuilder_Type/Subtype" in the same format as BlockVariantGroups
                BlockActions = new[] // List of modifications to make. Can be as many or few as desired
                {
                    new BlockAction
                    {
                        Action = ChangeComponentCount, // Changes the required number of the component at the given index
                        Index = 1,
                        Count = 40
                    },
                    new BlockAction
                    {
                        Action = ReplaceComponent, // Inserts the given number of the given component at the given index of a block's component list
                        Component = "MyObjectBuilder_Component/GoldWire",
                        Index = 2,
                        Count = 120
                    },
                    new BlockAction
                    {
                        Action = ChangeComponentCount, // Changes the required number of the component at the given index
                        Index = 4,
                        Count = 35
                    },
                    new BlockAction
                    {
                        Action = ChangeComponentCount, // Changes the required number of the component at the given index
                        Index = 5,
                        Count = 300
                    },
                    new BlockAction
                    {
                        Action = InsertComponent, // Inserts the given number of the given component at the given index of a block's component list
                        Component = "MyObjectBuilder_Component/Cryocooler",
                        Index = 5,
                        Count = 50
                    },
                    new BlockAction
                    {
                        Action = InsertComponent, // Inserts the given number of the given component at the given index of a block's component list
                        Component = "MyObjectBuilder_Component/JumpCore",
                        Index = 6,
                        Count = 45
                    },
                    new BlockAction
                    {
                        Action = ChangeComponentCount, // Changes the required number of the component at the given index
                        Index = 8,
                        Count = 10
                    },
                    new BlockAction
                    {
                        Action = InsertComponent, // Inserts the given number of the given component at the given index of a block's component list
                        Component = "MyObjectBuilder_Component/AdvancedComputer",
                        Index = 8,
                        Count = 100
                    }
                }
            },
           
			new BlockDef()
            {
                BlockName = "MyObjectBuilder_UpgradeModule/FSDriveLarge", // Name of the block to modify. Format is "MyObjectBuilder_Type/Subtype" in the same format as BlockVariantGroups
                BlockActions = new[] // List of modifications to make. Can be as many or few as desired
                {
                    new BlockAction
                    {
                        Action = ReplaceComponent, // Inserts the given number of the given component at the given index of a block's component list
                        Component = "MyObjectBuilder_Component/GoldWire",
                        Index = 2,
                        Count = 600
                    },
                    new BlockAction
                    {
                        Action = ChangeComponentCount, // Changes the required number of the component at the given index
                        Index = 4,
                        Count = 100
                    },
                    new BlockAction
                    {
                        Action = ChangeComponentCount, // Changes the required number of the component at the given index
                        Index = 5,
                        Count = 1500
                    },
                    new BlockAction
                    {
                        Action = InsertComponent, // Inserts the given number of the given component at the given index of a block's component list
                        Component = "MyObjectBuilder_Component/Cryocooler",
                        Index = 5,
                        Count = 250
                    },
                    new BlockAction
                    {
                        Action = InsertComponent, // Inserts the given number of the given component at the given index of a block's component list
                        Component = "MyObjectBuilder_Component/JumpCore",
                        Index = 6,
                        Count = 450
                    },
                    new BlockAction
                    {
                        Action = ChangeComponentCount, // Changes the required number of the component at the given index
                        Index = 8,
                        Count = 10
                    },
                    new BlockAction
                    {
                        Action = InsertComponent, // Inserts the given number of the given component at the given index of a block's component list
                        Component = "MyObjectBuilder_Component/AdvancedComputer",
                        Index = 8,
                        Count = 150
                    }
                }
            }
			
        };

    }
}
