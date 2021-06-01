using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeToxicNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeToxicNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeToxicNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Toxic Slime");
            Tooltip.SetDefault("Summons a friendly Cute Toxic Slime to follow you");
        }
    }
}
