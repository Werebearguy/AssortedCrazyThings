using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions
{
    public class CompanionDungeonSoulMinion : ModProjectile
    {
        //change damage here, reminder that there are two minions so you are effectively doubling the damage
        public static int Damage = 13;
        private int sincounter;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Spazmamini);
            projectile.width = 14;
            projectile.height = 24;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.minion = true; //only determines the damage type
            projectile.minionSlots = 0.5f;
            projectile.penetrate = -1;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            return true;
        }


        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.immune[projectile.owner] == 10)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCImmunity[target.whoAmI] = 10;
                target.immune[projectile.owner] = 8; //0
                //immunity frame now 8 instead of 10 ticks long
            }
        }

        public void Draw()
        {
            if(projectile.ai[0] == 2f)
            {
                projectile.rotation = projectile.velocity.X * 0.05f;
            }
            else
            {
                //projectile.rotation = projectile.velocity.ToRotation() + 3.14159274f;
                //+= projectile.velocity.X * 0.05f; makes it rotate around itself faster depending on its velo.x
                projectile.rotation = projectile.velocity.X * -0.05f;
            }

            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame > 3)
            {
                projectile.frame = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sinY = -10f;
            if (Main.hasFocus)  //here since we override the AI, we can use the projectiles own frame and frameCounter in Draw()
            {
                Draw();
                sincounter = sincounter > 120 ? 0 : sincounter + 1;
                sinY = (float)((Math.Sin((sincounter / 120f) * 2 * Math.PI) - 1) * 10);
            }

            lightColor = projectile.GetAlpha(lightColor) * 0.99f; //1f is opaque
            lightColor.R = Math.Max(lightColor.R, (byte)200); //100 for dark
            lightColor.G = Math.Max(lightColor.G, (byte)200);
            lightColor.B = Math.Max(lightColor.B, (byte)200);

            Lighting.AddLight(projectile.Center, new Vector3(0.15f, 0.15f, 0.35f));

            SpriteEffects effects = SpriteEffects.None;
            Texture2D image = Main.projectileTexture[projectile.type];
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = projectile.frame,
                Width = image.Bounds.Width,
                Height = (image.Bounds.Height / 4)
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

            //Generate visual dust
            if (Main.rand.NextFloat() < 0.015f)
            {
                Vector2 position = new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y);
                Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-1.5f, -1f)), 100, new Color(255, 255, 255), 1f);
                dust.noGravity = false;
                dust.noLight = true;
                dust.fadeIn = Main.rand.NextFloat(0.8f, 1.1f);
            }

            Vector2 stupidOffset = new Vector2(projectile.width / 2, (projectile.height - 10f) + sinY);

            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);
        }

        public override void AI()
        {
            //projectile.ai[0] == 0 : no targets found
            //projectile.ai[0] == 1 : noclipping to player
            //projectile.ai[0] == 2 : target found, attacking

            Player player = Main.player[projectile.owner];
            AssPlayer modPlayer = player.GetModPlayer<AssPlayer>(mod);
            if (player.dead)
            {
                modPlayer.soulMinion = false;
            }
            if (modPlayer.soulMinion)
            {
                projectile.timeLeft = 2;
            }

            if (projectile.localAI[0] < 60)
            {
                Vector2 position = new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 2);
                for (int i = 0; i < 1; i++)
                {
                    if (Main.rand.NextFloat() < (60 - projectile.localAI[0])/360f)
                    {
                        Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-1.5f, -1f)), 100, new Color(255, 255, 255), (60 - projectile.localAI[0]) / 60f + 1f);
                        dust.noGravity = false;
                        dust.noLight = true;
                        dust.fadeIn = Main.rand.NextFloat(0.0f, 0.2f);
                    }
                }
                projectile.localAI[0]++;
            }

            float distance1 = 700f;
            float distance2playerfaraway = 800f;
            float distance2playerfarawayWhenHasTarget = 1200f;
            float num633 = 150f;
            float dashDelay = 40f; //time it stays in the "dashing" state after a dash, he dashes when he is in state 0 aswell

            float overlapVelo = 0.04f; //0.05
            for (int i = 0; i < 1000; i++)
            {
                //fix overlap with other minions
                if (((i != projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].owner == projectile.owner) & true) && Math.Abs(projectile.position.X - Main.projectile[i].position.X) + Math.Abs(projectile.position.Y - Main.projectile[i].position.Y) < (float)projectile.width)
                {
                    if (projectile.position.X < Main.projectile[i].position.X)
                    {
                        projectile.velocity.X = projectile.velocity.X - overlapVelo;
                    }
                    else
                    {
                        projectile.velocity.X = projectile.velocity.X + overlapVelo;
                    }
                    if (projectile.position.Y < Main.projectile[i].position.Y)
                    {
                        projectile.velocity.Y = projectile.velocity.Y - overlapVelo;
                    }
                    else
                    {
                        projectile.velocity.Y = projectile.velocity.Y + overlapVelo;
                    }
                }
            }
            bool flag23 = false;
            if (projectile.ai[0] == 2f) //attack mode

            {
                projectile.ai[1] += 1f;
                projectile.extraUpdates = 1;

                if (projectile.ai[1] > dashDelay) //40f
                {
                    projectile.ai[1] = 1f;
                    projectile.ai[0] = 0f;
                    projectile.extraUpdates = 0;
                    projectile.numUpdates = 0;
                    projectile.netUpdate = true;
                }
                else
                {
                    flag23 = true;
                }
            }

            if (!flag23)
            {
                Vector2 vector40 = projectile.position;
                bool foundTarget = false;
                if (projectile.ai[0] != 1f)
                {
                    projectile.tileCollide = false; //true
                }
                if (projectile.tileCollide && WorldGen.SolidTile(Framing.GetTileSafely((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16)))
                {
                    projectile.tileCollide = false;
                }

                //only target closest NPC if that NPC is some range (200f) maybe

                NPC ownerMinionAttackTargetNPC3 = projectile.OwnerMinionAttackTargetNPC;
                if (ownerMinionAttackTargetNPC3 != null && ownerMinionAttackTargetNPC3.CanBeChasedBy(this))
                {
                    float between = Vector2.Distance(ownerMinionAttackTargetNPC3.Center, projectile.Center);
                    if (((Vector2.Distance(projectile.Center, vector40) > between && between < distance1) || !foundTarget) && 
                        Collision.CanHitLine(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC3.position, ownerMinionAttackTargetNPC3.width, ownerMinionAttackTargetNPC3.height))
                    {
                        distance1 = between;
                        vector40 = ownerMinionAttackTargetNPC3.Center;
                        foundTarget = true;
                    }
                }
                if (!foundTarget)
                {
                    for (int j = 0; j < 200; j++)
                    {
                        NPC nPC2 = Main.npc[j];
                        if (nPC2.CanBeChasedBy(this))
                        {
                            float between = Vector2.Distance(nPC2.Center, projectile.Center);
                            if (((Vector2.Distance(projectile.Center, vector40) > between && between < distance1) || !foundTarget) &&
                                //EITHER HE CAN SEE IT, OR THE TARGET IS (default case: 14) TILES AWAY BUT THE MINION IS INSIDE A TILE
                                //makes it so the soul can still attack if it dashed "through tiles"
                                (Collision.CanHitLine(projectile.position, projectile.width, projectile.height, nPC2.position, nPC2.width, nPC2.height) ||
                                (between < dashDelay * 5 && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))))
                            {
                                distance1 = between;
                                vector40 = nPC2.Center;
                                foundTarget = true;
                            }
                        }
                    }
                }
                float distanceNoclip = distance2playerfaraway;
                if (foundTarget)
                {
                    distanceNoclip = distance2playerfarawayWhenHasTarget;
                }
                if (Vector2.Distance(player.Center, projectile.Center) > distanceNoclip)
                {
                    projectile.ai[0] = 1f;
                    projectile.tileCollide = false; //true
                    projectile.netUpdate = true;
                }
                if (foundTarget && projectile.ai[0] == 0f)//idek
                {
                    Vector2 value16 = vector40 - projectile.Center;
                    float num646 = value16.Length();
                    value16.Normalize();
                    if (num646 > 20f) //200f //approach distance to enemy
                    {
                        //if its far away from it
                        //Main.NewText("first " + Main.time);
                        float scaleFactor2 = 6f; //8f
                        float acc1 = 16f; //41f
                        value16 *= scaleFactor2;
                        projectile.velocity = (projectile.velocity * (acc1 - 1) + value16) / acc1;
                    }
                    else //slowdown after a dash
                    {
                        //if its close to the enemy
                        //Main.NewText("second " + Main.time);
                        float scaleFactor3 = 8; //4f
                        float acc2 = 41; //41f
                        value16 *= 0f - scaleFactor3;
                        projectile.velocity = (projectile.velocity * (acc2 - 1) + value16) / acc2;
                    }
                }
                else //!(foundTarget && projectile.ai[0] == 0f)
                {
                    bool isNoclipping = false;
                    if (!isNoclipping)
                    {
                        isNoclipping = projectile.ai[0] == 1f;
                    }

                    float velocityFactor1 = 1f; //6f
                    if (isNoclipping)
                    {
                        velocityFactor1 = 12f; //15f
                    }
                    Vector2 center2 = projectile.Center;
                    Vector2 value17 = player.Center - center2 + new Vector2(0f, -60f);
                    float num649 = value17.Length();
                    if (num649 > 200f && velocityFactor1 < 8f) //8f
                    {
                        velocityFactor1 = 8f; //8f
                    }
                    if (num649 < num633 && isNoclipping && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                    {
                        projectile.ai[0] = 0f;
                        projectile.netUpdate = true;
                    }
                    if (num649 > 2000f)
                    {
                        projectile.position.X = player.Center.X - (projectile.width / 2);
                        projectile.position.Y = player.Center.Y - (projectile.height / 2);
                        projectile.netUpdate = true;
                    }
                    if (num649 > 70f) //the immediate range around the player (when it passively floats about)
                    {
                        value17.Normalize();
                        value17 *= velocityFactor1;
                        float acc3 = 100f; //41f
                        projectile.velocity = (projectile.velocity * (acc3 - 1) + value17) / acc3;
                    }
                    else if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                    {
                        projectile.velocity.X = -0.15f;
                        projectile.velocity.Y = -0.05f;
                    }
                }

                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] += Main.rand.Next(1, 4);
                }
                if (projectile.ai[1] > dashDelay)
                {
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
                if (projectile.ai[0] == 0f)
                {
                    if ((projectile.ai[1] == 0f & foundTarget) && distance1 < 30f) //500f //DASH HERE YEEEEEEE
                    {
                        projectile.ai[1] += 1f;
                        if (Main.myPlayer == projectile.owner)
                        {
                            //Main.NewText("dash " + Main.time);
                            projectile.ai[0] = 2f;
                            Vector2 value20 = vector40 - projectile.Center;
                            value20.Normalize();
                            projectile.velocity = value20 * 4f; //8f
                            projectile.netUpdate = true;
                        }
                    }
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = true;
            return false; //true
        }
    }
}