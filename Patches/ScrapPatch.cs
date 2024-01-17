using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Item bender = FuturamaItemModBase.benderAssetBundle.LoadAsset<Item>("/Assets/benderbundled");

            //Ensure audio doesn't play twice
            Utilities.FixMixerGroups(bender.spawnPrefab);
            
            //This makes it so the other clients know what this object is
            NetworkPrefabs.RegisterNetworkPrefab(bender.spawnPrefab);

            //Update bender item stats
            bender.isConductiveMetal = true;
            bender.maxValue = 200;
            bender.minValue = 40;

            //I'm using these to take sounds from temporarily
            Turret turret = new Turret();
            Shovel shovel = new Shovel();
            WalkieTalkie w = new WalkieTalkie();


            //Update sound effects
            bender.grabSFX = turret.chargingSFX;

            var dropIndex = UnityEngine.Random.Range(0, shovel.hitSFX.Length);
            bender.dropSFX = shovel.hitSFX[dropIndex];

            var pocketIndex = UnityEngine.Random.Range(0, w.startTransmissionSFX.Length);
            bender.pocketSFX = w.startTransmissionSFX[pocketIndex];

            //Register item as scrap and as shop item
            Items.RegisterScrap(bender, 300, Levels.LevelTypes.All);

            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.clearPreviousText = true;
            node.displayText = "Bite my shiny metal ass!\n\n";

            Items.RegisterShopItem(bender, null, null, node, 0);

            



        }
        

    }
}
