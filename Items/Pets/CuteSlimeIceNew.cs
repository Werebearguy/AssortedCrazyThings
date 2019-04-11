using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class CuteSlimeIceNew : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Ice Slime");
            Tooltip.SetDefault("Summons a friendly Cute Ice Slime to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LizardEgg);
            item.shoot = mod.ProjectileType<CuteSlimeIceNewProj>();
            item.buffType = mod.BuffType<CuteSlimeIceNewBuff>();
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
