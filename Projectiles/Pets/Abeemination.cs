using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class Abeemination : BabySlimeBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abeemination");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            drawOffsetX = 0;
            drawOriginOffsetY = 4;
        }

        /*					
            if (projPet[projectile[i].type] && !projectile[i].minion && projectile[i].owner != 255 && projectile[i].damage == 0 && !ProjectileID.Sets.LightPet[projectile[i].type])
            {
	            num3 = player[projectile[i].owner].cPet;
            }
         */

        public override void MoreSetDefaults()
        {
            //used to set dimensions and damage (if there is, defaults to 0)
            projectile.width = 38;
            projectile.height = 40;

            Damage = 0;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            if (Main.player[projectile.owner].dead)
            {
                modPlayer.Abeemination = false;
            }
            if (modPlayer.Abeemination)
            {
                projectile.timeLeft = 2;
            }
            return true;
        }
    }
}
