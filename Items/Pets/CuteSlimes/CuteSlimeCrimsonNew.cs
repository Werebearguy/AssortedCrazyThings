using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeCrimsonNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeCrimsonNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeCrimsonNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Crimson Slime");
            Tooltip.SetDefault("Summons a friendly Cute Crimson Slime to follow you");
        }
    }
}
