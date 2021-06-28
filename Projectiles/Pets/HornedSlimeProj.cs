using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    [Content(ContentType.HostileNPCs)]
    //check this file for more info vvvvvvvv
    public class HornedSlimeProj : BabySlimeBase
    {
        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Horned Slime");
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 30;

            Projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            if (Projectile.GetOwner().dead)
            {
                modPlayer.HornedSlime = false;
            }
            if (modPlayer.HornedSlime)
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }
    }
}
