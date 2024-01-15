using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;


namespace FuturamaItems
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class FuturamaItemModBase : BaseUnityPlugin
    {
        private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        private static FuturamaItemModBase Instance;

        internal ManualLogSource mls;

        internal static AssetBundle benderAssetBundle;

        
        void Awake()
        {
            //Create static reference to mod instance
            if(Instance == null)
            {
                Instance = this; 
            }

            //Create Logging Source
            mls = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_GUID);

            // Plugin startup logging
            mls.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            //Load bender asset bundle
            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "benderbundled");
            benderAssetBundle = AssetBundle.LoadFromFile(assetDir);

            if (benderAssetBundle == null)
            {
                mls.LogError("Failed to load custom assets."); // ManualLogSource for your plugin
                return;
            }

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.PLUGIN_GUID);
            mls.LogInfo("Patched Futurama Items Mod");


        }
    }
}
