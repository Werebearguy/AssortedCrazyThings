using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeToxicProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeToxic;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Toxic Slime");
            DrawOffsetX = -18;
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -16; //-20
        }

        public override void SafeSetDefaults()
        {
            Projectile.alpha = 75;
        }
    }
}
