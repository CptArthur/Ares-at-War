using Sandbox.Game;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI;

namespace AaW.Factions
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class FriendlyFactions : MySessionComponentBase
    {
        int timer = 0;
        long wealthMaintain = 1000000000;
        public bool done = false;
        IMyPlayerCollection playerCollection;
        List<TwoFactions> listOfTwoFactions = new List<TwoFactions>();
        List<IMyFaction> neutralFactions = new List<IMyFaction>();

        class TwoFactions
        {
            public long Faction1ID;
            public long Faction2ID;

            public TwoFactions(IMyFaction factionA, IMyFaction factionB)
            {
                Faction1ID = factionA.FactionId;
                Faction2ID = factionB.FactionId;
            }
        }

        public override void BeforeStart()
        {
            var UNION = MyAPIGateway.Session.Factions.TryGetFactionByTag("UNION");
            var ITC = MyAPIGateway.Session.Factions.TryGetFactionByTag("ITC");
            var GC = MyAPIGateway.Session.Factions.TryGetFactionByTag("GC");
            var ROS = MyAPIGateway.Session.Factions.TryGetFactionByTag("R.O.S");

            var SHIVAN = MyAPIGateway.Session.Factions.TryGetFactionByTag("SHIVAN");
            var CRUSADERS = MyAPIGateway.Session.Factions.TryGetFactionByTag("CRUSADERS");
            var DRA = MyAPIGateway.Session.Factions.TryGetFactionByTag("DRA");




            //CIV
            var DOOHAN = MyAPIGateway.Session.Factions.TryGetFactionByTag("DOOHAN");
            var AZURIS = MyAPIGateway.Session.Factions.TryGetFactionByTag("AZURIS");
            var BRATIS = MyAPIGateway.Session.Factions.TryGetFactionByTag("BRATIS");
            var THORRIX = MyAPIGateway.Session.Factions.TryGetFactionByTag("THORRIX");
            var SUNSETCITY = MyAPIGateway.Session.Factions.TryGetFactionByTag("SUNSETCITY");
            var ARES = MyAPIGateway.Session.Factions.TryGetFactionByTag("ARES");
            var CIVILIAN = MyAPIGateway.Session.Factions.TryGetFactionByTag("CIVILIAN");
            var UNKN = MyAPIGateway.Session.Factions.TryGetFactionByTag("UNKN");
            var SHIPPERS = MyAPIGateway.Session.Factions.TryGetFactionByTag("SHIPPERS");

            // Return early if any required faction is missing
            if (UNION == null || THORRIX == null || ITC == null || GC == null || ROS == null || 
                DOOHAN == null || AZURIS == null || BRATIS == null || SHIVAN == null || ARES ==null 
                || CIVILIAN == null || CRUSADERS ==null || SHIPPERS ==null || DRA == null || SUNSETCITY == null)
            {
                MyVisualScriptLogicProvider.ShowNotificationToAll($"Error: Factions not found. Please save and reload the game", 50000, "Red");
                return;
            }


            // Neutral factions — will be made neutral with everyone
            neutralFactions.Add(DOOHAN);
            neutralFactions.Add(AZURIS);
            neutralFactions.Add(BRATIS);
            neutralFactions.Add(THORRIX);
            neutralFactions.Add(ARES);
            neutralFactions.Add(CIVILIAN);
            neutralFactions.Add(UNKN);
            neutralFactions.Add(SHIPPERS);
            neutralFactions.Add(SUNSETCITY);

            MakeNeutralWithAll(neutralFactions, new List<IMyFaction> { UNION, ITC, GC, ROS, SHIVAN, CRUSADERS, SHIPPERS });

            // Manually define other specific peace pairs
            AddPeacePair(UNKN, DRA); 
            AddPeacePair(SHIVAN, DRA);


            AddPeacePair(GC, ROS);
            AddPeacePair(ITC, GC);
            AddPeacePair(ITC, ROS);
            AddPeacePair(ITC, UNION);




        }

        private void AddPeacePair(IMyFaction a, IMyFaction b)
        {
            listOfTwoFactions.Add(new TwoFactions(a, b));
        }

        private void MakeNeutralWithAll(List<IMyFaction> neutrals, List<IMyFaction> others)
        {
            foreach (var neutral in neutrals)
            {
                foreach (var other in others)
                {
                    if (neutral.FactionId != other.FactionId)
                        listOfTwoFactions.Add(new TwoFactions(neutral, other));
                }

                foreach (var otherNeutral in neutrals)
                {
                    if (neutral.FactionId != otherNeutral.FactionId)
                        listOfTwoFactions.Add(new TwoFactions(neutral, otherNeutral));
                }
            }
        }

        public override void UpdateAfterSimulation()
        {
            if (!done)
            {
                if (MyAPIGateway.Session.IsServer)
                {
                    if (timer == 60)
                    {
                        foreach (var pair in listOfTwoFactions)
                            MyAPIGateway.Session.Factions.SendPeaceRequest(pair.Faction1ID, pair.Faction2ID);
                    }

                    if (timer == 70)
                    {
                        foreach (var pair in listOfTwoFactions)
                            MyAPIGateway.Session.Factions.AcceptPeace(pair.Faction1ID, pair.Faction2ID);

                        done = true;
                    }
                }

                timer += 1;
            }
        }
        
        protected override void UnloadData() { }
    }
}
