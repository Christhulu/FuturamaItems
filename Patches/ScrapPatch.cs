using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using GameNetcodeStuff;
using HarmonyLib;
using LethalLib;
using LethalLib.Modules;
using UnityEngine;

namespace FuturamaItems.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class ScrapPatch
    {

        //Spawning items is determined in the round manager with the SpawnScrapInLevel method. It gets spawnableScrab from the SelectableLevel component and goes into that level's
        //List of spawnable scrap items until it hits a count
        //So we'd have to add the scrap to each level and give it a rarity, a value, 
        //Additionally, each item should extend GrabbableObject
        //We can use the Item class and update the spawnPrefab with our models
        //Additionally, there are audio clips for grabbing, dropping, pocketing, and throwing on the item that we can update with cool lines
        [HarmonyPostfix]
        static void scrapPatch()
        {
            RoundManager rm = UnityEngine.Object.FindObjectOfType<RoundManager>();

            SelectableLevel currentLevel = rm.currentLevel;

            /*Items.RegisterScrap();*/



        
        }
        

    }
}
