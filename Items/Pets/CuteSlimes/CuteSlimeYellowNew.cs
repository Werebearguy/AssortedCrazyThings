using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeYellowNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeYellowNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeYellowNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Yellow Slime");
            Tooltip.SetDefault("Summons a friendly Cute Yellow Slime to follow you");
        }
    }
}
