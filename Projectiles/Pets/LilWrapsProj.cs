using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    [Content(ContentType.DroppedPets)]
    public class LilWrapsProj : SimplePetProjBase
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
            Main.projFrames[Projectile.type] = 12;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyGrinch);
            AIType = ProjectileID.BabyGrinch;
            Projectile.width = 24;
            Projectile.height = 40;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.grinch = false; // Relic from AIType
            return true;
        }

        private void GetFrame()
        {
            if (Projectile.ai[0] == 0) //not flying
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
                        frame2Counter += (int)(2 * xAbs);
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
                        frame2Counter = 10;
                    }
                }
                else if (Projectile.velocity.Y != 0f)
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
                if (Projectile.velocity.X <= 0) Projectile.direction = -1;
                else Projectile.direction = 1;
                frame2Counter++;
                if (Projectile.velocity.Length() > 3.6f) Projectile.velocity *= 0.97f;
                if (frame2Counter > 4)
                {
                    frame2++;
                    frame2Counter = 0;
                }
                if (frame2 < 8 || frame2 > 11)
                {
                    frame2 = 8;
                }
                Projectile.rotation = Projectile.velocity.X * 0.01f;
            }
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.LilWraps = false;
            }
            if (modPlayer.LilWraps)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.hasFocus) GetFrame();

            lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), Color.White);
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.direction != -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/LilWrapsProj_" + mPlayer.lilWrapsType).Value;
            Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: frame2);
            Vector2 stupidOffset = new Vector2(10f, 23f + Projectile.gfxOffY);
            Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

            return false;
        }
    }
}
