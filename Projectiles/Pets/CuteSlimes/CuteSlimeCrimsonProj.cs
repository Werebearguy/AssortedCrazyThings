using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeCrimsonProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeCrimson;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Crimson Slime");
        }

        public override void SafeSetDefaults()
        {
            Projectile.scale = 1.2f;
            Projectile.alpha = 75;
            DrawOriginOffsetY = -14;
        }
    }
}
