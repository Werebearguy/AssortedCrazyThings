using AssortedCrazyThings.Base;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class MeatballSlimeProj : BabySlimeBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MeatballSlimeProj");
            Main.projFrames[Projectile.type] = 6;
            Main.projPet[Projectile.type] = true;
            DrawOffsetX = -6;
            DrawOriginOffsetY = -4;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
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
