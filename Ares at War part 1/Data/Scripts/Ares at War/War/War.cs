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
        public static readonly List<Node> _nodes = new List<Node>(); 
        public static readonly List<Frontline> _frontlines = new List<Frontline>();  
        public static readonly List<StaticEncounter> _staticEncounters = new List<StaticEncounter>();  
        public static readonly List<TradeRoute> _TradeRoutes = new List<TradeRoute>();

        private static Faction GC = new Faction("GC", new Color(255, 0, 0)); // red
        private static Faction AHE = new Faction("AHE", new Color(0, 255, 0)); // green
        private static Faction SYN = new Faction("SYN", new Color(255, 165, 0)); // orange

        private static Faction PURGE = new Faction("Purge", new Color(139, 0, 0), EventFaction: true); // darkred
        private static Faction SDC = new Faction("SDC", new Color(128, 0, 128), EventFaction: true); // purple

        private static Faction REM = new Faction("REM", new Color(165, 42, 42), MinorFaction: true); // brown
        private static Faction TIF = new Faction("TIF", new Color(128, 128, 128), MinorFaction: true); // gray
        private static Faction ITC = new Faction("ITC", new Color(173, 216, 230), MinorFaction: true); // lightblue



        public static Faction[] _factions = { AHE, SYN, GC, PURGE, SDC };

        public static Faction[] _allfactions = { AHE, SYN, GC, PURGE, SDC, REM, TIF, ITC };

        private const int Rounds = 67;
        private static readonly Random _random = new Random();


        public class TradeRoute
        {
            public List<string> Locations;

            public int Importance;

            // Constructor
            public TradeRoute(List<string> Locations, int importance)
            {
                this.Locations = Locations;

                Importance = importance;
            }
        }


        public static void Init()
        {
            var agarisSpace = new Node("AgarisSpace", "Agaris Space", GC, new Vector3D(-3512604, -1232206, -2690052), 350000);
            agarisSpace.SpaceNode = true;
            _nodes.Add(agarisSpace);

            var sunsetCity = new Node("SunsetCity", "Sector Sunset City", GC, new Vector3D(-3662454, -1299814, -2643727), 20000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Urban);
            _staticEncounters.Add(new StaticEncounter(sunsetCity.Id, "SunsetCity", "CIVILIAN", new Vector3D(-3661158.5103733, -1301788.86345045, -2642200.20693818), false));
            _staticEncounters.Add(new StaticEncounter(sunsetCity.Id, "GC_HQ", "GC", new Vector3D(-3666520.36037844, -1286146.69744133, -2637629.47543184), true)); //Replace code


            var azuris = new Node("Azuris", "Sector Azuris", GC, new Vector3D(-3619303, -1302582, -2625850), 20000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Rural);
            _staticEncounters.Add(new StaticEncounter(azuris.Id, "Azuris", "CIVILIAN", new Vector3D(-3620611.28859755, -1307127.11006586, -2624833.43832917), false));
            _staticEncounters.Add(new StaticEncounter(azuris.Id, "GC_Azuris", "GC", new Vector3D(-3621083.44123095, -1305017.63823832, -2626367.6057209), true));


            var carcosa = new Node("Carcosa", "Sector Carcosa", GC, new Vector3D(-3713255, -1328026, -2555371), 20000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Urban);
            _staticEncounters.Add(new StaticEncounter(carcosa.Id, "Carcosa", "CIVILIAN", new Vector3D(-3705652.28656772, -1337580.22062336, -2554087.27439561), false));
            _staticEncounters.Add(new StaticEncounter(carcosa.Id, "GC_CarcosaBase", "GC", new Vector3D(-3707838.9730467, -1342020.98487312, -2562220.14979517), true));


            var ahe = new Node("AHE", "Sector AHE", AHE, new Vector3D(-3695102, -1296077, -2534068), 20000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Urban);
            _staticEncounters.Add(new StaticEncounter(ahe.Id, "AHE_HQ", "AHE", new Vector3D(-3695935.39711949, -1297736.31794599, -2536595.20992627), true));
            _staticEncounters.Add(new StaticEncounter(ahe.Id, "AHE_Outpost3", "AHE", new Vector3D(-3703442.66211895, -1300817.03738934, -2541847.28840769), false));
            _staticEncounters.Add(new StaticEncounter(ahe.Id, "AHE_Outpost2", "AHE", new Vector3D(-3710629.45681993, -1308011.02317491, -2548826.88948266), false));
            _staticEncounters.Add(new StaticEncounter(ahe.Id, "AHE_Outpost1", "AHE", new Vector3D(-3700830.81050136, -1312992.16548631, -2537729.80624713), false));
            

            var thorrix = new Node("Thorrix", "Sector Thorrix", SYN, new Vector3D(-3713255, -1328026, -2555371), 18000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Rural); //Agaris Desert
            _staticEncounters.Add(new StaticEncounter(thorrix.Id, "Thorrix", "CIVILIAN", new Vector3D(-3666954.82093188, -1319040.62686428, -2525488.18626478), false));


            var bratis = new Node("Bratis", "Sector Bratis", GC, new Vector3D(-3715949, -1286742, -2567999), 20000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Rural); //ZoneIslands
            _staticEncounters.Add(new StaticEncounter(bratis.Id, "Bratis", "CIVILIAN", new Vector3D(-3710210, -1279156, -2566853), false));
            _staticEncounters.Add(new StaticEncounter(bratis.Id, "GC_BratisBase", "GC", new Vector3D(-3715028.32314452, -1284294.30765805, -2572561.20657818), true));



            var midway = new Node("Midway", "Sector Midway", GC, new Vector3D(-3715949, -1286742, -2567999), 17500, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Military); 

            _nodes.Add(sunsetCity);
            _nodes.Add(azuris);
            _nodes.Add(carcosa);
            _nodes.Add(ahe);
            _nodes.Add(thorrix);
            _nodes.Add(bratis);
            _nodes.Add(midway);

            var station27 = new Node("Station27", "Station 27", GC, new Vector3D(-1972242, -1017989, -2314831),50000, macro: MarcoLocation.Bylen, planet: Planet.Space, feel: Feel.Urban);
            station27.SpaceNode = true;
            _nodes.Add(station27);
            _staticEncounters.Add(new StaticEncounter(station27.Id, "Station27", "CIVILIAN", new Vector3D(-1970922.053788, -1016003.00749572, -2313711.812254), false));





            //Doohan space
            _staticEncounters.Add(new StaticEncounter(null, "Doohan", "CIVILIAN", new Vector3D(1573459.05187182, 1167676.70389646, 3320013.77777762), false));


            var moonSpace = new Node("MoonSpace", "Moon Space", AHE, new Vector3D(-2578714, -1046592, -4663973), 200000);
            moonSpace.SpaceNode = true;
            var moon = new Node("Moon", "Moon", AHE, new Vector3D(-2613043, -1055747, -4763524), 32000);
            _nodes.Add(moonSpace);
            _nodes.Add(moon);

            var thora4Space = new Node("Thora4Space", "Thora 4 Space", ITC, new Vector3D(-759347, -1089606, -3471518), 200000);
            thora4Space.SpaceNode = true;
            var thora4 = new Node("Thora4", "Thora 4", ITC, new Vector3D(-666090, -1101079, -3477316), 94894);

            _staticEncounters.Add(new StaticEncounter(thora4.Id, "Nyx", "CIVILIAN", new Vector3D(-628437.849787989, -1095745.23158828, -3508361.90366662), false));

            _nodes.Add(thora4Space);
            _nodes.Add(thora4);


            var rakSpace = new Node("RakSpace", "RakSpace", GC, new Vector3D(-37016, -4831, 52338), 300000, Tags: new List<string> { "Bylen", "Rural" });
            rakSpace.SpaceNode = true;
            _staticEncounters.Add(new StaticEncounter(rakSpace.Id, "Rak", "CIVILIAN", new Vector3D(-34995.4115883877, -3251.23684096853, 48703.7032755286), false));

            _nodes.Add(rakSpace);

            var lezunoSpace = new Node("LezunoSpace", "Lezuno Space", GC, new Vector3D(668643, 561533, 3606399), 250000);
            lezunoSpace.SpaceNode = true;
            var lezuno = new Node("Lezuno", "Lezuno", REM, new Vector3D(595790, 474550, 3465430), 94894);
            _nodes.Add(lezunoSpace);
            _nodes.Add(lezuno);

            var lorusSpace = new Node("LorusSpace", "Lorus Space", SYN, new Vector3D(2730463, 1286599, 3912652), 200000);
            lorusSpace.SpaceNode = true;
            var lorus = new Node("Lorus", "Lorus", SYN, new Vector3D(2732579, 1294490, 3968789), 40894);
            _nodes.Add(lorusSpace);
            _nodes.Add(lorus);

            var craitSpace = new Node("CraitSpace", "Crait Space", GC, new Vector3D(2896731, 1043674, 1886515), 200000);
            craitSpace.SpaceNode = true;
            var crait = new Node("Crait", "Crait", GC, new Vector3D(2934898, 1019809, 1826268), 49000);
            _nodes.Add(craitSpace);
            _nodes.Add(crait);




            //DoriumThora4


            //var fafFaction = new Node("FAFFaction", "...", AHE, new Vector3D(-1129033.50, 126871.50, 1293873.50), 0, true); // Agaris
            var gcFaction = new Node("GCFaction", "...", GC, new Vector3D(0, 0, 0), 0, true); //Bylen
            var synFaction = new Node("SYNFaction", "...", SYN, new Vector3D(-1249083.5, -521370.5, -1803753.5), 0, true); //Lorus 

            var PURGEFaction = new Node("PURGEFaction", "...", PURGE, new Vector3D(0, 0, 0), 0, true); // Bylen
            var SDCFaction = new Node("SDCFaction", "...", SDC, new Vector3D(0, 0, 0),0, true); //Some place


            _nodes.Add(PURGEFaction);
            _nodes.Add(SDCFaction);
            _nodes.Add(gcFaction);
            _nodes.Add(synFaction);


            //Agaria run
            _TradeRoutes.Add(new TradeRoute(new List<string> { "Carcosa", "Thorrix", "AHE_HQ", "Bratis" }, 5));

            //The great ocean run
            _TradeRoutes.Add(new TradeRoute(new List<string> { "Carcosa", "SunsetCity", "AHE_HQ", "Bratis" }, 3));


            //Global run
            _TradeRoutes.Add(new TradeRoute(new List<string> { "Azuris", "SunsetCity", "AHE_HQ", "Bratis", "Carcosa", "Thorrix", "Station27" }, 1));

            //Space run
            _TradeRoutes.Add(new TradeRoute(new List<string> { "Rak", "Station27", "Doohan", "Nyx" }, 3));



            /*
            _staticEncounters.Add(new StaticEncounter(synFaction.Id,"SYN-MiningOutpost3", "SYN", new Vector3D(-1229434.03820444, -521175.882389913, -1789244.09383711), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-MiningOutpost2", "SYN", new Vector3D(-1224847.84297878, -523019.024865274, -1801070.01883598), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-StripMiningPlatform4", "SYN", new Vector3D(-1254874.33395561, -521010.475227488, -1827504.95448953), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-StripMiningPlatform1", "SYN", new Vector3D(-1252113.35237604, -524524.015235651, -1827805.21299045), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-FuelDepot3", "SYN", new Vector3D(-1231876.8267543, -525240.903534725, -1786703.49699656), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-FuelDepot2", "SYN", new Vector3D(-1251503.78547039, -532230.449342892, -1825666.96640694), false));
            _staticEncounters.Add(new StaticEncounter(synFaction.Id, "SYN-FuelDepot1", "SYN", new Vector3D(-1264759.02008796, -517528.282412784, -1822256.60960163), false));
            _staticEncounters.Add(new StaticEncounter(,"SYN-TalisOutpost", "SYN", new Vector3D(-1129254.57184159, 104270.56515784, 1348435.49548304), false));







            

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

            */







            /*
_staticEncounters.Add(new StaticEncounter(tifHQ.Id, "TIF_HQ", "TIF", new Vector3D(2734050.68562431, 1276435.43322163, 3985276.4057518), false));
_staticEncounters.Add(new StaticEncounter(synForegoneStation.Id, "SYN_ForegoneStation", "SYN", new Vector3D(2601932.05315993, 1166322.50770169, 3744821.84143732), false));
_staticEncounters.Add(new StaticEncounter(rosCrashedROS1.Id, "ROS_CrashedROS1", "ROS", new Vector3D(480945.174339629, 569786.094115278, 3605482.07872022), false));
_staticEncounters.Add(new StaticEncounter(rosAgarisSouthPoleStation.Id, "ROS_AgarisSouthPoleStation", "ROS", new Vector3D(-3669595.05016885, -1368489.11152586, -2588908.49134022), false));
_staticEncounters.Add(new StaticEncounter(rosDeepSpaceStation.Id, "ROS_DeepSpaceStation", "ROS", new Vector3D(1409212.66274555, 1269755.79727905, 4322539.1548167), false));
_staticEncounters.Add(new StaticEncounter(rosLezunoCronyxResearch.Id, "ROS_LezunoCronyxResearch", "ROS", new Vector3D(557528.671733663, 488566.739911628, 3419285.41484963), false));
_staticEncounters.Add(new StaticEncounter(rosCraitCrystalResearch.Id, "ROS_CraitCrystalResearch", "ROS", new Vector3D(2928114.48434223, 1033055.92116714, 1853819.00355293), false));
_staticEncounters.Add(new StaticEncounter(remAgarisPegasusSurvivorsCamp.Id, "REM_AgarisPegasusSurvivorsCamp", "REM", new Vector3D(-3684858.9828521, -1340318.05365089, -2629888.09211866), false));
_staticEncounters.Add(new StaticEncounter(remAgarisCrashedPegasus.Id, "REM_AgarisCrashedPegasus", "REM", new Vector3D(-3684660.37842435, -1340333.81857279, -2629971.68203353), false));
_staticEncounters.Add(new StaticEncounter(remLezunoHQ.Id, "REM_LezunoHQ", "REM", new Vector3D(638654.102948221, 480874.683314898, 3424420.31737057), false));
_staticEncounters.Add(new StaticEncounter(itcHotel.Id, "ITC_Hotel", "ITC", new Vector3D(-1018002.33077798, -1016204.27518199, -3585528.06290849), false));
_staticEncounters.Add(new StaticEncounter(itcAether.Id, "ITC_Aether", "ITC", new Vector3D(1999675.78774544, 1167867.71608667, 2124757.03013696), false));
_staticEncounters.Add(new StaticEncounter(indepMack.Id, "INDEP_Mack", "INDEP", new Vector3D(-2976927.87489875, -1018098.76513987, -3064150.67856171), false));
_staticEncounters.Add(new StaticEncounter(gcAgarisMiningOutpost6.Id, "GC_AgarisMiningOutpost6", "GC", new Vector3D(-3635728.76042521, -1301802.24528268, -2635906.14801467), false));
_staticEncounters.Add(new StaticEncounter(gcAgarisMiningOutpost3.Id, "GC_AgarisMiningOutpost3", "GC", new Vector3D(-3668955.95144752, -1339084.73596799, -2634905.37326808), false));
_staticEncounters.Add(new StaticEncounter(gcAgarisMiningOutpost2.Id, "GC_AgarisMiningOutpost2", "GC", new Vector3D(-3658836.60768252, -1308455.05569391, -2642781.22025541), false));
_staticEncounters.Add(new StaticEncounter(gcAgarisMiningOutpost1.Id, "GC_AgarisMiningOutpost1", "GC", new Vector3D(-3635869.9614674, -1289861.9860142, -2633020.16869006), false));
_staticEncounters.Add(new StaticEncounter(gcAgarisMidWayBase.Id, "GC_AgarisMidWayBase", "GC", new Vector3D(-3713060.8323356, -1292017.64672996, -2609295.4960585), false));
_staticEncounters.Add(new StaticEncounter(gcAgarisSouthPoleBase.Id, "GC_AgarisSouthPoleBase", "GC", new Vector3D(-3668943.69378092, -1368590.00081129, -2579805.05893874), false));
_staticEncounters.Add(new StaticEncounter(gcHQ.Id, "GC_HQ", "GC", new Vector3D(-3666520.36037844, -1286146.69744133, -2637629.47543184), false));
_staticEncounters.Add(new StaticEncounter(draMiningOutpost2.Id, "DRA_MiningOutpost2", "DRA", new Vector3D(-3635898.90866217, -1321588.61523538, -2532896.49538995), false));
_staticEncounters.Add(new StaticEncounter(draMiningOutpost1.Id, "DRA_MiningOutpost1", "DRA", new Vector3D(-3679092.65422577, -1327946.79104204, -2530004.69182944), false));
_staticEncounters.Add(new StaticEncounter(draHQ.Id, "DRA_HQ", "DRA", new Vector3D(-3667011.40316274, -1319161.30054833, -2525507.58842412), false));
_staticEncounters.Add(new StaticEncounter(crusadersBaza.Id, "CRUSADERS_Baza", "CRUSADERS", new Vector3D(-656164.879712379, -1083872.66373435, -3522783.96144957), false));
_staticEncounters.Add(new StaticEncounter(crusadersFactory.Id, "CRUSADERS_Factory", "CRUSADERS", new Vector3D(-623016.227552846, -1099802.54340612, -3499833.93546406), false));
_staticEncounters.Add(new StaticEncounter(crusadersIceStation.Id, "CRUSADERS_IceStation", "CRUSADERS", new Vector3D(-621560.298283929, -1085208.02156901, -3465409.90665213), false));


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

        public static void ShuffleStaticEncounters()
        {

            int n = _staticEncounters.Count;

            for (int i = n - 1; i > 0; i--)
            {
                // Pick a random index from 0 to i
                int j = AaWSessionCore.GetRandomNumber(0,i + 1);

                // Swap staticEncounter[i] with the element at random index j
                var temp = _staticEncounters[i];
                _staticEncounters[i] = _staticEncounters[j];
                _staticEncounters[j] = temp;
            }
        }

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


            var forceA = Utils.NextGaussian(modifiedStrengthA, 75);
            var forceB = Utils.NextGaussian(modifiedStrengthB, 75);

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
