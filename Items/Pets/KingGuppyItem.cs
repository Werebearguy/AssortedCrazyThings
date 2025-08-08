using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class KingGuppyItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<KingGuppyProj>();

		public override int BuffType => ModContent.BuffType<KingGuppyBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;

			Item.value = Item.sellPrice(gold: 2, silver: 50);
		}

		public static bool loaded = false;
		public override void Load()
		{
			if (!loaded)
			{
				//Detour instead of using the ModPlayer hook because it has questsDone
				On_Player.GetAnglerReward_MainReward += On_Player_GetAnglerReward_MainReward;
				loaded = true;
			}
		}

		private static void On_Player_GetAnglerReward_MainReward(On_Player.orig_GetAnglerReward_MainReward orig, Player self, List<Item> rewardItems, IEntitySource source, int questsDone, float rarityReduction, int questItemType, ref GetItemSettings anglerRewardSettings)
		{
			orig(self, rewardItems, source, questsDone, rarityReduction, questItemType, ref anglerRewardSettings);

			if (questsDone == 3)
			{
				rewardItems.Add(new Item(ModContent.ItemType<KingGuppyItem>()));
			}
			else if (questsDone > 3)
			{
				if (Main.rand.NextBool(33))
				{
					rewardItems.Add(new Item(ModContent.ItemType<KingGuppyItem>()));
				}
			}
		}
	}

	public class KingGuppyItem_AoMM : SimplePetItemBase_AoMM<KingGuppyItem>
	{
		public override int BuffType => ModContent.BuffType<KingGuppyBuff_AoMM>();

		public static LocalizedText AttackPatternText { get; private set; }

		public override void EvenSaferSetStaticDefaults()
		{
			AttackPatternText = this.GetLocalization("AttackPattern");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int ttIndex = tooltips.FindLastIndex(t => t.Mod == "Terraria" && t.Name.StartsWith("Tooltip"));
			if (ttIndex < 0)
			{
				ttIndex = tooltips.Count - 1;
			}
			tooltips.Insert(ttIndex + 1, new TooltipLine(Mod, nameof(AttackPatternText), AttackPatternText.ToString()));
		}
	}
}
