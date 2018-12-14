using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class MiniMegalodon : ModProjectile
    {
        public bool flyForever = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Megalodon");
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.EyeSpring);
            projectile.aiStyle = -1;
            //aiType = ProjectileID.EyeSpring;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.eyeSpring = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            MegalodonAI();
        }

        private void MegalodonAI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
            if (!player.active)
            {
                projectile.active = false;
            }
            else
            {
                bool flag = false;
                bool flag2 = false;
                bool flag3 = false;
                bool flag4 = false;
                int num = 85;
				if (player.dead)
				{
					modPlayer.MiniMegalodon = false;
				}
				if (modPlayer.MiniMegalodon)
				{
					projectile.timeLeft = 2;
				}
                if (player.position.X + (float)(player.width / 2) < projectile.position.X + (float)(projectile.width / 2) - (float)num)
                {
                    flag = true;
                }
                else if (player.position.X + (float)(player.width / 2) > projectile.position.X + (float)(projectile.width / 2) + (float)num)
                {
                    flag2 = true;
                }
				if (projectile.ai[1] == 0f)
				{
					int num38 = 500;
					if (player.rocketDelay2 > 0)
					{
						projectile.ai[0] = 1f;
					}
					Vector2 vector6 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num39 = player.position.X + (float)(player.width / 2) - vector6.X;
					float num40 = player.position.Y + (float)(player.height / 2) - vector6.Y;
					float num41 = (float)Math.Sqrt((double)(num39 * num39 + num40 * num40));
					if (num41 > 2000f)
					{
						projectile.position.X = player.position.X + (float)(player.width / 2) - (float)(projectile.width / 2);
						projectile.position.Y = player.position.Y + (float)(player.height / 2) - (float)(projectile.height / 2);
					}
					else if (num41 > (float)num38 || (Math.Abs(num40) > 300f) || flyForever)
					{
						projectile.ai[0] = 1f;
					}
				}
				if (projectile.ai[0] != 0f)
				{
					float num42 = 0.2f;
					int num43 = 200;
                    projectile.tileCollide = false;
					Vector2 vector7 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num44 = player.position.X + (float)(player.width / 2) - vector7.X;
					float num51 = player.position.Y + (float)(player.height / 2) - vector7.Y;
					float num52 = (float)Math.Sqrt((double)(num44 * num44 + num51 * num51));
					float num53 = 10f;
					float num54 = num52;
					if (num52 < (float)num43 && player.velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= player.position.Y + (float)player.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
					{
						projectile.ai[0] = 0f;
						if (projectile.velocity.Y < -6f)
						{
							projectile.velocity.Y = -6f;
						}
					}
					if (num52 < 60f)
					{
						num44 = projectile.velocity.X;
						num51 = projectile.velocity.Y;
					}
					else
					{
						num52 = num53 / num52;
						num44 *= num52;
						num51 *= num52;
					}
					if (projectile.velocity.X < num44)
					{
						projectile.velocity.X += num42;
						if (projectile.velocity.X < 0f)
						{
							projectile.velocity.X += num42 * 1.5f;
						}
					}
					if (projectile.velocity.X > num44)
					{
						projectile.velocity.X -= num42;
						if (projectile.velocity.X > 0f)
						{
							projectile.velocity.X -= num42 * 1.5f;
						}
					}
					if (projectile.velocity.Y < num51)
					{
						projectile.velocity.Y += num42;
						if (projectile.velocity.Y < 0f)
						{
							projectile.velocity.Y += num42 * 1.5f;
						}
					}
					if (projectile.velocity.Y > num51)
					{
						projectile.velocity.Y -= num42;
						if (projectile.velocity.Y > 0f)
						{
							projectile.velocity.Y -= num42 * 1.5f;
						}
					}
					if ((double)projectile.velocity.X > 0.5)
					{
						projectile.spriteDirection = -1;
					}
					else if ((double)projectile.velocity.X < -0.5)
					{
						projectile.spriteDirection = 1;
					}
					projectile.frameCounter++;
					if (projectile.frameCounter > 4)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 6 || projectile.frame > 7)
					{
						projectile.frame = 6;
					}
					projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.58f;
				}
				else
				{
					Vector2 vector9 = Vector2.Zero;
					if (projectile.ai[1] != 0f)
					{
						flag = false;
						flag2 = false;
					}
					projectile.rotation = 0f;
					projectile.tileCollide = true;
					float num111 = 8f;
					float num110 = 0.4f;
					if (flag)
					{
						if ((double)projectile.velocity.X > -3.5)
						{
							projectile.velocity.X -= num110;
						}
						else
						{
							projectile.velocity.X -= num110 * 0.25f;
						}
					}
					else if (flag2)
					{
						if ((double)projectile.velocity.X < 3.5)
						{
							projectile.velocity.X += num110;
						}
						else
						{
							projectile.velocity.X += num110 * 0.25f;
						}
					}
					else
					{
						projectile.velocity.X *= 0.9f;
						if (projectile.velocity.X >= 0f - num110 && projectile.velocity.X <= num110)
						{
							projectile.velocity.X = 0f;
						}
					}
					if (flag | flag2)
					{
						int num112 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
						int j = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
						if (flag)
						{
							int num30 = num112;
							num112 = num30 - 1;
						}
						if (flag2)
						{
							int num30 = num112;
							num112 = num30 + 1;
						}
						num112 += (int)projectile.velocity.X;
						if (WorldGen.SolidTile(num112, j))
						{
							flag4 = true;
						}
					}
					if (player.position.Y + (float)player.height - 8f > projectile.position.Y + (float)projectile.height)
					{
						flag3 = true;
					}
					if (projectile.frameCounter < 10)
					{
						flag4 = false;
					}
                    //projectile.stepSpeed = 1f;
                    //projectile.gfxOffY = 0f;
                    //Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref stepSpeed, ref gfxOffY);
                    Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
					if (projectile.velocity.Y == 0f)
					{
						if (!flag3 && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
						{
							int num113 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
							int j2 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16 + 1;
							if (flag)
							{
								int num30 = num113;
								num113 = num30 - 1;
							}
							if (flag2)
							{
								int num30 = num113;
								num113 = num30 + 1;
							}
							WorldGen.SolidTile(num113, j2);
						}
						if (flag4)
						{
							int num114 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
							int num115 = (int)(projectile.position.Y + (float)projectile.height) / 16 + 1;
							if (WorldGen.SolidTile(num114, num115) || Main.tile[num114, num115].halfBrick() || Main.tile[num114, num115].slope() > 0)
							{
								try
								{
									num114 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
									num115 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
									if (flag)
									{
										int num30 = num114;
										num114 = num30 - 1;
									}
									if (flag2)
									{
										int num30 = num114;
										num114 = num30 + 1;
									}
									num114 += (int)projectile.velocity.X;
									if (!WorldGen.SolidTile(num114, num115 - 1) && !WorldGen.SolidTile(num114, num115 - 2))
									{
										projectile.velocity.Y = -5.1f;
									}
									else if (!WorldGen.SolidTile(num114, num115 - 2))
									{
										projectile.velocity.Y = -7.1f;
									}
									else if (WorldGen.SolidTile(num114, num115 - 5))
									{
										projectile.velocity.Y = -11.1f;
									}
									else if (WorldGen.SolidTile(num114, num115 - 4))
									{
										projectile.velocity.Y = -10.1f;
									}
									else
									{
										projectile.velocity.Y = -9.1f;
									}
								}
								catch
								{
									projectile.velocity.Y = -9.1f;
								}
							}
						}
					}
					if (projectile.velocity.X > num111)
					{
						projectile.velocity.X = num111;
					}
					if (projectile.velocity.X < 0f - num111)
					{
						projectile.velocity.X = 0f - num111;
					}
					if (projectile.velocity.X < 0f)
					{
                        projectile.direction = -1;
					}
					if (projectile.velocity.X > 0f)
					{
                        projectile.direction = 1;
					}
					if (projectile.velocity.X > num110 && flag2)
					{
                        projectile.direction = 1;
					}
					if (projectile.velocity.X < 0f - num110 && flag)
					{
                        projectile.direction = -1;
					}
					if (projectile.direction == -1)
					{
						projectile.spriteDirection = 1;
					}
					if (projectile.direction == 1)
					{
						projectile.spriteDirection = -1;
					}
					
					if (projectile.velocity.Y == 0f)
					{
						if (projectile.frame > 5)
						{
							projectile.frameCounter = 0;
						}
						if (projectile.velocity.X == 0f)
						{
							int num116 = 3;
							projectile.frameCounter++;
							if (projectile.frameCounter < num116)
							{
								projectile.frame = 0;
							}
							else if (projectile.frameCounter < num116 * 2)
							{
								projectile.frame = 1;
							}
							else if (projectile.frameCounter < num116 * 3)
							{
								projectile.frame = 2;
							}
							else if (projectile.frameCounter < num116 * 4)
							{
								projectile.frame = 3;
							}
							else
							{
								projectile.frameCounter = num116 * 4;
							}
						}
						else
						{
							projectile.velocity.X *= 0.8f;
							projectile.frameCounter++;
							int num117 = 3;
							if (projectile.frameCounter < num117)
							{
								projectile.frame = 0;
							}
							else if (projectile.frameCounter < num117 * 2)
							{
								projectile.frame = 1;
							}
							else if (projectile.frameCounter < num117 * 3)
							{
								projectile.frame = 2;
							}
							else if (projectile.frameCounter < num117 * 4)
							{
								projectile.frame = 3;
							}
							else if (flag | flag2)
							{
								projectile.velocity.X *= 2f;
								projectile.frame = 4;
								projectile.velocity.Y = -6.1f;
								projectile.frameCounter = 0;
								int num30;
								for (int num118 = 0; num118 < 4; num118 = num30 + 1)
								{
									int num119 = 0/*Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + (float)projectile.height - 2f), projectile.width, 4, 5)*/;
									//Dust dust = Main.dust[num119];
									//dust.velocity += projectile.velocity;
									//dust = Main.dust[num119];
									//dust.velocity *= 0.4f;
									num30 = num118;
								}
							}
							else
							{
								projectile.frameCounter = num117 * 4;
							}
						}
					}
					else if (projectile.velocity.Y < 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 5;
					}
					else
					{
						projectile.frame = 4;
						projectile.frameCounter = 3;
					}
					projectile.velocity.Y += 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
            }
        }
    }
}
