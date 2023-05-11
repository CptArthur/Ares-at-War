using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSyncManager {

	public enum SyncMode {
	
		None,
		ChatMessage,
		Clipboard,
		Audio,
	
	}

	[ProtoContract]
	public class SyncContainer {

		[ProtoMember(1)] public SyncMode Mode;
		[ProtoMember(2)] public byte[] Data;
		[ProtoMember(3)] public string Sender;

		public SyncContainer() {

			Mode = SyncMode.None;
			Data = null;
			Sender = null;

		}

	}
}
