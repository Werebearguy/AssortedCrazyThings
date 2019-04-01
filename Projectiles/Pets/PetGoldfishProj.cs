using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class PetGoldfishProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/PetGoldfishProj_0"; //temp
            }
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pet Goldfish");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyGrinch);
            projectile.height = 24;
            projectile.width = 24;
            //projectile.aiStyle = -1;
            aiType = ProjectileID.BabyGrinch;
        }

        private bool Swimming
        {
            get
            {
                return projectile.wet && Main.player[projectile.owner].wet;
            }
        }

        private void ZephyrfishAI()
        {
            Player player = Main.player[projectile.owner];
            if (!player.active)
            {
                projectile.active = false;
                return;
            }

            float num17 = 0.3f;
            projectile.tileCollide = true; //false
            int num18 = 100;
            Vector2 between = player.Center - projectile.Center;

            Vector2 desiredCenter = new Vector2(Main.rand.Next(-10, 21), Main.rand.Next(-10, 21));
            desiredCenter += new Vector2(60f * -player.direction, -60f);

            //Vector2 tilePos = (desiredCenter + player.Center) / 16;
            //int x = (int)tilePos.X;
            //int y = (int)tilePos.Y;














            between += desiredCenter;

            float distance = between.Length();
            //float magnitude = 6f;
            if (distance < (float)num18 && player.velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= player.position.Y + (float)player.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                if (projectile.velocity.Y < -6f)
                {
                    projectile.velocity.Y = -6f;
                }
            }
            if (distance < 50f)
            {
                if (Math.Abs(projectile.velocity.X) > 2f || Math.Abs(projectile.velocity.Y) > 2f)
                {
                    projectile.velocity *= 0.99f;
                }
                num17 = 0.01f;
            }
            else
            {
                if (distance < 100f)
                {
                    num17 = 0.1f;
                }
                if (distance > 300f)
                {
                    num17 = 0.4f;
                }
                between.Normalize();
                between *= 6f;
            }
            if (projectile.velocity.X < between.X)
            {
                projectile.velocity.X = projectile.velocity.X + num17;
                if (num17 > 0.05f && projectile.velocity.X < 0f)
                {
                    projectile.velocity.X = projectile.velocity.X + num17;
                }
            }
            if (projectile.velocity.X > between.X)
            {
                projectile.velocity.X = projectile.velocity.X - num17;
                if (num17 > 0.05f && projectile.velocity.X > 0f)
                {
                    projectile.velocity.X = projectile.velocity.X - num17;
                }
            }
            if (projectile.velocity.Y < between.Y)
            {
                projectile.velocity.Y = projectile.velocity.Y + num17;
                if (num17 > 0.05f && projectile.velocity.Y < 0f)
                {
                    projectile.velocity.Y = projectile.velocity.Y + num17 * 2f;
                }
            }
            if (projectile.velocity.Y > between.Y)
            {
                projectile.velocity.Y = projectile.velocity.Y - num17;
                if (num17 > 0.05f && projectile.velocity.Y > 0f)
                {
                    projectile.velocity.Y = projectile.velocity.Y - num17 * 2f;
                }
            }
            //if ((double)projectile.velocity.X > 0.25)
            //{
            //    projectile.direction = -1;
            //}
            //else if ((double)projectile.velocity.X < -0.25)
            //{
            //    projectile.direction = 1;
            //}
            //projectile.spriteDirection = projectile.direction;

            //fix, direction gets set automatically by tmodloader based on velocity.X for some reason
            if ((double)projectile.velocity.X > 0.25)
            {
                projectile.ai[0] = -1;
            }
            else if ((double)projectile.velocity.X < -0.25)
            {
                projectile.ai[0] = 1;
            }
            projectile.direction = (int)-projectile.ai[0];
            projectile.spriteDirection = projectile.direction;

            projectile.rotation = projectile.velocity.X * 0.05f;
        }

        private void GetFrame()
        {
            if (Swimming)
            {
                frame2Counter++;
                if (frame2Counter > 5)
                {
                    frame2++;
                    frame2Counter = 0;
                }
                if (frame2 < 6 || frame2 > 9)
                {
                    frame2 = 6;
                }
                return;
            }

            if (projectile.ai[0] == 0) //not flying
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
                        frame2Counter += (int)Math.Abs(2f * projectile.velocity.X);
                        frame2Counter++;
                        if (frame2Counter > 20) //6
                        {
                            frame2++;
                            frame2Counter = 0;
                        }
                        if (frame2 > 5) //frame 1 to 5 is running
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
                    frame2 = 1; //frame 1 is jumping
                }
            }
            else //flying
            {
                if (projectile.velocity.X <= 0) projectile.direction = -1;
                else projectile.direction = 1;
                frame2Counter++;
                if (projectile.velocity.Length() > 3.6f) projectile.velocity *= 0.97f;
                if (frame2Counter > 4)
                {
                    frame2++;
                    frame2Counter = 0;
                }
                if (frame2 < 6 || frame2 > 9)
                {
                    frame2 = 6;
                }
                projectile.rotation = projectile.velocity.X * 0.02f;
            }
        }

        private int frame2Counter = 0;
        private int frame2 = 0;

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            int tileX = (int)((player.position.X /*+ (float)(player.width / 2) + (float)(15 * player.direction)*/) / 16f);
            int tileY = (int)((player.position.Y + (float)player.height - 15f) / 16f);
            Main.NewText(Framing.GetTileSafely(tileX, tileY).liquid == 255);
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.PetGoldfish = false;
            }
            if (modPlayer.PetGoldfish)
            {
                projectile.timeLeft = 2;
            }

            if (Swimming)
            {
                Main.NewText("swimming");
                ZephyrfishAI();
                return false;
            }

            Main.NewText("not swimming");
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Main.hasFocus) GetFrame();

            lightColor = Lighting.GetColor((int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16), Color.White);
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.direction != -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            Texture2D image = mod.GetTexture("Projectiles/Pets/PetGoldfishProj_0"/* + mPlayer.lilWrapsType*/);
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = (int)(image.Bounds.Height / Main.projFrames[projectile.type]);
            bounds.Y = frame2 * bounds.Height;
            Vector2 stupidOffset = new Vector2(projectile.width * 0.5f, projectile.height * 0.5f - 2);
            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }
    }
}
