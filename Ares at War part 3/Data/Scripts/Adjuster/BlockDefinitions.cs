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
                        Action = InsertComponent, // Inserts the given number of the given component at the given index of a block's component list
                        Component = "MyObjectBuilder_Component/Thrust",
                        Index = 4,
                        Count = 250
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
                        Action = InsertComponent, // Inserts the given number of the given component at the given index of a block's component list
                        Component = "MyObjectBuilder_Component/Thrust",
                        Index = 4,
                        Count = 1500
                    }
                }
            }
			
        };

    }
}
