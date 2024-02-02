using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Utils;
using VRageMath;

/*
 *  Mod by Rexxar. Developed for Doctor Octoganapus. All planet assets belong
 *  exclusively to the creator, I claim only this script.
 *  
 *  No license. Feel free to use this script however you want. All I ask is that
 *  you give credit and leave this entire comment block intact.
 *  If you're feeling especialy nice, you can donate at http://paypal.me/rexxar
 *  
 *  This mod is kind of complicated, so here's a general overview:
 *  
 *  Each planet mod has a copy of this script. Alter the first few settings in Config
 *  to suit each planet. Never, ever change the settings at the bottom of Config. Doing
 *  so will break interop with other versions of this script, and weird shit will happen.
 *  
 *  Each planet gets a GameLogicComponent attached to it. This component will look for
 *  entities within the planet's influence, calculate the required damage, then send 
 *  the results back to the session component.
 *  
 *  Each mod will have a copy of the session component, but only one may run at a time.
 *  During init, a message is sent out to everyone listening to INIT_ID. The first one
 *  to get the message wins. It removes its hook from INIT_INHIBIT, then sends out a 
 *  message with that ID. All components receiving that message will immediately and
 *  permanently disable themselves, to prevent duplicate damage.
 *  
 *  This system is so that many planets can report damage into one session component,
 *  which doles out damage slowly over the given timespan. The idea is to reduce server
 *  lag by not processing enormous amounts of damage all at once.
 */

