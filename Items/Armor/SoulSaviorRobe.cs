using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
	[Content(ContentType.Bosses)]
	[AutoloadEquip(EquipType.Legs)]
	public class SoulSaviorRobe : AssItem
	{
		public static readonly int DamageIncrease = 10;
		public static readonly int MaxMinionsIncrease = 1;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DamageIncrease, MaxMinionsIncrease);

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 14;
			Item.value = Item.sellPrice(gold: 2, silver: 60);
			Item.rare = 3;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions += MaxMinionsIncrease;
			player.GetDamage(DamageClass.Summon) += DamageIncrease / 100f;
		}

		public override void EquipFrameEffects(Player player, EquipType type)
		{
			player.shoe = 0;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddIngredient(ItemID.Ectoplasm, 3).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 12).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
