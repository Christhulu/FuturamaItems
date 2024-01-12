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

            RoundManager rm = UnityEngine.Object.FindObjectOfType<RoundManager>(); ;


            /*IEnumerable<BoomboxItem> boombox = UnityEngine.G*/
            AudioClip futuramaTheme = new AudioClip();

            /*boombox.musicAudios.AddItem(futuramaTheme);*/



            //Adding audio
            //Make asset bundle from audio file using Asset Browser
            //Load it in like video
            //Add AudioClip
            //Technically the boombox is a store bought item, and as a result we can check the start of the round or maybe in all player items to add the audio



            //Spawning items is determined in the round manager with the SpawnScrapInLevel method. It gets spawnableScrab from the SelectableLevel component and goes into that level's
            //List of spawnable scrap items until it hits a count
            //So we'd have to add the scrap to each level and give it a rarity, a value, 
            //Additionally, each item should extend GrabbableObject
            //We can use the Item class and update the spawnPrefab with our models
            //Additionally, there are audio clips for grabbing, dropping, pocketing, and throwing on the item that we can update with cool lines
        }

    }
}
