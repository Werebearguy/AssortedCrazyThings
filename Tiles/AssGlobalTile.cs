using AssortedCrazyThings.Items.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Tiles
{
    class AssGlobalTile : GlobalTile
    {
        public override bool Drop(int i, int j, int type)
        {
            if (type == 186 && Main.tile[i, j].frameX >= 828 && Main.tile[i, j].frameX <= 844 && Main.tile[i, j].frameY >= 18) //fake sword shrine
            {
                if (Main.rand.NextBool(10))
                {
                    Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType<LegendaryWoodenSword>());
                }
            }
            return true;
        }
    }
}
