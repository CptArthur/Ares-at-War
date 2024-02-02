using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace AtmosphericDamage
{
    public static class Communication
    {
        public enum MessageType : byte
        {
            Particles,
            Rain
        }

        public static void RegisterHandlers()
        {
            MyAPIGateway.Multiplayer.RegisterMessageHandler(Config.NETWORK_ID, ReceiveHandler);
        }

        public static void UnregisterHandlers()
        {}

        public static void SendEmitters(Dictionary<IMySlimBlock, int> emitters)
        {
            var players = new List<IMyPlayer>();
            MyAPIGateway.Players.GetPlayers(players);

            foreach (IMyPlayer player in players)
            {
                Vector3D pos = player.GetPosition();
                List<KeyValuePair<IMySlimBlock, int>> eList = emitters.Where(e => Vector3D.DistanceSquared(pos, e.Key.GetPosition()) < Config.EMITTER_DRAW_DIST).ToList();
                if (!eList.Any()) //no emitters visible to player
                    continue;

                if (eList.Count > 150)
                {
                    eList.Shuffle();
                    eList = eList.GetRange(0, 150);
                }
                var byteList = new List<byte[]>(eList.Count);
                foreach (KeyValuePair<IMySlimBlock, int> e in eList)
                {
                    var em = new Emitter(e.Value, e.Key);
                    byteList.Add(MyAPIGateway.Utilities.SerializeToBinary(em));
                }

                var message = new List<byte>();
                message.Add((byte)MessageType.Particles);
                message.Add((byte)byteList[0].Length);

                //byte[] message = new byte[byteList.Sum( b => b.Length) + 2];
                //message[0] = (byte)MessageType.Particles;
                //message[1] = (byte)byteList[0].Length;

                //for (int i = 0; i < byteList.Count; i++)
                //{
                //    var c = byteList[i];
                //    Array.Copy(c, 0, message, 2 + c.Length * i, c.Length);
                //}

                foreach (byte[] b in byteList)
                    message.AddRange(b);

                MyAPIGateway.Multiplayer.SendMessageTo(Config.NETWORK_ID, message.ToArray(), player.SteamUserId);
            }
        }

        private static void ReceiveHandler(byte[] data)
        {
            var type = (MessageType)data[0];

            switch (type)
            {
                case MessageType.Particles:
                    HandleEmitters(data);
                    break;
                case MessageType.Rain:
                    break;
                default: //received message from incompatible version, or other mod. Ignore.
                    return;
            }
        }

        private static void HandleEmitters(byte[] data)
        {
            byte lengh = data[1];

            var emitters = new List<Emitter>();

            for (var i = 2; i < data.Length; i += lengh)
            {
                var em = new byte[lengh];
                Array.Copy(data, i, em, 0, lengh);

                var emitter = MyAPIGateway.Utilities.SerializeFromBinary<Emitter>(em);
                emitters.Add(emitter);
            }

            var emitterDic = new Dictionary<IMySlimBlock, int>();

            foreach (Emitter emitter in emitters)
            {
                IMyEntity entity;
                MyAPIGateway.Entities.TryGetEntityById(emitter.GridId, out entity);

                if (entity == null)
                    MyAPIGateway.Utilities.ShowMessage(emitter.GridId.ToString(), "NULL GRID");
                var pos = new Vector3I(emitter.PosX, emitter.PosY, emitter.PosZ);
                IMySlimBlock block = (entity as IMyCubeGrid)?.GetCubeBlock(pos);

                if (block == null)
                {
                    MyAPIGateway.Utilities.ShowMessage(pos.ToString(), "NULL BLOCK");
                    continue;
                }

                emitterDic.Add(block, emitter.EffectId);
            }

            DamageCore.CheckAndRemoveEmitters(emitterDic);
            DamageCore.AddNewEmitters(emitterDic);
        }

        [ProtoContract]
        public class Emitter
        {
            [ProtoMember(1)]
            public int EffectId { get; set; } //4B

            [ProtoMember(2)]
            public long GridId { get; set; } //8B

            [ProtoMember(3)]
            public int PosX { get; set; } //4B

            [ProtoMember(4)]
            public int PosY { get; set; } //4B

            [ProtoMember(5)]
            public int PosZ { get; set; } //4B

            public Emitter(int effectId, long gridId, Vector3I pos)
            {
                EffectId = effectId;
                GridId = gridId;
                PosX = pos.X;
                PosY = pos.Y;
                PosZ = pos.Z;
            }

            public Emitter(int effectId, IMySlimBlock block)
            {
                EffectId = effectId;
                GridId = block.CubeGrid.EntityId;
                Vector3I pos = block.Position;
                PosX = pos.X;
                PosY = pos.Y;
                PosZ = pos.Z;
            }
        }
    }
}
