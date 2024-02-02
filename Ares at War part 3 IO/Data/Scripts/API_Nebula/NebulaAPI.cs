﻿using ProtoBuf;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace Jakaria.API
{
    //Only Include this file in your project
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class NebulaAPI : MySessionComponentBase
    {
        //Do not touch anything else

        public static string ModName = "UnknownMod";
        public const ushort ModHandlerID = 13377;
        public const ushort ModHandlerIDWeather = 13378;
        public const int ModAPIVersion = 7;
        public bool Registered { get; private set; } = false;

        private static Dictionary<string, Delegate> ModAPIMethods;

        private static Func<int, string, bool> _VerifyVersion;
        private static Func<Vector3D, bool> _InsideNebulaBounding;
        private static Func<Vector3D, bool> _InsideNebula;
        private static Func<Vector3D, float> _GetNebulaDensity;
        private static Func<Vector3D, float> _GetMaterial;
        private static Action<Vector3D> _CreateLightning;
        private static Func<Vector3D, string, bool, bool> _CreateWeather;
        private static Func<Vector3D, bool> _CreateRandomWeather;
        private static Func<Vector3D, bool> _RemoveWeather;
        private static Func<Vector3D, string> _GetWeather;
        private static Action<int?> _ForceRenderRadiation;
        private static Action<bool?> _ForceRenderIons;
        private static Action<string> _RunCommand;

        private static Action<bool?> _ForceRenderComets;
        private static Func<string> _GetRandomWeather;
        private static Func<Vector3D, bool> _IsNearWeather;

        private static Action<Vector3D, int> _CreateNebula;

        private static Action<Vector3D, Vector4, Vector4> _SetColors;
        private static Func<Vector3D, Vector4?> _GetPrimaryColor;
        private static Func<Vector3D, Vector4?> _GetSecondaryColor;
        private static Func<Vector3D, bool> _RemoveNebula;

        /// <summary>
        /// Returns true if the version is compatibile with the API Backend, this is automatically called
        /// </summary>
        public static bool VerifyVersion(int Version, string ModName) => _VerifyVersion?.Invoke(Version, ModName) ?? false;

        /// <summary>
        /// Returns true if the position is inside of a nebula's bounding box
        /// </summary>
        public static bool InsideNebulaBounding(Vector3D Position) => _InsideNebulaBounding?.Invoke(Position) ?? false;

        /// <summary>
        /// Returns true if the position is inside of a nebula cloud
        /// </summary>
        public static bool InsideNebula(Vector3D Position) => _InsideNebula?.Invoke(Position) ?? false;

        /// <summary>
        /// Returns the density (0-1) of the nebula at the position
        /// </summary>
        public static float GetNebulaDensity(Vector3D Position) => _GetNebulaDensity?.Invoke(Position) ?? 0;

        /// <summary>
        /// Returns the ratio of the primary and secondary color, values closer to 0 are primary and 1 are secondary
        /// </summary>
        public static float GetMaterial(Vector3D Position) => _GetMaterial?.Invoke(Position) ?? 0;

        /// <summary>
        /// Creates a lightning at the provided position
        /// </summary>
        public static void CreateLightning(Vector3D Position) => _CreateLightning?.Invoke(Position);

        /// <summary>
        /// Creates a weather at the provided position with the weather string, returns false if not possible
        /// </summary>
        public static bool CreateWeather(Vector3D Position, string Weather, bool Natural) => _CreateWeather?.Invoke(Position, Weather, Natural) ?? false;

        /// <summary>
        /// Creates a random weather, returns false if not possible
        /// </summary>
        public static bool CreateRandomWeather(Vector3D Position) => _CreateRandomWeather?.Invoke(Position) ?? false;

        /// <summary>
        /// Removes the weather at the position, returns false if no weather is found
        /// </summary>
        public static bool RemoveWeather(Vector3D Position) => _RemoveWeather?.Invoke(Position) ?? false;

        /// <summary>
        /// Gets the current weather at the provided position
        /// </summary>
        public static string GetWeather(Vector3D Position) => _GetWeather?.Invoke(Position) ?? null;

        /// <summary>
        /// Forces Radiation to render with the provided amount, set null to reset to default
        /// </summary>
        public static void ForceRenderRadiation(int? Amount) => _ForceRenderRadiation?.Invoke(Amount);

        /// <summary>
        /// Forces Ions to render, set null to reset
        /// </summary>
        public static void ForceRenderIons(bool? Enabled) => _ForceRenderIons?.Invoke(Enabled);

        /// <summary>
        /// Forces Comets to render, set null to reset
        /// </summary>
        public static void ForceRenderComets(bool? Enabled) => _ForceRenderComets?.Invoke(Enabled);

        /// <summary>
        /// Returns a random weather using the weighted randomizer
        /// </summary>
        public static string GetRandomWeather() => _GetRandomWeather?.Invoke();

        /// <summary>
        /// Simulates the player running a command
        /// </summary>
        public static void RunCommand(string Command) => _RunCommand?.Invoke(Command);

        /// <summary>
        /// Creates a nebula of radius (km) at the provided position
        /// </summary>
        public static void CreateNebula(Vector3D position, int radius) => _CreateNebula?.Invoke(position, radius);

        /// <summary>
        /// Returns true if a weather is occuring or incoming at the position
        /// </summary>
        public static bool IsNearWeather(Vector3D Position) => _IsNearWeather?.Invoke(Position) ?? false;

        /// <summary>
        /// Sets the colors of the nebula at the provided position
        /// </summary>
        public static void SetColors(Vector3D position, Vector4 primaryColor, Vector4 secondaryColor) => _SetColors?.Invoke(position, primaryColor, secondaryColor);

        /// <summary>
        /// Removes the nebula at the provided position, returns true if it was successfull
        /// </summary>
        public static bool RemoveNebula(Vector3D position) => _RemoveNebula?.Invoke(position) ?? false;

        /// <summary>
        /// Gets the primary color of the nebula at the provided position
        /// </summary>
        public static Vector4? GetPrimaryColor(Vector3D position) => _GetPrimaryColor?.Invoke(position);

        /// <summary>
        /// Gets the secondary color of the nebula at the provided position
        /// </summary>
        public static Vector4? GetSecondaryColor(Vector3D position) => _GetSecondaryColor?.Invoke(position);

        public override void LoadData()
        {
            MyAPIGateway.Utilities.RegisterMessageHandler(ModHandlerID, ModHandler);
        }

        protected override void UnloadData()
        {
            MyAPIGateway.Utilities.UnregisterMessageHandler(ModHandlerID, ModHandler);
        }

        private void ModHandler(object obj)
        {
            if (MyAPIGateway.Utilities.GamePaths?.ModScopeName != null)
                ModName = MyAPIGateway.Utilities.GamePaths.ModScopeName.Split('_')[1];

            if (obj == null)
            {
                return;
            }

            if (obj is Dictionary<string, Delegate>)
            {
                ModAPIMethods = (Dictionary<string, Delegate>)obj;
                _VerifyVersion = (Func<int, string, bool>)ModAPIMethods["VerifyVersion"];
            }

            Registered = VerifyVersion(ModAPIVersion, ModName);

            if (Registered)
            {
                try
                {
                    _InsideNebulaBounding = (Func<Vector3D, bool>)ModAPIMethods["InsideNebulaBounding"];
                    _InsideNebula = (Func<Vector3D, bool>)ModAPIMethods["InsideNebula"];
                    _GetNebulaDensity = (Func<Vector3D, float>)ModAPIMethods["GetNebulaDensity"];
                    _GetMaterial = (Func<Vector3D, float>)ModAPIMethods["GetMaterial"];
                    _CreateLightning = (Action<Vector3D>)ModAPIMethods["CreateLightning"];
                    _CreateWeather = (Func<Vector3D, string, bool, bool>)ModAPIMethods["CreateWeather"];
                    _CreateRandomWeather = (Func<Vector3D, bool>)ModAPIMethods["CreateRandomWeather"];
                    _RemoveWeather = (Func<Vector3D, bool>)ModAPIMethods["RemoveWeather"];
                    _GetWeather = (Func<Vector3D, string>)ModAPIMethods["GetWeather"];
                    _ForceRenderRadiation = (Action<int?>)ModAPIMethods["ForceRenderRadiation"];
                    _ForceRenderIons = (Action<bool?>)ModAPIMethods["ForceRenderIons"];
                    _RunCommand = (Action<string>)ModAPIMethods["RunCommand"];

                    //5
                    _ForceRenderComets = (Action<bool?>)ModAPIMethods["ForceRenderComets"];
                    _GetRandomWeather = (Func<string>)ModAPIMethods["GetRandomWeather"];
                    _IsNearWeather = (Func<Vector3D, bool>)ModAPIMethods["IsNearWeather"];

                    //6
                    _CreateNebula = (Action<Vector3D, int>)ModAPIMethods["CreateNebula"];

                    _SetColors = (Action<Vector3D, Vector4, Vector4>)ModAPIMethods["SetColors"];
                    _GetPrimaryColor = (Func<Vector3D, Vector4?>)ModAPIMethods["GetPrimaryColor"];
                    _GetSecondaryColor = (Func<Vector3D, Vector4?>)ModAPIMethods["GetSecondaryColor"];
                    _RemoveNebula = (Func<Vector3D, bool>)ModAPIMethods["RemoveNebula"];
    }
                catch (Exception e)
                {
                    MyAPIGateway.Utilities.ShowMessage("NebulaMod", "Mod '" + ModName + "' encountered an error when registering the Nebula Mod API, see log for more info.");
                    MyLog.Default.WriteLine(e);
                }
            }
        }
    }
}