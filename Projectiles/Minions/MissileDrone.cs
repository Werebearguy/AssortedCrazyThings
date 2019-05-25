using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions
{
    /// <summary>
    /// Fires a salvo of homing rockets with a long delay
    /// </summary>
    public class MissileDrone : CombatDroneBase
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

        public const int AttackDelay = 180; //120 but incremented by 1.5f

        private const int MissileDamage = 20;

        public int AttackCounter
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
            DisplayName.SetDefault("Missile Drone");
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
            projectile.minionSlots = 2f;
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
            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin, 1f, effects, 0f);

            image = mod.GetTexture(nameLowerGlow);
            spriteBatch.Draw(image, drawPos, bounds, Color.White, projectile.rotation, drawOrigin, 1f, effects, 0f);

            return false;
        }

        protected override void CustomAI()
        {
            Player player = Main.player[projectile.owner];

            AttackCounter += Main.rand.Next(1, 3);
            if (AttackCounter % AttackDelay == 0 || AttackCounter % AttackDelay == 15 || AttackCounter % AttackDelay == 30)
            {
                if (Main.myPlayer == projectile.owner)
                {
                    int targetIndex = AssAI.FindTarget(projectile, projectile.Center, range: 600f);
                    if (targetIndex != -1 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                    {
                        if (Collision.CanHitLine(projectile.Center, 1, 1, projectile.Center + new Vector2(0, -16 * 8), 1, 1) &&
                            Collision.CanHitLine(projectile.Center + new Vector2(0, -16 * 8), 1, 1, projectile.Center + new Vector2(0, -16 * 8) + new Vector2(-16 * 5, -16 * 8), 1, 1) &&
                            Collision.CanHitLine(projectile.Center + new Vector2(0, -16 * 8), 1, 1, projectile.Center + new Vector2(0, -16 * 8) + new Vector2(16 * 5, -16 * 8), 1, 1))
                        {
                            if (AttackCounter == AttackDelay) AttackCounter += AttackDelay;
                            Vector2 position = projectile.Center;
                            position.Y -= 6f;
                            Projectile.NewProjectile(position, new Vector2(Main.rand.NextFloat(-2, 2), -5), mod.ProjectileType<MissileDroneRocket>(), MissileDamage, 2f, Main.myPlayer, 0f, 0f);
                            projectile.velocity.Y += 2f;
                            if (AttackCounter % AttackDelay == 0) projectile.netUpdate = true;
                        }
                    }
                    else
                    {
                        if (AttackCounter > AttackDelay)
                        {
                            AttackCounter -= AttackDelay;
                            projectile.netUpdate = true;
                        }
                    }
                }
                if (AttackCounter % AttackDelay == 0) AttackCounter -= AttackDelay;
            }
        }

        protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {
            //frame 0, 1: above two thirds health
            //frame 2, 3: above half health, below two thirds health
            //frame 4, 5: below half health, healing
            Player player = Main.player[projectile.owner];

            int frameOffset = 0; //frame 0, 1
            
            if (player.statLife < player.statLifeMax2 / 2) //frame 4, 5
            {
                frameOffset = 4;
            }
            else if (player.statLife < player.statLifeMax2 / 1.5f) //frame 2, 3
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
