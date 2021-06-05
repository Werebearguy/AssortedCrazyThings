using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeYellowProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeYellow;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Yellow Slime");
        }

        public override void SafeSetDefaults()
        {
            Projectile.alpha = 75;
        }
    }
}
