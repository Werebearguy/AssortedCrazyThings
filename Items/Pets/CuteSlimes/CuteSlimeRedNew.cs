using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeRedNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeRedNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeRedNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Red Slime");
            Tooltip.SetDefault("Summons a friendly Cute Red Slime to follow you");
        }
    }
}
