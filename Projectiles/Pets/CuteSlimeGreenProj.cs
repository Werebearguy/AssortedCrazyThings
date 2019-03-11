using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeGreenProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Green Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -20;
            //drawOriginOffsetX = 0;
            drawOriginOffsetY = -18; //-22
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.scale = 0.9f;
            projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimeGreen = false;
            }
            if (modPlayer.CuteSlimeGreen)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
