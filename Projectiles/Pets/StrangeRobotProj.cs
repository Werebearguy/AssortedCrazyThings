using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class StrangeRobotProj : SimplePetProjBase
    {
        private int frame2Counter = 0;
        private int frame2 = 0;

        private static Asset<Texture2D> eyesAsset;
        private static Asset<Texture2D> fireAsset;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                eyesAsset = ModContent.Request<Texture2D>(Texture + "_Eyes");
                fireAsset = ModContent.Request<Texture2D>(Texture + "_Fire");
            }
        }

        public override void Unload()
        {
            eyesAsset = null;
            fireAsset = null;
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
            Projectile.height = 42;
            AIType = ProjectileID.BabyGrinch;
            DrawOriginOffsetY = 4;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D image = eyesAsset.Value;
            Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 stupidOffset = new Vector2(0, Projectile.gfxOffY);
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(image.Width / 2, Projectile.height / 2);
            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
            Vector2 origin = bounds.Size() / 2 - new Vector2(0, DrawOriginOffsetY); //PROPER WAY OF USING DrawOriginOffsetY I THINK
            float rotation = Projectile.rotation;
            float scale = Projectile.scale;
            Main.EntitySpriteDraw(image, drawPos, bounds, Color.White * Projectile.Opacity, rotation, origin, (float)scale, effects, 0);

            image = fireAsset.Value;
            Main.EntitySpriteDraw(image, drawPos, bounds, Color.White * Projectile.Opacity, rotation, origin, (float)scale, effects, 0);
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
            if (!InAir) //not flying
            {
                if (Projectile.velocity.Y == 0f)
                {
                    if (Projectile.velocity.X == 0f)
                    {
                        frame2 = 0;
                        frame2Counter = 0;
                    }
                    else if (Projectile.velocity.X < -0.8f || Projectile.velocity.X > 0.8f)
                    {
                        frame2Counter += Math.Min((int)Math.Abs(Projectile.velocity.X), 2);
                        frame2Counter++;
                        if (frame2Counter > 12) //6
                        {
                            frame2++;
                            frame2Counter = 0;
                        }
                        if (frame2 > 2) //frame 1 to 2 is running
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
                else if (Projectile.velocity.Y != 0f)
                {
                    frame2Counter++;
                    if (frame2Counter > 6) //6
                    {
                        frame2++;
                        frame2Counter = 0;
                    }
                    if (frame2 > 2) //frame 1 to 2 is jumping aswell
                    {
                        frame2 = 1;
                    }
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
