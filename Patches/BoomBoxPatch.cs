using System;
using System.Collections.Generic;
using System.Text;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace FuturamaItems.Patches
{
    [HarmonyPatch(typeof(BoomboxItem))]
    internal class BoomBoxPatch
    {

        [HarmonyPostfix]
        static void songPatch()
        {


            //If currently held item is boombox, add the futurama theme song to the end?
            //Maybe add the theme to the constructor
            /* IEnumerable<BoomboxItem> boombox = RoundManager.;*/

            RoundManager rm = UnityEngine.Object.FindObjectOfType<RoundManager>();


            /*IEnumerable<BoomboxItem> boombox = UnityEngine.G*/
            AudioClip futuramaTheme = new AudioClip();

            /*boombox.musicAudios.AddItem(futuramaTheme);*/



            //Adding audio
            //Make asset bundle from audio file using Asset Browser
            //Load it in like video
            //Add AudioClip
            //Technically the boombox is a store bought item, and as a result we can check the start of the round or maybe in all player items to add the audio



        }

    }
}
