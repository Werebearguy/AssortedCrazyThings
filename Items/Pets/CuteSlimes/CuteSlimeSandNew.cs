using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeSandNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeSandNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeSandNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Sand Slime");
            Tooltip.SetDefault("Summons a friendly Cute Sand Slime to follow you");
        }
    }
}
