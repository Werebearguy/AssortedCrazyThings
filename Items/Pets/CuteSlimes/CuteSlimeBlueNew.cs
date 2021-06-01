using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeBlueNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeBlueNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeBlueNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Blue Slime");
            Tooltip.SetDefault("Summons a friendly Cute Blue Slime to follow you");
        }
    }
}
