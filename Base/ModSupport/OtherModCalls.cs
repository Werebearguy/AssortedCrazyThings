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
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.ModSupport
{
	public struct BoomerangInfo
	{
		public int[] projectileTypes;
		public int numBoomerangs;
		public Func<Player, Item, int, bool> canUseItemFunc;

		/// <summary>
		/// numBoomerangs -1 for infinite
		/// </summary>
		public BoomerangInfo(int[] projectileTypes, int numBoomerangs, Func<Player, Item, int, bool> canUseItemFunc = null)
		{
			this.projectileTypes = projectileTypes;
			this.numBoomerangs = numBoomerangs;
			this.canUseItemFunc = canUseItemFunc;
		}

		/// <inheritdoc cref="BoomerangInfo(int[], int, Func{Player, Item, int, bool})"/>
		public BoomerangInfo(int projectileType, int numBoomerangs, Func<Player, Item, int, bool> canUseItemFunc = null) :
			this(new int[] { projectileType }, numBoomerangs, canUseItemFunc)
		{

		}
	}

	[Content(ConfigurationSystem.AllFlags)]
	public class OtherModCalls : AssSystem
	{
		private static Dictionary<int, BoomerangInfo> boomerangInfos;

		public static void RegisterBoomerang(int type, BoomerangInfo info)
		{
			if (boomerangInfos == null)
			{
				//Mod not loaded: don't do anything
				return;
			}

			boomerangInfos[type] = info;
		}

		public override void Load()
		{
			if (ModLoader.HasMod("Bangarang"))
			{
				boomerangInfos = new Dictionary<int, BoomerangInfo>();
			}
		}

		public override void Unload()
		{
			boomerangInfos = null;
		}

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

			if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage))
			{
				if (ContentConfig.Instance.Bosses)
				{
					RegisterShadowDiamondDrop(magicStorage, AssortedCrazyThings.harvester);
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

			if (ModLoader.TryGetMod("Bangarang", out Mod bangarang))
			{
				foreach (var pair in boomerangInfos)
				{
					var info = pair.Value;
					bangarang.Call(pair.Key, info.projectileTypes, info.numBoomerangs, info.canUseItemFunc);
				}
			}
		}

		private static IItemDropRule GetShadowDiamondDropRule(Mod mod, int normal = 1, int expert = -1)
		{
			return (IItemDropRule)mod.Call("Get Shadow Diamond Drop Rule", normal, expert);
		}

		private static void SetShadowDiamondDropRule(Mod mod, int npcType, IItemDropRule rule)
		{
			mod.Call("Set Shadow Diamond Drop Rule", npcType, rule);
		}

		private static void RegisterShadowDiamondDrop(Mod mod, int npcType, int normal = 1, int expert = -1)
		{
			SetShadowDiamondDropRule(mod, npcType, GetShadowDiamondDropRule(mod, normal, expert));
		}
	}
}
