using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeSandProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeSand;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Sand Slime");
            DrawOffsetX = -20;
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -16; //-21
        }

        public override void SafeSetDefaults()
        {
            //projectile.scale = 1.025f;
            Projectile.alpha = 45;
        }
    }
}
