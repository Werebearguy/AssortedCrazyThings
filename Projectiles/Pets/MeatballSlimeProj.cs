using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    [Content(ContentType.HostileNPCs)]
    //check this file for more info vvvvvvvv
    public class MeatballSlimeProj : BabySlimeBase
    {
        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Meatball Slime");
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 30;
            Projectile.alpha = 0;

            Projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            if (Projectile.GetOwner().dead)
            {
                modPlayer.MeatballSlime = false;
            }
            if (modPlayer.MeatballSlime)
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }
    }
}
