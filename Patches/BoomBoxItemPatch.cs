using HarmonyLib;
using System.Linq;
using System.Threading.Tasks;
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

            #region Boombox Audio Patch Asset Loading
            //Load and append songs to boombox audio

            AudioClip themeSong = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/futuramatheme_chopped_and_chewed.mp3");
            AudioClip robotHellSong = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/robothell.mp3");
            AudioClip heWantsABrainSong = FuturamaItemModBase.Instance.futuramaBundle.LoadAsset<AudioClip>("Assets/FuturamaItems/hewantsabrain.mp3");

            #endregion Boombox Audio Patch Asset Loading

            AudioClip[] originalMusic = __instance.musicAudios;
            AudioClip[] newMusic = { themeSong, robotHellSong, heWantsABrainSong};

            __instance.musicAudios = originalMusic.Concat(newMusic).ToArray();

            FuturamaItemModBase.Instance.mls.LogInfo($"Patched {__instance} with {newMusic.Length} new music tracks!");
        }


    }
}
