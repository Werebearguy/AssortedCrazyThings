using AssortedCrazyThings.Items.Weapons;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingPlayer : AssPlayerBase
	{
		public HashSet<int> firstSummon;
		public Dictionary<int, Ref<bool>> hasMinion;

		public int numUnderlings = 0;
		public bool spawned = false;

		public void SetHasMinion(int type, bool val)
		{
			if (hasMinion.TryGetValue(type, out var value))
			{
				value.Value = val;
			}
		}

		public bool GetHasMinion(int type)
		{
			if (hasMinion.TryGetValue(type, out var value))
			{
				return value.Value;
			}
			return false;
		}

		public override void Initialize()
		{
			firstSummon = new();

			hasMinion = new();
			foreach (var type in GoblinUnderlingTierSystem.GoblinUnderlingProjs.Keys)
			{
				hasMinion[type] = new Ref<bool>();
			}
		}

		public override void Load()
		{
			On_Player.Spawn += OnSpawnSummonGoblinUnderling;
		}

		private record struct InventoryIndex(bool Flag, int Index);

		private static void OnSpawnSummonGoblinUnderling(On_Player.orig_Spawn orig, Player player, PlayerSpawnContext context)
		{
			orig(player, context);

			if (player.whoAmI == Main.myPlayer && (context == PlayerSpawnContext.ReviveFromDeath || context == PlayerSpawnContext.SpawningIntoWorld))
			{
				if (!ClientConfig.Instance.GoblinUnderlingAutosummon)
				{
					return;
				}

				var items = new List<InventoryIndex>();
				foreach (var itemType in GoblinUnderlingItem.Items)
				{
					int i = player.FindItemInInventoryOrOpenVoidBag(itemType, out bool voidBag);
					if (i != -1)
					{
						items.Add(new InventoryIndex(voidBag, i));
					}
				}

				//Sort by index of inventory, with void bag being after
				items = items.OrderBy(ii => !ii.Flag ? ii.Index : ii.Index + Main.InventorySlotsTotal).ToList();
				foreach (var (flag, index) in items)
				{
					var inv = !flag ? player.inventory : player.bank4.item;
					player.AddBuff(inv[index].buffType, 3600, false);
				}
			}
		}

		public override void ResetEffects()
		{
			foreach (var value in hasMinion.Values)
			{
				value.Value = false;
			}
		}

		public override void OnEnterWorld()
		{
			spawned = false;
		}

		public override void PreUpdate()
		{
			spawned = true;
			numUnderlings = 0;
		}

		public override void LoadData(TagCompound tag)
		{
			if (tag.ContainsKey("firstSummon2"))
			{
				foreach (var fullName in tag.GetList<string>("firstSummon2"))
				{
					int type = 0;
					if (ProjectileID.Search.TryGetId(fullName, out int id))
					{
						//Vanilla or modded
						type = id;
					}
					else if (ModContent.TryFind<ModProjectile>(fullName, out var modProj))
					{
						//LegacyName modded
						type = modProj.Type;
					}

					if (type > 0)
					{
						firstSummon.Add(type);
					}
				}
			}
		}

		public override void SaveData(TagCompound tag)
		{
			var firstSummonList = new List<string>();
			foreach (var type in firstSummon)
			{
				var proj = ContentSamples.ProjectilesByType[type];
				firstSummonList.Add(proj.ModProjectile?.FullName ?? proj.Name);
			}

			if (firstSummonList.Count > 0)
			{
				tag["firstSummon2"] = firstSummonList;
			}
		}
	}
}
