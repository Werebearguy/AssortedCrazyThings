using AssortedCrazyThings.Tiles;
using Terraria;

namespace AssortedCrazyThings.Items.Placeable
{
	[Content(ContentType.Bosses)]
	public class AntiqueChestItem : PlaceableItem<AntiqueChestTile>
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType);
			Item.width = 32;
			Item.height = 28;
			Item.maxStack = 99;
			Item.rare = 3;
			Item.value = Item.sellPrice(0, 0, 10, 0);
		}
	}
}
