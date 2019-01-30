using Terraria;
using AssortedCrazyThings.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Tiles
{
    class AssGlobalTile : GlobalTile
    {
        public override bool Drop(int i, int j, int type)
        {
            if(type == 186) //fake sword shrine
            {
                Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType<CaughtDungeonSoul>());
            }
            return true;
        }
    }
}
