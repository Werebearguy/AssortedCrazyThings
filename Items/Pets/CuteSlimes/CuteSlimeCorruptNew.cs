using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeCorruptNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeCorruptNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeCorruptNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Corrupt Slime");
            Tooltip.SetDefault("Summons a friendly Cute Corrupt Slime to follow you");
        }
    }
}
