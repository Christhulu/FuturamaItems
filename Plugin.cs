using BepInEx;
using BepInEx.Logging;
using FuturamaItems.Patches;
using HarmonyLib;
using LethalLib.Modules;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;

namespace FuturamaItems
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class FuturamaItemModBase : BaseUnityPlugin
    {
        private const string PLUGIN_GUID = "csalex.futuramaItems";
        private const string PLUGIN_NAME = "Futurama Items";
        private const string PLUGIN_VERSION = "2.0.0";

        public static FuturamaItemModBase Instance;
        //Create Logging Source
        internal ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(PLUGIN_GUID);

        private readonly Harmony harmony = new Harmony(PLUGIN_GUID);

        internal AssetBundle futuramaBundle;

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

            Item bender = futuramaBundle.LoadAsset<Item>("Assets/FuturamaItems/BenderFigurine.asset");

            if(bender.itemIcon == null) {

                mls.LogError("For some reason Bender doesn't have any icon attached");

                Sprite benderSprite = futuramaBundle.LoadAsset<Sprite>("Assets/FuturamaItems/benderbyjordandiazandres.png");
                bender.itemIcon = benderSprite;
            
            }


            if(bender.dropSFX == null || bender.grabSFX == null  || bender.pocketSFX == null || bender.throwSFX == null)
            {

                mls.LogError("For some reason Bender doesn't have any sound effects attached");

                AudioClip benderGrabClip = futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_heywhatsthis.mp3");
                AudioClip benderPocketClip = futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_youstupid.mp3");
                AudioClip benderDropClip = futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_heyfleshies.mp3");
                AudioClip benderThrowClip = futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_doaflip.mp3");

                bender.dropSFX = benderDropClip;
                bender.grabSFX = benderGrabClip;
                bender.pocketSFX = benderPocketClip;
                bender.throwSFX = benderThrowClip;
            }



            if (bender.spawnPrefab.GetComponent<PhysicsProp>() == null)
            {

                mls.LogError("There's no physics prop on this for some reason");

                bender.spawnPrefab.AddComponent<PhysicsProp>();

                var benderPhysicsProp = bender.spawnPrefab.GetComponent<PhysicsProp>();

                benderPhysicsProp.grabbableToEnemies = true;
                benderPhysicsProp.grabbable = true;
                benderPhysicsProp.itemProperties = bender;
            }

            if(bender.spawnPrefab.GetComponent<GrabbableObject>() == null)
            {
                mls.LogError("There's nothing grabbable on this for some reason");
            }




            if (bender.spawnPrefab.GetComponent<AudioSource>() == null)
            {

                mls.LogError("We don't have an audio source on this?");

                bender.spawnPrefab.AddComponent<AudioSource>();

                var benderAudioSource = bender.spawnPrefab.GetComponent<AudioSource>();

                if(benderAudioSource.outputAudioMixerGroup == null) {
                    mls.LogError("We don't have an audio mixer group on this either?");

                    AudioMixerGroup benderMixerGroup = futuramaBundle.LoadAsset<AudioMixerGroup>("Assets/FuturamaItems/DiageticMixer.mixer");

                    benderAudioSource.outputAudioMixerGroup = benderMixerGroup;
                    benderAudioSource.playOnAwake = false;
                    benderAudioSource.volume = 1.0f;
                    benderAudioSource.minDistance = 1.0f;
                    benderAudioSource.maxDistance = 25.0f;
                }

            }

            if (bender.spawnPrefab.GetComponentInChildren<ScanNodeProperties>() == null)
            {

                mls.LogError("For some reason there is no scan node properties script on this");

                bender.spawnPrefab.AddComponent<ScanNodeProperties>();

                ScanNodeProperties benderScanNodeScript = bender.spawnPrefab.GetComponent<ScanNodeProperties>();

                benderScanNodeScript.maxRange = 13;
                benderScanNodeScript.minRange = 1;
                benderScanNodeScript.scrapValue = 50;
                benderScanNodeScript.headerText = "Bender Figurine";
                benderScanNodeScript.subText = "Value: ";
                benderScanNodeScript.creatureScanID = -1;
                benderScanNodeScript.nodeType = 2;
                benderScanNodeScript.requiresLineOfSight = true;
            }

            NetworkPrefabs.RegisterNetworkPrefab(bender.spawnPrefab);

            //Ensure audio doesn't play twice
            Utilities.FixMixerGroups(bender.spawnPrefab);

            //Register item as scrap and as shop item
            Items.RegisterScrap(bender, 1000, Levels.LevelTypes.All);

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
            mls.LogInfo("Patched Futurama Items Mod");

        }
    }
}
