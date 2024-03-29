using AssortedCrazyThings.Tiles;
using Terraria;

namespace AssortedCrazyThings.Items.Placeable
{
	//Only places the opened variant as decoration
	[Content(ContentType.Bosses)]
	public class AntiqueCageItem : PlaceableItem<AntiqueCageOpenTile>
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType); //to test: Terraria.ModLoader.ModContent.TileType<AntiqueCageLockedTile>()
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = 3;
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}
	}
}
