using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable
{
	[Content(ContentType.Bosses)]
	public class HarvesterRelicItem : PlaceableItem<HarvesterRelicTile>
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
	}
}
