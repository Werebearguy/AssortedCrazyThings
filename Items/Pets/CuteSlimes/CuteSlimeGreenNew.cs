using AssortedCrazyThings.Buffs.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            Item.CloneDefaults(ItemID.LizardEgg);
            Item.shoot = ModContent.ProjectileType<CuteSlimeGreenNewProj>();
            Item.buffType = ModContent.BuffType<CuteSlimeGreenNewBuff>();
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}