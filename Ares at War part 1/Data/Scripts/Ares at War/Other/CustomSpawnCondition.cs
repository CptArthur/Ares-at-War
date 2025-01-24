using System;
using System.Collections.Generic;
using System.Linq;
using AresAtWar.API;
using AresAtWar.War;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
using static AresAtWar.War.WarSim;

namespace AresAtWar.SessionCore
{
    public class CustomSpawnConditions
    {
        public static Vector3D PlanetCenter = new Vector3D(0, 0, 0);

        public static Vector3D NebulaCenter = new Vector3D(-17115, -4610, -8171);

        public static Vector3D MilaCenter = new Vector3D(2299140.85489699, 1166634.01537175, 2935888.23468203);


        public static Vector3D BylenCenter = new Vector3D(-2070540.14510301, -1017587.98462825, -3367463.76531797);
        public static Vector3D AgarisCenter = new Vector3D(-3663015.14510301, -1309985.98462825, -2583899.76531797);
        public static Vector3D Thora4Center = new Vector3D(-666090.15, -1101079.98, -3477316.77);




        public static Node GetClosestNode(Vector3D position, ref bool inside)
        {
            Node closestNode = null;
            double closestDistance = double.MaxValue;

            foreach (var node in _nodes)
            {

                double distance = Vector3D.Distance(node.Location, position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNode = node;
                }
            }


            if ((closestNode.Location - position).Length() < closestNode.Radius)
                inside = true;


            return closestNode;
        }


        public static bool AaW_Home(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            //MyAPIGateway.Utilities.ShowMessage("AaW", "AaW_Home");
            if (string.IsNullOrEmpty(SpawnGroupSubtypeID) || !SpawnGroupSubtypeID.Contains("_"))
            {
                return false;
            }

            // Retrieve the FactionTag from the SpawnGroupSubtypeID
            string FactionTag = SpawnGroupSubtypeID.Split('_')[0];

            if (FactionTag == "CIVILIAN")
                return false;

            if (FactionTag == "SPRT")
                return false;



            var factionobj = _allfactions.FirstOrDefault(n => n.Tag == FactionTag);
            if (factionobj == null)
            {
                MyAPIGateway.Utilities.ShowMessage("AaW", $"Oh, oh. *{FactionTag}* is not defined. Please notify CptArthur");
                return false;
            }


            bool inside = false;
            var closestnode = GetClosestNode(location, ref inside);

            //If not inside
            if (!inside)
                return false;

            if (closestnode.Faction.Tag != FactionTag)
                return false;


            return true;

        }

        public static bool AaW_FrontLine(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            //MyAPIGateway.Utilities.ShowMessage("AaW", "AaW_Home");
            if (string.IsNullOrEmpty(SpawnGroupSubtypeID) || !SpawnGroupSubtypeID.Contains("_"))
            {
                return false;
            }

            // Retrieve the FactionTag from the SpawnGroupSubtypeID
            string FactionA = "";
            string FactionB = "";

            if (SpawnConditionsProfileSubtypeID.Contains("GCUNIONWar"))
            {
                FactionA = "GC";
                FactionB = "UNION";
            }


            if (string.IsNullOrEmpty(FactionA))
                return false;

            if (string.IsNullOrEmpty(FactionB))
                return false;



            for (int i = 0; i < WarSim._frontlines.Count; i++)
            {
                var frontline = WarSim._frontlines[i];

                if (frontline.Active)
                {
                    var nodeA = WarSim._nodes.FirstOrDefault(n => n.Id == frontline.NodeAId);
                    var nodeB = WarSim._nodes.FirstOrDefault(n => n.Id == frontline.NodeBId);

                    if (nodeA.Faction.Tag == nodeB.Faction.Tag)
                        continue;

                    if (nodeA.Faction.Tag != FactionA && nodeA.Faction.Tag != FactionB)
                        continue;

                    if (nodeB.Faction.Tag != FactionA && nodeB.Faction.Tag != FactionB)
                        continue;

                    var distance = Math.Abs(Vector3D.Distance(frontline.CurrentCoords, location));

                    if(nodeA.SpaceNode == false && nodeB.SpaceNode == false)
                    {
                        //Planet to planet?
                        if (distance < 20000)
                            return true;
                    }
                    else if(nodeA.SpaceNode == true && nodeB.SpaceNode == true)
                    {
                        //Space to Space? 
                        if (distance < 300000)
                            return true;
                    }
                    else
                    {
                        //Planet to Space?
                        if (distance < 30000)
                            return true;
                    }

                }
                continue;
            }

            return false;

        }






        public static bool AaW_Outside(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            //MyAPIGateway.Utilities.ShowMessage("AaW", "AaW_Outside");
            if (string.IsNullOrEmpty(SpawnGroupSubtypeID) || !SpawnGroupSubtypeID.Contains("_"))
            {
                return false;
            }

            // Retrieve the FactionTag from the SpawnGroupSubtypeID
            string FactionTag = SpawnGroupSubtypeID.Split('_')[0];
            if (FactionTag == "CIVILIAN")
                return true;
            if (FactionTag == "SPRT")
                return true;


            var factionobj = _allfactions.FirstOrDefault(n => n.Tag == FactionTag);
            if (factionobj == null)
            {
                MyAPIGateway.Utilities.ShowMessage("AaW", $"Oh, oh. *{FactionTag}* is not defined. Please notify CptArthur");
                return false;
            }


            bool inside = false;
            var closestnode = GetClosestNode(location, ref inside);



            //If inside
            if (inside)
                return false;

            if (!factionobj.HasPresenceInMacro(closestnode.Macro))
                return false;

            if (!closestnode.SpaceNode)
                if (factionobj.HasPresenceOnPlanet(closestnode.Planet))
                    return true;
                else
                    return false;


            return true;


        }


