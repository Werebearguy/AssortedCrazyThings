using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            projectile.width = 38;
            projectile.height = 38;
            projectile.tileCollide = false;
        }

        private const int FireballDamage = 20;

        private float sinY; //depends on projectile.ai[0], no need to sync

        private float Sincounter
        {
            get
            {
                return projectile.localAI[0];
            }
            set
            {
                projectile.localAI[0] = value;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D image = Main.projectileTexture[projectile.type];

            if (projectile.ai[1] > 60)
            {
                if (projectile.ai[1] < 90)
                {
                    projectile.frame = 1;
                }
                else
                {
                    projectile.frame = 0;
                }
            }
            else
            {
                projectile.frame = 0;
            }


                Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[projectile.type];
            bounds.Y = projectile.frame * bounds.Height;

            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Sincounter = Sincounter > 360 ? 0 : Sincounter + 1;
            sinY = (float)((Math.Sin((Sincounter / 180f) * 2 * Math.PI) - 1) * 4);

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2 + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;

            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin, 1f, effects, 0f);

            return false;
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
            //AssAI.ZephyrfishAI(projectile, velocityFactor: 1f, sway: 2, random: false, swapSides: 0, offsetX: -60, offsetY: -40);
            AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, staticDirection: true, vanityPet: true, veloSpeed: 0.5f, offsetX: -30f, offsetY: -100f);

            //AssAI.ZephyrfishDraw(projectile);
            projectile.rotation = 0f;

            projectile.ai[1]++;
            if ((int)projectile.ai[1] % 60 == 0)
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
                    if (targetIndex != -1 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                    {
                        if (projectile.ai[1] == 60) projectile.ai[1] += 60;
                        Vector2 position = projectile.Center;
                        position.Y += 6f;
                        Vector2 velocity = targetCenter + Main.npc[targetIndex].velocity * 5f - projectile.Center;
                        velocity.Normalize();
                        velocity *= 7f;
                        //velocity.Y -= Math.Abs(velocity.Y) * 0.5f;
                        int index = Projectile.NewProjectile(position, velocity, mod.ProjectileType<PetGolemHeadFireball>(), FireballDamage, 2f, Main.myPlayer, 0f, 0f);
                        Main.projectile[index].timeLeft = 300;
                        Main.projectile[index].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                    else
                    {
                        if (projectile.ai[1] > 60) projectile.ai[1] -= 60;
                    }
                }
                projectile.ai[1] -= 60;
            }
        }
    }
}
