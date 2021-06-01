using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeGreenProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeGreen;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Green Slime");
            DrawOffsetX = -18;
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -18; //-22
        }

        public override void SafeSetDefaults()
        {
            Projectile.scale = 0.9f;
            Projectile.alpha = 75;
        }
    }
}
