using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeBlackBuff : CuteSlimeBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeBlackProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeBlack;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Black Slime");
            Description.SetDefault("A cute black slime is following you");
        }
    }
}
