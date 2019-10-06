using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    public class PlagueOfToadsFired : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "Terraria/Projectile_" + ProjectileID.RainCloudMoving;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plague of Toads Fired");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.active && Main.myPlayer == projectile.owner)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<PlagueOfToadsCloud>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
        }

        public override void AI()
        {
            float destinationX = projectile.ai[0];
            float destinationY = projectile.ai[1];
            if (destinationX != 0f && destinationY != 0f)
            {
                bool reachedX = false;
                bool reachedY = false;
                if ((projectile.velocity.X < 0f && projectile.Center.X < destinationX) || (projectile.velocity.X > 0f && projectile.Center.X > destinationX))
                {
                    reachedX = true;
                }
                if ((projectile.velocity.Y < 0f && projectile.Center.Y < destinationY) || (projectile.velocity.Y > 0f && projectile.Center.Y > destinationY))
                {
                    reachedY = true;
                }
                if (reachedX & reachedY)
                {
                    projectile.Kill();
                }
            }
            projectile.rotation += projectile.velocity.X * 0.02f;
            projectile.frameCounter++;
            if (projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                    return;
                }
            }
        }
    }
}
