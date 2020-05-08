using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    public class PlagueOfToadsCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plague of Toads Cloud");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.width = 54;
            projectile.height = 28;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 8)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 5)
                {
                    projectile.frame = 0;
                }
            }
            projectile.ai[1] += 1f;
            if (projectile.ai[1] >= 7200f)
            {
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                }
            }
            else
            {
                projectile.ai[0] += 1f;
                //8f
                if (projectile.ai[0] > 21f)
                {
                    projectile.ai[0] = 0f;
                    if (projectile.owner == Main.myPlayer)
                    {
                        int rainSpawnX = (int)(projectile.position.X + 14f + Main.rand.Next(projectile.width - 28));
                        int rainSpawnY = (int)(projectile.position.Y + projectile.height + 4f);
                        //speedY = 5f;
                        Projectile.NewProjectile(rainSpawnX, rainSpawnY, 0f, 2f, ModContent.ProjectileType<PlagueOfToadsProj>(), projectile.damage, 0f, projectile.owner, Main.rand.Next(5), Main.rand.NextFloat(0.005f, 0.015f));
                    }
                }
            }
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] >= 10f)
            {
                projectile.localAI[0] = 0f;
                int cloudCount = 0;
                int cloudIndex = 0;
                float cloudAi1 = 0f;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    //check for both crimson rod and plague of toads cloud
                    if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && (Main.projectile[i].type == projectile.type || Main.projectile[i].type == ProjectileID.BloodCloudRaining) && Main.projectile[i].ai[1] < 3600f)
                    {
                        cloudCount++;
                        if (Main.projectile[i].ai[1] > cloudAi1)
                        {
                            cloudIndex = i;
                            cloudAi1 = Main.projectile[i].ai[1];
                        }
                    }
                }
                if (cloudCount > 1)
                {
                    Main.projectile[cloudIndex].netUpdate = true;
                    Main.projectile[cloudIndex].ai[1] = 36000f;
                }
            }
        }
    }
}
