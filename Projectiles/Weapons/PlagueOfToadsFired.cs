using System;
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
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType<PlagueOfToadsCloud>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
        }

        public override void AI()
        {
            if (projectile.type == mod.ProjectileType<PlagueOfToadsFired>()) //moving cloud
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
            else if (projectile.type == 238) //stationary cloud
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
                    if (projectile.ai[0] > 8f)
                    {
                        projectile.ai[0] = 0f;
                        if (projectile.owner == Main.myPlayer)
                        {
                            int rainSpawnX = (int)(projectile.position.X + 14f + Main.rand.Next(projectile.width - 28));
                            int rainSpawnY = (int)(projectile.position.Y + projectile.height + 4f);
                            Projectile.NewProjectile(rainSpawnX, rainSpawnY, 0f, 5f, 239, projectile.damage, 0f, projectile.owner, 0f, 0f);
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
                    int typeSelf = projectile.type;
                    for (int num425 = 0; num425 < 1000; num425++)
                    {
                        if (Main.projectile[num425].active && Main.projectile[num425].owner == projectile.owner && Main.projectile[num425].type == typeSelf && Main.projectile[num425].ai[1] < 3600f)
                        {
                            cloudCount++;
                            if (Main.projectile[num425].ai[1] > cloudAi1)
                            {
                                cloudIndex = num425;
                                cloudAi1 = Main.projectile[num425].ai[1];
                            }
                        }
                    }
                    if (cloudCount > 2)
                    {
                        Main.projectile[cloudIndex].netUpdate = true;
                        Main.projectile[cloudIndex].ai[1] = 36000f;
                    }
                }
            }
            else
            {
                if (projectile.type == 239) //fired rain
                {
                    projectile.alpha = 50;
                }
            }
        }
    }
}
