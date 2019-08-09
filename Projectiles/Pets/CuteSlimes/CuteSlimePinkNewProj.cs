using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimePinkNewProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Pink Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -18; //-18
            //drawOriginOffsetX = 0;
            drawOriginOffsetY = -21;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.scale = 0.5f;
            projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimePinkNew = false;
            }
            if (modPlayer.CuteSlimePinkNew)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
