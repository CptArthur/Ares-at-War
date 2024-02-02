using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

[MyEntityComponentDescriptor(typeof(MyObjectBuilder_ShipWelder), false, "LargeShipLaserWelder", "SmallShipLaserWelder")]
	 
public class AdvancedShipWelder : UpgradedShipTool
{		
    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
        base.Init(objectBuilder);

        CanAffectOwnGrid = true;

        if (((IMyCubeBlock)Entity).CubeGrid.GridSizeEnum == MyCubeSize.Large)
        {
            ToolSphereOffset = 3.0f;
            ToolSphereRadius = 3.0f;
        }
        else
        {
            ToolSphereOffset = 2.0f;
            ToolSphereRadius = 2.0f;
        }

    }

    protected override void Action(float AdvancedMultiplier, BoundingSphereD sphere, IMyCubeGrid targetGrid, IMyCubeBlock welderBlock)
    {
        var inventory = ((MyEntity)welderBlock).GetInventory();
        bool isHelping = ((IMyShipWelder)welderBlock).HelpOthers;
        var amount = MyAPIGateway.Session.WelderSpeedMultiplier * AdvancedMultiplier;
        var blocks = targetGrid.GetBlocksInsideSphere(ref sphere);
        foreach (var block in blocks)
            block.IncreaseMountLevel(amount, welderBlock.OwnerId, inventory, 0.6f, isHelping);
    }
}

[MyEntityComponentDescriptor(typeof(MyObjectBuilder_ShipGrinder), false, "LargeShipLaserGrinder", "SmallShipLaserGrinder")]
public class AdvancedShipGrinder : UpgradedShipTool
{
    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
        base.Init(objectBuilder);

        CanAffectOwnGrid = false;

        if (((IMyCubeBlock)Entity).CubeGrid.GridSizeEnum == MyCubeSize.Large)
        {
            ToolSphereOffset = 3.0f;
            ToolSphereRadius = 3.0f;
        }
        else
        {
            ToolSphereOffset = 2.0f;
            ToolSphereRadius = 2.0f;
        }
    }

    protected override void Action(float AdvancedMultiplier, BoundingSphereD sphere, IMyCubeGrid targetGrid, IMyCubeBlock thisBlock)
    {
        var inventory = ((MyEntity)thisBlock).GetInventory();
        var amount = MyAPIGateway.Session.GrinderSpeedMultiplier * AdvancedMultiplier;
        var blocks = targetGrid.GetBlocksInsideSphere(ref sphere);
        foreach (var block in blocks)
            block.DecreaseMountLevel(amount, inventory);
    }
}


public abstract class UpgradedShipTool : MyGameLogicComponent
{

    protected float ToolSphereOffset = 4f;
    protected float ToolSphereRadius = 1.6f;

    protected bool CanAffectOwnGrid = true;
	
    protected float AdvancedMultiplier;

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
        NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        
        AdvancedMultiplier = 0.5f;

    }

    public override void UpdateBeforeSimulation10()
    {
        if (((Entity as IMyGunObject<MyToolBase>).IsShooting || (Entity as IMyFunctionalBlock).Enabled))
        {
            //List of grids that are in range of the welder
            List<IMyEntity> potentialGrids2;
            BoundingSphereD ToolSphere = new BoundingSphereD(Entity.GetPosition() + Entity.WorldMatrix.Forward * ToolSphereOffset, ToolSphereRadius);
            
            potentialGrids2 = MyAPIGateway.Entities.GetTopMostEntitiesInSphere(ref ToolSphere);

            //If tool is a grinder, and own grid is in the list, remove it.
            if (!CanAffectOwnGrid && potentialGrids2.Contains(((IMyCubeBlock)Entity).CubeGrid))
                potentialGrids2.Remove(((IMyCubeBlock)Entity).CubeGrid);

            foreach (IMyEntity e2 in potentialGrids2)
                if (e2 is IMyCubeGrid && e2.Physics != null) {
					
                    Action(AdvancedMultiplier, ToolSphere, (IMyCubeGrid)e2, (IMyCubeBlock)Entity);
					
                }
        }
    }
	
    protected abstract void Action(float AdvancedMultiplier, BoundingSphereD sphere, IMyCubeGrid targetGrid, IMyCubeBlock thisBlock);
}


