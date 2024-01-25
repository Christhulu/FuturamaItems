using HarmonyLib;
using UnityEngine;

namespace FuturamaItems.Patches
{
    [HarmonyPatch(typeof(BoomboxItem))]
    internal class BoomBoxItemPatch
    {

        [HarmonyPatch(nameof(BoomboxItem.Start))]
        [HarmonyPostfix]
        public static void Start_Patch(BoomboxItem __instance)
        {

            AudioClip[] originalMusic = __instance.musicAudios;

            __instance.musicAudios = new AudioClip[originalMusic.Length + 3];

            for (int i = 0; i < originalMusic.Length; i++)
            {
                __instance.musicAudios[i] = originalMusic[i];
            }

            
            //Load and append songs to boombox audio
            AudioClip themeSong = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/futuramatheme_chopped_and_chewed.mp3");
            AudioClip robotHell = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/robothell.mp3");
            AudioClip heWantsABrain = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/hewantsabrain.mp3");

            AudioClip[] newMusic = { themeSong, robotHell, heWantsABrain};

            __instance.musicAudios.AddRangeToArray(newMusic);
            FuturamaItemModBase.Instance.mls.LogInfo($"Patched {__instance} with 3 new music track!");
        }


    }
}
