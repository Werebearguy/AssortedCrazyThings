using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.Projectiles.Minions.Drones;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class PetGolemHeadProj : DroneBase
    {
        public const int AttackDelay = 60;

        private const int FireballDamage = 20;

        public override bool IsCombatDrone
        {
            get
            {
                return false;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Replica Golem Head");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
            drawOriginOffsetY = -10;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            projectile.aiStyle = -1;
            projectile.width = 38;
            projectile.height = 38;
            projectile.tileCollide = false;
        }

        protected override void CheckActive()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.PetGolemHead = false;
            }
            if (modPlayer.PetGolemHead)
            {
                projectile.timeLeft = 2;
            }
        }

        protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {
            if (Counter > AttackDelay)
            {
                if (Counter < (int)(AttackDelay * 1.5f))
                {
                    projectile.frame = 1;
                }
                else
                {
                    projectile.frame = 0;
                }
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

            Sincounter = Sincounter > 360 ? 0 : Sincounter + 1;
            sinY = (float)((Math.Sin((Sincounter / 180f) * MathHelper.TwoPi) - 1) * 4);

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2 + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;

            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin, 1f, effects, 0f);
            spriteBatch.Draw(mod.GetTexture("Projectiles/Pets/" + Name + "_Glowmask"), drawPos, bounds, Color.White, projectile.rotation, drawOrigin, 1f, effects, 0f);

            return false;
        }

        protected override bool Bobbing()
        {
            return false;
        }

        protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
        {
            AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, staticDirection: true, vanityPet: true, veloSpeed: 0.5f, offsetX: -30f, offsetY: -100f);
            return false;
        }

        protected override void CustomAI()
        {
            projectile.rotation = 0f;

            Counter++;
            if (Counter % AttackDelay == 0)
            {
                if (Main.myPlayer == projectile.owner)
                {
                    int targetIndex = AssAI.FindTarget(projectile, projectile.Center, 600);
                    if (targetIndex != -1 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                    {
                        if (Counter == AttackDelay) Counter += AttackDelay;
                        Vector2 position = projectile.Center;
                        position.Y += 6f;
                        Vector2 velocity = Main.npc[targetIndex].Center + Main.npc[targetIndex].velocity * 5f - position;
                        velocity.Normalize();
                        velocity *= 7f;
                        Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<PetGolemHeadFireball>(), FireballDamage, 2f, Main.myPlayer, 0f, 0f);
                        projectile.netUpdate = true;
                    }
                    else
                    {
                        if (Counter > AttackDelay)
                        {
                            Counter -= AttackDelay;
                            projectile.netUpdate = true;
                        }
                    }
                }
                Counter -= AttackDelay;
            }
        }
    }
}
