using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Pets;
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
				if (bossChecklist.Version >= new Version(1, 3, 1))
				{
					List<int> collection = new List<int>()
					{
						ModContent.ItemType<HarvesterRelicItem>(),
						ModContent.ItemType<PetHarvesterItem>(),
						ModContent.ItemType<HarvesterTrophyItem>(),
						ModContent.ItemType<SoulHarvesterMask>()
					};

					int summonItem = ModContent.ItemType<IdolOfDecay>();

					//TODO harvester, include 15 x ModContent.ItemType<CaughtDungeonSoulFreed>() in loot but dont spawn it

					/*
                     * "AddBoss",
                        args[1] as Mod, // Mod
						args[2] as string, // Boss Name
						InterpretObjectAsListOfInt(args[3]), // IDs
						Convert.ToSingle(args[4]), // Prog
						args[5] as Func<bool>, // Downed
						args[6] as Func<bool>, // Available
						InterpretObjectAsListOfInt(args[7]), // Collection
						InterpretObjectAsListOfInt(args[8]), // Spawn Items
						args[9] as string, // Spawn Info
						InterpretObjectAsStringFunction(args[10]), // Despawn message
						args[11] as Action<SpriteBatch, Rectangle, Color> // Custom Drawing
                     */
					bossChecklist.Call(
						"AddBoss",
						Mod,
						Harvester.name,
						AssortedCrazyThings.harvester,
						5.1f,
						(Func<bool>)(() => AssWorld.downedHarvester),
						(Func<bool>)(() => true),
						collection,
						summonItem,
						$"Enter the dungeon for the first time, or use a [i:{summonItem}] in the dungeon"
					);
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
