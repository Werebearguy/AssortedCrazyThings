using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class HealingDroneProj : ModProjectile
    {
        private static readonly string nameGlow = "Projectiles/Pets/" + "HealingDroneProj_Glowmask";
        private static readonly string nameLower = "Projectiles/Pets/" + "HealingDroneProj_Lower";
        private static readonly string nameLowerGlow = "Projectiles/Pets/" + "HealingDroneProj_Lower_Glowmask";
        private float sinY;
        private float addRotation;

        private float Sincounter
        {
            get
            {
                return projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Healing Drone");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2PetGhost);
            projectile.aiStyle = -1;
            projectile.width = 34;
            projectile.height = 30;
            projectile.alpha = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D image = Main.projectileTexture[projectile.type];
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = (image.Bounds.Height / Main.projFrames[projectile.type]);
            bounds.Y = projectile.frame * bounds.Height;

            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 stupidOffset = new Vector2(projectile.width / 2, (projectile.height - 8f) + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;

            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, bounds.Size() / 2, 1f, effects, 0f);

            image = mod.GetTexture(nameGlow);
            spriteBatch.Draw(image, drawPos, bounds, Color.White, projectile.rotation, bounds.Size() / 2, 1f, effects, 0f);

            //Dust dust = Dust.NewDustPerfect(projectile.position + stupidOffset, 135, new Vector2(0f, 0f), 100, Color.White, 0.6f);
            //dust.noGravity = true;
            //dust.noLight = true;

            //Dust dust2 = Dust.NewDustPerfect(projectile.position + stupidOffset - bounds.Size() / 2, 136, new Vector2(0f, 0f), 100, Color.White, 0.6f);
            //dust2.noGravity = true;
            //dust2.noLight = true;

            //ROTATION CENTER IS position WHEN origin IS bounds.Size() / 2
            image = mod.GetTexture(nameLower);
            spriteBatch.Draw(image, drawPos + new Vector2(0f, -2f), bounds, lightColor, addRotation, bounds.Size() / 2 + new Vector2(0f, -2f), 1f, effects, 0f);

            image = mod.GetTexture(nameLowerGlow);
            spriteBatch.Draw(image, drawPos + new Vector2(0f, -2f), bounds, Color.White, addRotation, bounds.Size() / 2 + new Vector2(0f, -2f), 1f, effects, 0f);

            return false;
        }

        public static Dust QuickDust(Vector2 pos, Color color, Vector2 dustVelo)
        {
            int type = 61;
            Dust dust = Dust.NewDustPerfect(pos, type, dustVelo, 120, color, 2f);
            dust.position = pos;
            dust.velocity = dustVelo;
            dust.fadeIn = 1f;
            dust.noLight = false;
            dust.noGravity = true;
            return dust;
        }

        public static void QuickDustLine(Vector2 start, Vector2 end, float splits, Color color, Vector2 dustVelo)
        {
            QuickDust(start, color, dustVelo);
            float num = 1f / splits;
            for (float num2 = 0f; num2 < 1f; num2 += num)
            {
                QuickDust(Vector2.Lerp(start, end, num2), color, dustVelo);
            }
        }

        private void CustomAI()
        {
            Player player = Main.player[projectile.owner];
            Sincounter = Sincounter > 120 ? 0 : Sincounter + 1;
            //Sincounter = 0;
            sinY = (float)((Math.Sin((Sincounter / 120f) * 2 * Math.PI) - 1) * 4);
            //sinY = 0f;
            
            if(player.statLife < player.statLifeMax2 / 2)
            {
                Vector2 shootOffset = new Vector2(projectile.width / 2, (projectile.height - 8f) + sinY);
                Vector2 shootOrigin = projectile.position + shootOffset;
                Vector2 target = player.MountedCenter + new Vector2(0f, -5f);

                Vector2 between = target - shootOrigin;
                shootOrigin += Vector2.Normalize(between) * 16f;
                target += -Vector2.Normalize(between) * 12f;

                addRotation = between.ToRotation();

                if (projectile.spriteDirection == 1)
                {
                    addRotation -= (float)Math.PI;
                    if (addRotation > 2 * Math.PI)
                    {
                        addRotation = -addRotation;
                    }
                }

                bool canShoot = true;

                if (projectile.spriteDirection == -1)
                {
                    if (addRotation <= projectile.rotation)
                    {
                        canShoot = false;
                        addRotation = projectile.rotation;
                    }
                }
                else
                {
                    if (addRotation >= projectile.rotation)
                    {
                        canShoot = false;
                        addRotation = projectile.rotation;
                    }
                }

                //Main.NewText(projectile.rotation);
                //Main.NewText(addRotation);

                if (canShoot && shootOrigin.Y < target.Y) //when target below drone
                {
                    if (Sincounter % 60 == 30) //only shoot once a second, when target below drone and when turret aligned properly
                    {
                        int heal = 2;
                        player.statLife += heal;
                        player.HealEffect(heal);
                    }
                    if (Sincounter % 60 == 35)
                    {
                        QuickDustLine(shootOrigin, target, between.Length() / 3, Color.White, Vector2.Zero);
                    }
                }
            }
            else
            {
                addRotation = projectile.rotation;
            }
        }

        private void CustomDraw(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {
            //frame 0, 1: above two thirds health
            //frame 2, 3: above half health, below two thirds health
            //frame 4, 5: below half health, healing
            Player player = Main.player[projectile.owner];

            Vector2 lightPos = projectile.position + new Vector2((projectile.spriteDirection == 1? 0f : projectile.width), projectile.height / 2);

            int frameOffset = 0; //frame 0, 1
            
            if (player.statLife < player.statLifeMax2 / 2) //frame 4, 5
            {
                Lighting.AddLight(lightPos, new Vector3(153 / 700f, 63 / 700f, 66 / 700f));
                frameOffset = 4;
            }
            else if (player.statLife < player.statLifeMax2 / 1.5f) //frame 2, 3
            {
                Lighting.AddLight(lightPos, new Vector3(240 / 700f, 198 / 700f, 0f));
                frameOffset = 2;
            }
            else
            {
                Lighting.AddLight(lightPos, new Vector3(124 / 700f, 251 / 700f, 34 / 700f));
                //frameoffset 0
            }
            
            projectile.frame -= frameOffset;

            if (projectile.velocity.Length() > 6f)
            {
                if (++projectile.frameCounter >= frameCounterMaxFar)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= 2)
                    {
                        projectile.frame = 0;
                    }
                }
            }
            else
            {
                if (++projectile.frameCounter >= frameCounterMaxClose)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= 2)
                    {
                        projectile.frame = 0;
                    }
                }
            }

            projectile.frame += frameOffset;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.HealingDrone = false;
            }
            if (modPlayer.HealingDrone)
            {
                projectile.timeLeft = 2;

                CompanionDungeonSoulPetProj.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, reverseSide: true, veloXToRotationFactor: 0.5f, offsetX: 16f, offsetY: (player.statLife < player.statLifeMax2 / 2)? -26f: -26f); //2f

                CustomAI();

                CustomDraw();
            }
        }
    }
}
