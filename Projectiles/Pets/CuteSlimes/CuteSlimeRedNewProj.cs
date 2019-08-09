using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeRedNewProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Red Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -18;
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
                modPlayer.CuteSlimeRedNew = false;
            }
            if (modPlayer.CuteSlimeRedNew)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
