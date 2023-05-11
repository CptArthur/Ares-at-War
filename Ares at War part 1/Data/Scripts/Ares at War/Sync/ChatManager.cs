using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using AresAtWar.Command;
using AresAtWar.Init;

namespace SimpleSyncManager
{
	public static class ChatManager
	{

		public static void MessageEnteredDel(string messageText, ref bool sendToOthers)
		{

			if (!messageText.StartsWith("/AaW."))
				return;
			sendToOthers = false;

			var syncContainer = new SyncContainer();
			syncContainer.Mode = SyncMode.ChatMessage;
			syncContainer.Data = MyAPIGateway.Utilities.SerializeToBinary<string>(messageText);
			syncContainer.Sender = "AaW";
			SyncManager.SendNetworkData(syncContainer, 0, true, false);

		}

		public static void ServerChatProcessing(SyncContainer container, ulong sender)
		{

			var hasinfo = false;
			var msg = MyAPIGateway.Utilities.SerializeFromBinary<string>(container.Data);
			var sb = new StringBuilder();


			if (msg == null)
				return;

			if (msg.EndsWith(".Info", StringComparison.OrdinalIgnoreCase))
			{
				hasinfo = true;
				sb = AaWCommand.SendAllInfo(sb, AaWMain.listOfFactions);

			}

			if (msg.EndsWith(".Item", StringComparison.OrdinalIgnoreCase))
			{
				hasinfo = true;
				sb = AaWCommand.GetItemStuff();

			}










			if (sb.ToString() == null)
				return;



			var identity = MyAPIGateway.Players.TryGetIdentityId(sender);

			if (!hasinfo)
            {
				MyVisualScriptLogicProvider.ShowNotification("AaW Command not found", 4000, "White", identity);
				return;
			}
				
			var newContainer = new SyncContainer();
			newContainer.Mode = SyncMode.Clipboard;
			newContainer.Data = MyAPIGateway.Utilities.SerializeToBinary<string>(sb.ToString());
			newContainer.Sender = "AaW";



			if (identity > 0)
				MyVisualScriptLogicProvider.ShowNotification("Data Sent To Clipboard", 4000, "White", identity);
			SyncManager.SendNetworkData(newContainer, sender);

		} 

	}
}
