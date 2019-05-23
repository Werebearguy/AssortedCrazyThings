using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.Projectiles.Minions;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class PetGolemHeadProj : CombatDroneBase
    {
        public int AttackCounter
        {
            get
            {
                return (int)projectile.ai[1];
            }
            set
            {
                projectile.ai[1] = value;
            }
        }

        public const int AttackDelay = 60;

        private const int FireballDamage = 20;

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
            sinY = (float)((Math.Sin((Sincounter / 180f) * 2 * Math.PI) - 1) * 4);

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2 + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;

            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin, 1f, effects, 0f);

            return false;
        }

        protected override void CustomDraw(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {
            if (AttackCounter > AttackDelay)
            {
                if (AttackCounter < (int)(AttackDelay * 1.5f))
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

        protected override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.PetGolemHead = false;
            }
            if (modPlayer.PetGolemHead)
            {
                projectile.timeLeft = 2;
            }
        }

        protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
        {
            return false;
        }

        protected override void CustomAI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, staticDirection: true, vanityPet: true, veloSpeed: 0.5f, offsetX: -30f, offsetY: -100f);
            
            projectile.rotation = 0f;

            AttackCounter++;
            if (AttackCounter % AttackDelay == 0)
            {
                if (Main.myPlayer == projectile.owner)
                {
                    int targetIndex = AssAI.FindTarget(projectile, projectile.Center, range: 600f);
                    if (targetIndex != -1 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                    {
                        if (AttackCounter == AttackDelay) AttackCounter += AttackDelay;
                        Vector2 position = projectile.Center;
                        position.Y += 6f;
                        Vector2 velocity = Main.npc[targetIndex].Center + Main.npc[targetIndex].velocity * 5f - position;
                        velocity.Normalize();
                        velocity *= 7f;
                        Projectile.NewProjectile(position, velocity, mod.ProjectileType<PetGolemHeadFireball>(), FireballDamage, 2f, Main.myPlayer, 0f, 0f);
                        projectile.netUpdate = true;
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
                AttackCounter -= AttackDelay;
            }
        }
    }
}
