using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable
{
    [Content(ContentType.Placeables | ContentType.Bosses)]
    public class HarvesterTrophyItem : PlaceableItem<HarvesterTrophyTile>
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Harvester Trophy");
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlacableTile(TileType);
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 1);
        }
    }
}
