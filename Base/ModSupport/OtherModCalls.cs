using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Items.Placeable;
using AssortedCrazyThings.Items.VanityArmor;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.ModSupport
{
	[Content(ConfigurationSystem.AllFlags)]
	public class OtherModCalls : AssSystem
	{
		public override void PostSetupContent()
		{
			//https://github.com/JavidPack/BossChecklist/wiki/%5B1.4.4%5D-Boss-Log-Entry-Mod-Call
			//https://forums.terraria.org/index.php?threads/boss-checklist-in-game-progression-checklist.50668/
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
			{
				if (ContentConfig.Instance.Bosses)
				{
					//5.1f means just after skeletron
					List<int> collection = new List<int>()
					{
						ModContent.ItemType<HarvesterRelicItem>(),
						ModContent.ItemType<PetHarvesterItem>(),
						ModContent.ItemType<HarvesterTrophyItem>(),
						ModContent.ItemType<SoulHarvesterMask>()
					};

					int summonItem = ModContent.ItemType<IdolOfDecay>();

					/*
					"LogBoss",
					submittedMod, // Mod
					internalName, // Internal Name
					Convert.ToSingle(args[3]), // Prog
					args[4] as Func<bool>, // Downed
					InterpretObjectAsListOfInt(args[5]), // NPC IDs
					args[6] as Dictionary<string, object>
					*/
					bossChecklist.Call(
						"LogBoss",
						Mod,
						"SoulHarvester",
						5.1f,
						(Func<bool>)(() => AssWorld.downedHarvester),
						AssortedCrazyThings.harvester,
						new Dictionary<string, object>()
						{
							["spawnItems"] = summonItem,
							["collectibles"] = collection
							// Other optional arguments as needed...
						}
					);
				}

				if (ContentConfig.Instance.Weapons)
				{
					List<int> goblinInvasion = new List<int>()
					{
						ModContent.ItemType<EagerUnderlingItem>()
					};

					bossChecklist.Call(
						"SubmitEntryLoot",
						Mod,
						new Dictionary<string, object>()
						{
							["Terraria GoblinArmy"] = goblinInvasion,
						}
					);
				}
			}

			if (ModLoader.TryGetMod("SummonersAssociation", out Mod summonersAssociation))
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
