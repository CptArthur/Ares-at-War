using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using VRageMath;
using Sandbox.Game;
using AresAtWar.Logging;
using AresAtWar.Configuration;
using Sandbox.ModAPI;
using AresAtWar.SessionCore;
using VRage.Game.ModAPI;

namespace AresAtWar.War
{
    public static partial class WarSim
    {
        public static readonly List<Node> _nodes = new List<Node>(); 
        public static readonly List<Frontline> _frontlines = new List<Frontline>();  
        public static readonly List<StaticEncounter> _staticEncounters = new List<StaticEncounter>();  
        public static readonly List<TradeRoute> _TradeRoutes = new List<TradeRoute>();

        private static Faction GC = new Faction("GC", new Color(255, 0, 0)); // red
        private static Faction UNION = new Faction("UNION", new Color(0, 255, 0)); // green
        private static Faction SHIVAN = new Faction("SHIVAN", new Color(255, 165, 0)); // orange

        private static Faction PURGE = new Faction("PURGE", new Color(139, 0, 0), EventFaction: true); // darkred
        private static Faction CRUSADERS = new Faction("CRUSADERS", new Color(255, 106, 0), EventFaction: true); // orange

        private static Faction SHIPPERS = new Faction("SHIPPERS", new Color(165, 42, 42), MinorFaction: true); // brown
        //private static Faction IRONFIST = new Faction("IRONFIST", new Color(128, 128, 128), MinorFaction: true); // gray
        private static Faction ITC = new Faction("ITC", new Color(173, 216, 230), MinorFaction: true); // lightblue
        private static Faction CIVILIAN = new Faction("CIVILIAN", new Color(212, 216, 230), MinorFaction: true); //Whiteish I hope
        private static Faction AZURIS = new Faction("AZURIS", new Color(212, 216, 230), MinorFaction: true); //Whiteish I hope
        private static Faction BRATIS = new Faction("BRATIS", new Color(212, 216, 230), MinorFaction: true); //Whiteish I hope
        private static Faction THORRIX = new Faction("THORRIX", new Color(212, 216, 230), MinorFaction: true); //Whiteish I hope
        private static Faction SUNSETCITY = new Faction("SUNSETCITY", new Color(212, 216, 230), MinorFaction: true); //Whiteish I hope
        private static Faction DRA = new Faction("DRA", new Color(212, 111, 230), MinorFaction: true); //Whiteish I hope


        //public static List<Faction> _factions = new List<Faction>() { UNION, SYN, GC, PURGE, SDC };

        public static List<Faction> _allfactions = new List<Faction>(){ UNION, SHIVAN, GC, PURGE, CRUSADERS, SHIPPERS, ITC, CIVILIAN, SUNSETCITY, AZURIS, DRA,BRATIS,THORRIX };

        private const int Rounds = 67;
        private static readonly Random _random = new Random();


        public class TradeRoute
        {
            public List<string> FactionTags;
            public List<StaticEncounter> Locations;

            public int Importance;

            // Constructor
            public TradeRoute(List<StaticEncounter> Locations, int importance, List<string> factionTags = null)
            {
                this.FactionTags = factionTags;
                this.Locations = Locations;

                Importance = importance;
            }
        }


