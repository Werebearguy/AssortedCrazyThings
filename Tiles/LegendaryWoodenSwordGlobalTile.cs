using AssortedCrazyThings.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Tiles
{
    [Content(ContentType.Weapons)]
    class LegendaryWoodenSwordGlobalTile : AssGlobalTile
    {
        public override bool Drop(int i, int j, int type)
        {
            if (type == TileID.LargePiles && Main.tile[i, j] is Tile tile && tile.frameX >= 828 && tile.frameX <= 844 && tile.frameY >= 18) //fake sword shrine
            {
                if (Main.rand.NextBool(10))
                {
                    Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<LegendaryWoodenSword>(), pfix: -1);
                }
            }
            return true;
        }
    }
}
