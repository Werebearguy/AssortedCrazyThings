using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeYellowProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeYellow;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Yellow Slime");
            DrawOffsetX = -18;
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -16; //-14
        }

        public override void SafeSetDefaults()
        {
            //projectile.scale = 1.2f;
            Projectile.alpha = 75;
        }
    }
}
