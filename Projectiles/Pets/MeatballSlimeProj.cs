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
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -6;
            drawOriginOffsetY = -4;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 34;
            projectile.height = 30;
            projectile.alpha = 0;

            projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>();
            if (projectile.GetOwner().dead)
            {
                modPlayer.MeatballSlime = false;
            }
            if (modPlayer.MeatballSlime)
            {
                projectile.timeLeft = 2;
            }
            return true;
        }
    }
}
