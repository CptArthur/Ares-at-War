using AresAtWar.BlockExtensionsAPI;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace PublicOverride
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class PublicOverrideCore : MySessionComponentBase
	{
        DefinitionExtensionsAPI defext;
		MyStringId PublicOverrideGroup = MyStringId.GetOrCompute("AaWShipyard");
		MyStringId PublicID = MyStringId.GetOrCompute("ShipyardId");
		public PublicOverrideCore()
		{
			defext = new DefinitionExtensionsAPI(onEnabled);
		}

		private void onEnabled()
		{
			MyAPIGateway.Utilities.ShowMessage("AaW block replacer", $"enabled");
			var defs = MyDefinitionManager.Static.GetAllDefinitions();


			foreach(var def in defs)
			{
				string overridepublic = "";
				if(defext.TryGetString(def.Id, PublicOverrideGroup, PublicID, out overridepublic))
				{
					//MyAPIGateway.Utilities.ShowMessage($"{overridepublic}", $"{def.Id}");

					MyCubeBlockDefinition cubeBlockDef = def as MyCubeBlockDefinition;
					if (cubeBlockDef != null)
					{
						MyCubeBlockDefinition.Component[] components = cubeBlockDef.Components;

						// New component to insert
						MyCubeBlockDefinition.Component newComponent = new MyCubeBlockDefinition.Component();

						if(cubeBlockDef.CubeSize == MyCubeSize.Large)
                        {
							newComponent.Count = 5;
						}
                        else
                        {
							newComponent.Count = 1;
						}
						var Scrap = MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Scrap"));
						newComponent.DeconstructItem = Scrap;


						var compdef = new MyComponentDefinition();


						if(MyDefinitionManager.Static.TryGetComponentDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Component), overridepublic), out compdef))
                        {
							newComponent.Definition = compdef;

							// Create a new array with an additional slot
							MyCubeBlockDefinition.Component[] updatedComponents = new MyCubeBlockDefinition.Component[components.Length + 1];

							// Copy elements before the insertion point
							Array.Copy(components, 0, updatedComponents, 0, 1);

							// Insert the new component at index 1
							updatedComponents[1] = newComponent;

							// Copy remaining elements
							Array.Copy(components, 1, updatedComponents, 2, components.Length - 1);


							cubeBlockDef.Components = updatedComponents;
						}


					}


				}
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			defext?.UnloadData();
			defext = null;
		}
	}
}
