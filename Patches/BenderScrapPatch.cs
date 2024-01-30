﻿using HarmonyLib;
using LethalLib.Modules;
using UnityEngine;
using UnityEngine.Audio;

namespace FuturamaItems.Patches
{

    //Maybe this should be roundmanager, maybe it should be selectableLevel. Not sure
    [HarmonyPatch(typeof(SelectableLevel))]

    internal class BenderScrapPatch
    {
        [HarmonyPatch(nameof(RoundManager.SpawnScrapInLevel))]
        [HarmonyPostfix]
        public static void Start_Patch()
        {
            Item bender = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<Item>("Assets/FuturamaItems/BenderFigurine.asset");

            if (bender.itemIcon == null)
            {

                FuturamaItemModBase.Instance.mls.LogError("For some reason Bender doesn't have any icon attached");

                Sprite benderSprite = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<Sprite>("Assets/FuturamaItems/benderbyjordandiazandres.png");
                bender.itemIcon = benderSprite;

            }


            if (bender.dropSFX == null || bender.grabSFX == null || bender.pocketSFX == null || bender.throwSFX == null)
            {

                FuturamaItemModBase.Instance.mls.LogError("For some reason Bender doesn't have any sound effects attached");

                AudioClip benderGrabClip = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_heywhatsthis.mp3");
                AudioClip benderPocketClip = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_youstupid.mp3");
                AudioClip benderDropClip = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_heyfleshies.mp3");
                AudioClip benderThrowClip = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_doaflip.mp3");

                bender.dropSFX = benderDropClip;
                bender.grabSFX = benderGrabClip;
                bender.pocketSFX = benderPocketClip;
                bender.throwSFX = benderThrowClip;
            }



            if (bender.spawnPrefab.GetComponent<PhysicsProp>() == null)
            {

                FuturamaItemModBase.Instance.mls.LogError("There's no physics prop on this for some reason");

                bender.spawnPrefab.AddComponent<PhysicsProp>();

                var benderPhysicsProp = bender.spawnPrefab.GetComponent<PhysicsProp>();

                benderPhysicsProp.grabbableToEnemies = true;
                benderPhysicsProp.grabbable = true;
                benderPhysicsProp.itemProperties = bender;
            }

            if (bender.spawnPrefab.GetComponent<GrabbableObject>() == null)
            {
                FuturamaItemModBase.Instance.mls.LogError("There's nothing grabbable on this for some reason");
            }




            if (bender.spawnPrefab.GetComponent<AudioSource>() == null)
            {

                FuturamaItemModBase.Instance.mls.LogError("We don't have an audio source on this?");

                bender.spawnPrefab.AddComponent<AudioSource>();

                var benderAudioSource = bender.spawnPrefab.GetComponent<AudioSource>();

                if (benderAudioSource.outputAudioMixerGroup == null)
                {
                    FuturamaItemModBase.Instance.mls.LogError("We don't have an audio mixer group on this either?");

                    AudioMixerGroup benderMixerGroup = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioMixerGroup>("Assets/FuturamaItems/DiageticMixer.mixer");

                    benderAudioSource.outputAudioMixerGroup = benderMixerGroup;
                    benderAudioSource.playOnAwake = false;
                    benderAudioSource.volume = 1.0f;
                    benderAudioSource.minDistance = 1.0f;
                    benderAudioSource.maxDistance = 25.0f;
                }

            }

            if (bender.spawnPrefab.GetComponentInChildren<ScanNodeProperties>() == null)
            {

                FuturamaItemModBase.Instance.mls.LogError("For some reason there is no scan node properties script on this");

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
            Items.RegisterScrap(bender, 300, Levels.LevelTypes.All);

        }
    }
}
