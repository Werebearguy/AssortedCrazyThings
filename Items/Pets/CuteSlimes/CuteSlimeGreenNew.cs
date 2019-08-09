using AssortedCrazyThings.Buffs.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeGreenNew : CuteSlimeItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Green Slime");
            Tooltip.SetDefault("Summons a friendly Cute Green Slime to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LizardEgg);
            item.shoot = mod.ProjectileType<CuteSlimeGreenNewProj>();
            item.buffType = mod.BuffType<CuteSlimeGreenNewBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }
    }
}