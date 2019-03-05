using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class WallFragmentItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Fragment");
            Tooltip.SetDefault("Summons several fragments of the Wall to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.width = 22;
            item.height = 26;
            item.shoot = mod.ProjectileType("WallFragmentProj");
            item.buffType = mod.BuffType("WallFragmentBuff");
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
