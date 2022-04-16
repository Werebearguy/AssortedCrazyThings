using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.VanityArmor
{
	[Content(ContentType.VanityArmor)]
	[AutoloadEquip(EquipType.Head)]
	public class PrettyMask : AssItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pretty Mask");
			Tooltip.SetDefault("'Goes well with a red scarf, red gloves, red boots, and your birthday suit'");

			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.BallaHat);
			Item.width = 26;
			Item.height = 26;
			Item.rare = 1;
			Item.value = Item.sellPrice(0, 0, 2 * 50, 0);
			Item.vanity = true;
			Item.maxStack = 1;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Silk, 50).AddIngredient(ItemID.Feather, 1).AddTile(TileID.Loom).Register();
		}
	}
}
