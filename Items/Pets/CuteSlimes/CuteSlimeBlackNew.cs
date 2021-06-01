using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeBlackNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeBlackNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeBlackNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Black Slime");
            Tooltip.SetDefault("Summons a friendly Cute Black Slime to follow you");
        }
    }
}
