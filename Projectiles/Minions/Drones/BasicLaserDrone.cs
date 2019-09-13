using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
    /// <summary>
    /// Fires a weak laser rapidly
    /// </summary>
    public class BasicLaserDrone : DroneBase
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

        private const int AttackDelay = 25;
        private const int SearchDelay = 30;

        private const byte STATE_IDLE = 0;
        private const byte STATE_TARGET_FOUND = 1;
        private const byte STATE_TARGET_ACQUIRED = 2;
        private const byte STATE_TARGET_FIRE = 3;

        private byte AI_STATE = 0;
        private int Direction = -1;
        private float addRotation; //same
        private NPC Target;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Basic Laser Drone");
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
            Player player = projectile.GetOwner();

            int frameOffset = 0; //frame 0, 1

            if (AI_STATE == STATE_TARGET_FIRE) //frame 4, 5
            {
                frameOffset = 4;
            }
            else if (AI_STATE == STATE_TARGET_FOUND || AI_STATE == STATE_TARGET_ACQUIRED) //frame 2, 3
            {
                frameOffset = 2;
            }
            //else
            //{
            //    //frameoffset 0
            //}

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

            Vector2 stupidOffset = new Vector2(projectile.width / 2, (projectile.height - 8f) + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;

            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin, 1f, effects, 0f);

            image = mod.GetTexture(nameGlow);
            spriteBatch.Draw(image, drawPos, bounds, Color.White, projectile.rotation, drawOrigin, 1f, effects, 0f);

            Vector2 rotationOffset = new Vector2(0f, -2f); //-2f
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
            if (AI_STATE == STATE_TARGET_FIRE)
            {
                Vector2 between = Target.Center - projectile.Center;
                //between.Length(): 100 is "close", 1000 is "edge of screen"
                //15.6f = 1000f / 64f
                float magnitude = Utils.Clamp(between.Length() / 15.6f, 6f, 64f);
                between.Normalize();
                Vector2 offset = between * magnitude;
                offset.Y *= 0.5f;
                offsetX += offset.X;
                offsetY += (offset.Y > 0) ? -(32 - offset.Y) : 0;
            }
            return true;
        }

        protected override void ModifyDroneControllerHeld(ref float dmgModifier, ref float kbModifier)
        {
            if (AI_STATE == STATE_TARGET_FIRE)
            {
                if (Main.rand.NextBool(3)) Counter++;
            }
            dmgModifier = 1.25f;
            kbModifier = 1.25f;
        }

        protected override void CustomAI()
        {
            Player player = projectile.GetOwner();
            //Main.NewText("State: " + AI_STATE);
            //Main.NewText("Counter: " + Counter);

            #region Handle State
            int targetIndex = AssAI.FindTarget(projectile, projectile.Center, range: 1000, ignoreTiles: true, useSlowLOS: true);
            if (targetIndex != -1)
            {
                if (AI_STATE == STATE_IDLE) AI_STATE = STATE_TARGET_FOUND;
                Target = Main.npc[targetIndex];

                targetIndex = FindClosestTargetBelow(1000);
                if (targetIndex != -1)
                {
                    Target = Main.npc[targetIndex];
                    if (AI_STATE != STATE_TARGET_FIRE)
                    {
                        AI_STATE = STATE_TARGET_ACQUIRED;
                    }

                    if (AI_STATE == STATE_TARGET_ACQUIRED)
                    {
                        if (Counter > SearchDelay)
                        {
                            Counter = 0;
                            AI_STATE = STATE_TARGET_FIRE;
                        }
                    }
                }
                else
                {
                    Counter = 0;
                    AI_STATE = STATE_TARGET_FOUND;
                }
            }
            else
            {
                AI_STATE = STATE_IDLE;
            }

            if (AI_STATE == STATE_IDLE || AI_STATE == STATE_TARGET_FOUND)
            {
                Direction = player.direction;
            }
            else //definitely has a target (may or may not shoot)
            {
                Direction = (Target.Center.X - projectile.Center.X > 0f).ToDirectionInt();
            }

            if (AI_STATE == STATE_IDLE) Counter = 2 * MinionPos;
            else Counter++;

            projectile.spriteDirection = projectile.direction = -Direction;
            #endregion

            if (AI_STATE == STATE_TARGET_FIRE)
            {
                Vector2 shootOffset = new Vector2(projectile.width / 2 + projectile.spriteDirection * 4f, (projectile.height - 2f) + sinY);
                Vector2 shootOrigin = projectile.position + shootOffset;
                Vector2 target = Target.Center + new Vector2(0f, -5f);

                Vector2 between = target - shootOrigin;
                shootOrigin += Vector2.Normalize(between) * 16f; //roughly tip of turret

                float rotationAmount = between.ToRotation();

                if (projectile.spriteDirection == 1) //adjust rotation based on direction
                {
                    rotationAmount -= (float)Math.PI;
                    if (rotationAmount > 2 * Math.PI)
                    {
                        rotationAmount = -rotationAmount;
                    }
                }

                bool canShoot = true;/*shootOrigin.Y < target.Y + Target.height / 2 + 40;*/

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
                    if (Counter > AttackDelay)
                    {
                        Counter = 0;
                        if (RealOwner)
                        {
                            if (targetIndex != -1 && !Collision.SolidCollision(shootOrigin, 1, 1))
                            {
                                between = target + Target.velocity * 6f - shootOrigin;
                                between.Normalize();
                                between *= 6f;
                                Projectile.NewProjectile(shootOrigin, between, mod.ProjectileType<PetDestroyerDroneLaser>(), CustomDmg, CustomKB, Main.myPlayer, 0f, 0f);

                                //projectile.netUpdate = true;
                            }
                        }
                    }
                }
                else
                {
                    Counter = 0;
                    AI_STATE = STATE_TARGET_ACQUIRED;
                }
            }
            else //if no target, addRotation should go down to projectile.rotation
            {
                //if addRotation is bigger than projectile.rotation by a small margin, reduce it down to projectile.rotation slowly
                //if (Math.Abs(addRotation) > Math.Abs(projectile.rotation) + 0.006f)
                //{
                //    float rotDiff = projectile.rotation - addRotation;
                //    if (Math.Abs(rotDiff) < 0.005f)
                //    {
                //        addRotation = projectile.rotation;
                //    }
                //    else
                //    {
                //        addRotation += addRotation * -0.15f;
                //    }
                //}
                //else
                //{
                //    //fix rotation so it doesn't get adjusted anymore
                //    addRotation = projectile.rotation;
                //}
                addRotation = addRotation.AngleLerp(projectile.rotation, 0.1f);
            }
        }


        private int FindClosestTargetBelow(int range = 1000)
        {
            Player player = projectile.GetOwner();
            int targetIndex = -1;
            float distanceFromTarget = 100000f;
            Vector2 targetCenter = projectile.Center;
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy())
                {
                    float between = Vector2.Distance(npc.Center, projectile.Center);
                    if (((between < range &&
                        Vector2.Distance(player.Center, targetCenter) > between && between < distanceFromTarget) || targetIndex == -1) &&
                        projectile.Bottom.Y < npc.Top.Y + 40 && AssAI.CheckLineOfSight(projectile.Center, npc.Center))
                    {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        targetIndex = k;
                    }
                }
            }
            return (distanceFromTarget < range) ? targetIndex : -1;
        }
    }
}
