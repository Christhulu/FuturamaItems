﻿using BepInEx;
using BepInEx.Logging;
using FuturamaItems.Patches;
using HarmonyLib;
using LethalLib.Modules;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Audio;
using FuturamaItems.Modules;

namespace FuturamaItems
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class FuturamaItemModBase : BaseUnityPlugin
    {
        private const string PLUGIN_GUID = "csalex.futuramaItems";
        private const string PLUGIN_NAME = "Futurama Items";
        private const string PLUGIN_VERSION = "2.1.0";

        public static FuturamaItemModBase Instance;
        //Create Logging Source
        internal ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(PLUGIN_GUID);

        internal AssetBundle futuramaBundle;
        internal Item bender;
        internal BenderUtilities benderUtilities;

        void Awake()
        {
            //Create static reference to mod instance
            if(Instance == null)
            {
                Instance = this;
            }

            // Plugin startup logging
            mls.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");

            //Load bender asset bundle
            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "benderbundle");
            futuramaBundle = AssetBundle.LoadFromFile(assetDir);

            if (futuramaBundle == null)
            {
                mls.LogError("Failed to load custom assets."); // ManualLogSource for your plugin
                return;
            }

            //Set up scrap utility classes
            benderUtilities = new BenderUtilities();
            benderUtilities.SetupBenderItem();


            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
            mls.LogInfo("Patched Futurama Items Mod");

        }

    }
}
