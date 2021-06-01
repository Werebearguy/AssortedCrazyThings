using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeIceNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeIceNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeIceNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Ice Slime");
            Tooltip.SetDefault("Summons a friendly Cute Ice Slime to follow you");
        }
    }
}
