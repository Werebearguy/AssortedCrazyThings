using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    [Content(ContentType.CuteSlimes | ContentType.DroppedPets)]
    public class CuteSlimeQueenItem : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeQueenProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeQueenBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Queen Slime");
            Tooltip.SetDefault("Summons a friendly Cute Queen Slime to follow you");
        }
    }
}
