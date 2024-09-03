using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;
using Sandbox.Game;
using System.Drawing;
using AresAtWar.GPSManagers;
using AresAtWar.Logging;
using AresAtWar.Configuration;
using Sandbox.ModAPI;
using AresAtWar.SessionCore;

namespace AresAtWar.War
{
    public static partial class WarSim
    {
        public static readonly List<Node> _nodes = new List<Node>(); //SAVE
        public static readonly List<Frontline> _frontlines = new List<Frontline>();  //SAVE
        public static readonly List<StaticEncounter> _staticEncounters = new List<StaticEncounter>();  //SAVE


        private static Faction GC = new Faction("GC", new Color(255, 0, 0)); // red
        private static Faction FAF = new Faction("FAF", new Color(0, 255, 0)); // green
        private static Faction SYN = new Faction("SYN", new Color(255, 165, 0)); // orange

        private static Faction PURGE = new Faction("Purge", new Color(139, 0, 0), EventFaction: true); // darkred
        private static Faction SDC = new Faction("SDC", new Color(128, 0, 128), EventFaction: true); // purple

        private static Faction REM = new Faction("REM", new Color(165, 42, 42), MinorFaction: true); // brown
        private static Faction TIF = new Faction("TIF", new Color(128, 128, 128), MinorFaction: true); // gray
        private static Faction ITC = new Faction("ITC", new Color(173, 216, 230), MinorFaction: true); // lightblue



        public static Faction[] _factions = { FAF, SYN, GC, PURGE, SDC };

        public static Faction[] _allfactions = { FAF, SYN, GC, PURGE, SDC, REM, TIF, ITC };

        private const int Rounds = 67;
        private static readonly Random _random = new Random();



        // Define the factions


