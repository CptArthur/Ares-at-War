using AaWFoodScript;
using BigGustave;
using OreDetectorReforged;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Utils;
using VRageMath;

namespace AaWFoodScript
{
    static class MapUtilities
    {
        public static readonly PermaCache<string, Png> materialPngs = new PermaCache<string, Png>(LoadPlanetFacePng);
        private static Vector3 baseSouth = new Vector3(0f, 1f, 0f);

        public static readonly Vector3D agarisCenter = new Vector3D(-3663015.14510301,-1309985.98462825,-2583899.76531797);

        public const int mapWidth = 8192;
        public const int mapHeight = 4096;

        // 🔹 Global offsets (degrees)
        public const double LATITUDE_OFFSET_DEG = 0.0;
        public const double LONGITUDE_OFFSET_DEG = 0.0;

        public static bool IsAtFishLocation(Vector3D worldPos)
        {
            if ((worldPos - agarisCenter).Length() > 61000)
                return false;

            MyTuple<int, int> tja = WorldToPixel(worldPos);


            return ReadTheImage("AaWAgaris_Fish", tja.Item1,tja.Item2);

        }



        public static MyTuple<int, int> WorldToPixel(Vector3D worldPos, int width = mapWidth, int height = mapHeight)
        {
            // Vector relative to planet center
            Vector3D rel = worldPos - agarisCenter;

            // Normalize to unit sphere
            Vector3D n = rel;
            n.Normalize();

            // Recover latitude & longitude
            double lat_r = Math.Asin(n.Y);
            double lon_r = Math.Atan2(n.X, -n.Z);

            // Convert to degrees
            double lat_deg = lat_r * (180.0 / Math.PI);
            double lon_deg = lon_r * (180.0 / Math.PI);

            // Re-apply offsets (inverse of PixelToWorld)
            lat_deg -= LATITUDE_OFFSET_DEG;
            lon_deg -= LONGITUDE_OFFSET_DEG;

            // Clamp & wrap
            lat_deg = Math.Max(Math.Min(lat_deg, 90.0), -90.0);
            lon_deg = ((lon_deg + 180.0) % 360.0) - 180.0;

            // Convert to pixel coords
            double x = (180.0 - lon_deg) / 360.0 * width;
            double y = (90.0 - lat_deg) / 180.0 * height;

            return MyTuple.Create((int)Math.Round(x), (int)Math.Round(y));
        }



        static BinaryReader MyReadPlanetMat(string f)
        {
            var file = "Textures/Maps/" + f + ".png";
            foreach (var mod in MyAPIGateway.Session.Mods)
            {
                try { return MyAPIGateway.Utilities.ReadBinaryFileInModLocation(file, mod); }
                catch { }
            }
            return MyAPIGateway.Utilities.ReadBinaryFileInGameContent(file);
        }

        public static Png LoadPlanetFacePng(string f)
        {
            Png png;
            using (var filestream = MyReadPlanetMat(f))
                png = Png.Open(filestream.BaseStream);
            return png;
        }


        public static bool ReadTheImage(string image ,int x, int y)
        {
            var png = materialPngs.Get(image);

            if(png == null) return false;  

            var px = png.GetPixel(x, y);

            if (px.R == 10) //Hardcode
                return true;

            return false;
        }






    }
}
