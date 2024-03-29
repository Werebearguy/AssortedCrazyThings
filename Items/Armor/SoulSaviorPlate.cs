using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
	[Content(ContentType.Bosses)]
	[AutoloadEquip(EquipType.Body)]
	public class SoulSaviorPlate : AssItem
	{
		public static readonly int DamageIncrease = 10;
		public static readonly int MaxMinionsIncrease = 2;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DamageIncrease, MaxMinionsIncrease);

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 22;
			Item.value = Item.sellPrice(gold: 3, silver: 70);
			Item.rare = 3;
			Item.defense = 18;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions += MaxMinionsIncrease;
			player.GetDamage(DamageClass.Summon) += DamageIncrease / 100f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddIngredient(ItemID.Ectoplasm, 4).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 24).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
