using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class PetGolemHeadProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Replica Golem Head");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
            drawOriginOffsetY = -10;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            projectile.aiStyle = -1;
            projectile.width = 30;
            projectile.height = 32;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.PetGolemHead = false;
            }
            if (modPlayer.PetGolemHead)
            {
                projectile.timeLeft = 2;
            }
            AssAI.ZephyrfishAI(projectile, velocityFactor: 1f, sway: 2, random: false, swapSides: 0, offsetX: -60, offsetY: -40);
            AssAI.ZephyrfishDraw(projectile);
            projectile.rotation = 0f;

            projectile.ai[1]++;
            if (projectile.ai[1] > 60)
            {
                if (Main.myPlayer == projectile.owner)
                {
                    int targetIndex = -1;
                    float distanceFromTarget = 100000f;
                    Vector2 targetCenter = projectile.position;
                    for (int k = 0; k < 200; k++)
                    {
                        NPC npc = Main.npc[k];
                        if (npc.CanBeChasedBy(this))
                        {
                            float between = Vector2.Distance(npc.Center, projectile.Center);
                            if (((Vector2.Distance(projectile.Center, targetCenter) > between && between < distanceFromTarget) || targetIndex == -1) &&
                                Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                            {
                                distanceFromTarget = between;
                                targetCenter = npc.Center;
                                targetIndex = k;
                            }
                        }
                    }
                    if (targetIndex != -1)
                    {
                        Vector2 position = projectile.Center;
                        Vector2 velocity = targetCenter + Main.npc[targetIndex].velocity * 5f - projectile.Center;
                        velocity.Normalize();
                        velocity *= 7f;
                        //velocity.Y -= Math.Abs(velocity.Y) * 0.5f;
                        int index = Projectile.NewProjectile(position, velocity, mod.ProjectileType<PetGolemHeadFireball>(), 10, 2f, Main.myPlayer, 0f, 0f);
                        Main.projectile[index].timeLeft = 300;
                        Main.projectile[index].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
                projectile.ai[1] = 0;
            }
        }
    }
}
