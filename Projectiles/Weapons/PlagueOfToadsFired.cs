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
                return "Terraria/Images/Projectile_" + ProjectileID.RainCloudMoving;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plague of Toads Fired");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.active && Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<PlagueOfToadsCloud>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

        public override void AI()
        {
            float destinationX = Projectile.ai[0];
            float destinationY = Projectile.ai[1];
            if (destinationX != 0f && destinationY != 0f)
            {
                bool reachedX = false;
                bool reachedY = false;
                if ((Projectile.velocity.X < 0f && Projectile.Center.X < destinationX) || (Projectile.velocity.X > 0f && Projectile.Center.X > destinationX))
                {
                    reachedX = true;
                }
                if ((Projectile.velocity.Y < 0f && Projectile.Center.Y < destinationY) || (Projectile.velocity.Y > 0f && Projectile.Center.Y > destinationY))
                {
                    reachedY = true;
                }
                if (reachedX & reachedY)
                {
                    Projectile.Kill();
                }
            }
            Projectile.rotation += Projectile.velocity.X * 0.02f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                    return;
                }
            }
        }
    }
}
