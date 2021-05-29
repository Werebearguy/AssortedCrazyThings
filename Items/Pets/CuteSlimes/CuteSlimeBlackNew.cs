using AssortedCrazyThings.Buffs.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeBlackNew : CuteSlimeItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Black Slime");
            Tooltip.SetDefault("Summons a friendly Cute Black Slime to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LizardEgg);
            Item.shoot = ModContent.ProjectileType<CuteSlimeBlackNewProj>();
            Item.buffType = ModContent.BuffType<CuteSlimeBlackNewBuff>();
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