        public static void Init()
        {
            //var agarisOrbit = new Node("AgarisOrbit", "Agraris Space", GC, new Vector3D(-1129033.50, 126871.50, 1293873.50));
            var agarisOrbit = new Node("AgarisOrbit", "Agraris Space", GC, new Vector3D(-1127291, 223414.47, 1507151.44));
            agarisOrbit.SpaceNode = true;


            var sunsetCity = new Node("SunsetCity", "Sector Sunset City", GC, new Vector3D(-1127111.42, 136759.17, 1233915.55));
            var azuris = new Node("Azuris", "Sector Azuris", GC, new Vector3D(-1085148.41, 132345.68, 1252869.66));
            var carcosa = new Node("Carcosa", "Sector Carcosa", GC, new Vector3D(-1179270, 108838, 1323048));

            



            var station27 = new Node("Station27", "Station 27", GC, new Vector3D(1021847.01, 2624.77, -240317.27));
            station27.SpaceNode = true;

            var craitOrbit = new Node("CraitOrbit", "Crait Orbit", GC, new Vector3D(1400279.11, -616717.41, -2772159.72));
            craitOrbit.SpaceNode = true;

            var lezunoLorusOrbit = new Node("LezunoLorusOrbit", "Lezuno&Lorus Orbit", GC, new Vector3D(-1400516.03, -394463.12, -1619521.91));
            lezunoLorusOrbit.SpaceNode = true;

            var spice = new Node("SpiceLorus", "Spicefields", SYN, new Vector3D(-1254616.19, -523298.60, -1828837.31));

            //DoriumThora4


            var fafFaction = new Node("FAFFaction", "...", FAF, new Vector3D(-1129033.50, 126871.50, 1293873.50), true); // Agaris
            var gcFaction = new Node("GCFaction", "...", GC, new Vector3D(0, 0, 0), true); //Bylen
            var synFaction = new Node("SYNFaction", "...", SYN, new Vector3D(-1249083.5, -521370.5, -1803753.5), true); //Lorus 

            var PURGEFaction = new Node("PURGEFaction", "...", PURGE, new Vector3D(0, 0, 0), true); // Bylen
            var SDCFaction = new Node("SDCFaction", "...", SDC, new Vector3D(0, 0, 0), true); //Some place


            var TIFFaction = new Node("TIFFaction", "...", TIF, new Vector3D(-1249083.5, -521370.5, -1803753.5), true); //Lorus 
            var ITCFaction = new Node("ITCFaction", "...", ITC, new Vector3D(x: 1404449.5, y: -83492.5, z: -109853.5), true); //Thora 4
            var REMFaction = new Node("REMFaction", "...", REM, new Vector3D(x: -1307059.5, y: -325291.5, z: -1466477.5), true); //Lezuno C
            

            _nodes.Add(agarisOrbit);

            _nodes.Add(sunsetCity);

            _nodes.Add(azuris);

            _nodes.Add(carcosa);

            _nodes.Add(station27);

            _nodes.Add(craitOrbit);

            _nodes.Add(lezunoLorusOrbit);

            _nodes.Add(spice);
            _staticEncounters.Add(new StaticEncounter(spice.Id,"SYN-HQ", "SYN", new Vector3D(-1253813.36256464, -522694.97793912, -1828182.16404181), false));




            _nodes.Add(fafFaction);

            _nodes.Add(gcFaction);


            _nodes.Add(synFaction);
            _staticEncounters.Add(new StaticEncounter(synFaction.Id,"SYN-MiningOutpost3", "SYN", new Vector3D(-1229434.03820444, -521175.882389913, -1789244.09383711), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-MiningOutpost2", "SYN", new Vector3D(-1224847.84297878, -523019.024865274, -1801070.01883598), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-StripMiningPlatform4", "SYN", new Vector3D(-1254874.33395561, -521010.475227488, -1827504.95448953), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-StripMiningPlatform1", "SYN", new Vector3D(-1252113.35237604, -524524.015235651, -1827805.21299045), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-FuelDepot3", "SYN", new Vector3D(-1231876.8267543, -525240.903534725, -1786703.49699656), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-FuelDepot2", "SYN", new Vector3D(-1251503.78547039, -532230.449342892, -1825666.96640694), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-FuelDepot1", "SYN", new Vector3D(-1264759.02008796, -517528.282412784, -1822256.60960163), false));



            _staticEncounters.Add(new StaticEncounter(,"SYN-TalisOutpost", "SYN", new Vector3D(-1129254.57184159, 104270.56515784, 1348435.49548304), false));







            _nodes.Add(PURGEFaction);
            _nodes.Add(SDCFaction);


            _nodes.Add(TIFFaction);
            _staticEncounters.Add(new StaticEncounter(TIFFaction.Id, "TIF-HQ", "TIF", new Vector3D(-1247612.66927268, -539425.082150115, -1787266.32893023), true));


            _nodes.Add(ITCFaction);
            _nodes.Add(REMFaction);

            _frontlines.Add(new Frontline("CraitOrbit", "PURGEFaction", new Vector3D(0, 0, 0)));

            _frontlines.Add(new Frontline("ITCFaction", "Station27", new Vector3D(1262731.90938399, -57496.7583857419, -170986.107558704)));
            _frontlines.Add(new Frontline("SDCFaction", "Station27", new Vector3D(0, 0, 0)));
            _frontlines.Add(new Frontline("GCFaction", "Station27", new Vector3D(0, 0, 0)));

            _frontlines.Add(new Frontline("SpiceLorus", "TIFFaction", new Vector3D(-1249083.5, -521370.5, -1803753.5))); // Lorus Center
            _frontlines.Add(new Frontline("LezunoLorusOrbit", "REMFaction", new Vector3D(-1307059.5, -325291.5, -1466477.5))); // Lezuno Center

            _frontlines.Add(new Frontline("AgarisOrbit", "SunsetCity", new Vector3D(-1127291, 223414.47, 1507151.44))); // Agaris Space
            _frontlines.Add(new Frontline("AgarisOrbit", "Azuris", new Vector3D(-1127291, 223414.47, 1507151.44))); // Agaris Space
            _frontlines.Add(new Frontline("AgarisOrbit", "Carcosa", new Vector3D(-1127291, 223414.47, 1507151.44))); // Agaris Space
            _frontlines.Add(new Frontline("Azuris", "SunsetCity", new Vector3D(-1104197.96, 140871.70, 1239444.86)));
            _frontlines.Add(new Frontline("Carcosa", "SunsetCity", new Vector3D(-1184090.20, 142684.06, 1269996.58)));

            _frontlines.Add(new Frontline("Carcosa", "FAFFaction", new Vector3D(-1129033.50, 126871.50, 1293873.50))); // Agaris Center

            _frontlines.Add(new Frontline("SpiceLorus", "SYNFaction", new Vector3D(-1249083.5, -521370.5, -1803753.5))); // Lorus Center

            _frontlines.Add(new Frontline("AgarisOrbit", "Station27", new Vector3D(131020.88, -639.0, 822275.38)));
            _frontlines.Add(new Frontline("CraitOrbit", "Station27", new Vector3D(1446326.18, -302990.8, -1794173.35)));

            _frontlines.Add(new Frontline("LezunoLorusOrbit", "Station27", new Vector3D(-163848.09, -49.47, -1023686.9)));
            _frontlines.Add(new Frontline("LezunoLorusOrbit", "SpiceLorus", new Vector3D(-1283460.48, -521102.40, -1847874.03)));









            /*
            _staticEncounters.Add(new StaticEncounter("SDC-Baza", "SDC", new Vector3D(1414374.76539063, -66285.1791060999, -155320.696131601), false));
            _staticEncounters.Add(new StaticEncounter("SDC-Factory", "SDC", new Vector3D(1448417.80036266, -82188.5341196633, -132838.215067967), false));
            _staticEncounters.Add(new StaticEncounter("SDC-IceStation", "SDC", new Vector3D(1449425.42494408, -67461.5408470133, -97827.3649669675), false));
            _staticEncounters.Add(new StaticEncounter("ROS-AgarisSouthPoleStation", "ROS", new Vector3D(-1135613.40506584, 68368.3731023855, 1288864.77397775), false));
            _staticEncounters.Add(new StaticEncounter("ROS-DeepSpaceStation", "ROS", new Vector3D(1409212.66274555, 1269755.79727905, 4322539.1548167), false));
            _staticEncounters.Add(new StaticEncounter("ROS-LezunoCronyxResearch", "ROS", new Vector3D(-1345321.68316333, -311274.775460122, -1512622.3198324), false));
            _staticEncounters.Add(new StaticEncounter("ROS-CraitCrystalResearch", "ROS", new Vector3D(1442645.12944524, -609572.594204612, -2826836.7311291), false));
            _staticEncounters.Add(new StaticEncounter("REM-AgarisPegasusSurvivorsCamp", "REM", new Vector3D(-1150920.75767096, 96479.1380086092, 1247793.75913681), false));
            _staticEncounters.Add(new StaticEncounter("REM-AgarisCrashedPegasus", "REM", new Vector3D(-1150714.04777446, 96474.1523835873, 1247726.41140944), false));
            _staticEncounters.Add(new StaticEncounter("REM-LezunoHQ", "REM", new Vector3D(-1308380.83074185, -309148.219564841, -1408842.20772728), false));
            _staticEncounters.Add(new StaticEncounter("CrashedAmbassador", "", new Vector3D(-1337641.2377426, -282640.186555188, -1436612.79788229), false));
            _staticEncounters.Add(new StaticEncounter("AHE HQ Wasted Event", "AHE", new Vector3D(-1161953.74881786, 139121.165492067, 1341178.05079544), false));
            _staticEncounters.Add(new StaticEncounter("ITC-ProcessingPlant", "ITC", new Vector3D(1358512.49187609, -84083.4160778683, -91985.7865920572), false));
            _staticEncounters.Add(new StaticEncounter("ITC-Thora4Capital", "ITC", new Vector3D(1357482.16759975, -94148.4256546545, -120209.554021562), false));
            _staticEncounters.Add(new StaticEncounter("ITC-Aether", "ITC", new Vector3D(1471331.9089127, -71413.8339805347, -52528.3761462755), false));
            _staticEncounters.Add(new StaticEncounter("ITC-AgarisWarehouse", "ITC", new Vector3D(-1082120.45148516, 108108.325993618, 1324693.95112241), false));
            _staticEncounters.Add(new StaticEncounter("ITC-AgarisStopNGO", "ITC", new Vector3D(-1175351.26863503, 122715.874160074, 1330273.09240926), false));
            _staticEncounters.Add(new StaticEncounter("ITC-AgarisVinyTradeOutpost", "ITC", new Vector3D(-1094787.67563701, 137852.392834221, 1245950.13934922), false));
            _staticEncounters.Add(new StaticEncounter("ITC-AgarisAtlas", "ITC", new Vector3D(-1173526.5894738, 153122.39634304, 1322481.33507799), false));
            _staticEncounters.Add(new StaticEncounter("GC-AgarisMiningOutpost6", "GC", new Vector3D(-1101747.1153222, 135055.239345568, 1241867.1173033), false));
            _staticEncounters.Add(new StaticEncounter("GC-AgarisMiningOutpost3", "GC", new Vector3D(-1134974.30634451, 97772.7486602582, 1242867.89204989), false));
            _staticEncounters.Add(new StaticEncounter("GC-AgarisMiningOutpost2", "GC", new Vector3D(-1124854.96257951, 128402.428934341, 1234992.04506256), false));
            _staticEncounters.Add(new StaticEncounter("GC-AgarisMiningOutpost1", "GC", new Vector3D(-1101888.31636439, 146995.498614052, 1244753.09662791), false));
            _staticEncounters.Add(new StaticEncounter("GC-AgarisMidWayBase", "GC", new Vector3D(-1179079.18723259, 144839.837898291, 1268477.76925947), false));
            _staticEncounters.Add(new StaticEncounter("GC-AgarisMarsaBase", "GC", new Vector3D(-1107394.66345341, 97948.4919168743, 1247027.94896228), false));
            _staticEncounters.Add(new StaticEncounter("GC-HQ", "GC", new Vector3D(-1132538.71527543, 150710.787186924, 1240143.78988613), false));
            _staticEncounters.Add(new StaticEncounter("GC-LorusStation", "GC", new Vector3D(-1215952.01296593, -529934.190624479, -1781219.09465844), false));
            _staticEncounters.Add(new StaticEncounter("GC-CraitStation", "GC", new Vector3D(1436450.74967084, -599990.490504346, -2801182.58223604), false));
            _staticEncounters.Add(new StaticEncounter("GC-BylenStation", "GC", new Vector3D(-743011.513424989, -642.058053975779, 764039.700333453), false));
            _staticEncounters.Add(new StaticEncounter("GC-LezunoStation", "GC", new Vector3D(-1385652.10310275, -323878.76312341, -1524230.34517791), false));
            _staticEncounters.Add(new StaticEncounter("FAF-HQ", "FAF", new Vector3D(-1096295.71439344, 175964.317905326, 1292565.09279302), false));
            _staticEncounters.Add(new StaticEncounter("DRA-MiningOutpost2", "DRA", new Vector3D(-1101867.00574666, 115247.364510054, 1344971.30117802), false));
            _staticEncounters.Add(new StaticEncounter("DRA-MiningOutpost1", "DRA", new Vector3D(-1145111.00912276, 108910.693586212, 1347768.57348853), false));
            _staticEncounters.Add(new StaticEncounter("DRA-HQ", "DRA", new Vector3D(-1133029.74927066, 117696.204587734, 1352265.55189385), false));
            _staticEncounters.Add(new StaticEncounter("Bratis", "", new Vector3D(1430093.57173546, -83370.5986257671, -67588.9682139887), false));
            _staticEncounters.Add(new StaticEncounter("Thorrix", "", new Vector3D(1388533.32514774, -76047.6219617924, -62924.0242155304), false));
            _staticEncounters.Add(new StaticEncounter("COL-Bridge", "COL", new Vector3D(-1171982.7759584, 99902.0615879026, 1324095.6664353), false));
            _staticEncounters.Add(new StaticEncounter("Azuris", "", new Vector3D(-1083758.95479578, 125933.59277764, 1256220.63233289), false));
            _staticEncounters.Add(new StaticEncounter("Carcosa", "", new Vector3D(-1171670.64146471, 99277.2640048867, 1323685.99092236), false));
            _staticEncounters.Add(new StaticEncounter("SunsetCity", "", new Vector3D(-1133703.76683291, 133985.403589644, 1235588.47353097), false));
            _staticEncounters.Add(new StaticEncounter("Anf-Zaoa", "Anf", new Vector3D(1378746.28881089, -124494.791459621, -126237.140862596), false));
            _staticEncounters.Add(new StaticEncounter("Anf-Harbinger", "Anf", new Vector3D(-103.709755313563, -297.249276173619, 2578132.43704597), false));
            _staticEncounters.Add(new StaticEncounter("FAF-AHE-Outpost3", "FAF", new Vector3D(-1169461.01701594, 136040.447238912, 1335925.97691028), false));
            _staticEncounters.Add(new StaticEncounter("FAF-AHE-Outpost2", "FAF", new Vector3D(-1176647.81171692, 128846.461453344, 1328946.37583531), false));
            _staticEncounters.Add(new StaticEncounter("FAF-AHE-Outpost1", "FAF", new Vector3D(-1166849.16539835, 123865.319141937, 1340043.45907084), false));
            _staticEncounters.Add(new StaticEncounter("FAF-AHE-HQ", "FAF", new Vector3D(-1161953.75201648, 139121.166682262, 1341178.0553917), false));
            _staticEncounters.Add(new StaticEncounter("AHE-Outpost3", "AHE", new Vector3D(-1169461.01701594, 136040.447238912, 1335925.97691028), false));
            _staticEncounters.Add(new StaticEncounter("AHE-Outpost2", "AHE", new Vector3D(-1176647.81171692, 128846.461453344, 1328946.37583531), false));
            _staticEncounters.Add(new StaticEncounter("AHE-Outpost1", "AHE", new Vector3D(-1166849.16539835, 123865.319141937, 1340043.45907084), false));
            _staticEncounters.Add(new StaticEncounter("AHE-HQ", "AHE", new Vector3D(-1161953.75201648, 139121.166682262, 1341178.0553917), false));
            _staticEncounters.Add(new StaticEncounter("SYN-ForegoneStation", "SYN", new Vector3D(-1222071.7453009, -519946.691144704, -1835853.98113092), false));
            */






        }


