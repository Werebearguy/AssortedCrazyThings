using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
    /// <summary>
    /// Fires a penetrating laser beam horizontally to the player with a very long delay.
    /// Only recognizes enemies at around the y level of the player
    /// </summary>
    public class HeavyLaserDrone : DroneBase
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

        private const int AttackCooldown = 180;
        private const int SearchDelay = 90; //60 but incremented 1.5f
        private const int ChargeDelay = 120;

        private const byte STATE_COOLDOWN = 0;
        private const byte STATE_IDLE = 1;
        private const byte STATE_CHARGE = 2;
        private const byte STATE_RECOIL = 3;

        private byte AI_STATE = 0;
        private byte PosInCharge = 0;
        private int Direction = -1;
        private float InitialDistance = 0;

        private Vector2 BarrelPos
        {
            get
            {
                Vector2 position = projectile.Center;
                position.Y += sinY;
                return position;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heavy Laser Drone");
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
            projectile.minionSlots = 2f;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            writer.Write((byte)AI_STATE);
            writer.Write((byte)PosInCharge);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            AI_STATE = reader.ReadByte();
            PosInCharge = reader.ReadByte();
        }

        protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {
            //frame 0, 1: above two thirds health
            //frame 2, 3: above half health, below two thirds health
            //frame 4, 5: below half health, healing

            int frameOffset = 0; //frame 0, 1

            if (AI_STATE == STATE_CHARGE) //frame 4, 5
            {
                frameOffset = 4;
            }
            else if (AI_STATE == STATE_COOLDOWN) //frame 2, 3
            {
                frameOffset = 2;
            }
            else
            {
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

            Vector2 stupidOffset = new Vector2(projectile.width / 2, (projectile.height - 8f) + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;

            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin, 1f, effects, 0f);

            image = mod.GetTexture(nameGlow);
            spriteBatch.Draw(image, drawPos, bounds, Color.White, projectile.rotation, drawOrigin, 1f, effects, 0f);

            Vector2 rotationOffset = new Vector2(0f, -2f);
            drawPos += rotationOffset;
            drawOrigin += rotationOffset;

            //AssUtils.ShowDustAtPos(135, projectile.position + stupidOffset);

            //AssUtils.ShowDustAtPos(136, projectile.position + stupidOffset - drawOrigin);

            //rotation origin is (projectile.position + stupidOffset) - drawOrigin; //not including Main.screenPosition
            image = mod.GetTexture(nameLower);
            bounds.Y = 0;
            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin, 1f, effects, 0f);

            image = mod.GetTexture(nameLowerGlow);
            bounds.Y = 0;
            spriteBatch.Draw(image, drawPos, bounds, Color.White, projectile.rotation, drawOrigin, 1f, effects, 0f);

            return false;
        }

        protected override void ModifyDroneControllerHeld(ref float dmgModifier, ref float kbModifier)
        {
            dmgModifier = 1.1f;
            kbModifier = 1.1f;

            if (AI_STATE == STATE_COOLDOWN)
            {
                Counter += Main.rand.Next(2);
            }
        }

        protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
        {
            if (AI_STATE == STATE_CHARGE)
            {
                //offset x = 30 when facing right
                offsetX = Direction == 1 ? -80 : 20;
                offsetX += Math.Sign(offsetX) * PosInCharge * projectile.width * 1.5f;
                offsetY = 10;
                veloSpeed = 0.5f;
            }
            else if (AI_STATE == STATE_RECOIL)
            {
                //150 to 50 is smooth, distance
                Vector2 pos = new Vector2(offsetX, offsetY) - new Vector2(-30, 20);
                //veloSpeed = (float)Math.Pow((double)Counter / AttackCooldown, 2) + 0.05f;
                Vector2 distanceToTargetVector = (pos + projectile.GetOwner().Center) - projectile.Center;
                float distanceToTarget = distanceToTargetVector.Length();
                if (Counter == 0) InitialDistance = distanceToTarget;
                //Main.NewText("proper: " + distanceToTargetVector.Length());
                float magnitude = 1f + distanceToTargetVector.LengthSquared() / (InitialDistance * InitialDistance);
                distanceToTargetVector.Normalize();
                distanceToTargetVector *= magnitude;
                //-(Counter - AttackCooldown / 5) -> goes from 36 to 0
                float accel = Utils.Clamp(-(Counter - 36), 4, 20);
                projectile.velocity = (projectile.velocity * (accel - 1) + distanceToTargetVector) / accel;
                return false;
            }
            return true;
        }

        protected override bool Bobbing()
        {
            if (AI_STATE == STATE_CHARGE)
            {
                sinY = 0;
                return false;
            }
            return true;
            //else
            //{
            //    Sincounter = Sincounter > 240 ? 0 : Sincounter + 1;
            //    sinY = (float)((Math.Sin(((Sincounter + MinionPos * 10f) / 120f) * 2 * Math.PI) - 1) * 4);
            //}
        }

        protected override void CustomAI()
        {
            //Main.NewText("State: " + AI_STATE);
            //Main.NewText("Counter: " + Counter);

            #region Handle State
            if (AI_STATE == STATE_COOLDOWN)
            {
                if (Counter > AttackCooldown)
                {
                    Counter = 0;
                    //Main.NewText("Change from cooldown to idle");
                    AI_STATE = STATE_IDLE;
                    if (RealOwner) projectile.netUpdate = true;
                }
                //else stay in cooldown and wait for counter to reach 
            }
            else if (AI_STATE == STATE_IDLE)
            {
                if (Counter > SearchDelay)
                {
                    Counter = 0;
                    int targetIndex = FindClosestHorizontalTarget();
                    if (targetIndex != -1)
                    {
                        PosInCharge = (byte)GetChargePosition();
                        //Main.NewText("Change from idle to charge");
                        AI_STATE = STATE_CHARGE;
                        if (RealOwner) projectile.netUpdate = true;
                    }
                    //else stay in idle until target found
                }
            }
            else if (AI_STATE == STATE_RECOIL)
            {
                if (Counter > 60)
                {
                    Counter = 0;
                    AI_STATE = STATE_COOLDOWN;
                }
            }
            #endregion

            Counter += Main.rand.Next(1, AI_STATE != STATE_IDLE ? 2 : 3);

            if (AI_STATE == STATE_CHARGE)
            {
                int targetIndex = FindClosestHorizontalTarget();

                projectile.spriteDirection = projectile.direction = -Direction;

                if (Counter <= ChargeDelay)
                {
                    //if lose target
                    if (RealOwner && targetIndex == -1)
                    {
                        //Main.NewText("Change from charge to idle cuz no target");
                        AI_STATE = STATE_IDLE;
                        projectile.netUpdate = true;
                    }

                    float ratio = Counter / (float)ChargeDelay;
                    float otherRatio = 1f - ratio;

                    //make sound
                    if (projectile.soundDelay <= 0)
                    {
                        projectile.soundDelay = 20;
                        Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.Item15.Style, 0.7f + ratio * 0.5f, -0.1f + ratio * 0.4f);
                        //Main.PlaySound(SoundID.Item15.WithVolume(0.7f + (Counter / (float)ChargeDelay) * 0.5f), projectile.position);
                        //Main.NewText("volume : " + (0.7f + volumeCounter * 0.1f));
                    }

                    //spawn dust
                    for (int i = 0; i < 3; i++)
                    {
                        if (Counter <= ChargeDelay && Main.rand.NextFloat() < ratio)
                        {
                            int dustType = 60;
                            //if facing left: + Direction * 48
                            int height = (int)(9 * otherRatio) + 4;
                            Rectangle rect = new Rectangle((int)BarrelPos.X + Direction * (Direction == 1 ? 16 : 48), (int)BarrelPos.Y - height, 32, 2 * height);
                            Dust d = Dust.NewDustDirect(rect.TopLeft(), rect.Width, rect.Height, dustType);
                            d.noGravity = true;
                            d.velocity.X *= 0.75f;
                            d.velocity.Y *= (d.position.Y > rect.Center().Y).ToDirectionInt(); //y velocity goes "inwards"
                            d.velocity *= 3 * otherRatio;
                        }
                    }
                }
                else
                {
                    if (RealOwner)
                    {
                        Counter = 0;
                        if (targetIndex != -1 && !Collision.SolidCollision(BarrelPos, 1, 1))
                        {
                            Vector2 velocity = Main.npc[targetIndex].Center - BarrelPos;
                            velocity.Normalize();
                            velocity *= 10f;
                            projectile.velocity += -velocity * 0.75f; //recoil
                            Projectile.NewProjectile(BarrelPos, velocity, mod.ProjectileType<HeavyLaserDroneLaser>(), CustomDmg, CustomKB, Main.myPlayer, 0f, 0f);

                            AI_STATE = STATE_RECOIL;
                            projectile.netUpdate = true;
                        }
                        if (targetIndex == -1)
                        {
                            //Main.NewText("Change from charge to idle");
                            AI_STATE = STATE_IDLE;
                            projectile.netUpdate = true;
                        }
                    }
                }
            }
        }

        private int FindClosestHorizontalTarget()
        {
            Player player = projectile.GetOwner();
            int targetIndex = -1;
            float distanceFromTarget = 100000f;
            Vector2 targetCenter = player.Center;
            float margin = 200;
            int range = 1000;
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.active && npc.CanBeChasedBy(projectile))
                {
                    float between = Vector2.Distance(npc.Center, player.Center);
                    if (((between < range &&
                        Vector2.Distance(player.Center, targetCenter) > between && between < distanceFromTarget) || targetIndex == -1) &&
                         AssAI.CheckLineOfSight(player.Center, npc.Center))
                    {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        targetIndex = k;
                    }
                }
            }
            Direction = (targetCenter.X - player.Center.X > 0f).ToDirectionInt();
            float betweenY = targetCenter.Y - player.Top.Y; //bigger margin upwards
            //Main.NewText("betweenY: " + betweenY);
            return (Math.Abs(betweenY) < margin && distanceFromTarget < range) ? targetIndex : -1;
        }

        /// <summary>
        /// Called before switching to STATE_CHARGE. Returns the minionPos for the laser charge
        /// </summary>
        private int GetChargePosition()
        {
            int pos = 0;
            int min = 1000;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == projectile.owner && proj.type == projectile.type)
                {
                    HeavyLaserDrone h = (HeavyLaserDrone)proj.modProjectile;
                    if (h.AI_STATE == STATE_CHARGE)
                    {
                        byte projPos = h.PosInCharge;
                        min = Math.Min(min, projPos);
                        if (projPos > pos) pos = projPos;
                    }
                    //also works on itself but since AI_STATE is only switched to STATE_CHARGE AFTER this gets called it doesn't have an effect
                }
            }
            if (min > 0) return 0;

            return pos + 1;
        }
    }
}
