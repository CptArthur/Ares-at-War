using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AaWSyncManager
{
	public static class ClientProcessing {

		public static void ProcessClipboard(SyncContainer container) {

			var msg = MyAPIGateway.Utilities.SerializeFromBinary<string>(container.Data);

			if (msg == null)
				return;
			if (string.IsNullOrEmpty(msg))
				return;

			VRage.Utils.MyClipboardHelper.SetClipboard(msg);

		}

	}
}
