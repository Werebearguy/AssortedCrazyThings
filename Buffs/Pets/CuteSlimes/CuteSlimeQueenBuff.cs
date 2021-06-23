using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    [Content(ContentType.CuteSlimes | ContentType.DroppedPets)]
    public class CuteSlimeQueenBuff : CuteSlimeBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeQueenProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeQueen;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Queen Slime");
            Description.SetDefault("A cute royal slime girl is following you");
        }
    }
}
