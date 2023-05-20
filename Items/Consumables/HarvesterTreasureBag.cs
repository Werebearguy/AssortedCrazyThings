using AssortedCrazyThings.Items.VanityArmor;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Consumables
{
	[Content(ContentType.Bosses)]
	public class HarvesterTreasureBag : AssItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.BossBag[Type] = true; //This set is one that every boss bag should have, it, for example, lets our boss bag drop dev armor..
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; //..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.

			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 40, 60));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DesiccatedLeather>(), 2));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulHarvesterMask>(), 7));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(AssortedCrazyThings.harvester));
		}
	}
}
