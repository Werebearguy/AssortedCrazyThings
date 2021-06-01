using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimePinkNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimePinkNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimePinkNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Pink Slime");
            Tooltip.SetDefault("Summons a friendly Cute Pink Slime to follow you");
        }
    }
}
