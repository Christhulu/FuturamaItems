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

            /*__instance.musicAudios.AddRangeToArray(FuturamaItemModBase.newMusic);*/
            __instance.musicAudios[__instance.musicAudios.Length - 1] = FuturamaItemModBase.themeSong;

            FuturamaItemModBase.Instance.mls.LogInfo($"Patched {__instance} with 3 new music track!");
        }


    }
}
