using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using AresAtWar.Command;
using AresAtWar.Logging;
using AresAtWar.War;
namespace AaWSyncManager
{
	public static class ChatManager
	{

		public static void MessageEnteredDel(string messageText, ref bool sendToOthers)
		{

			var thisPlayer = MyAPIGateway.Session.LocalHumanPlayer;

			if (MyAPIGateway.Session.LocalHumanPlayer == null)
			{
				return;
			}




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
			var sendinfoback = false;
			var commandfound = false;
			var msg = MyAPIGateway.Utilities.SerializeFromBinary<string>(container.Data);
			var sb = new StringBuilder();


			var identity = MyAPIGateway.Players.TryGetIdentityId(sender);

			if (msg == null)
				return;

			if (msg.EndsWith(".Info", StringComparison.OrdinalIgnoreCase))
			{
				commandfound = true;
				sb = AaWCommand.SendAllInfo(sb);
				sendinfoback = true;
			}

			if (msg.EndsWith(".Item", StringComparison.OrdinalIgnoreCase))
			{
				commandfound = true;
				sb = AaWCommand.GetItemStuff();
				sendinfoback = true;
			}

			if (msg.EndsWith(".War", StringComparison.OrdinalIgnoreCase))
			{
				commandfound = true;
				sb = Logger.War;
				sendinfoback = true;
			}

			if (msg.EndsWith(".WarNextRound", StringComparison.OrdinalIgnoreCase))
			{
				commandfound = true;
				WarSim.SimulateWarRound();
				sendinfoback = false;
			}
			if (msg.EndsWith(".EnableStoryEvents", StringComparison.OrdinalIgnoreCase))
			{
				commandfound = true;
				MyVisualScriptLogicProvider.ShowNotification("Enabling Story Events", 5000, "Green", identity);
				MyAPIGateway.Utilities.SetVariable<bool>("AaWStoryEvents", true);
			}

			if (msg.EndsWith(".DisableStoryEvents", StringComparison.OrdinalIgnoreCase))
			{
				commandfound = true;
				MyVisualScriptLogicProvider.ShowNotification("Disabling Story Events", 5000, "Red", identity);
				MyAPIGateway.Utilities.SetVariable<bool>("AaWStoryEvents", false);
			}


			if (sb.ToString() == null)
				return;



			if (!commandfound)
            {
				MyVisualScriptLogicProvider.ShowNotification("AaW Command not found", 4000, "White", identity);
				return;
			}


			if (sendinfoback)
			{
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
}
