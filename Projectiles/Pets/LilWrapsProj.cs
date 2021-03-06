﻿using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class LilWrapsProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/LilWrapsProj_0"; //temp
            }
        }

        private int frame2Counter = 0;
        private int frame2 = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lil' Wraps");
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
            Player player = projectile.GetOwner();
            player.grinch = false; // Relic from aiType
            return true;
        }

        private void GetFrame()
        {
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
                        if (frame2 > 6) //frame 1 to 6 is running
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
                if (projectile.velocity.X <= 0) projectile.direction = -1;
                else projectile.direction = 1;
                frame2Counter++;
                if (projectile.velocity.Length() > 3.6f) projectile.velocity *= 0.97f;
                if (frame2Counter > 4)
                {
                    frame2++;
                    frame2Counter = 0;
                }
                if (frame2 < 8 || frame2 > 11)
                {
                    frame2 = 8;
                }
                projectile.rotation = projectile.velocity.X * 0.01f;
            }
        }

        public override void AI()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.LilWraps = false;
            }
            if (modPlayer.LilWraps)
            {
                projectile.timeLeft = 2;
            }
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
            PetPlayer mPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>();
            Texture2D image = mod.GetTexture("Projectiles/Pets/LilWrapsProj_" + mPlayer.lilWrapsType);
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[projectile.type];
            bounds.Y = frame2 * bounds.Height;
            Vector2 stupidOffset = new Vector2(10f, 23f + projectile.gfxOffY);
            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }
    }
}
