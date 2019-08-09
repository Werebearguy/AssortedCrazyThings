using AssortedCrazyThings.Buffs.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeRedNew : CuteSlimeItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Red Slime");
            Tooltip.SetDefault("Summons a friendly Cute Red Slime to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LizardEgg);
            item.shoot = mod.ProjectileType<CuteSlimeRedNewProj>();
            item.buffType = mod.BuffType<CuteSlimeRedNewBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }
    }
}
