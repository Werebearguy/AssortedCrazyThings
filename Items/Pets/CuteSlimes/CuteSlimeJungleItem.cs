using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    [LegacyName("CuteSlimeJungleNew")]
    public class CuteSlimeJungleItem : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeJungleProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeJungleBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Jungle Slime");
            Tooltip.SetDefault("Summons a friendly Cute Jungle Slime to follow you");
        }
    }
}