        public static void Init()
        {
            // Nodes and StaticEncounters for Agaris and related locations
            var agarisSpaceNode = new Node("AgarisSpace", "Agaris Space", GC, new Vector3D(-3512604, -1232206, -2690052), 350000);
            agarisSpaceNode.SpaceNode = true;

            //Where old Sunset City used to be
            var AgarisGC = new Node("Jungle", "Sector Agaris GC", GC, new Vector3D(-3656958.78203563, -1294191.06964652, -2641742.52983799), 25000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Military);



            //Independents
            var azurisNode = new Node("Azuris", "Sector Azuris", AZURIS, new Vector3D(-3684664.31116301, -1332121.38896001, -2634179.10134753), 8000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Rural);
            var azuris = new StaticEncounter(azurisNode.Id, "Azuris", "AZURIS", new Vector3D(-3684664.31116301, -1332121.38896001, -2634179.10134753), false, new List<string>() { "Settlement" });

            var thorrixNode = new Node("Thorrix", "Sector Thorrix", THORRIX, new Vector3D(-3664945.28584011, -1323993.39580602, -2526529.90910624), 8000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Rural);
            var thorrix = new StaticEncounter(thorrixNode.Id, "Thorrix", "THORRIX", new Vector3D(-3664945.28584011, -1323993.39580602, -2526529.90910624), false);


            var bratisNode = new Node("Bratis", "Sector Bratis", BRATIS, new Vector3D(-3627814.23905667, -1262975.57687012, -2588390.45887829), 8000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Rural);
            var bratis = new StaticEncounter(bratisNode.Id, "Bratis", "BRATIS", new Vector3D(-3627814.23905667, -1262975.57687012, -2588390.45887829), false);


            // UNION Farm land
            var agaria = new Node("Agaria", "Sector Agaria", GC, new Vector3D(-3702782, -1299410, -2541753), 20000, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Urban);
            var sunsetcity = new StaticEncounter(agaria.Id, "SunsetCity", "SUNSETCITY", new Vector3D(-3703710.56552889, -1300687.78803807, -2542134.43874369), true, new List<string>() { "Settlement" });
            var aheEncounter2 = new StaticEncounter(agaria.Id, "AgarisOutpost1", "SUNSETCITY", new Vector3D(-3703442.66211895, -1300817.03738934, -2541847.28840769), false);
            var aheEncounter3 = new StaticEncounter(agaria.Id, "AgarisOutpost2", "SUNSETCITY", new Vector3D(-3710629.45681993, -1308011.02317491, -2548826.88948266), false);
            var aheEncounter4 = new StaticEncounter(agaria.Id, "AgarisOutpost3", "SUNSETCITY", new Vector3D(-3700830.81050136, -1312992.16548631, -2537729.80624713), false);

            //Update coords
            var ITC_AgarisVinyTradeOutpost = new StaticEncounter(agaria.Id, "ITC_AgarisVinyTradeOutpost", "ITC", new Vector3D(-3715028.32314452, -1284294.30765805, -2572561.20657818), true, new List<string>() { "TradeStation" });



            
            //Midway
            var midwayNode = new Node("Midway", "Sector Midway", GC, new Vector3D(-3715949, -1286742, -2567999), 17500, macro: MarcoLocation.Bylen, planet: Planet.Agaris, feel: Feel.Military);

            // Nodes and StaticEncounters for space locations
            var station27Node = new Node("Station27Space", "Station27 Sector", GC, new Vector3D(-2070540.14510301, -1017587.98462825, -3367463.76531797), 1100000, macro: MarcoLocation.Bylen, planet: Planet.Space, feel: Feel.Barren);
            station27Node.SpaceNode = true;
            var station27 = new StaticEncounter(station27Node.Id, "Station27", "CIVILIAN", new Vector3D(-1970922.053788, -1016003.00749572, -2313711.812254), false);
            var doohan = new StaticEncounter(station27Node.Id, "Doohan", "CIVILIAN", new Vector3D(1573459.05187182, 1167676.70389646, 3320013.77777762), false);

            // Nodes and StaticEncounters for space locations
            var MilaNode = new Node("MilaBelt", "Mila Belt", GC, new Vector3D(2299140.85489699, 1166634.01537175, 2935888.23468203), 880000, macro: MarcoLocation.Mila, planet: Planet.Space, feel: Feel.Barren);
            MilaNode.SpaceNode = true;



            // Nodes and StaticEncounters for Thora 4
            var thora4SpaceNode = new Node("Thora4Space", "Thora 4 Space", ITC, new Vector3D(-759347, -1089606, -3471518), 200000, macro: MarcoLocation.Bylen, planet: Planet.Thora4, feel: Feel.Barren);
            thora4SpaceNode.SpaceNode = true;


            var thora4Node = new Node("Thora4", "Thora 4", ITC, new Vector3D(-666090, -1101079, -3477316), 94894, macro: MarcoLocation.Bylen, planet: Planet.Thora4, feel: Feel.Barren);
            var starlight = new StaticEncounter(thora4Node.Id, "StarlightCity", "CIVILIAN", new Vector3D(-712725.31332499, -1112936.04750601, -3487850.09379849), false);

            //var nyx = new StaticEncounter(thora4Node.Id, "Nyx", "CIVILIAN", new Vector3D(-665793.281388648, -1100790.49719151, -3477104.76459571), false, new List<string>() { "Settlement" });



            var rakSpaceNode = new Node("RakSpace", "RakSpace", GC, new Vector3D(-37016, -4831, 52338), 300000, Tags: new List<string> { "Bylen", "Rural" }, macro: MarcoLocation.Rak, planet: Planet.Space, feel: Feel.Barren);
            rakSpaceNode.SpaceNode = true;
            var rak = new StaticEncounter(rakSpaceNode.Id, "Rak", "CIVILIAN", new Vector3D(-34995.4115883877, -3251.23684096853, 48703.7032755286),false);



            var lezunoSpaceNode = new Node("LezunoSpace", "Lezuno Space", GC, new Vector3D(668643, 561533, 3606399), 250000, macro: MarcoLocation.Mila, planet: Planet.Lezuno, feel: Feel.Barren);
            lezunoSpaceNode.SpaceNode = true;
            var lezunoNode = new Node("Lezuno", "Lezuno", SHIPPERS, new Vector3D(595790, 474550, 3465430), 94894, macro: MarcoLocation.Mila, planet: Planet.Lezuno, feel: Feel.Barren);


            var lorusSpaceNode = new Node("LorusSpace", "Lorus Space", SHIVAN, new Vector3D(2730463, 1286599, 3912652), 200000, macro: MarcoLocation.Mila, planet: Planet.Lorus, feel: Feel.Barren);
            lorusSpaceNode.SpaceNode = true;
            var lorusNode = new Node("Lorus", "Lorus", SHIVAN, new Vector3D(2732579, 1294490, 3968789), 40894, macro: MarcoLocation.Mila, planet: Planet.Lorus, feel: Feel.Barren);


            var craitSpaceNode = new Node("CraitSpace", "Crait Space", GC, new Vector3D(2896731, 1043674, 1886515), 200000, macro: MarcoLocation.Mila, planet: Planet.Crait, feel: Feel.Barren);
            craitSpaceNode.SpaceNode = true;
            var craitNode = new Node("Crait", "Crait", GC, new Vector3D(2934898, 1019809, 1826268), 49000, macro: MarcoLocation.Mila, planet: Planet.Crait, feel: Feel.Barren);
            //var voss = new StaticEncounter(craitNode.Id, "Voss", "CIVILIAN", new Vector3D(2962729.53024317, 1031267.33046815, 1817036.9772462), false);
            //var basin = new StaticEncounter(craitNode.Id, "Basin", "CIVILIAN", new Vector3D(2928114, 1033055, 1853819), false);


            //DoriumThora4

            //var fafFaction = new Node("FAFFaction", "...", AHE, new Vector3D(-1129033.50, 126871.50, 1293873.50), 0, true); // Agaris
            var gcFactionNode = new Node("GCFaction", "...", GC, new Vector3D(0, 0, 0), 0, true); //Bylen
            var synFactionNode = new Node("SHIVANFaction", "...", SHIVAN, new Vector3D(-1249083.5, -521370.5, -1803753.5), 0, true); //Lorus 

            var PURGEFactionNode = new Node("PURGEFaction", "...", PURGE, new Vector3D(0, 0, 0), 0, true); // Bylen
            var SDCFactionNode = new Node("CRUSADERSFaction", "...", CRUSADERS, new Vector3D(0, 0, 0),0, true); //Some place




            //Agaria West
            _TradeRoutes.Add(new TradeRoute(new List<StaticEncounter> { azuris, sunsetcity,  bratis }, 5, new List<string>() { "CIVILIAN" }));

            //Agaria East
            _TradeRoutes.Add(new TradeRoute(new List<StaticEncounter> { sunsetcity, thorrix }, 3, new List<string>() { "CIVILIAN" }));


            //Everything run
            _TradeRoutes.Add(new TradeRoute(new List<StaticEncounter> { sunsetcity, starlight, station27 }, 5));

            //Space run
            _TradeRoutes.Add(new TradeRoute(new List<StaticEncounter> { rak, station27 }, 10, new List<string>() { "CIVILIAN" }));

            //ITC only
            //_TradeRoutes.Add(new TradeRoute(new List<StaticEncounter> { sunsetcity, station27 , ITC_AgarisAtlas, ITC_AgarisVinyTradeOutpost }, 3, new List<string>() { "ITC" }));



            /*

            _frontlines.Add(new Frontline("Thorrix", "AgarisSpace"));
            _frontlines.Add(new Frontline("Thorrix", "AHE"));
            _frontlines.Add(new Frontline("Thorrix", "Carcosa"));
            _frontlines.Add(new Frontline("Azuris", "SunsetCity"));
            _frontlines.Add(new Frontline("Carcosa", "AHE"));
            _frontlines.Add(new Frontline("Bratis", "AHE"));
            _frontlines.Add(new Frontline("Bratis", "Carcosa"));
            _frontlines.Add(new Frontline("Bratis", "Midway"));
            _frontlines.Add(new Frontline("SunsetCity", "Midway"));
            _frontlines.Add(new Frontline("Carcosa", "Midway"));

            //Planet to Orbit
            _frontlines.Add(new Frontline("Thorrix", "AgarisSpace"));
            _frontlines.Add(new Frontline("Midway", "AgarisSpace"));
            _frontlines.Add(new Frontline("Bratis", "AgarisSpace"));
            _frontlines.Add(new Frontline("Carcosa", "AgarisSpace"));
            _frontlines.Add(new Frontline("AHE", "AgarisSpace"));
            _frontlines.Add(new Frontline("SunsetCity", "AgarisSpace"));
            _frontlines.Add(new Frontline("Azuris", "AgarisSpace"));
            _frontlines.Add(new Frontline("Midway", "AgarisSpace"));

            _frontlines.Add(new Frontline("Thora4", "Thora4Space"));
            _frontlines.Add(new Frontline("Crait", "CraitSpace"));
            _frontlines.Add(new Frontline("Lezuno", "LezunoSpace"));
            _frontlines.Add(new Frontline("Lorus", "LorusSpace"));


            //Belt

            _frontlines.Add(new Frontline("Station27Space", "AgarisSpace"));
            _frontlines.Add(new Frontline("Station27Space", "Thora4Space"));
            _frontlines.Add(new Frontline("AgarisSpace", "Thora4Space"));
            //Moon Space
            
            _frontlines.Add(new Frontline("LezunoSpace", "CraitSpace"));
            _frontlines.Add(new Frontline("LorusSpace", "LezunoSpace"));
            _frontlines.Add(new Frontline("CraitSpace", "LorusSpace"));

            //RakSpace nodes as last. Because it is better to take control of their own gas giant
            _frontlines.Add(new Frontline("Station27Space", "RakSpace"));
            _frontlines.Add(new Frontline("AgarisSpace", "RakSpace"));
            _frontlines.Add(new Frontline("Thora4Space", "RakSpace"));

            //_frontlines.Add(new Frontline("DoohanSpace", "RakSpace"));
            _frontlines.Add(new Frontline("LezunoSpace", "RakSpace"));
            _frontlines.Add(new Frontline("CraitSpace", "RakSpace"));
            */



        }


        public static bool FactionsAtWar(string factionA, string factionB)
        {
            if (factionA == factionB)
                return false;


            bool isAtWar = false;

            // Ensure factionA and factionB are in alphabetical order
            string firstFaction = string.Compare(factionA, factionB) < 0 ? factionA : factionB;
            string secondFaction = firstFaction == factionA ? factionB : factionA;

            // Check if the war status exists for the alphabetically ordered key
            if (MyAPIGateway.Utilities.GetVariable<bool>(firstFaction + secondFaction + "War", out isAtWar))
            {
                return isAtWar;
            }

            return isAtWar;
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

                    activeBattles.Add(factionsInvolved);
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

 

                string[] validTags = { "UNION", "GC", "SHIVAN" };

                if (!validTags.Contains(node.Faction.Tag))
                    continue;

                //node.BuildFort();

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

            public static void SendChatMessageToAll(string Author, string Message)
            {
                List<IMyPlayer> all_players = new List<IMyPlayer>();
                MyAPIGateway.Multiplayer.Players.GetPlayers(all_players);

                foreach (var p in all_players)
                {
                    MyVisualScriptLogicProvider.SendChatMessage(Message, Author, p.IdentityId);
                }

            }

        }





    }
}
