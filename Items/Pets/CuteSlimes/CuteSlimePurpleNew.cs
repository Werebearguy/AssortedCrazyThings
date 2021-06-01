using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimePurpleNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimePurpleNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimePurpleNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Purple Slime");
            Tooltip.SetDefault("Summons a friendly Cute Purple Slime to follow you");
        }
    }
}
