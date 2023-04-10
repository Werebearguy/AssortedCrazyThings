using AssortedCrazyThings.Tiles;
using Terraria;

namespace AssortedCrazyThings.Items.Placeable
{
	//Only places the opened variant as decoration
	[Content(ContentType.Bosses)]
	public class AntiqueCageItem : PlaceableItem<AntiqueCageOpenTile>
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Antique Cage");

			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType); //to test: Terraria.ModLoader.ModContent.TileType<AntiqueCageLockedTile>()
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 99;
			Item.rare = 3;
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}
	}
}
