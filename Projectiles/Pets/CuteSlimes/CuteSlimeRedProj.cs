using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeRedProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeRed;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Red Slime");
            DrawOffsetX = -18;
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -16; //-21
        }

        public override void SafeSetDefaults()
        {
            //projectile.scale = 1.025f;
            Projectile.alpha = 75;
        }
    }
}
