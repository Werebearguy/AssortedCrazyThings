using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class GobletPet : ModProjectile
    {
        private int frame2Counter = 0;
        private int frame2 = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblet");
            Main.projFrames[projectile.type] = 12;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyGrinch);
            aiType = ProjectileID.BabyGrinch;
            projectile.width = 24; //40 for flying
            projectile.height = 38;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.grinch = false; // Relic from aiType
            return true;
        }

        private void GetFrame()
        {
            if (projectile.ai[0] == 0)
            {
                if (projectile.velocity.Y == 0f)
                {
                    if (projectile.velocity.X == 0f)
                    {
                       frame2 = 0;
                       frame2Counter = 0;
                    }
                    else if (projectile.velocity.X < -0.8f || projectile.velocity.X > 0.8f)
                    {
                       frame2Counter += (int)Math.Abs(projectile.velocity.X);
                       frame2Counter++;
                        if (projectile.frameCounter > 6)
                        {
                           frame2++;
                           frame2Counter = 0;
                        }
                        if (projectile.frame > 6) //frame 1 to 6 is running
                        {
                           frame2 = 1;
                        }
                    }
                    else
                    {
                       frame2 = 0; //frame 0 is idle
                       frame2Counter = 0;
                    }
                }
                else if (projectile.velocity.Y != 0f)
                {
                   frame2Counter = 0;
                   frame2 = 7; //frame 7 is jumping
                }
                //projectile.velocity.Y += 0.4f;
                //if (projectile.velocity.Y > 10f)
                //{
                //    projectile.velocity.Y = 10f;
                //}
            }
            else //flying
            {
               frame2Counter++;
                if (projectile.frameCounter > 6)
                {
                   frame2++;
                   frame2Counter = 0;
                }
                if (projectile.frame < 8 ||frame2 > 11)
                {
                   frame2 = 10;
                }
                projectile.rotation = projectile.velocity.X * 0.05f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            GetFrame();

            double cX = projectile.position.X + projectile.width * 2 + drawOffsetX;
            double cY = projectile.position.Y + projectile.height * 2;
            lightColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), lightColor);
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.direction != -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D image = Main.projectileTexture[projectile.type];
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = (int)(image.Bounds.Height / Main.projFrames[projectile.type]);
            bounds.Y = frame2 * bounds.Height;
            Vector2 stupidOffset = new Vector2(12f, 6f);
            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.GobletPet = false;
            }
            if (modPlayer.GobletPet)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
