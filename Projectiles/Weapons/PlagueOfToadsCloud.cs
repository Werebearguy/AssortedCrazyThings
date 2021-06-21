using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    [Autoload]
    public class PlagueOfToadsCloud : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plague of Toads Cloud");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.width = 54;
            Projectile.height = 28;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 5)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] >= 7200f)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
                //8f
                if (Projectile.ai[0] > 21f)
                {
                    Projectile.ai[0] = 0f;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        int rainSpawnX = (int)(Projectile.position.X + 14f + Main.rand.Next(Projectile.width - 28));
                        int rainSpawnY = (int)(Projectile.position.Y + Projectile.height + 4f);
                        //speedY = 5f;
                        Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), rainSpawnX, rainSpawnY, 0f, 2f, ModContent.ProjectileType<PlagueOfToadsProj>(), Projectile.damage, 0f, Projectile.owner, Main.rand.Next(5), Main.rand.NextFloat(0.005f, 0.015f));
                    }
                }
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] >= 10f)
            {
                Projectile.localAI[0] = 0f;
                int cloudCount = 0;
                int cloudIndex = -1;
                float cloudAi1 = 0f;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    //check for both crimson rod and plague of toads cloud
                    Projectile other = Main.projectile[i];
                    if (other.active && other.owner == Projectile.owner && (other.type == Projectile.type || other.type == ProjectileID.BloodCloudRaining) && other.ai[1] < 3600f)
                    {
                        cloudCount++;
                        if (other.ai[1] > cloudAi1)
                        {
                            cloudIndex = i;
                            cloudAi1 = other.ai[1];
                        }
                    }
                }
                if (cloudCount > -1)
                {
                    Projectile projectile = Main.projectile[cloudIndex];
                    projectile.netUpdate = true;
                    projectile.ai[1] = 36000f;
                }
            }
        }
    }
}
