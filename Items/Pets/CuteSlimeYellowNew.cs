using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Pets
{
    public class CuteSlimeYellowNew : CuteSlimeItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Yellow Slime");
            Tooltip.SetDefault("Summons a friendly Cute Yellow Slime to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LizardEgg);
            item.shoot = mod.ProjectileType<CuteSlimeYellowNewProj>();
            item.buffType = mod.BuffType<CuteSlimeYellowNewBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }
    }
}
