using AssortedCrazyThings.Buffs.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeCorruptNew : CuteSlimeItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Corrupt Slime");
            Tooltip.SetDefault("Summons a friendly Cute Corrupt Slime to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LizardEgg);
            Item.shoot = ModContent.ProjectileType<CuteSlimeCorruptNewProj>();
            Item.buffType = ModContent.BuffType<CuteSlimeCorruptNewBuff>();
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}