using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
    /// <summary>
    /// Fires a penetrating laser beam horizontally to the player with a very long delay.
    /// Only recognizes enemies at around the y level of the player
    /// </summary>
    public class HeavyLaserDrone : DroneBase
    {
        private static readonly string nameGlow = "Projectiles/Minions/Drones/" + "HeavyLaserDrone_Glowmask";
        private static readonly string nameOverlay = "Projectiles/Minions/Drones/" + "HeavyLaserDrone_Overlay";

        private const int AttackCooldown = 180;
        private const int RecoilDuration = 60;
        private const int SearchDelay = 90; //60 but incremented 1.5f
        private const int ChargeDelay = 120;
        private const int AnimationDuration = 32;
        private const int AnimationFrameTime = 8;

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
                Vector2 position = projectile.Bottom;
                position.Y += sinY;
                return position;
            }
        }

        #region overlay
        private int ChargeTimer = 0;

        private bool CanOverlay => ChargeTimer >= AnimationDuration && (projectile.frame == 3 || projectile.frame == 7);

        private float OverlayOpacity => (ChargeTimer - AnimationDuration) / (float)byte.MaxValue;

        private bool playedOverheatSound = false;

        private SoundEffectInstance overheatSound = null;

        private void IncreaseCharge()
        {
            if (ChargeTimer < byte.MaxValue) ChargeTimer++;
        }

        private void DecreaseCharge()
        {
            if (ChargeTimer > 0) ChargeTimer--;
        }
        #endregion

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heavy Laser Drone");
            Main.projFrames[projectile.type] = 8;
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
            writer.Write((byte)ChargeTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            AI_STATE = reader.ReadByte();
            PosInCharge = reader.ReadByte();
            ChargeTimer = (int)reader.ReadByte();
        }

        protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {
            if (AI_STATE == STATE_CHARGE)
            {
                if (ChargeTimer < AnimationDuration)
                {
                    projectile.frame = ChargeTimer / AnimationFrameTime;
                }
                else
                {
                    projectile.frame = 3;
                }
            }
            else
            {
                if (ChargeTimer <= 0)
                {
                    projectile.frame = 0;
                }
                else if (ChargeTimer < AnimationDuration)
                {
                    projectile.frame = 4 + ChargeTimer / AnimationFrameTime;
                }
                else
                {
                    projectile.frame = 7;
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

            if (CanOverlay)
            {
                image = mod.GetTexture(nameOverlay);
                spriteBatch.Draw(image, drawPos, image.Bounds, Color.White * OverlayOpacity, projectile.rotation, drawOrigin, 1f, effects, 0f);
            }

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
                projectile.rotation = projectile.velocity.X * 0.05f;
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
        }

        protected override void CustomAI()
        {
            //Main.NewText("State: " + AI_STATE);
            //Main.NewText("Counter: " + Counter);
            //Main.NewText("Opacity: " + ChargeTimer);
            //Main.NewText("Color: " + OverlayColor);

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
                if (Counter > RecoilDuration)
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
                        //Counter = 0;
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
                        float volume = FixVolume(0.7f + ratio * 0.5f);
                        float pitch = -0.1f + ratio * 0.4f;
                        Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.Item15.Style, volume, pitch);
                        //Main.PlaySound(SoundID.Item15.WithVolume(0.7f + (Counter / (float)ChargeDelay) * 0.5f), projectile.position);
                        //Main.NewText("volume : " + (0.7f + volumeCounter * 0.1f));
                    }

                    //spawn dust
                    for (int i = 0; i < 3; i++)
                    {
                        if (Main.rand.NextFloat() < ratio)
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
                            Projectile.NewProjectile(BarrelPos, velocity, ModContent.ProjectileType<HeavyLaserDroneLaser>(), CustomDmg, CustomKB, Main.myPlayer, 0f, 0f);

                            AI_STATE = STATE_RECOIL;
                            ChargeTimer = byte.MaxValue;
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

            if (AI_STATE == STATE_CHARGE)
            {
                IncreaseCharge();
            }
            else
            {
                DecreaseCharge();

                if (CanOverlay && Main.rand.NextFloat() < OverlayOpacity * 0.5f)
                {
                    Gore gore = Gore.NewGorePerfect(BarrelPos + new Vector2(Direction == 1 ? -8f : -projectile.width - 4f, Direction == 1 ? -14f : -16f), Vector2.Zero, Main.rand.Next(61, 64));
                    gore.position.X += Main.rand.NextFloat(8);
                    gore.scale *= 0.18f;
                    gore.velocity *= 0.6f;
                }
            }

            if (AI_STATE == STATE_RECOIL)
            {
                if (overheatSound == null && !playedOverheatSound)
                {
                    float volume = FixVolume(1.5f);
                    overheatSound = Main.PlaySound(SoundID.Trackable, (int)projectile.position.X, (int)projectile.position.Y, 224, volume, 0.1f);
                    playedOverheatSound = true;
                }
            }
            else
            {
                playedOverheatSound = false;
            }

            if (overheatSound != null)
            {
                float f = 0.008f * Main.soundVolume;
                if (overheatSound.Volume > f)
                {
                    overheatSound.Volume -= f;
                    if (overheatSound.Volume < f)
                    {
                        overheatSound.Stop();
                    }
                }
                if (overheatSound.State == SoundState.Stopped)
                {
                    overheatSound = null;
                }
            }
        }

        public static float FixVolume(float volume)
        {
            return Main.soundVolume * volume > 1 ? Main.soundVolume / volume : volume;
        }

        private int FindClosestHorizontalTarget()
        {
            Player player = projectile.GetOwner();
            int targetIndex = -1;
            float distanceFromTarget = 100000f;
            Vector2 targetCenter = player.Center;
            float margin = 200;
            int range = 1000;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy(projectile))
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
            int min = Main.maxProjectiles;
            for (int i = 0; i < Main.maxProjectiles; i++)
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
