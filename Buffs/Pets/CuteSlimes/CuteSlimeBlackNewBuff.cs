using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeBlackNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeBlackNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeBlackNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Black Slime");
            Description.SetDefault("A cute black slime girl is following you");
        }
    }
}
