using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalLib.Modules;
using System.IO;
using System.Reflection;
using UnityEngine;


namespace FuturamaItems
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class FuturamaItemModBase : BaseUnityPlugin
    {
        const string GUID = "csalex.futuramaItems";
        const string NAME = "Futurama Items";
        const string VERSION = "1.0.0";

        public static FuturamaItemModBase Instance;

        [HarmonyPostfix]
        void Awake()
        {
            //Create static reference to mod instance
            if(Instance == null)
            {
                Instance = this;
            }
            

            //Create Logging Source
            ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(GUID);

            // Plugin startup logging
            mls.LogInfo($"Plugin {GUID} is loaded!");

            //Load bender asset bundle
            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "benderbundle");
            AssetBundle benderAssetBundle = AssetBundle.LoadFromFile(assetDir);

            if (benderAssetBundle == null)
            {
                mls.LogError("Failed to load custom assets."); // ManualLogSource for your plugin
                return;
            }

            Item bender = benderAssetBundle.LoadAsset<Item>("Assets/FuturamaItems/BenderFigurine.asset");

            //Ensure audio doesn't play twice
            Utilities.FixMixerGroups(bender.spawnPrefab);

            //This makes it so the other clients know what this object is
            NetworkPrefabs.RegisterNetworkPrefab(bender.spawnPrefab);

            //Register item as scrap and as shop item
            Items.RegisterScrap(bender, 1000, Levels.LevelTypes.All);

            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.clearPreviousText = true;
            node.displayText = "Bite my shiny metal ass!\n\n";

            Items.RegisterShopItem(bender, null, null, node, 0);

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
            mls.LogInfo("Patched Futurama Items Mod");


        }
    }
}
