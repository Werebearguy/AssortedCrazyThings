using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeJungleNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeJungleNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeJungleNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Jungle Slime");
            Tooltip.SetDefault("Summons a friendly Cute Jungle Slime to follow you");
        }
    }
}
