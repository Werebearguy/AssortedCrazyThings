using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
    /// <summary>
    /// Fires a salvo of homing rockets with a long delay
    /// </summary>
    public class MissileDrone : DroneBase
    {
        private static readonly string nameGlow = "Projectiles/Minions/Drones/" + "MissileDrone_Glowmask";

        public const int AttackCooldown = 360; //120 but incremented by 1.5f
        public const int AttackDelay = 60;
        public const int AttackDuration = 60;

        //in cooldown: lamps turn on in order for "charge up"

        private const byte STATE_COOLDOWN = 0;
        private const byte STATE_IDLE = 1;
        private const byte STATE_FIRING = 2;

        private byte AI_STATE = 0;
        private int RocketNumber = 0;

        private Vector2 ShootOrigin
        {
            get
            {
                Vector2 position = projectile.Top;
                position.Y += sinY + 2f;
                return position;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Missile Drone");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
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

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            writer.Write((byte)AI_STATE);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            AI_STATE = reader.ReadByte();
        }

        protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {
            if (AI_STATE == STATE_FIRING)
            {
                if (RocketNumber > 0)
                {
                    projectile.frame = 3;
                }
                else
                {
                    projectile.frame = 2;
                }
            }
            else if (AI_STATE == STATE_COOLDOWN)
            {
                projectile.frame = 1;
            }
            else
            {
                projectile.frame = 0;
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

        protected override void CustomAI()
        {
            Player player = projectile.GetOwner();
            //Main.NewText("##");
            //Main.NewText(AI_STATE);

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
                if (Counter > AttackDelay)
                {
                    Counter = 0;
                    int targetIndex = AssAI.FindTarget(projectile, projectile.Center, range: 900, useSlowLOS: true);
                    if (targetIndex != -1)
                    {
                        Vector2 aboveCheck = new Vector2(0, -16 * 8);
                        if (Collision.CanHitLine(ShootOrigin, 1, 1, ShootOrigin + aboveCheck, 1, 1) &&
                            Collision.CanHitLine(ShootOrigin + aboveCheck, 1, 1, ShootOrigin + aboveCheck + new Vector2(-16 * 5, 0), 1, 1) &&
                            Collision.CanHitLine(ShootOrigin + aboveCheck, 1, 1, ShootOrigin + aboveCheck + new Vector2(16 * 5, 0), 1, 1))
                        {
                            //Main.NewText("Change from idle to firing");
                            AI_STATE = STATE_FIRING;
                            if (RealOwner) projectile.netUpdate = true;
                        }
                    }
                    //else stay in idle until target found
                }
            }
            else if (AI_STATE == STATE_FIRING)
            {
                if (Counter > AttackDuration)
                {
                    Counter = 0;
                    RocketNumber = 0;
                    AI_STATE = STATE_COOLDOWN;
                    //no sync since this counts down automatically after STATE_FIRING
                }
            }
            #endregion

            if (AI_STATE == STATE_FIRING)
            {
                int targetIndex = AssAI.FindTarget(projectile, projectile.Center, 900, useSlowLOS: true);

                if (targetIndex != -1)
                {
                    projectile.direction = projectile.spriteDirection = -(Main.npc[targetIndex].Center.X - player.Center.X > 0f).ToDirectionInt();
                    if (RealOwner)
                    {
                        int firerate = AttackDelay / 4;
                        RocketNumber = Counter / firerate;
                        if (Counter % firerate == 0 && RocketNumber > 0 && RocketNumber < 4)
                        {
                            if (!Collision.SolidCollision(ShootOrigin, 1, 1))
                            {
                                //Main.NewText(Counter);
                                Vector2 velocity = new Vector2(Main.rand.NextFloat(-1f, 1f) - projectile.direction * 0.5f, -5);
                                Projectile.NewProjectile(ShootOrigin, velocity, ModContent.ProjectileType<MissileDroneRocket>(), CustomDmg, CustomKB, Main.myPlayer);
                                projectile.velocity.Y += 2f;
                                projectile.netUpdate = true;
                            }
                        }
                    }
                }
            }

            Counter += Main.rand.Next(1, AI_STATE != STATE_IDLE ? 2 : 3);
        }
    }
}
