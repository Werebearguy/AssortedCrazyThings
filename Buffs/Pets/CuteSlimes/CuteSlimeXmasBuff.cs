using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeXmasBuff : CuteSlimeBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeXmasProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeXmas;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Christmas Slime");
            Description.SetDefault("A cute christmas slime girl is following you");
        }
    }
}
