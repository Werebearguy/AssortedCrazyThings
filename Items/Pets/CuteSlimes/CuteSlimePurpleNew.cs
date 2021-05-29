using AssortedCrazyThings.Buffs.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimePurpleNew : CuteSlimeItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Purple Slime");
            Tooltip.SetDefault("Summons a friendly Cute Purple Slime to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LizardEgg);
            Item.shoot = ModContent.ProjectileType<CuteSlimePurpleNewProj>();
            Item.buffType = ModContent.BuffType<CuteSlimePurpleNewBuff>();
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}