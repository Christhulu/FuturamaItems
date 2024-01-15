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

        public static AssetBundle benderModelAsset;

        
        void Awake()
        {
            //Create static reference to mod instance
            if(Instance == null)
            {
                Instance = this; 
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_GUID);
            // Plugin startup logic
            mls.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            //Load bender model
            string sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            benderModelAsset = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "bender"));
            if (benderModelAsset == null)
            {
                mls.LogError("Failed to load custom assets."); // ManualLogSource for your plugin
                return;
            }

            harmony.PatchAll(typeof(FuturamaItemModBase));


        }
    }
}
