using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
    public class MissileDroneRocket : ModProjectile
    {
        private bool justCollided = false;
        private bool inflatedHitbox = false;
        private const int inflationAmount = 80;

        public override string Texture
        {
            get
            {
                return "Terraria/Projectile_" + ProjectileID.RocketIII;
            }
        }
        public int FirstTarget
        {
            get
            {
                return (int)projectile.localAI[1] - 1;
            }
            set
            {
                projectile.localAI[1] = value + 1;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Missile Drone Rocket");
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
			//projectile.CloneDefaults(ProjectileID.RocketIII);
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 240;

            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            projectile.position.X = projectile.position.X + projectile.width / 2;
            projectile.position.Y = projectile.position.Y + projectile.height / 2;
            projectile.width = inflationAmount;
            projectile.height = inflationAmount;
            projectile.position.X = projectile.position.X - projectile.width / 2;
            projectile.position.Y = projectile.position.Y - projectile.height / 2;
            for (int i = 0; i < 20; i++) //40
            {
                Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 2f)];
                dust.velocity *= 2f; //3f
                if (Main.rand.NextBool(2))
                {
                    dust.scale = 0.5f;
                    dust.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int i = 0; i < 35; i++) //70
            {
                Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default(Color), 3f)];
                dust.noGravity = true;
                dust.velocity *= 4f; //5f
                dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default(Color), 2f)];
                dust.velocity *= 2f;
            }
            for (int i = 0; i < 2; i++) //3
            {
                float scaleFactor10 = 0.33f;
                if (i == 1)
                {
                    scaleFactor10 = 0.66f;
                }
                if (i == 2)
                {
                    scaleFactor10 = 1f;
                }
                Gore gore = Main.gore[Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
                gore.velocity *= scaleFactor10;
                gore.velocity.X += 1f;
                gore.velocity.Y += 1f;
                gore = Main.gore[Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
                gore.velocity *= scaleFactor10;
                gore.velocity.X += - 1f;
                gore.velocity.Y += 1f;
                gore = Main.gore[Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
                gore.velocity *= scaleFactor10;
                gore.velocity.X += 1f;
                gore.velocity.Y += - 1f;
                gore = Main.gore[Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
                gore.velocity *= scaleFactor10;
                gore.velocity.X += - 1f;
                gore.velocity.Y += - 1f;
            }
            projectile.position.X = projectile.position.X + projectile.width / 2;
            projectile.position.Y = projectile.position.Y + projectile.height / 2;
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - projectile.width / 2;
            projectile.position.Y = projectile.position.Y - projectile.height / 2;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            if (!inflatedHitbox) justCollided = true;
            return false;
        }

        //kind of a useless hack but I couldn't work out how vanilla makes the hitbox increase on tile collide
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            if (justCollided)
            {
                justCollided = false;
                inflatedHitbox = true;
                hitbox.Inflate(inflationAmount / 2, inflationAmount / 2);
                projectile.timeLeft = 3;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.timeLeft > 3)
            {
                projectile.timeLeft = 3;
            }
            projectile.direction = (target.Center.X < projectile.Center.X).ToDirectionInt();
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0)
            {
                Main.PlaySound(SoundID.Item66, projectile.Center); //62, 66, 82, 88
                projectile.localAI[0]++;
            }
            if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3)
            {
                projectile.tileCollide = false;
                projectile.ai[1] = 0f;
                projectile.alpha = 255;

                projectile.position.X = projectile.position.X + projectile.width / 2;
                projectile.position.Y = projectile.position.Y + projectile.height / 2;
                projectile.width = inflationAmount;
                projectile.height = inflationAmount;
                projectile.position.X = projectile.position.X - projectile.width / 2;
                projectile.position.Y = projectile.position.Y - projectile.height / 2;
                projectile.knockBack = 8f;
            }
            else
            {
                //8f
                if (projectile.ai[0] > 60 || projectile.localAI[0] < 11)
                {
                    projectile.localAI[0]++;
                    for (int i = 0; i < 2; i++)
                    {
                        float xOff = 0f;
                        float yOff = 0f;
                        if (i == 1)
                        {
                            xOff = projectile.velocity.X * 0.5f;
                            yOff = projectile.velocity.Y * 0.5f;
                        }
                        Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + 3f + xOff, projectile.position.Y + 3f + yOff) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, 6, 0f, 0f, 100, default(Color), 1f)];
                        dust.scale *= 2f + (float)Main.rand.Next(10) * 0.1f;
                        dust.velocity *= 0.2f;
                        dust.noGravity = true;
                        dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + 3f + xOff, projectile.position.Y + 3f + yOff) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, 31, 0f, 0f, 100, default(Color), 0.5f)];
                        dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                        dust.velocity *= 0.05f;
                    }
                }

                #region Find Target
                int targetIndex;
                if (projectile.ai[0] > 60)
                {
                    targetIndex = AssAI.FindTarget(projectile, projectile.Center, range: 1200f, ignoreTiles: true);
                    if (targetIndex != -1)
                    {
                        if (FirstTarget == -1) FirstTarget = targetIndex;
                        //only home in on the first target it finds
                        //(assuming during the rocket longevity there won't be another NPC spawning in distance with the same index)
                        if (FirstTarget == targetIndex)
                        {
                            Vector2 velocity = Main.npc[targetIndex].Center + Main.npc[targetIndex].velocity * 5f - projectile.Center;
                            velocity.Normalize();
                            velocity *= 6f;
                            //for that nice initial curving
                            //accel starts at 30, then goes down to 4
                            float accel = Utils.Clamp(-(projectile.ai[0] - 90), 4, 30);
                            projectile.velocity = (projectile.velocity * (accel - 1) + velocity) / accel;
                        }
                    }
                }
                else
                {
                    projectile.velocity.Y += 0.1f; //0.015f;
                }
                #endregion

                //speedup
                if (Math.Abs(projectile.velocity.X) < 15f && Math.Abs(projectile.velocity.Y) < 15f)
                {
                    if (projectile.ai[0] > 60)
                    {
                        projectile.velocity *= 1.1f;
                    }
                }
            }
            projectile.ai[0] += 1f;
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
        }
    }
}
