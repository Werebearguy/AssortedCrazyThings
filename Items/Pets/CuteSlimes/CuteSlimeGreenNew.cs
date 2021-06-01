using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeGreenNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeGreenNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeGreenNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Green Slime");
            Tooltip.SetDefault("Summons a friendly Cute Green Slime to follow you");
        }
    }
}
