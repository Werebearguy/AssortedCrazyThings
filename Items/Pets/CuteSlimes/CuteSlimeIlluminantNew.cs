using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeIlluminantNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeIlluminantNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeIlluminantNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Illuminant Slime");
            Tooltip.SetDefault("Summons a friendly Cute Illuminant Slime to follow you");
        }
    }
}
