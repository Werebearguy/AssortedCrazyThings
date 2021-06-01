using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeDungeonNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeDungeonNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeDungeonNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Dungeon Slime");
            Tooltip.SetDefault("Summons a friendly Cute Dungeon Slime to follow you");
        }
    }
}
