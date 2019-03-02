using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class StingSlimeBlackProj : BabySlimeBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sting Slime");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            drawOffsetX = 0;
            drawOriginOffsetY = 4;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions and damage (if there is, defaults to 0)
            projectile.width = 52;
            projectile.height = 38;

            Damage = 0;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            if (Main.player[projectile.owner].dead)
            {
                modPlayer.StingSlimeBlackPet = false;
            }
            if (modPlayer.StingSlimeBlackPet)
            {
                projectile.timeLeft = 2;
            }
            return true;
        }
    }
}