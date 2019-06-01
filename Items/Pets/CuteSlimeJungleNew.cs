using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Pets
{
    public class CuteSlimeJungleNew : CuteSlimeItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Jungle Slime");
            Tooltip.SetDefault("Summons a friendly Cute Jungle Slime to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LizardEgg);
            item.shoot = mod.ProjectileType<CuteSlimeJungleNewProj>();
            item.buffType = mod.BuffType<CuteSlimeJungleNewBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }
    }
}
