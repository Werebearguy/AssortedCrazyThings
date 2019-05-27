using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssortedCrazyThings.Base;
using System;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions
{
    /// <summary>
    /// Heals the player if below max health
    /// Heals faster when below 50% health
    /// </summary>
    public class HealingDrone : DroneBase
    {
        private static readonly string nameGlow = "Projectiles/Minions/" + "HealingDrone_Glowmask";
        private static readonly string nameLower = "Projectiles/Minions/" + "HealingDrone_Lower";
        private static readonly string nameLowerGlow = "Projectiles/Minions/" + "HealingDrone_Lower_Glowmask";
        private float addRotation; //same

        private bool CanHeal
        {
            get
            {
                return Main.player[projectile.owner].statLife < Main.player[projectile.owner].statLifeMax2;
            }
        }

        protected override bool IsCombatDrone
        {
            get
            {
                return false;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Healing Drone");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2PetGhost);
            projectile.aiStyle = -1;
            projectile.width = 38;
            projectile.height = 30;
            projectile.alpha = 0;
            projectile.minion = true;
            projectile.minionSlots = 1f;
        }

        protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {
            //frame 0, 1: full life
            //frame 2, 3: above half health, healing
            //frame 4, 5: below half health, healing faster
            Player player = Main.player[projectile.owner];

            Vector2 lightPos = projectile.position + new Vector2(projectile.spriteDirection == 1 ? 0f : projectile.width, projectile.height / 2);

            int frameOffset = 0; //frame 0, 1

            if (player.statLife < player.statLifeMax2 / 2) //frame 4, 5
            {
                Lighting.AddLight(lightPos, new Vector3(153 / 700f, 63 / 700f, 66 / 700f));
                frameOffset = 4;
            }
            else if (CanHeal) //frame 2, 3
            {
                Lighting.AddLight(lightPos, new Vector3(240 / 700f, 198 / 700f, 0f));
                frameOffset = 2;
            }
            else
            {
                Lighting.AddLight(lightPos, new Vector3(124 / 700f, 251 / 700f, 34 / 700f));
                //frameoffset 0
            }

            if (projectile.frame < frameOffset) projectile.frame = frameOffset;

            if (projectile.velocity.Length() > 6f)
            {
                if (++projectile.frameCounter >= frameCounterMaxFar)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= 2 + frameOffset)
                    {
                        projectile.frame = frameOffset;
                    }
                }
            }
            else
            {
                if (++projectile.frameCounter >= frameCounterMaxClose)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= 2 + frameOffset)
                    {
                        projectile.frame = frameOffset;
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D image = Main.projectileTexture[projectile.type];
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[projectile.type];
            bounds.Y = projectile.frame * bounds.Height;

            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height - 8f + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;

            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin, 1f, effects, 0f);

            image = mod.GetTexture(nameGlow);
            spriteBatch.Draw(image, drawPos, bounds, Color.White, projectile.rotation, drawOrigin, 1f, effects, 0f);

            Vector2 rotationOffset = new Vector2(0f, -4f); //-2f)
            drawPos += rotationOffset;
            drawOrigin += rotationOffset;

            //AssUtils.ShowDustAtPos(135, projectile.position + stupidOffset);

            //AssUtils.ShowDustAtPos(136, projectile.position + stupidOffset - drawOrigin);

            //rotation origin is (projectile.position + stupidOffset) - drawOrigin; //not including Main.screenPosition
            image = mod.GetTexture(nameLower);
            spriteBatch.Draw(image, drawPos, bounds, lightColor, addRotation, drawOrigin, 1f, effects, 0f);

            image = mod.GetTexture(nameLowerGlow);
            spriteBatch.Draw(image, drawPos, bounds, Color.White, addRotation, drawOrigin, 1f, effects, 0f);

            return false;
        }

        protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
        {
            AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, reverseSide: true, veloXToRotationFactor: 0.5f, offsetX: 16f, offsetY: CanHeal ? -16f : 4f); //2f
            return false;
        }

        protected override void CustomAI()
        {
            Player player = Main.player[projectile.owner];

            if (CanHeal)
            {
                Vector2 shootOffset = new Vector2(projectile.width / 2, (projectile.height - 2f) + sinY);
                Vector2 shootOrigin = projectile.position + shootOffset;
                Vector2 target = player.MountedCenter + new Vector2(0f, -5f);

                Vector2 between = target - shootOrigin;
                shootOrigin += Vector2.Normalize(between) * 16f; //roughly tip of turret
                target += -Vector2.Normalize(between) * 12f; //roughly center of head with a buffer

                addRotation = between.ToRotation();

                if (projectile.spriteDirection == 1) //adjust rotation based on direction
                {
                    addRotation -= (float)Math.PI;
                    if (addRotation > 2 * Math.PI)
                    {
                        addRotation = -addRotation;
                    }
                }

                bool canShoot = shootOrigin.Y < target.Y + player.height / 2;

                if (projectile.spriteDirection == -1) //reset canShoot properly if rotation is too much (aka target is too fast for the drone to catch up)
                {
                    if (addRotation <= projectile.rotation)
                    {
                        canShoot = false;
                        addRotation = projectile.rotation;
                    }
                }
                else
                {
                    if (addRotation <= projectile.rotation - Math.PI)
                    {
                        canShoot = false;
                        addRotation = projectile.rotation;
                    }
                }

                if (canShoot) //when target below drone
                {
                    float delay = player.statLife < player.statLifeMax2 / 2 ? 60 : 80;

                    if (Sincounter % delay == 30) //only shoot once every 1.333 or 1.5 seconds, when target below drone and when turret aligned properly
                    {
                        int heal = 1;
                        player.statLife += heal;
                        player.HealEffect(heal, false);
                    }
                    if (Sincounter % delay == 35)
                    {
                        AssUtils.QuickDustLine(61, shootOrigin, target, between.Length() / 3, Color.White, alpha: 120, scale: 2f);
                    }
                }
            }
            else //if above 50%, addRotation should go down to projectile.rotation
            {
                //if addRotation is bigger than projectile.rotation by a small margin, reduce it down to projectile.rotation slowly
                if (Math.Abs(addRotation) > Math.Abs(projectile.rotation) + 0.006f)
                {
                    float rotDiff = projectile.rotation - addRotation;
                    if (Math.Abs(rotDiff) < 0.005f)
                    {
                        addRotation = projectile.rotation;
                    }
                    else
                    {
                        addRotation += addRotation * -0.15f;
                    }
                }
                else
                {
                    //fix rotation so it doesn't get adjusted anymore
                    addRotation = projectile.rotation;
                }
            }
        }
    }
}
