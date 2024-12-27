using AaW.BlockExtensionsAPI;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.Components;
using VRage.Utils;

namespace PublicOverride
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class PublicOverrideCore : MySessionComponentBase
	{
        DefinitionExtensionsAPI defext;
		MyStringId PublicOverrideGroup = MyStringId.GetOrCompute("OverridePublic");
		MyStringId PublicID = MyStringId.GetOrCompute("Public");
		public PublicOverrideCore()
		{
			defext = new DefinitionExtensionsAPI(onEnabled);
		}

		private void onEnabled()
		{
			var defs = MyDefinitionManager.Static.GetAllDefinitions();
			foreach(var def in defs)
			{
				bool overridepublic = false;
				if(defext.TryGetBool(def.Id, PublicOverrideGroup, PublicID, out overridepublic))
				{
					//MyAPIGateway.Utilities.ShowMessage($"{overridepublic}", $"{def.Id}");
					def.Public = overridepublic;
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
