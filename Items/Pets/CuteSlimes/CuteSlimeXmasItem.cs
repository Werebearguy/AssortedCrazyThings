using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    [LegacyName("CuteSlimeXmasNew")]
    public class CuteSlimeXmasItem : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeXmasProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeXmasBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Christmas Slime");
            Tooltip.SetDefault("Summons a friendly Cute Christmas Slime to follow you");
        }
    }
}
