using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeXmasNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeXmasNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeXmasNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Christmas Slime");
            Tooltip.SetDefault("Summons a friendly Cute Christmas Slime to follow you");
        }
    }
}
