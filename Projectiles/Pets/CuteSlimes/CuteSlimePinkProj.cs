using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimePinkProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimePink;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Pink Slime");
            DrawOffsetX = -18; //-18
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -21;
        }

        public override void SafeSetDefaults()
        {
            Projectile.scale = 0.5f;
            Projectile.alpha = 75;
        }
    }
}
