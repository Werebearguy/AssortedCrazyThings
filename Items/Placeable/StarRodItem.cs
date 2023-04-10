using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable
{
	public class StarRodItem : PlaceableItem<StarRodTile>
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Star Rod");
			// Tooltip.SetDefault("Attracts Falling Stars at night if you are nearby");

			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType);
			Item.width = 18;
			Item.height = 32;
			Item.maxStack = 99;
			Item.rare = 2;
			Item.value =
				Item.sellPrice(0, 0, 12, 0) * 10 +
				Item.sellPrice(0, 0, 30, 0) * 5 +
				Item.sellPrice(0, 0, 5, 0) * 25 - 10; //Using the cheaper bar variants, minus rounding due to shop happiness variance
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(AssRecipes.GoldPlatinumGroup, 10)
				.AddRecipeGroup(AssRecipes.DemoniteCrimtaneGroup, 5)
				.AddIngredient(ItemID.FallenStar, 25)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
