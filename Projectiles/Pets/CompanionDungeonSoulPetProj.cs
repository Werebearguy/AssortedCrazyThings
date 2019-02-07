using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CompanionDungeonSoulPetProj : ModProjectile
    {
        private int sincounter;

        public override void SetStaticDefaults()
        {
            //I didnt change anything regarding ai, so this is a straight up clone of this https://terraria.gamepedia.com/Creeper_Egg
            DisplayName.SetDefault("Companion Soul");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2PetGhost);
            projectile.aiStyle = -1;
            projectile.width = 14;
            projectile.height = 24;
            projectile.alpha = 0;
        }

        //draw it with 78% "brightness" (like the NPC and item version of that soul), plus that "up/down" motion
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D image = Main.projectileTexture[projectile.type];
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = (image.Bounds.Height / Main.projFrames[projectile.type]);
            bounds.Y = projectile.frame * bounds.Height;

            float sinY = 0;
            sincounter = sincounter > 120 ? 0 : sincounter + 1;
            sinY = (float)((Math.Sin((sincounter / 120f) * 2 * Math.PI) - 1) * 10);

            Vector2 stupidOffset = new Vector2(projectile.width / 2, (projectile.height - 10f) + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;

            lightColor = projectile.GetAlpha(lightColor) * 0.99f; //1f is opaque
            lightColor.R = Math.Max(lightColor.R, (byte)200); //100 for dark
            lightColor.G = Math.Max(lightColor.G, (byte)200);
            lightColor.B = Math.Max(lightColor.B, (byte)200);

            spriteBatch.Draw(image, drawPos, bounds, Color.White, 0f, bounds.Size() / 2, 1f, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }

        //public override bool PreAI()
        //{
        //    Player player = Main.player[projectile.owner];
        //    player.petFlagDD2Ghost = false; // Relic from aiType
        //    return true;
        //}

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.SoulLightPet = false;
            }
            if (modPlayer.SoulLightPet)
            {
                projectile.timeLeft = 2;

                FlickerwickPet(projectile);
            }
        }

        //
        public static void FlickerwickPet(Projectile projectile, bool lightPet = true, bool reverseSide = false, bool vanityPet = false, float offsetX = 0f, float offsetY = 0f)
        {
            Player player = Main.player[projectile.owner];
            float num = 6f;
            int num2 = 10;
            int num3 = 4;
            int num4 = Main.projFrames[projectile.type];
            Vector2 value = new Vector2((player.direction * 30) + player.direction * offsetX, -20f + offsetY);

            //up and down bobbing
            //projectile.localAI[0] += 1f;
            //if (projectile.localAI[0] > 120f)
            //{
            //    projectile.localAI[0] = 0f;
            //}
            //value.Y += (float)Math.Cos((double)(projectile.localAI[0] * 0.05235988f)) * 2f;

            Vector2 value2 = new Vector2((projectile.spriteDirection == -1) ? -6 : -2, -20f).RotatedBy(projectile.rotation);

            projectile.direction = projectile.spriteDirection = player.direction;

            if (reverseSide)
            {
                value.X = -value.X;
                //value2.X = -value2.X;
                projectile.direction = -projectile.direction;
                projectile.spriteDirection = -projectile.spriteDirection;
            }

            if (lightPet && Main.rand.Next(24) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.Center + value2, 4, 4, 135, 0f, 0f, 100);
                if (Main.rand.Next(3) != 0)
                {
                    dust.noGravity = true;
                    dust.velocity.Y = dust.velocity.Y - 3f;
                    dust.noLight = true;
                }
                else if (Main.rand.Next(2) != 0)
                {
                    dust.noLight = true;
                }
                dust.velocity *= 0.5f;
                dust.velocity.Y = dust.velocity.Y - 0.9f;
                dust.scale += 0.1f + Main.rand.NextFloat() * 0.6f;
                dust.shader = GameShaders.Armor.GetSecondaryShader(!vanityPet? player.cLight : player.cPet, player);
            }

            if (lightPet)
            {
                Vector3 vector = DelegateMethods.v3_1 = new Vector3(0.3f, 0.5f, 1f);
                Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * 6f, 20f, DelegateMethods.CastLightOpen);
                Utils.PlotTileLine(projectile.Left, projectile.Right, 20f, DelegateMethods.CastLightOpen);
                Utils.PlotTileLine(player.Center, player.Center + player.velocity * 6f, 40f, DelegateMethods.CastLightOpen);
                Utils.PlotTileLine(player.Left, player.Right, 40f, DelegateMethods.CastLightOpen);
            }

            Vector2 vector2 = player.MountedCenter + value;
            float num6 = Vector2.Distance(projectile.Center, vector2);
            if (num6 > 1000f)
            {
                projectile.Center = player.Center + value;
            }
            Vector2 vector3 = vector2 - projectile.Center;
            if (num6 < num)
            {
                projectile.velocity *= 0.25f;
            }
            if (vector3 != Vector2.Zero)
            {
                if (vector3.Length() < num * 0.5f)
                {
                    projectile.velocity = vector3;
                }
                else
                {
                    projectile.velocity = vector3 * 0.1f;
                }
            }
            if (projectile.velocity.Length() > 6f)
            {
                float num7 = projectile.velocity.X * 0.08f + projectile.velocity.Y * projectile.spriteDirection * 0.02f;
                if (Math.Abs(projectile.rotation - num7) >= 3.14159274f)
                {
                    if (num7 < projectile.rotation)
                    {
                        projectile.rotation -= 6.28318548f;
                    }
                    else
                    {
                        projectile.rotation += 6.28318548f;
                    }
                }
                float num8 = 12f;
                projectile.rotation = (projectile.rotation * (num8 - 1f) + num7) / num8;
                if (++projectile.frameCounter >= num3)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 0;
                    }
                }
            }
            else
            {
                if (projectile.rotation > 3.14159274f)
                {
                    projectile.rotation -= 6.28318548f;
                }
                if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
                {
                    projectile.rotation = 0f;
                }
                else
                {
                    projectile.rotation *= 0.96f;
                }
                if (++projectile.frameCounter >= num2)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 0;
                    }
                }
            }
        }
    }
}
