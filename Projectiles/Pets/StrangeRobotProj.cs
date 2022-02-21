using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class StrangeRobotProj : SimplePetProjBase
    {
        private int frame2Counter = 0;
        private int frame2 = 0;

        private int faceFrame = 0;
        private int faceFrameCounter = 0;
        private const int faceFrameSpeed = 6;
        private const int faceFrameCount = 4;

        private int bobbingCounter = 0;
        private const int bobbingCounterMax = 2;
        private bool bobbing = false;
        private const float bobAmonut = 1f; //Code bobbing, 1f is half a terraria pixel

        /// <summary>
        /// Only has <see cref="faceFrameCount"/> frames
        /// </summary>
        private static Asset<Texture2D> faceAsset;
        private static Asset<Texture2D> fireAsset;
        private static Asset<Texture2D> rearAsset;
        private static Asset<Texture2D> frontAsset;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                faceAsset = ModContent.Request<Texture2D>(Texture + "_Face");
                fireAsset = ModContent.Request<Texture2D>(Texture + "_Fire");
                rearAsset = ModContent.Request<Texture2D>(Texture + "_RearWheel");
                frontAsset = ModContent.Request<Texture2D>(Texture + "_FrontWheel");
            }
        }

        public override void Unload()
        {
            faceAsset = null;
            fireAsset = null;
            rearAsset = null;
            frontAsset = null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange Robot");
            Main.projFrames[Projectile.type] = 5;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyGrinch);
            Projectile.width = 32;
            Projectile.height = 38;
            AIType = ProjectileID.BabyGrinch;
            DrawOriginOffsetY = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D image = fireAsset.Value; //Use a sprite with all 5 frames
            Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 stupidOffset = new Vector2(0, Projectile.gfxOffY);
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(bounds.Width / 2, Projectile.height / 2);
            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
            Vector2 origin = bounds.Size() / 2 - new Vector2(0, DrawOriginOffsetY); //PROPER WAY OF USING DrawOriginOffsetY I THINK
            float rotation = Projectile.rotation;
            float scale = Projectile.scale;
            Color white = Color.White * Projectile.Opacity;

            bool flying = Projectile.frame == 3 || Projectile.frame == 4;
            bool jumping = Projectile.frame == 2;
            if (flying)
            {
                //Fire
                Main.EntitySpriteDraw(image, drawPos, bounds, white, rotation, origin, scale, effects, 0);
            }

            //Rear
            image = rearAsset.Value;
            Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, rotation, origin, scale, effects, 0);

            //Body
            image = TextureAssets.Projectile[Type].Value;
            Vector2 bodyDrawPos = drawPos;
            if (!flying)
            {
                bodyDrawPos.Y += bobbing ? -bobAmonut : 0f;
            }
            Main.EntitySpriteDraw(image, bodyDrawPos, bounds, lightColor, rotation, origin, scale, effects, 0);

            //Front
            image = frontAsset.Value;
            Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, rotation, origin, scale, effects, 0);

            //Face
            image = faceAsset.Value;
            Rectangle faceBounds = bounds;
            faceBounds.Y = faceBounds.Height * faceFrame;
            if (flying || jumping)
            {
                faceBounds.Y += 2;
            }

            Main.EntitySpriteDraw(image, bodyDrawPos, faceBounds, white, rotation, origin, scale, effects, 0);

            return false;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.grinch = false; // Relic from AIType

            GetFrame();

            return true;
        }

        public bool InAir => Projectile.ai[0] != 0f;

        private void GetFrame()
        {
            faceFrameCounter++;
            if (faceFrameCounter >= faceFrameSpeed)
            {
                faceFrameCounter = 0;
                faceFrame++;
                if (faceFrame >= faceFrameCount)
                {
                    faceFrame = 0;
                }
            }

            if (!InAir) //not flying
            {
                if (Projectile.velocity.Y == 0f)
                {
                    float xAbs = Math.Abs(Projectile.velocity.X);
                    if (Projectile.velocity.X == 0f)
                    {
                        frame2 = 0;
                        frame2Counter = 0;
                    }
                    else if (xAbs > 0.5f)
                    {
                        frame2Counter += Math.Min((int)xAbs, 2);
                        frame2Counter++;
                        if (frame2Counter > 12) //6
                        {
                            bobbingCounter++;
                            if (bobbingCounter >= bobbingCounterMax)
                            {
                                bobbingCounter = 0;
                                bobbing = !bobbing;
                            }
                            frame2++;
                            frame2Counter = 0;
                        }
                        if (frame2 > 1) //frame 0 to 1 is running
                        {
                            frame2 = 0;
                        }
                    }
                    else
                    {
                        frame2 = 0; //frame 0 is idle
                        frame2Counter = 6;
                    }
                }
                else if (Projectile.velocity.Y != 0f)
                {
                    frame2 = 2; //frame 2 is jumping
                    frame2Counter = 0;
                }
            }
            else //flying
            {
                frame2Counter++;
                if (frame2Counter > 6) //6
                {
                    frame2++;
                    frame2Counter = 0;
                }

                if (frame2 < 3 || frame2 > 4) //frame 3 to 4 are flying
                {
                    frame2 = 3;
                }

                Projectile.rotation = Projectile.velocity.X * 0.04f;
                
                //Propulsion dust
                float dustChance = Math.Clamp(Math.Abs(Projectile.velocity.Length()) / 5f, 0.3f, 0.9f);
                if (Main.rand.NextFloat() < dustChance)
                {
                    Vector2 dustOrigin = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * 12;
                    Dust dust = Dust.NewDustDirect(dustOrigin - Vector2.One * 4f, 8, 8, DustID.Torch, -Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f + 5f, 50, default(Color), 1.7f);
                    dust.noLightEmittence = true;
                    dust.velocity.X *= 0.2f;
                    dust.noGravity = true;
                }
            }
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.StrangeRobot = false;
            }
            if (modPlayer.StrangeRobot)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override void PostAI()
        {
            Projectile.frame = frame2;
        }
    }
}
