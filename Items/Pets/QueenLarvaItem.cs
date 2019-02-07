using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class QueenLarvaItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Queen Larva");
            Tooltip.SetDefault("Summons a Queen Bee Larva to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("QueenLarva");
            item.buffType = mod.BuffType("QueenLarvaBuff");
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
