using LethalLib.Modules;
using UnityEngine;
using UnityEngine.Audio;

namespace FuturamaItems.Modules
{
    public class BenderUtilities
    {

        public static BenderUtilities Instance;

        public BenderUtilities() {

            Instance ??= this;

            Init();
        }

        private static void Init()
        {
            Instance.LoadBenderAsset(ref FuturamaItemModBase.Instance.bender, ref FuturamaItemModBase.Instance.futuramaBundle);
        }

        private void LoadBenderAsset(ref Item bender, ref AssetBundle futuramaBundle)
        {
            bender = futuramaBundle.LoadAsset<Item>("Assets/FuturamaItems/BenderFigurine.asset");
        }

        private void UpdateBenderIcon(ref Item bender, ref AssetBundle futuramaBundle)
        {
            if (bender.itemIcon == null)
            {

                FuturamaItemModBase.Instance.mls.LogError("For some reason Bender doesn't have any icon attached");

                Sprite benderSprite = futuramaBundle.LoadAsset<Sprite>("Assets/FuturamaItems/benderbyjordandiazandres.png");
                bender.itemIcon = benderSprite;

            }
        }

        private void UpdateBenderSFX(ref Item bender, ref AssetBundle futuramaBundle)
        {

            if (bender.dropSFX == null || bender.grabSFX == null || bender.pocketSFX == null || bender.throwSFX == null)
            {

                FuturamaItemModBase.Instance.mls.LogError("For some reason Bender doesn't have any sound effects attached");

                AudioClip benderGrabClip = futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_heywhatsthis.mp3");
                AudioClip benderPocketClip = futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_youstupid.mp3");
                AudioClip benderDropClip = futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_heyfleshies.mp3");
                AudioClip benderThrowClip = futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/bender_doaflip.mp3");

                bender.dropSFX = benderDropClip;
                bender.grabSFX = benderGrabClip;
                bender.pocketSFX = benderPocketClip;
                bender.throwSFX = benderThrowClip;
            }

        }

        private void UpdateBenderComponents(ref Item bender, ref AssetBundle futuramaBundle)
        {
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
        }




        private void RegisterItem(ref Item bender)
        {

            NetworkPrefabs.RegisterNetworkPrefab(bender.spawnPrefab);

            //Ensure audio doesn't play twice
            Utilities.FixMixerGroups(bender.spawnPrefab);

            //Register item as scrap and as shop item
            Items.RegisterScrap(bender, 300, Levels.LevelTypes.All);

            FuturamaItemModBase.Instance.mls.LogInfo($"Patched {FuturamaItemModBase.Instance} with additional item: Bender Figurine");

        }

        public void SetupBenderItem()
        {
            LoadBenderAsset(ref FuturamaItemModBase.Instance.bender, ref FuturamaItemModBase.Instance.futuramaBundle);
            UpdateBenderIcon(ref FuturamaItemModBase.Instance.bender, ref FuturamaItemModBase.Instance.futuramaBundle);
            UpdateBenderSFX(ref FuturamaItemModBase.Instance.bender, ref FuturamaItemModBase.Instance.futuramaBundle);
            UpdateBenderComponents(ref FuturamaItemModBase.Instance.bender, ref FuturamaItemModBase.Instance.futuramaBundle);
            RegisterItem(ref FuturamaItemModBase.Instance.bender);

        }


    }
}
