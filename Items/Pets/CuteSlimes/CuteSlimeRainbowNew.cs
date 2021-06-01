using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeRainbowNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeRainbowNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeRainbowNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Rainbow Slime");
            Tooltip.SetDefault("Summons a friendly Cute Rainbow Slime to follow you");
        }
    }
}
