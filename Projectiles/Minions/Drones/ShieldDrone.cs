using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private static readonly string nameLamps = "Projectiles/Minions/Drones/" + "ShieldDrone_Lamps";
        private static readonly string nameLower = "Projectiles/Minions/Drones/" + "ShieldDrone_Lower";
        private float addRotation; //same
        private const int ShieldDelay = 180;
        public const byte ShieldIncreaseAmount = 10;
        private float LowerOutPercent = 0f;

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
                AssPlayer mPlayer = projectile.GetOwner().GetModPlayer<AssPlayer>();
                return mPlayer.shieldDroneReduction < AssPlayer.shieldDroneReductionMax && LowerOutPercent == 1f;
            }
        }

        private int Stage
        {
            get
            {
                AssPlayer mPlayer = projectile.GetOwner().GetModPlayer<AssPlayer>();
                return mPlayer.shieldDroneReduction / 10;
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
            projectile.width = 34;
            projectile.height = 30;
            projectile.alpha = 0;
            projectile.minion = true;
            projectile.minionSlots = 1f;
        }

        protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {
            projectile.frame = Stage;

            float intensity = 700f - 25f * projectile.frame;
            Vector2 lightPos = projectile.Top + new Vector2(0f, sinY);
            Vector3 lightCol = default;
            if (projectile.frame == 5)
            {
                lightCol = new Vector3(124, 251, 34);
            }
            else if (projectile.frame > 2)
            {
                lightCol = new Vector3(200, 150, 0f);
            }
            else if (projectile.frame > 0)
            {
                lightCol = new Vector3(153, 63, 66);
            }
            Lighting.AddLight(lightPos, lightCol / intensity);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D image = mod.GetTexture(nameLower);
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[projectile.type];
            bounds.Y = projectile.frame * bounds.Height;

            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2 + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;

            if (LowerOutPercent > 0f)
            {
                //Vector2 rotationOffset = new Vector2(0f, -16 + LowerOutPercent * 16);
                Vector2 rotationOffset = new Vector2(0f, 16 * (LowerOutPercent - 1f));

                //rotation origin is (projectile.position + stupidOffset) - drawOrigin; //not including Main.screenPosition
                spriteBatch.Draw(image, drawPos + rotationOffset, bounds, lightColor, addRotation, drawOrigin, 1f, effects, 0f);
            }

            image = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin, 1f, effects, 0f);

            image = mod.GetTexture(nameLamps);
            spriteBatch.Draw(image, drawPos, bounds, Color.White, projectile.rotation, drawOrigin, 1f, effects, 0f);


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
            Player player = projectile.GetOwner();
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

            if (CanShield)
            {
                Vector2 shootOffset = new Vector2(0 , sinY);
                Vector2 shootOrigin = projectile.Center + shootOffset;
                Vector2 target = player.MountedCenter + new Vector2(0f, -5f);

                Vector2 between = target - shootOrigin;
                shootOrigin += Vector2.Normalize(between) * 19f; //roughly tip of turret
                target += -Vector2.Normalize(between) * 12f; //roughly center of head with a buffer

                float rotationAmount = between.ToRotation();

                if (projectile.spriteDirection == 1) //adjust rotation based on direction
                {
                    rotationAmount -= (float)Math.PI / 2;
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
                        AssUtils.QuickDustLine(16, shootOrigin, target, between.Length() / 3, Color.White, alpha: 120, scale: 1.5f);
                    }
                }
            }
            else //if above 50%, addRotation should go down to projectile.rotation
            {
                addRotation = addRotation.AngleLerp(projectile.rotation, 0.1f);
            }

            if (Stage < 5)
            {
                if (LowerOutPercent < 1f)
                {
                    LowerOutPercent += 0.015f;
                    if (LowerOutPercent > 1f) LowerOutPercent = 1f;
                }
            }
            else
            {
                if (LowerOutPercent > 0f)
                {
                    LowerOutPercent -= 0.015f;
                    if (LowerOutPercent < 0f) LowerOutPercent = 0f;
                }
            }
        }
    }
}
