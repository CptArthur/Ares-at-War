using AresAtWar.Configuration.Editor;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using AresAtWar.Configuration;

namespace AresAtWar.Configuration.Editor {
    public static class EditorTools {

        private static List<string> _stringList = new List<string>();

        public static string EditSettings(string receivedCommand) {

            if (receivedCommand.StartsWith("/MES.Settings.ResetAll")) {

                Settings.General = new ConfigGeneral();
                Settings.General.SaveSettings();

                Settings.War = new ConfigWar();
                Settings.War.SaveSettings();

                return "All Settings Have Been Reset";
            
            }

 

            if (receivedCommand.StartsWith("/MES.Settings.General."))
                return Settings.General.EditFields(receivedCommand);

            if (receivedCommand.StartsWith("/MES.Settings.War."))
                return Settings.War.EditFields(receivedCommand);



            return "Command Not Recognized";
        
        }

        public static bool SetCommandValueBool(string command, ref bool target) {

            var splitCommand = (command.Trim()).Split('.');
            bool result = false;

            if (splitCommand.Length < 5 || !bool.TryParse(splitCommand[4], out result))
                return false;

            target = result;
            return true;
        
        }

        public static bool SetCommandValueDouble(string command, ref double target) {

            var splitCommand = (command.Trim()).Split('.');
            double result = 0;

            if (splitCommand.Length < 5 || !double.TryParse(splitCommand[4], out result))
                return false;

            target = result;
            return true;

        }

        public static bool SetCommandValueFloat(string command, ref float target) {

            var splitCommand = (command.Trim()).Split('.');
            float result = 0;

            if (splitCommand.Length < 5 || !float.TryParse(splitCommand[4], out result))
                return false;

            target = result;
            return true;

        }

        public static bool SetCommandValueInt(string command, ref int target) {

            var splitCommand = (command.Trim()).Split('.');
            int result = 0;

            if (splitCommand.Length < 5 || !int.TryParse(splitCommand[4], out result))
                return false;

            target = result;
            return true;

        }

        public static bool SetCommandValueLong(string command, ref long target) {

            var splitCommand = (command.Trim()).Split('.');
            long result = 0;

            if (splitCommand.Length < 5 || !long.TryParse(splitCommand[4], out result))
                return false;

            target = result;
            return true;

        }

        public static bool SetCommandValueString(string command, ref string target) {

            var splitCommand = (command.Trim()).Split('.');

            if (splitCommand.Length < 5)
                return false;

            target = splitCommand[4];
            return true;

        }

        ///MES.Settings.Grids.GlobalReplenishmentProfiles.Add.MES-Replenishment-BaseRules
        public static bool SetCommandValueStringArray(string command, ref string[] target) {

            var splitCommand = (command.Trim()).Split('.');

            if (splitCommand.Length < 6)
                return false;

            _stringList.Clear();
            _stringList.AddArray<string>(target);

            if (splitCommand[4] == "Add") {

                if (!_stringList.Contains(splitCommand[5])) {

                    _stringList.Add(splitCommand[5]);
                    target = _stringList.ToArray();

                }
                
                return true;

            }

            if (splitCommand[4] == "Remove") {

                if (_stringList.Contains(splitCommand[5])) {

                    _stringList.Remove(splitCommand[5]);
                    target = _stringList.ToArray();

                }
                
                return true;

            }

            return false;

        }



    }

}
