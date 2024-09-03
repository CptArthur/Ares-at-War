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

using AresAtWar.Command;

namespace AresAtWar.Init
{
    
    public class AaWMain
    {

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

            if (!MyAPIGateway.Utilities.GetVariable<bool>("AaWStoryEvents", out boolplaceholder))
                MyAPIGateway.Utilities.SetVariable<bool>("AaWStoryEvents", true);



            if (!MyAPIGateway.Utilities.GetVariable<bool>("PurgeActive", out boolplaceholder))
                MyAPIGateway.Utilities.SetVariable<bool>("PurgeActive", false);




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
        }

        public static void SetStartingValues()
        {

            MyAPIGateway.Utilities.SetVariable<bool>("AaWStoryEvents", true);

            MyAPIGateway.Utilities.SetVariable<int>("GC_Strength_Counter", 200);
            MyAPIGateway.Utilities.SetVariable<int>("FAF_Strength_Counter", 66);
            MyAPIGateway.Utilities.SetVariable<int>("AHE_Strength_Counter", 31);
            MyAPIGateway.Utilities.SetVariable<int>("SYN_Strength_Counter", 40);
            MyAPIGateway.Utilities.SetVariable<int>("ITC_Strength_Counter", 125);
            MyAPIGateway.Utilities.SetVariable<int>("DRA_Strength_Counter", 15);
            MyAPIGateway.Utilities.SetVariable<int>("TIF_Strength_Counter", 20);
            MyAPIGateway.Utilities.SetVariable<int>("REM_Strength_Counter", 19);



        }










    }
}

