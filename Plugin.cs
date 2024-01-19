using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalLib.Modules;
using System.IO;
using System.Reflection;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Audio;

namespace FuturamaItems
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class FuturamaItemModBase : BaseUnityPlugin
    {
        const string GUID = "csalex.futuramaItems";
        const string NAME = "Futurama Items";
        const string VERSION = "1.0.0";

        public static FuturamaItemModBase Instance;

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

            if(bender.itemIcon == null) {

                mls.LogError("For some reason Bender doesn't have any icon attached");

                Sprite benderSprite = benderAssetBundle.LoadAsset<Sprite>("Assets/FuturamaItems/benderbyjordandiazandres.png");
                bender.itemIcon = benderSprite;
            
            }


            if(bender.dropSFX == null || bender.grabSFX == null  || bender.pocketSFX == null || bender.throwSFX == null)
            {

                mls.LogError("For some reason Bender doesn't have any sound effects attached");

                AudioClip benderGrabClip = benderAssetBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_heywhatsthis.mp3");
                AudioClip benderPocketClip = benderAssetBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_youstupid.mp3");
                AudioClip benderDropClip = benderAssetBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_heyfleshies.mp3");
                AudioClip benderThrowClip = benderAssetBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_doaflip.mp3");

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

                    AudioMixerGroup benderMixerGroup = benderAssetBundle.LoadAsset<AudioMixerGroup>("Assets/FuturamaItems/DiageticMixer.mixer");

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


            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
            mls.LogInfo("Patched Futurama Items Mod");


        }
    }
}
