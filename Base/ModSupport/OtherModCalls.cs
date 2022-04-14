using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Accessories.Useful;
using AssortedCrazyThings.Items.Placeable;
using AssortedCrazyThings.Items.VanityArmor;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.ModSupport
{
    [Content(ConfigurationSystem.AllFlags, needsAllToFilter: true)]
    public class OtherModCalls : AssSystem
    {
        public override void PostSetupContent()
        {
            //https://forums.terraria.org/index.php?threads/boss-checklist-in-game-progression-checklist.50668/
            if (ContentConfig.Instance.Bosses && ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
            {
                //5.1f means just after skeletron
                if (bossChecklist.Version >= new Version(1, 0))
                {
                    /*
                     * "AddMiniBoss",
                     * float progression,
                     * int/List<int> miniBossNPCIDs,
                     * Mod mod,
                     * string minibossName,
                     * Func<bool> downedMiniBoss,
                     * int/List<int> SpawnItemIDs,
                     * int/List<int> CollectionItemIDs,
                     * int/List<int> LootItemIDs,
                     * [string spawnInfo],
                     * [string despawnMessage],
                     * [string texture],
                     * [string overrideHeadIconTexture],
                     * [Func<bool> miniBossAvailable]
                     */

                    int summonItem = ModContent.ItemType<IdolOfDecay>();

                    List<int> collection = new List<int>();
                    collection.AddRange(new List<int>
                    {
                        ModContent.ItemType<HarvesterRelicItem>(),
                        ModContent.ItemType<HarvesterTrophyItem>()
                    });

                    collection.AddRange(new List<int>
                    {
                        ModContent.ItemType<SoulHarvesterMask>()
                    });

                    List<int> loot = new List<int>
                    {
                        summonItem,
                        ModContent.ItemType<CaughtDungeonSoulFreed>(),
                    };

                    loot.AddRange(new List<int>
                    {
                        ModContent.ItemType<SigilOfRetreat>(),
                        ModContent.ItemType<SigilOfEmergency>(),
                        ModContent.ItemType<SigilOfPainSuppression>()
                    });

                    bossChecklist.Call(
                        "AddMiniBoss",
                        5.1f,
                        AssortedCrazyThings.harvester,
                        this,
                        Harvester.name,
                        (Func<bool>)(() => AssWorld.downedHarvester),
                        summonItem,
                        collection,
                        loot,
                        $"Use a [i:{summonItem}] in the dungeon after Skeletron has been defeated",
                        null,
                        $"{this.Name}/NPCs/DungeonBird/HarvesterPreview"
                    );
                }
                else
                {
                    bossChecklist.Call("AddMiniBossWithInfo", Harvester.name, 5.1f, (Func<bool>)(() => AssWorld.downedHarvester), "Use a [i:" + ModContent.ItemType<IdolOfDecay>() + "] in the dungeon after Skeletron has been defeated");
                }
            }

            if (ModLoader.TryGetMod("SummonersAssociation", out Mod summonersAssociation) && summonersAssociation.Version > new Version(0, 4, 1))
            {
                if (ContentConfig.Instance.Bosses)
                {
                    int soulBuff = ModContent.BuffType<CompanionDungeonSoulMinionBuff>();
                    summonersAssociation.Call("AddMinionInfo", ModContent.ItemType<EverglowLantern>(), soulBuff, ModContent.ProjectileType<CompanionDungeonSoulPreWOFMinion>());

                    summonersAssociation.Call("AddMinionInfo", ModContent.ItemType<EverhallowedLantern>(), soulBuff, new List<int>
                    {
                        ModContent.ProjectileType<CompanionDungeonSoulPostWOFMinion>(),
                        ModContent.ProjectileType<CompanionDungeonSoulFrightMinion>(),
                        ModContent.ProjectileType<CompanionDungeonSoulMightMinion>(),
                        ModContent.ProjectileType<CompanionDungeonSoulSightMinion>()
                    });
                }

                if (ContentConfig.Instance.Weapons)
                {
                    List<int> slimes = new List<int>()
                    {
                        ModContent.ProjectileType<SlimePackMinion>(),
                        ModContent.ProjectileType<SlimePackSpikedMinion>(),
                        ModContent.ProjectileType<SlimePackAssortedMinion>(),
                    };
        
                    summonersAssociation.Call("AddMinionInfo", ModContent.ItemType<SlimeHandlerKnapsack>(), ModContent.BuffType<SlimePackMinionBuff>(), slimes);

                    List<int> drones = new List<int>();
                    foreach (var drone in DroneController.DataList)
                    {
                        drones.Add(drone.ProjType);
                    }

                    summonersAssociation.Call("AddMinionInfo", ModContent.ItemType<DroneController>(), ModContent.BuffType<DroneControllerBuff>(), drones);
                }
            }
        }
    }
}
