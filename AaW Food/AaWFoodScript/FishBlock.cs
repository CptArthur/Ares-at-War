using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using SpaceEngineers.Game.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using Jakaria.API;

namespace AaWFoodScript
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_FunctionalBlock), false, "LargeBlockFishNet", "SmallBlockFishNet")]
    public class FishCollectorComponent : MyGameLogicComponent
    { 
        private IMyFunctionalBlock _fishCollector;
        private int count = 0;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            _fishCollector = Entity as IMyFunctionalBlock;
            if (MyAPIGateway.Session.IsServer)
            {
                NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;
            }
        }

        public override void UpdateAfterSimulation100()
        {
            if (count < 15)
            {
                count++;
                return;
            }


            if (!_fishCollector.HasInventory || !_fishCollector.IsWorking)
                return;

            Vector3D worldPosition = _fishCollector.PositionComp.GetPosition();

            if ((worldPosition - MapUtilities.agarisCenter).Length() > 60000)
                return;

            var Agaris = MyGamePruningStructure.GetClosestPlanet(worldPosition);

            //
            if (WaterAPI.IsUnderwater(worldPosition, Agaris) && MapUtilities.IsAtFishLocation(worldPosition))
            {

                var fuelId = new MyDefinitionId(typeof(MyObjectBuilder_ConsumableItem), "Fish");
                var content = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(fuelId);
                var fuelItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = content };


                IMyInventory inventory = _fishCollector.GetInventory();

                if (inventory != null)
                {
                    float amount = 1;

                    if (_fishCollector.CubeGrid.GridSizeEnum == MyCubeSize.Large)
                        inventory.AddItems((MyFixedPoint)amount, fuelItem.Content);
                    else
                        inventory.AddItems((MyFixedPoint)(amount), fuelItem.Content);

                    WaterAPI.CreateBubble(worldPosition, 2);

                }
            }
            count = 0;
        }

    }
}