namespace AtmosphericDamage
{
    //static class so that both components can access it

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class DamageCore : MySessionComponentBase
    {
        public static HashSet<MyPlanet> Planets = new HashSet<MyPlanet>();
        private static readonly Dictionary<IMySlimBlock, MyParticleEffect> _emitters = new Dictionary<IMySlimBlock, MyParticleEffect>();
        private readonly Random _random = new Random();
        private readonly Queue<KeyValuePair<IMyDestroyableObject, float>> _actionQueue = new Queue<KeyValuePair<IMyDestroyableObject, float>>();
        private int _actionsPerTick;
        private MyStringHash _damageHash;
        private bool _debug;
        private bool _disable;
        private bool _init;
        private bool _initSecondFrame;
        private bool _isServer;
        private bool _processing;
        private int _updateCount;

        private Vector4 color = Color.OrangeRed.ToVector4();
        private readonly List<LineD> lines = new List<LineD>();

        public override void Draw()
        {
            UpdateEmitters();
        }

        public override void UpdateBeforeSimulation()
        {
            try
            {
                if (MyAPIGateway.Session == null)
                    return;

                if (_disable)
                    return;

                if (_debug)
                    DrawDebug();

                if (!_init)
                {
                    Initialize();
                    return;
                }

                if (!_initSecondFrame)
                {
                    _initSecondFrame = true;
                    MyAPIGateway.Utilities.UnregisterMessageHandler(Config.INIT_INHIBIT_ID, InitInhibitHandler);
                    MyAPIGateway.Utilities.SendModMessage(Config.INIT_ID, true);
                    return;
                }

                ProcessQueue();

                if (++_updateCount % Config.UPDATE_RATE != 0)
                    return;

                var damageEntites = new Dictionary<IMyDestroyableObject, float>();
                MyAPIGateway.Utilities.SendModMessage(Config.DAMAGE_LIST_ID, damageEntites);

                if (_debug)
                    MyAPIGateway.Utilities.ShowMessage("Damage" + DateTime.Now.Millisecond, "received damage queue: " + damageEntites.Count);

                var emitterEntities = new Dictionary<IMySlimBlock, int>();
                MyAPIGateway.Utilities.SendModMessage(Config.PARTICLE_LIST_ID, emitterEntities);

                if (_debug)
                    MyAPIGateway.Utilities.ShowMessage("Emitters", "Receive " + emitterEntities.Count);

                CheckAndRemoveEmitters(emitterEntities);
                AddNewEmitters(emitterEntities);
                //Communication.SendEmitters(emitterEntities);

                List<KeyValuePair<IMyDestroyableObject, float>> list = damageEntites.ToList();
                list.Shuffle();
                foreach (KeyValuePair<IMyDestroyableObject, float> entry in list)
                {
                    if (_actionQueue.Count < Config.MAX_QUEUE)
                        _actionQueue.Enqueue(entry);
                    else
                        break;
                }
                _actionsPerTick = (int)Math.Ceiling((double)_actionQueue.Count / Config.UPDATE_RATE);

                lines.Clear();
            }
            catch (Exception ex)
            {
                MyAPIGateway.Utilities.ShowMessage("", ex.ToString());
                MyLog.Default.WriteLineAndConsole("##MOD:" + ex);
                //throw;
            }
        }

        public static void CheckAndRemoveEmitters(Dictionary<IMySlimBlock, int> newEmitters)
        {
            var toRemove = new HashSet<IMySlimBlock>();

            foreach (KeyValuePair<IMySlimBlock, MyParticleEffect> e in _emitters)
            {
                if (MyAPIGateway.Session.Camera != null)
                {
                    if (Vector3D.DistanceSquared(MyAPIGateway.Session.Camera.Position, e.Value.WorldMatrix.Translation) > 200 * 200)
                    {
                        toRemove.Add(e.Key);
                        MyParticlesManager.RemoveParticleEffect(e.Value);
                        continue;
                    }
                }

                if (!newEmitters.ContainsKey(e.Key))
                {
                    toRemove.Add(e.Key);
                    MyParticlesManager.RemoveParticleEffect(e.Value);
                }
            }

            foreach (IMySlimBlock r in toRemove)
                _emitters.Remove(r);
        }

        public static void AddNewEmitters(Dictionary<IMySlimBlock, int> newEmitters)
        {
            List<KeyValuePair<IMySlimBlock, int>> eList = newEmitters.Where(e => Vector3D.DistanceSquared(e.Key.GetPosition(), MyAPIGateway.Session.Camera.Position) < 100000).ToList();

            eList.Shuffle();

            //_effect.UserScale = 0.5f * Grid.GridSize;
            //Vector3 normal = -Vector3.Normalize(Grid.Physics.LinearVelocity);
            //MatrixD effectMatrix = MatrixD.CreateWorld(Grid.GridIntegerToWorld(kpair.Value), normal, Vector3.CalculatePerpendicularVector(normal));
            //effectMatrix.Translation = Grid.GridIntegerToWorld(kpair.Value);
            //_effect.WorldMatrix = effectMatrix;

            for (var index = 0; index < eList.Count; index++)
            {
                KeyValuePair<IMySlimBlock, int> e = eList[index];
                if (!_emitters.ContainsKey(e.Key))
                {
                    MyParticleEffect eff;
                    MyParticlesManager.TryCreateParticleEffect(e.Value, out eff);
                    var mat = new MatrixD();
                    mat.Translation = e.Key.GetPosition();

                    eff.WorldMatrix = mat;

                    // TODO: KEEN UPDATE
                    // eff.Start(e.Value, eff.Name);
                    if(e.Key.CubeGrid.GridSizeEnum == MyCubeSize.Small)
                        eff.UserEmitterScale = 0.1f;
                    _emitters.Add(e.Key, eff);
                }
            }
        }

        protected override void UnloadData()
        {
            MyAPIGateway.Utilities.MessageEntered -= Utilities_MessageEntered;
            MyAPIGateway.Utilities.UnregisterMessageHandler(Config.INIT_INHIBIT_ID, InitInhibitHandler);
            MyAPIGateway.Utilities.UnregisterMessageHandler(Config.INIT_ID, InitMessageHandler);
        }

        private void DrawDebug()
        {
            for (var i = 0; i < lines.Count; i++)
            {
                LineD line = lines[i];
                MySimpleObjectDraw.DrawLine(line.From, line.To, MyStringId.GetOrCompute("WeaponLaser"), ref color, 0.1f);
            }
        }

        private void Initialize()
        {
            _init = true;
            _isServer = MyAPIGateway.Session.OnlineMode == MyOnlineModeEnum.OFFLINE || MyAPIGateway.Multiplayer.IsServer;
            Communication.RegisterHandlers();
            Config.InitVoxelIDs();

            if (Config.UPDATE_RATE % 10 != 0)
                throw new Exception("UPDATE_RATE must be divisible by 10!");
            MyAPIGateway.Utilities.RegisterMessageHandler(Config.INIT_ID, InitMessageHandler);
            MyAPIGateway.Utilities.RegisterMessageHandler(Config.INIT_INHIBIT_ID, InitInhibitHandler);
        }

        private void InitInhibitHandler(object o)
        {
            if (o is bool && (bool)o)
                _disable = true;
        }

        private void Initialize_Continuation()
        {
            if (_disable)
                return;

            MyAPIGateway.Utilities.MessageEntered += Utilities_MessageEntered;
            _damageHash = MyStringHash.GetOrCompute(Config.DAMAGE_STRING);
        }

        private void HandleDrawQueue(object o)
        {
            //throw new NotImplementedException();
        }

        private void InitMessageHandler(object o)
        {
            if (!(o is bool))
                return;

            if ((bool)o)
            {
                MyAPIGateway.Utilities.SendModMessage(Config.INIT_INHIBIT_ID, o);
                Initialize_Continuation();
            }
        }

        private void Utilities_MessageEntered(string messageText, ref bool sendToOthers)
        {
            if (messageText == "!$debug")
                _debug = !_debug;
        }

        //spread our invoke queue over many updates to avoid lag spikes
        private void ProcessQueue()
        {
            //if (_debug)
            //    MyAPIGateway.Utilities.ShowMessage("Damage", "Processing " + _actionQueue.Count);
            if (_actionQueue.Count == 0)
                return;

            for (var i = 0; i < _actionsPerTick; i++)
            {
                KeyValuePair<IMyDestroyableObject, float> pair;
                if (!_actionQueue.TryDequeue(out pair))
                    return;
                try
                {
                    pair.Key.DoDamage(pair.Value, _damageHash, true);
                }
                catch
                {
                    //don't care
                }
            }
        }

        private void UpdateEmitters()
        {
            var toRemove = new HashSet<IMySlimBlock>();
            foreach (KeyValuePair<IMySlimBlock, MyParticleEffect> e in _emitters)
            {
                Vector3D pos = e.Key.GetPosition();

                bool closed = e.Key.Closed();
                bool zero = Vector3D.IsZero(pos);

                if (closed || zero)
                {
                    //if(zero)
                    //MyAPIGateway.Utilities.ShowMessage("Emitter", $"{closed}:{zero}");
                    toRemove.Add(e.Key);
                    e.Value.Stop();
                    MyParticlesManager.RemoveParticleEffect(e.Value);
                    continue;
                }
                MatrixD mat = e.Value.WorldMatrix;
                mat.Translation = pos;
                e.Value.WorldMatrix = mat;
            }

            foreach (IMySlimBlock r in toRemove)
                _emitters.Remove(r);
        }
    }
}
