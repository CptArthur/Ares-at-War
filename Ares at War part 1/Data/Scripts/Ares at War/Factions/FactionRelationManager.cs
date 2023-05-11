using System;
using System.Collections.Generic;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRageMath;


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

        private void IncreaseBalance()
        {
            if (playerCollection != null)
            {
                //playerCollection.RequestChangeBalance(mainF.FounderId, wealthMaintain);
            }
        }
  
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
            var FAF = MyAPIGateway.Session.Factions.TryGetFactionByTag("FAF");
            if (FAF == null)
                return;

            var AHE = MyAPIGateway.Session.Factions.TryGetFactionByTag("AHE");
            if (AHE == null)
                return;
            var ITC = MyAPIGateway.Session.Factions.TryGetFactionByTag("ITC");
            if (ITC == null)
                return;
            var GGC = MyAPIGateway.Session.Factions.TryGetFactionByTag("GC");
            if (GGC == null)
                return;

            var ROS = MyAPIGateway.Session.Factions.TryGetFactionByTag("ROS");
            if (ROS == null)
                return;

            var COL = MyAPIGateway.Session.Factions.TryGetFactionByTag("COL-A");
            if (COL == null)
                return;

            var COLT4 = MyAPIGateway.Session.Factions.TryGetFactionByTag("COL-T4");
            if (COLT4 == null)
                return;

            var COLB = MyAPIGateway.Session.Factions.TryGetFactionByTag("COL-B");
            if (COLB == null)
                return;

            var SYN = MyAPIGateway.Session.Factions.TryGetFactionByTag("SYN");
            if (SYN == null)
                return;

            //AHE, FAF

            var AHE_FAF = new TwoFactions(AHE, FAF);
            listOfTwoFactions.Add(AHE_FAF);


            //GGC&ROS
            var GGC_ROS = new TwoFactions(GGC,ROS);
            listOfTwoFactions.Add(GGC_ROS);

            var ITC_GGC = new TwoFactions(ITC,GGC);
            listOfTwoFactions.Add(ITC_GGC);

            var ITC_ROS = new TwoFactions(ITC,ROS);
            listOfTwoFactions.Add(ITC_ROS);

            var ITC_FAF = new TwoFactions(ITC,FAF);
            listOfTwoFactions.Add(ITC_FAF);

            var ITC_AHE = new TwoFactions(ITC,AHE);
            listOfTwoFactions.Add(ITC_ROS);
            

            var COL_GGC = new TwoFactions(COL,GGC);
            listOfTwoFactions.Add(COL_GGC);

            var COL_ROS = new TwoFactions(COL,ROS);
            listOfTwoFactions.Add(COL_ROS);

            var COL_ITC = new TwoFactions(COL,ITC);
            listOfTwoFactions.Add(COL_ITC);

            var COL_FAF = new TwoFactions(COL, FAF);
            listOfTwoFactions.Add(COL_FAF);

            var COL_AHE = new TwoFactions(COL, AHE);
            listOfTwoFactions.Add(COL_AHE);

            var COL_SYN = new TwoFactions(COL, SYN);
            listOfTwoFactions.Add(COL_SYN);
            


            //Bylen
            var COLB_GGC = new TwoFactions(COLB, GGC);
            listOfTwoFactions.Add(COLB_GGC);

            var COLB_ROS = new TwoFactions(COLB, ROS);
            listOfTwoFactions.Add(COLB_ROS);

            var COLB_ITC = new TwoFactions(COLB, ITC);
            listOfTwoFactions.Add(COLB_ITC);

            var COLB_FAF = new TwoFactions(COLB, FAF);
            listOfTwoFactions.Add(COLB_FAF);

            var COLB_AHE = new TwoFactions(COLB, AHE);
            listOfTwoFactions.Add(COLB_AHE);

            var COLB_SYN = new TwoFactions(COLB, SYN);
            listOfTwoFactions.Add(COLB_SYN);




            //T4
            var COLT4_GGC = new TwoFactions(COLT4, GGC);
            listOfTwoFactions.Add(COLT4_GGC);

            var COLT4_ROS = new TwoFactions(COLT4, ROS);
            listOfTwoFactions.Add(COLT4_ROS);

            var COLT4_ITC = new TwoFactions(COLT4, ITC);
            listOfTwoFactions.Add(COLT4_ITC);

            var COLT4_FAF = new TwoFactions(COLT4, FAF);
            listOfTwoFactions.Add(COLT4_FAF);

            var COLT4_AHE = new TwoFactions(COLT4, AHE);
            listOfTwoFactions.Add(COLT4_AHE);

            var COLT4_SYN = new TwoFactions(COLT4, SYN);
            listOfTwoFactions.Add(COLT4_SYN);
        }

        public override void UpdateAfterSimulation()
        {
            if (!done)
            {
                if (MyAPIGateway.Session.IsServer)
                {
                    if (timer == 60)
                    {
                        for (int i = 0; i < listOfTwoFactions.Count; i++)
                        {
                            MyAPIGateway.Session.Factions.SendPeaceRequest(listOfTwoFactions[i].Faction1ID, listOfTwoFactions[i].Faction2ID);
                        }



                    }
                    if (timer == 70)
                    {
                        for (int i = 0; i < listOfTwoFactions.Count; i++)
                        {
                            MyAPIGateway.Session.Factions.AcceptPeace(listOfTwoFactions[i].Faction1ID, listOfTwoFactions[i].Faction2ID);
                        }
                        done = true;

                    }
                }

                timer += 1;
            }

        }


        protected override void UnloadData()
        {

        }
    }
}