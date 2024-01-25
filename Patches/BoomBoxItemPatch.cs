using HarmonyLib;
using UnityEngine;
using LCSoundTool.Resources;
using BepInEx.Logging;


namespace FuturamaItems.Patches
{
    [HarmonyPatch(typeof(BoomboxItem))]
    internal class BoomBoxItemPatch
    {

        [HarmonyPatch(nameof(BoomboxItem.Start))]
        [HarmonyPostfix]
        public static void Start_Patch(BoomboxItem __instance)
        {

            const string GUID = "csalex.futuramaItems";

            AudioClip[] originalMusic = __instance.musicAudios;

            __instance.musicAudios = new AudioClip[originalMusic.Length + 3];

            for (int i = 0; i < originalMusic.Length; i++)
            {
                __instance.musicAudios[i] = originalMusic[i];
            }

            __instance.musicAudios.AddRangeToArray(originalMusic);
            FuturamaItemModBase.Instance.mls.LogInfo($"Patched {__instance} with 3 new music track!");
        }


    }
}
