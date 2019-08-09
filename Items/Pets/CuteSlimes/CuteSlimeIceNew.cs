using AssortedCrazyThings.Buffs.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeIceNew : CuteSlimeItem
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
    }
}
