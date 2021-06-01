using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeLavaNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeLavaNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeLavaNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Lava Slime");
            Tooltip.SetDefault("Summons a friendly Cute Lava Slime to follow you");
        }
    }
}
