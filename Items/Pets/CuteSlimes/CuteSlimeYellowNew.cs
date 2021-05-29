using AssortedCrazyThings.Buffs.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
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
            Item.CloneDefaults(ItemID.LizardEgg);
            Item.shoot = ModContent.ProjectileType<CuteSlimeYellowNewProj>();
            Item.buffType = ModContent.BuffType<CuteSlimeYellowNewBuff>();
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
