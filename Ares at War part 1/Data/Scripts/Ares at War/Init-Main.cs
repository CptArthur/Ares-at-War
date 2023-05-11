using System;
using System.Collections.Generic;
using AresAtWar.API;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using System.Text;
using VRage.Utils;
using VRageMath;
using AresAtWar.Factions;
using AresAtWar.Command;

namespace AresAtWar.Init
{
    
    public class AaWMain
    {
        public static List<AaWFactions.FactionData> listOfFactions = new List<AaWFactions.FactionData>();
        public static void init()
        {
             //GC
            AaWFactions.FactionData GC = new AaWFactions.FactionData("GC");
            GC.Modes.Add("GCAgarisMode");
            listOfFactions.Add(GC);

            AaWHoldings.HoldingData GCHQ = new AaWHoldings.HoldingData("GC-HQ");
            GCHQ.Cap = 50;
            GCHQ.Production = 5;
            GC.HoldingsList.Add(GCHQ);

            AaWHoldings.HoldingData GCAgarisMarsaBase = new AaWHoldings.HoldingData("GC-AgarisMarsaBase");
            GCAgarisMarsaBase.Cap = 30;
            GCAgarisMarsaBase.Production = 2;
            GC.HoldingsList.Add(GCAgarisMarsaBase);

            AaWHoldings.HoldingData GCAgarisMidWayBase = new AaWHoldings.HoldingData("GC-AgarisMidWayBase");
            GCAgarisMidWayBase.Cap = 15;
            GCAgarisMidWayBase.Production = 1;
            GC.HoldingsList.Add(GCAgarisMidWayBase);



            AaWHoldings.HoldingData GCBylenStation = new AaWHoldings.HoldingData("GC-BylenStation");
            GCBylenStation.Cap = 50;
            GCBylenStation.Production = 2;
            GC.HoldingsList.Add(GCBylenStation);

            AaWHoldings.HoldingData GCLezunoStation = new AaWHoldings.HoldingData("GC-LezunoStation");
            GCLezunoStation.Cap = 50;
            GCLezunoStation.Production = 2;
            GC.HoldingsList.Add(GCLezunoStation);

            AaWHoldings.HoldingData GCCraitStation = new AaWHoldings.HoldingData("GC-CraitStation");
            GCCraitStation.Cap = 50;
            GCCraitStation.Production = 2;
            GC.HoldingsList.Add(GCCraitStation);

            AaWHoldings.HoldingData GCCarcosa = new AaWHoldings.HoldingData();
            GCCarcosa.Name = "GCCarcosa";
            GCCarcosa.CheckTrueBooleans = true;
            GCCarcosa.TrueBooleans.Add("GCCarcosa");
            GCCarcosa.Cap = 80;
            GCCarcosa.Production = 2;
            GC.HoldingsList.Add(GCCarcosa);

            AaWHoldings.HoldingData GCSunsetCity = new AaWHoldings.HoldingData();
            GCSunsetCity.Name = "GCSunsetCity";
            GCSunsetCity.CheckTrueBooleans = true;
            GCSunsetCity.TrueBooleans.Add("GCSunsetCity");
            GCSunsetCity.Cap = 100;
            GCSunsetCity.Production = 4;
            GC.HoldingsList.Add(GCSunsetCity);

            AaWHoldings.HoldingData GCAzuris = new AaWHoldings.HoldingData();
            GCAzuris.Name = "GCAzuris";
            GCAzuris.CheckTrueBooleans = true;
            GCAzuris.TrueBooleans.Add("GCAzuris");
            GCAzuris.Cap = 50;
            GCAzuris.Production = 2;
            GC.HoldingsList.Add(GCAzuris);

            //FAF
            AaWFactions.FactionData FAF = new AaWFactions.FactionData("FAF");
            listOfFactions.Add(FAF);

            AaWHoldings.HoldingData FAFXu = new AaWHoldings.HoldingData("FAF-Xu");
            FAFXu.Cap = 40;
            FAFXu.Production = 3;
            FAF.HoldingsList.Add(FAFXu);


            AaWHoldings.HoldingData FAFHQ = new AaWHoldings.HoldingData("FAF-HQ");
            FAFHQ.Cap = 50;
            FAFHQ.Production = 2;
            FAF.HoldingsList.Add(FAFHQ);

            AaWHoldings.HoldingData FAFCarcosa = new AaWHoldings.HoldingData();
            FAFCarcosa.Name = "FAFCarcosa";
            FAFCarcosa.CheckTrueBooleans = true;
            FAFCarcosa.TrueBooleans.Add("FAFCarcosa");
            FAFCarcosa.Cap = 80;
            FAFCarcosa.Production = 2;
            FAF.HoldingsList.Add(FAFCarcosa);

            AaWHoldings.HoldingData FAFSunsetCity = new AaWHoldings.HoldingData();
            FAFSunsetCity.Name = "FAFSunsetCity";
            FAFSunsetCity.CheckTrueBooleans = true;
            FAFSunsetCity.TrueBooleans.Add("FAFSunsetCity");
            FAFSunsetCity.Cap = 100;
            FAFSunsetCity.Production = 4;
            FAF.HoldingsList.Add(FAFSunsetCity);

            AaWHoldings.HoldingData FAFAzuris = new AaWHoldings.HoldingData();
            FAFAzuris.Name = "FAFAzuris";
            FAFAzuris.CheckTrueBooleans = true;
            FAFAzuris.TrueBooleans.Add("FAFAzuris");
            FAFAzuris.Cap = 50;
            FAFAzuris.Production = 2;
            FAF.HoldingsList.Add(FAFAzuris);

            AaWHoldings.HoldingData FAFAHEHQ = new AaWHoldings.HoldingData("FAF-AHE-HQ");
            FAFAHEHQ.FalseBooleans.Add("AHE-HQDestroyed");
            FAFAHEHQ.Cap = 100;
            FAFAHEHQ.Production = 3;
            FAF.HoldingsList.Add(FAFAHEHQ);

            //FAF
            AaWFactions.FactionData SYN = new AaWFactions.FactionData("SYN");
            listOfFactions.Add(SYN);

            AaWHoldings.HoldingData SYNStripMining1 = new AaWHoldings.HoldingData("SYN-StripMiningPlatform1");
            SYNStripMining1.Cap = 50;
            SYNStripMining1.Production = 2;
            SYN.HoldingsList.Add(SYNStripMining1);

            AaWHoldings.HoldingData SYNStripMining4 = new AaWHoldings.HoldingData("SYN-StripMiningPlatform4");
            SYNStripMining4.Cap = 50;
            SYNStripMining4.Production = 2;
            SYN.HoldingsList.Add(SYNStripMining4);

            AaWHoldings.HoldingData SYNHQ = new AaWHoldings.HoldingData("SYN-HQ");
            SYNHQ.Cap = 50;
            SYNHQ.Production = 3;
            SYN.HoldingsList.Add(SYNHQ);


            AaWHoldings.HoldingData TalisOutpost = new AaWHoldings.HoldingData("SYN-TalisOutpost");
            TalisOutpost.Cap = 50;
            TalisOutpost.Production = 3;
            SYN.HoldingsList.Add(TalisOutpost);

            AaWHoldings.HoldingData SYNAgarisSpiceTrade = new AaWHoldings.HoldingData();
            SYNAgarisSpiceTrade.Name = "SYNAgarisSpiceTrade";
            SYNAgarisSpiceTrade.CheckTrueBooleans = true;
            SYNAgarisSpiceTrade.TrueBooleans.Add("SYN-AgarisSpiceTrade");
            SYNAgarisSpiceTrade.Cap = 50;
            SYNAgarisSpiceTrade.Production = 1;
            SYN.HoldingsList.Add(SYNAgarisSpiceTrade);

            AaWHoldings.HoldingData SYNS27SpiceTrade = new AaWHoldings.HoldingData();
            SYNS27SpiceTrade.Name = "SYNS27SpiceTrade";
            SYNS27SpiceTrade.CheckTrueBooleans = true;
            SYNS27SpiceTrade.TrueBooleans.Add("SYN-S27SpiceTrade");
            SYNS27SpiceTrade.Cap = 50;
            SYNS27SpiceTrade.Production = 1;
            SYN.HoldingsList.Add(SYNS27SpiceTrade);

            AaWHoldings.HoldingData SYNLeader = new AaWHoldings.HoldingData();
            //TO do add conditions
            SYNLeader.Name = "SYNLeader";
            SYNLeader.AllowAnyTrueBoolean = true;
            SYNLeader.TrueBooleans.Add("SYNLeaderAlive");
            SYNLeader.Cap = 100;
            SYNLeader.Production = 1;
            SYN.HoldingsList.Add(SYNLeader);


            //ITC
            AaWFactions.FactionData ITC = new AaWFactions.FactionData("ITC");
            listOfFactions.Add(ITC);

            AaWHoldings.HoldingData ITCThora4Capital = new AaWHoldings.HoldingData("ITC-Thora4Capital");
            ITCThora4Capital.Cap = 100;
            ITCThora4Capital.Production = 3;
            ITC.HoldingsList.Add(ITCThora4Capital);

            AaWHoldings.HoldingData ITCProcessingPlant = new AaWHoldings.HoldingData("ITC-ProcessingPlant");
            ITCProcessingPlant.Cap = 20;
            ITCProcessingPlant.Production = 1;
            ITC.HoldingsList.Add(ITCProcessingPlant);

            AaWHoldings.HoldingData ITCAgarisAtlas = new AaWHoldings.HoldingData("ITC-AgarisAtlas");
            ITCAgarisAtlas.Cap = 50;
            ITCAgarisAtlas.Production = 1;
            ITC.HoldingsList.Add(ITCAgarisAtlas);

            AaWHoldings.HoldingData ITCAgarisVinyTradeOutpost = new AaWHoldings.HoldingData("ITC-AgarisVinyTradeOutpost");
            ITCAgarisVinyTradeOutpost.Cap = 50;
            ITCAgarisVinyTradeOutpost.Production = 1;
            ITC.HoldingsList.Add(ITCAgarisVinyTradeOutpost);


            AaWHoldings.HoldingData ITCS27 = new AaWHoldings.HoldingData();
            ITCS27.Name = "ITCS27";
            ITCS27.CheckTrueBooleans = true;
            ITCS27.TrueBooleans.Add("ITC-S27");
            ITCS27.Cap = 50;
            ITCS27.Production = 1;
            ITC.HoldingsList.Add(ITCS27);

            //AHE
            AaWFactions.FactionData AHE = new AaWFactions.FactionData("AHE");
            listOfFactions.Add(AHE);

            AaWHoldings.HoldingData AHEHQ = new AaWHoldings.HoldingData("AHE-HQ");
            AHEHQ.Cap = 100;
            AHEHQ.Production = 3;
            AHE.HoldingsList.Add(AHEHQ);


            //AaWHoldings.HoldingData AHEShipyard = new AaWHoldings.HoldingData("AHE-Shipyard");
            //AHEShipyard.Cap = 100;
            //AHEShipyard.Production = 3;
            //AHE.HoldingsList.Add(AHEShipyard);

            //REM
            AaWFactions.FactionData REM = new AaWFactions.FactionData("REM");
            listOfFactions.Add(REM);

            AaWHoldings.HoldingData REMLezunoHQ = new AaWHoldings.HoldingData("REM-LezunoHQ");
            REMLezunoHQ.Cap = 75;
            REMLezunoHQ.Production = 2;
            REM.HoldingsList.Add(REMLezunoHQ);

            AaWHoldings.HoldingData REMLezunoAirstrip = new AaWHoldings.HoldingData("REM-LezunoAirstrip");
            REMLezunoAirstrip.Cap = 75;
            REMLezunoAirstrip.Production = 2;
            REM.HoldingsList.Add(REMLezunoAirstrip);

            //TIF
            AaWFactions.FactionData TIF = new AaWFactions.FactionData("TIF");
            listOfFactions.Add(TIF);

            AaWHoldings.HoldingData TIFHQ = new AaWHoldings.HoldingData("TIF-HQ");
            TIFHQ.Cap = 75;
            TIFHQ.Production = 2;
            TIF.HoldingsList.Add(TIFHQ);

            //DRA
            AaWFactions.FactionData DRA = new AaWFactions.FactionData("DRA");
            listOfFactions.Add(DRA);

            AaWHoldings.HoldingData DRAHQ = new AaWHoldings.HoldingData("DRA-HQ");
            DRAHQ.Cap = 75;
            DRAHQ.Production = 2;
            DRA.HoldingsList.Add(DRAHQ);





            //Faction check 
            for (int i = 0; i < listOfFactions.Count; i++)
            {
                var fac = listOfFactions[i];
                fac.StrengthCalculator();
                fac.Holders();
            }
        }
        public static void CheckValues()
        {
            bool boolplaceholder;
            //Colonies
            if (!MyAPIGateway.Utilities.GetVariable<bool>("GCSunsetCity", out boolplaceholder))
                MyAPIGateway.Utilities.SetVariable<bool>("GCSunsetCity", true);
            if (!MyAPIGateway.Utilities.GetVariable<bool>("GCAzuris", out boolplaceholder))
                MyAPIGateway.Utilities.SetVariable<bool>("GCAzuris", true);
            if (!MyAPIGateway.Utilities.GetVariable<bool>("GCCarcosa", out boolplaceholder))
                MyAPIGateway.Utilities.SetVariable<bool>("GCCarcosa", true);

            //Events
            if (!MyAPIGateway.Utilities.GetVariable<bool>("GCvsAHE", out boolplaceholder))
                MyAPIGateway.Utilities.SetVariable<bool>("GCvsAHE", false);
            



            int intplaceholder;

            if (!MyAPIGateway.Utilities.GetVariable<int>("GCMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("GCMode", 0);
            if (!MyAPIGateway.Utilities.GetVariable<int>("GCAgarisMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("GCAgarisMode", 1);
            if (!MyAPIGateway.Utilities.GetVariable<int>("GCAgarisMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("GCAgarisMode", 1);
            if (!MyAPIGateway.Utilities.GetVariable<int>("GCAgarisMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("GCAgarisMode", 1);
            if (!MyAPIGateway.Utilities.GetVariable<int>("FAFMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("FAFMode", 1);
            if (!MyAPIGateway.Utilities.GetVariable<int>("AHEMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("AHEMode", 0);
            if (!MyAPIGateway.Utilities.GetVariable<int>("SYNMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("SYNMode", 0);
            if (!MyAPIGateway.Utilities.GetVariable<int>("ITCMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("ITCMode", 0);
            if (!MyAPIGateway.Utilities.GetVariable<int>("REMMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("REMMode", 0);
            if (!MyAPIGateway.Utilities.GetVariable<int>("TIFMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("TIFMode", 0);
            if (!MyAPIGateway.Utilities.GetVariable<int>("DRAMode", out intplaceholder))
                MyAPIGateway.Utilities.SetVariable<int>("DRAMode", 0);



            //Faction check 
            for (int i = 0; i < listOfFactions.Count; i++)
            {
                var fac = listOfFactions[i];
                fac.StrengthCalculator();
                fac.Holders();
            }
        }

        public static void SetStartingValues()
        {

            MyAPIGateway.Utilities.SetVariable<int>("GC_Strength_Counter", 200);
            MyAPIGateway.Utilities.SetVariable<int>("FAF_Strength_Counter", 66);
            MyAPIGateway.Utilities.SetVariable<int>("AHE_Strength_Counter", 31);
            MyAPIGateway.Utilities.SetVariable<int>("SYN_Strength_Counter", 40);
            MyAPIGateway.Utilities.SetVariable<int>("ITC_Strength_Counter", 125);
            MyAPIGateway.Utilities.SetVariable<int>("DRA_Strength_Counter", 15);
            MyAPIGateway.Utilities.SetVariable<int>("TIF_Strength_Counter", 20);
            MyAPIGateway.Utilities.SetVariable<int>("REM_Strength_Counter", 19);

            //Faction check 
            for (int i = 0; i < listOfFactions.Count; i++)
            {
                var fac = listOfFactions[i];
                fac.StrengthCalculator();
                fac.Holders();
            }
        }










    }
}