        /*
        private bool FactionsAtWar(string factionA, string factionB)
        {
            bool isAtWar = false;
            if (MyAPIGateway.Utilities.GetVariable<bool>(factionA + factionB + "War", out isAtWar))
            {
                return isAtWar;
            }
            if (MyAPIGateway.Utilities.GetVariable<bool>(factionB + factionA + "War", out isAtWar))
            {
                return isAtWar;
            }

            return isAtWar;
        }
        */

        private static bool FactionsAtWar(string factionA, string factionB)
        {
            var warPairs = new HashSet<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("GC", "FAF"),
                new KeyValuePair<string, string>("SYN", "GC")
            };

            return warPairs.Contains(new KeyValuePair<string, string>(factionA, factionB)) ||
                   warPairs.Contains(new KeyValuePair<string, string>(factionB, factionA));
        }

        private static int SimulateBattle(Node nodeA, Node nodeB, double score)
        {
            var factionA = nodeA.Faction;
            var factionB = nodeB.Faction;

            double aBonus = 0;
            double bBonus = 0;

            if (score > 0)
            {
                bBonus = Utils.Bonus(score);
            }

            if (score < 0)
            {
                aBonus = Utils.Bonus(score);
            }

            var modifiedStrengthA = factionA.Strength + factionA.Modifier + aBonus;
            var modifiedStrengthB = factionB.Strength + factionB.Modifier + bBonus;


            var forceA = Utils.NextGaussian(modifiedStrengthA, 75); ;
            var forceB = Utils.NextGaussian(modifiedStrengthB, 75); ;

            if (forceA < forceB)
            {
                
                score += _random.Next(Settings.War.MinBattleSimIniativeScore, Settings.War.MaxBattleSimIniativeScore);
            }
            else if (forceA > forceB)
            {
                score -= _random.Next(Settings.War.MinBattleSimIniativeScore, Settings.War.MaxBattleSimIniativeScore);
            }

            return (int)Math.Max(Math.Min(score, 100), -100);
        }