        public static bool MilaRing(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            //MyAPIGateway.Utilities.ShowMessage("AaW", "MilaRing");
            var distance = Math.Abs(Vector3D.Distance(MilaCenter, location));
            var ydistance = Math.Abs((location.Y - MilaCenter.Y));


            if (((610000 <= distance && distance <= 700000) || (800000 <= distance && distance <= 835000) || (850000 <= distance && distance <= 880000)) && ydistance <= 5500)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool BylenRing(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            //MyAPIGateway.Utilities.ShowMessage("AaW", "BylenRing");
            var distance = Math.Abs(Vector3D.Distance(BylenCenter, location));
            var ydistance = Math.Abs((location.Y - BylenCenter.Y));

            if (((650000 <= distance && distance <= 740000) || (780000 <= distance && distance <= 985000) || (1025000 <= distance && distance <= 1100000)) && ydistance <= 6000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool Thora4DeepOcean(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            var distancefromlocation = (location - Thora4Center).Length();
            //MyAPIGateway.Utilities.ShowMessage("AaW", "Thora4DeepOcean");
            if (distancefromlocation > 58000.0)
            {
                return false;
            }

            MyPlanet Thora4 = MyGamePruningStructure.GetClosestPlanet(location);

            if (Thora4 == null)
                return false;

            if (!Thora4.Name.Contains("Planet Thora 4"))
                return false;


            //49.12km waterlevel

            var SurfacePoint = Thora4.GetClosestSurfacePointGlobal(location);
            var distancefromsurface = (SurfacePoint - Thora4Center).Length();

            //300 meter
            if (distancefromsurface > 48820)
            {
                return false;
            }
            return true;
        }

        public static bool Thora4Land(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            var distancefromlocation = (location - Thora4Center).Length();
            //MyAPIGateway.Utilities.ShowMessage("AaW", "Thora4Land");
            if (distancefromlocation > 58000.0)
            {
                return false;
            }

            MyPlanet Thora4 = MyGamePruningStructure.GetClosestPlanet(location);

            if (Thora4 == null)
                return false;

            if (!Thora4.Name.Contains("Planet Thora 4"))
                return false;


            var SurfacePoint = Thora4.GetClosestSurfacePointGlobal(location);
            var distancefromsurface = (SurfacePoint - Thora4Center).Length();
            //49.12km waterlevel
            //AboveWaterLevel
            if (distancefromsurface < 49120)
            {
                return false;
            }
            return true;
        }


        public static bool AgarisDeepOcean(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            var distancefromlocation = (location - AgarisCenter).Length();
            //MyAPIGateway.Utilities.ShowMessage("AaW", "AgarisDeepOcean");
            if (distancefromlocation > 65000.0)
            {
                return false;
            }

            MyPlanet Agaris = MyGamePruningStructure.GetClosestPlanet(location);

            if (Agaris == null)
                return false;

            if(!Agaris.Name.Contains("Planet Agaris"))
                return false;


            //58.90km waterlevel

            var SurfacePoint = Agaris.GetClosestSurfacePointGlobal(location);
            var distancefromsurface = (SurfacePoint - AgarisCenter).Length();

            //300 meter
            if(distancefromsurface > 58600)
            {
                return false;
            }
            return true;
        }

        public static bool AgarisLand(string SpawnGroupSubtypeID, string SpawnConditionsProfileSubtypeID, string typeofspawn, Vector3D location)
        {
            var distancefromlocation = (location - AgarisCenter).Length();
            //MyAPIGateway.Utilities.ShowMessage("AaW", "AgarisLand");

            if (distancefromlocation > 65000.0)
            {
                return false;
            }

            MyPlanet Agaris = MyGamePruningStructure.GetClosestPlanet(location);

            if (Agaris == null)
                return false;

            if (!Agaris.Name.Contains("Planet Agaris"))
                return false;


            var SurfacePoint = Agaris.GetClosestSurfacePointGlobal(location);
            var distancefromsurface = (SurfacePoint - AgarisCenter).Length();
            //AboveWaterLevel
            if (distancefromsurface < 58880)
            {
                return false;
            }
            return true;
        }



        public static void CustomSpawnInit(MESApi MESApi)
        {

            MESApi.ToggleSpawnGroupEnabled("Reaver - Group - Test - A", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Aggressor", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Berserker", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Butcher", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Carver", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Charger", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Cockroach", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Crimson", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - CrimsonTormentor", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Devourer", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - GroupEncounter - Hard - A", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - GroupEncounter - Hard - B", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - GroupEncounter - Medium - A", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - GroupEncounter - Medium - B", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Havelock", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Impaler", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Interloper", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Invader - Space", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Mutilator", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Nikkadudu", false);

            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Piercer", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Scourge", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Slayer", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Thrasher", false);
            MESApi.ToggleSpawnGroupEnabled("Reaver - SpawnGroup - Tormentor", false);


        }




    }


}

