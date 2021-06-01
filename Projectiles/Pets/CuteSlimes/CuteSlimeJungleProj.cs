using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeJungleProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeJungle;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Jungle Slime");
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
