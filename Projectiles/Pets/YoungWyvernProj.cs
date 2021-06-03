using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class YoungWyvernProj : ModProjectile
    {
        public int frame = 0;
        public int frameCounter = 0;

        public bool InAir => Projectile.ai[0] != 0f;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/YoungWyvernProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Young Wyvern");
            Main.projFrames[Projectile.type] = 9;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BlackCat);
            DrawOffsetX = -6;
            Projectile.width = 44;
            Projectile.height = 30;
            AIType = ProjectileID.BlackCat;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.blackCat = false; // Relic from AIType
            return true;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.YoungWyvern = false;
            }
            if (modPlayer.YoungWyvern)
            {
                Projectile.timeLeft = 2;
            }

            //BLACK CAT
            //bool flag11 = Projectile.position.X - Projectile.oldPosition.X == 0f;
            //if (InAir)
            //{
            //    rotation = base.Projectile.velocity.X * 0.05f;
            //    frameCounter++;
            //    if (frameCounter >= 6)
            //    {
            //        frame++;
            //        frameCounter = 0;
            //    }

            //    if (frame > 10)
            //        frame = 6;

            //    if (frame < 6)
            //        frame = 6;
            //}
            //else
            //{
            //    if (base.Projectile.velocity.Y >= 0f && (double)base.Projectile.velocity.Y <= 0.8)
            //    {
            //        if (flag11)
            //        {
            //            frame = 0;
            //            frameCounter = 0;
            //        }
            //        else if ((double)base.Projectile.velocity.X < -0.8 || (double)base.Projectile.velocity.X > 0.8)
            //        {
            //            frameCounter += (int)Math.Abs(base.Projectile.velocity.X);
            //            frameCounter++;
            //            if (frameCounter > 8)
            //            {
            //                frame++;
            //                frameCounter = 0;
            //            }

            //            if (frame > 5)
            //                frame = 2;
            //        }
            //        else
            //        {
            //            frame = 0;
            //            frameCounter = 0;
            //        }
            //    }
            //    else
            //    {
            //        frameCounter = 0;
            //        frame = 1;
            //    }
            //}
        }

        public override void PostAI()
        {
            //Black cat AI values, so we have to check those
            if (!InAir)
            {
                //DESERT TIGER
                int lastFrame = 8; //frame #8 is spinning, frame #9 is blank

                //if (fancy desert tigers)
                //    lastFrame = 10;

                //Projectile.rotation = 0f;
                if (Projectile.velocity.Y >= 0f && Projectile.velocity.Y <= 0.8f)
                {
                    if (Projectile.position.X - Projectile.oldPosition.X == 0f)
                    {
                        frame = 0;
                        frameCounter = 0;
                    }
                    else if (Math.Abs(Projectile.velocity.X) >= 0.5f)
                    {
                        frameCounter += (int)Math.Abs(Projectile.velocity.X);
                        frameCounter++;
                        if (frameCounter > 10)
                        {
                            frame++;
                            frameCounter = 0;
                        }

                        if (frame >= lastFrame || frame < 2)
                            frame = 2;
                    }
                    else
                    {
                        frame = 0;
                        frameCounter = 0;
                    }
                }
                else if (Projectile.velocity.Y != 0f)
                {
                    frameCounter = 0;
                    frame = 1;
                    //if (fancy desert tigers)
                    //    frame = 9;
                }
            }
            else
            {
                frameCounter = 0;
                frame = 1;
            }

            Projectile.frame = frame;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D image = Mod.GetTexture("Projectiles/Pets/YoungWyvernProj_" + mPlayer.youngWyvernType).Value;
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = Projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / Main.projFrames[Projectile.type]
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

            Vector2 stupidOffset = new Vector2(Projectile.width / 2 + DrawOffsetX, Projectile.height / 2 + Projectile.gfxOffY + 4f);

            Main.spriteBatch.Draw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0f);

            return false;
        }
    }
}
