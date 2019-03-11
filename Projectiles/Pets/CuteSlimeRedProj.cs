using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeRedProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Red Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -20;
            //drawOriginOffsetX = 0;
            drawOriginOffsetY = -16; //-21
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            //projectile.scale = 1.025f;
            projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimeRed = false;
            }
            if (modPlayer.CuteSlimeRed)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
