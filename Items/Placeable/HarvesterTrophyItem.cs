using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable
{
	[Content(ContentType.Bosses)]
	public class HarvesterTrophyItem : PlaceableItem<HarvesterTrophyTile>
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Harvester Trophy");

			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType);
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 99;
			Item.rare = 1;
			Item.value = Item.buyPrice(0, 1);
		}
	}
}
