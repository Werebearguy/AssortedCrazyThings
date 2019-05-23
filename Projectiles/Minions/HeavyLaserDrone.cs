using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssortedCrazyThings.Base;
using System;
using Terraria;
using Terraria.ID;
using System.IO;

namespace AssortedCrazyThings.Projectiles.Minions
{
    /// <summary>
    /// Fires a penetrating laser beam horizontally to the player with a very long delay
    /// </summary>
    public class HeavyLaserDrone : CombatDroneBase
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/HealingDroneProj";
            }
        }

        private static readonly string nameGlow = "Projectiles/Pets/" + "HealingDroneProj_Glowmask";
        private static readonly string nameLower = "Projectiles/Pets/" + "HealingDroneProj_Lower";
        private static readonly string nameLowerGlow = "Projectiles/Pets/" + "HealingDroneProj_Lower_Glowmask";

        private const int AttackCooldown = 180;
        private const int SearchDelay = 60;
        private const int ChargeDelay = 120;

        private const int LaserDamage = 200;

        private const byte STATE_COOLDOWN = 0;
        private const byte STATE_IDLE = 1;
        private const byte STATE_CHARGE = 2;

        private byte AI_STATE = 0;

        public int Counter
        {
            get
            {
                return (int)projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heavy Laser Drone");
            Main.projFrames[projectile.type] = 6;
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
            projectile.minionSlots = 1f; //2f
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            //AssUtils.Print("send netupdate " + PickedTexture + " " + ShootTimer);
            writer.Write((byte)AI_STATE);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AI_STATE = reader.ReadByte();
            //AssUtils.Print("recv netupdate " + PickedTexture + " " + ShootTimer);
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

        protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
        {
            if (AI_STATE == STATE_CHARGE)
            {
                //offset x = 30 when facing right
                staticDirection = true;
                offsetX = (Main.player[projectile.owner].direction == 1? -80: 20);
                offsetY = 10;
            }
            Main.NewText("offset: " + offsetX + " : " + offsetY);
            return true;
        }

        protected override void CustomAI()
        {
            Player player = Main.player[projectile.owner];
            Main.NewText("State: " + AI_STATE);
            Main.NewText("Counter: " + Counter);

            #region Handle State
            if (AI_STATE == STATE_COOLDOWN)
            {
                if (Counter > AttackCooldown)
                {
                    Counter = 0;
                    AI_STATE = STATE_IDLE;
                    projectile.netUpdate = true;
                }
                //else stay in cooldown and wait for counter to reach 
            }
            else if (AI_STATE == STATE_IDLE)
            {
                if (Counter > SearchDelay)
                {
                    Counter = 0;
                    int targetIndex = AssAI.FindTarget(projectile, projectile.Center, range: 600f);
                    if (targetIndex != -1)
                    {
                        AI_STATE = STATE_CHARGE;
                        projectile.netUpdate = true;
                    }
                    //else stay in idle until target found
                }
            }
            //else if (AI_STATE == STATE_CHARGE)
            //{
            //    //other code handles transition to STATE_COOLDOWN
            //}
            #endregion

            Counter++;

            if (AI_STATE == STATE_CHARGE)
            {
                Sincounter = 0;
                int targetIndex = AssAI.FindTarget(projectile, projectile.Center, range: 600f);

                projectile.spriteDirection = projectile.direction = -player.direction;

                if (Counter <= ChargeDelay)
                {
                    if (projectile.soundDelay <= 0)
                    {
                        projectile.soundDelay = 10;
                        projectile.soundDelay *= 2;
                        Main.PlaySound(SoundID.Item15.WithVolume(0.7f + (Counter / (float)ChargeDelay) * 0.5f), projectile.position);
                        //Main.NewText("volume : " + (0.7f + volumeCounter * 0.1f));
                    }
                }
                else
                {
                    if (Main.myPlayer == projectile.owner)
                    {
                        Counter = 0;
                        if (targetIndex != -1 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                        {
                            Vector2 position = projectile.Center;
                            position.Y += -6f + sinY;
                            Vector2 velocity = Main.npc[targetIndex].Center - position;
                            velocity.Normalize();
                            velocity *= 6f;
                            Projectile.NewProjectile(position, velocity, mod.ProjectileType<HeavyLaserDroneLaser>(), LaserDamage, 2f, Main.myPlayer, 0f, 0f);
                            AI_STATE = STATE_COOLDOWN;
                            projectile.netUpdate = true;
                        }
                        if (targetIndex == -1)
                        {
                            AI_STATE = STATE_IDLE;
                            projectile.netUpdate = true;
                        }
                    }
                }
            }
        }

        protected override void CustomDraw(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {
            //frame 0, 1: above two thirds health
            //frame 2, 3: above half health, below two thirds health
            //frame 4, 5: below half health, healing
            Player player = Main.player[projectile.owner];

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

        protected override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            AssPlayer modPlayer = player.GetModPlayer<AssPlayer>(mod);
            if (player.dead)
            {
                modPlayer.droneControllerMinion = false;
            }
            if (modPlayer.droneControllerMinion)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
