using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    [LegacyName("CuteSlimeBlackNew")]
    public class CuteSlimeBlackItem : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeBlackProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeBlackBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Black Slime");
            Tooltip.SetDefault("Summons a friendly Cute Black Slime to follow you");
        }
    }
}