        private static bool CanAttack(Node node, Faction attackingFaction)
        {
            if (node.StartNode)
            {
                return _nodes.All(n => n.Faction.Tag != node.Faction.Tag || n.Id == node.Id);
            }

            if (node.Id == "AgarisOrbit")
            {
                var requiredNodes = new[] { "SunsetCity", "Azuris", "Carcosa" };
                return requiredNodes.All(n => _nodes.First(nd => nd.Id == n).Faction.Tag == attackingFaction.Tag) ||
                       (_nodes.Any(n => n.Id == "Station27" && n.Faction.Tag == attackingFaction.Tag) &&
                        _nodes.Any(n => n.Id == "CraitOrbit" && n.Faction.Tag == attackingFaction.Tag));
            }
            return true;
        }

        public static void Refresh(bool skipNodes = false, bool skipFaction = false, bool skipfrontlines = false)
        {
            if (!skipNodes)
            {
                foreach (var node in _nodes)
                {
                    node.Refresh();
                }
            }

            if (!skipFaction)
            {
                foreach (var faction in _allfactions)
                {
                    faction.Refresh();
                }

            }

            if (!skipfrontlines)
            {
                foreach (var frontline in _frontlines)
                {
                    frontline.Refresh();
                }

            }
        }

        public static void SimulateWarRound(params object[] arguments)
        {
            var activeBattles = new HashSet<string>();
            var capturedNodes = new HashSet<string>();

            Refresh(skipfrontlines:true);

            foreach (var frontline in _frontlines)
            {
                frontline.Refresh();


                var nodeA = _nodes.FirstOrDefault(n => n.Id == frontline.NodeAId);
                var nodeB = _nodes.FirstOrDefault(n => n.Id == frontline.NodeBId);




                bool fail = false;


                if (nodeA == null || nodeB == null)
                {
                    Logger.Write($"{frontline.Id} ", SpawnerDebugEnum.War);
                    frontline.Active = false;
                    continue;
                }




                var score = frontline.Score;


                if (nodeA.StartNode && nodeA.Inactive || nodeB.StartNode && nodeB.Inactive)
                {
                    frontline.Active = false;
                    continue;
                }


                if (nodeA.Faction == nodeB.Faction)
                {
                    frontline.Active = false;
                    continue;
                }


                if (!FactionsAtWar(nodeA.Faction.Tag, nodeB.Faction.Tag))
                {
                    frontline.Active = false;
                    continue;
                }



                var factionsInvolved = string.Join("-", nodeA.Faction.Tag, nodeB.Faction.Tag);
                var factionsInvolved2 = string.Join("-", nodeB.Faction.Tag, nodeA.Faction.Tag);

                if (activeBattles.Contains(factionsInvolved))
                {
                    frontline.Active = false;
                    continue;
                }

                if (activeBattles.Contains(factionsInvolved2))
                {
                    frontline.Active = false;
                    continue;
                }


                if (!CanAttack(nodeA, nodeB.Faction) || !CanAttack(nodeB, nodeA.Faction))
                {
                    frontline.Active = false;
                    continue;
                }


                //FrontLine Count
                nodeA.FrontLineCount++;
                nodeB.FrontLineCount++;

                bool FortA = nodeA.Fort;
                bool FortAUnderSiege = nodeA.FortUnderSiege;
                bool FortB = nodeB.Fort;
                bool FortBUnderSiege = nodeB.FortUnderSiege;
                bool frontLineHasSiege = frontline.HasSiege;

                if (frontLineHasSiege)
                {
                    //This frontline has a siege. So I should freeze the scores and wait for the outcome of the siege.
                    if(FortA || FortAUnderSiege)
                    {
                        frontline.UpdateScore(100);
                        activeBattles.Add(factionsInvolved);
                    }
                    else if(FortB || FortBUnderSiege)
                    {
                        frontline.UpdateScore(-100);
                        activeBattles.Add(factionsInvolved);
                    }

                    continue;
                }


                var newScore = SimulateBattle(nodeA, nodeB, score);
                frontline.UpdateScore(newScore);
                activeBattles.Add(factionsInvolved);


                if (newScore == -100)
                {
                    if (FortB)
                    {
                        //B has a fort. So I should start a siege if it isn't already being sieged.
                        if (FortBUnderSiege)
                        {
                            //Oh so this fort is already undersiege but by some other frontline.
                            Logger.Write($"{nodeA.Faction.Tag} cannot start siege on {nodeB.Faction.Tag} {nodeB.Id} because it is already undersiege?", SpawnerDebugEnum.War);
                        }
                        else
                        {
                            //Start siege

                            Logger.Write($"Starting siege at {nodeB.Id}, on frontline {frontline.Id}", SpawnerDebugEnum.War);


   
                            List<string> keys = new List<string>();
                            List<string> values = new List<string>();


                            keys.Add("{EventName}");
                            values.Add($"Battle for {nodeB.FancyName}");

                            keys.Add("{NodeId}");
                            values.Add($"{nodeB.Id}");

                            keys.Add("{FancyName}");
                            values.Add($"{nodeB.FancyName}");

                            keys.Add("{AttackingFaction}");
                            values.Add($"{nodeA.Faction.Tag}");

                            keys.Add("{DefendingFaction}");
                            values.Add($"{nodeB.Faction.Tag}");

                            var coord = nodeB.FortCoords;


                            keys.Add("{Coords}");
                            values.Add($"{{X:{coord.X} Y:{coord.Y} Z:{coord.Z}}}");

                            keys.Add("{FrontLineId}");
                            values.Add($"{frontline.Id}");



                            AaWSessionCore.MESApi.InsertInstanceEventGroup("EventGroup_AttackOnfort", keys, values);

                            nodeB.FortUnderSiege = true;
                            frontline.HasSiege = true;

                        }

                    }
                    else
                    {
                        //No fort no worries.
                        Logger.Write($"{nodeB.Faction.Tag} loses {nodeB.Id} to {nodeA.Faction.Tag}", SpawnerDebugEnum.War);
                        nodeB.Faction = nodeA.Faction;
                        frontline.Deactivate();
                        capturedNodes.Add(nodeB.Id);
                    }


                    continue;

                }

                else if (newScore == 100)
                {
                    if (FortA)
                    {
                        //B has a fort. So I should start a siege if it isn't already being sieged.
                        if (FortAUnderSiege)
                        {
                            //Oh so this fort is already undersiege but by some other frontline.
                            Logger.Write($"{nodeB.Faction.Tag} cannot start siege on {nodeA.Faction.Tag} {nodeA.Id} because it is already undersiege?", SpawnerDebugEnum.War);
                        }
                        else
                        {


                            Logger.Write($"Starting siege at {nodeA.Id}, on frontline {frontline.Id}", SpawnerDebugEnum.War);


          

                            List<string> keys = new List<string>();
                            List<string> values = new List<string>();


                            keys.Add("{EventName}");
                            values.Add($"Battle for {nodeA.FancyName}");

                            keys.Add("{NodeId}");
                            values.Add($"{nodeA.Id}");

                            keys.Add("{FancyName}");
                            values.Add($"{nodeA.FancyName}");

                            keys.Add("{AttackingFaction}");
                            values.Add($"{nodeB.Faction.Tag}");

                            keys.Add("{DefendingFaction}");
                            values.Add($"{nodeA.Faction.Tag}");

                            var coord = nodeA.FortCoords;


                            keys.Add("{Coords}");
                            values.Add($"{{X:{coord.X} Y:{coord.Y} Z:{coord.Z}}}");

                            keys.Add("{FrontLineId}");
                            values.Add($"{frontline.Id}");

                            AaWSessionCore.MESApi.InsertInstanceEventGroup("EventGroup_AttackOnfort", keys, values);
                            nodeA.FortUnderSiege = true;
                            frontline.HasSiege = true;

                        }

                    }
                    else
                    {
                        //No fort no worries.
                        Logger.Write($"{nodeA.Faction.Tag} loses {nodeA.Id} to {nodeB.Faction.Tag}", SpawnerDebugEnum.War);

                        nodeA.Faction = nodeB.Faction;
                        frontline.Deactivate();
                        capturedNodes.Add(nodeA.Id);
                    }

                    continue;
                }

            }


            //Reset All frontlines scores when one is captured
            foreach (var capturedNodeName in capturedNodes)
            {
                foreach (var frontline in _frontlines)
                {
                    if (frontline.NodeAId == capturedNodeName || frontline.NodeBId == capturedNodeName)
                    {
                        frontline.UpdateScore(0,false);
                    }
                }
            }





            foreach (var node in _nodes)
            {
                if (node.FrontLineCount > 0)
                    continue;

                if (node.Inactive)
                    continue;

                if (node.StartNode)
                    continue;

                if (node.Fort)
                    continue;

 

                string[] validTags = { "FAF", "GC", "SYN" };

                if (!validTags.Contains(node.Faction.Tag))
                    continue;

                node.BuildFort();

            }




        }


    

        public static class Utils
        {
            private static readonly Random _random = new Random();

            public static double Bonus(double x)
            {
                double bonus;
                if (Math.Abs(x) < 80)
                {
                    bonus = Math.Abs(x) / 1.25;
                }
                else
                {
                    bonus = Math.Pow(x, 2) / 100;
                }

                bonus = Math.Round(bonus, 0);

 
                return bonus;
            }

            public static double NextGaussian(double mean, double stdDev)
            {
                // Box-Muller transform
                double u1 = 1.0 - _random.NextDouble(); // Uniform(0,1] random doubles
                double u2 = 1.0 - _random.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); // Standard normal (mean 0, std dev 1)
                double randNormal = mean + stdDev * randStdNormal; // Transform to desired mean and std dev
                return randNormal;
            }


        }





    }
}
