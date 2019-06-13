using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public abstract class MinionItemBase : ModItem
    {
        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
