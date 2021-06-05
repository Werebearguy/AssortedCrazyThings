using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeSandProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeSand;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Sand Slime");
        }

        public override void SafeSetDefaults()
        {
            Projectile.alpha = 45;
        }
    }
}
