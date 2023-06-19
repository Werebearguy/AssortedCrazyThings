using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
	[Content(ContentType.Bosses)]
	public class HarvesterRelicColoredItem : PlaceableItem<HarvesterRelicColoredTile>
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType);
			Item.width = 30;
			Item.height = 40;
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = ItemRarityID.Master;
			Item.master = true;
			Item.value = Item.buyPrice(0, 5);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<HarvesterRelicItem>())
				.Register();

			Recipe.Create(ModContent.ItemType<HarvesterRelicItem>())
				.AddIngredient(Type)
				.Register();
		}
	}
}
