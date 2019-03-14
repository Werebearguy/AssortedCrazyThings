using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class TurtleSlimeProj : BabySlimeBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Turtle Slime");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -10;
            drawOriginOffsetY = -4;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 32;
            projectile.height = 30;
            projectile.alpha = 0;

            projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            if (Main.player[projectile.owner].dead)
            {
                modPlayer.TurtleSlime = false;
            }
            if (modPlayer.TurtleSlime)
            {
                projectile.timeLeft = 2;
            }
            return true;
        }
    }
}
