using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssortedCrazyThings.Base;
using System;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
    /// <summary>
    /// Creates a damage reducing shield
    /// Checks if its active for the player in AssPlayer.PreUpdate, then resets shield
    /// </summary>
    public class ShieldDrone : DroneBase
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Minions/Drones/HealingDrone";
            }
        }

        private static readonly string nameGlow = "Projectiles/Minions/Drones/" + "HealingDrone_Glowmask";
        private static readonly string nameLower = "Projectiles/Minions/Drones/" + "HealingDrone_Lower";
        private static readonly string nameLowerGlow = "Projectiles/Minions/Drones/" + "HealingDrone_Lower_Glowmask";
        private float addRotation; //same
        private const int ShieldDelay = 180;
        private const byte ShieldIncreaseAmount = 10;

        private float ShieldCounter
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

        private bool CanShield
        {
            get
            {
                AssPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<AssPlayer>();
                return mPlayer.shieldDroneReduction < AssPlayer.shieldDroneReductionMax;
            }
        }

        public override bool IsCombatDrone
        {
            get
            {
                return false;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shield Drone");
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

            int frameOffset = 0; //frame 0, 1

            if (CanShield) //frame 4, 5
            {
                frameOffset = 4;
            }
            else if (CanShield) //frame 2, 3
            {
                frameOffset = 2;
            }
            else
            {
                //frameoffset 0
            }

            if (projectile.frame < frameOffset) projectile.frame = frameOffset;

            if (++projectile.frameCounter >= ((projectile.velocity.Length() > 6f) ? frameCounterMaxFar : frameCounterMaxClose))
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 2 + frameOffset)
                {
                    projectile.frame = frameOffset;
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

            Vector2 rotationOffset = new Vector2(0f, -2f); //-2f)
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

        protected override void ModifyDroneControllerHeld(ref float dmgModifier, ref float kbModifier)
        {
            if (CanShield) ShieldCounter += 0.333f;
        }

        protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
        {
            AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, staticDirection: true, vanityPet: true, veloXToRotationFactor: 0.5f, offsetX: -30f, offsetY: -50f);
            return false;
        }

        protected override void CustomAI()
        {
            Player player = Main.player[projectile.owner];
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

            if (CanShield)
            {
                Vector2 shootOffset = new Vector2(projectile.width / 2 + projectile.spriteDirection * 4f, (projectile.height - 2f) + sinY);
                Vector2 shootOrigin = projectile.position + shootOffset;
                Vector2 target = player.MountedCenter + new Vector2(0f, -5f);

                Vector2 between = target - shootOrigin;
                shootOrigin += Vector2.Normalize(between) * 19f; //roughly tip of turret
                target += -Vector2.Normalize(between) * 12f; //roughly center of head with a buffer

                float rotationAmount = between.ToRotation();

                if (projectile.spriteDirection == 1) //adjust rotation based on direction
                {
                    rotationAmount -= (float)Math.PI;
                    if (rotationAmount > 2 * Math.PI)
                    {
                        rotationAmount = -rotationAmount;
                    }
                }

                bool canShoot = shootOrigin.Y < target.Y + player.height / 2;

                if (projectile.spriteDirection == -1) //reset canShoot properly if rotation is too much (aka target is too fast for the drone to catch up)
                {
                    if (rotationAmount <= projectile.rotation)
                    {
                        canShoot = false;
                        rotationAmount = projectile.rotation;
                    }
                }
                else
                {
                    if (rotationAmount <= projectile.rotation - Math.PI)
                    {
                        canShoot = false;
                        rotationAmount = projectile.rotation;
                    }
                }
                addRotation = addRotation.AngleLerp(rotationAmount, 0.1f);

                if (canShoot) //when target below drone
                {
                    ShieldCounter++;

                    if (ShieldCounter > ShieldDelay)
                    {
                        ShieldCounter = 0;
                        if (Main.netMode != NetmodeID.Server && Main.myPlayer == player.whoAmI) mPlayer.shieldDroneReduction += ShieldIncreaseAmount;
                        CombatText.NewText(player.getRect(), Color.LightBlue, ShieldIncreaseAmount);
                        AssUtils.QuickDustLine(16, shootOrigin, target, between.Length() / 3, Color.White, alpha: 120, scale: 2f);
                    }
                }
            }
            else //if above 50%, addRotation should go down to projectile.rotation
            {
                addRotation = addRotation.AngleLerp(projectile.rotation, 0.1f);
            }

            //if (Main.rand.NextFloat() < 0.05f)
            //{
            //    float speedX = Main.rand.NextFloat(-7, 7);
            //    float speedY = Main.rand.NextFloat(-6, -3);
            //    Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 1, 1, 222, speedX, speedY, 100, default(Color), 0.8f)];
            //    dust.velocity *= 0.2f;
            //}
        }
    }
}
