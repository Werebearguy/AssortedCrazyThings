using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class Abeemination : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abeemination");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            drawOffsetX = 0;
            drawOriginOffsetY = 4;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabySlime);
            projectile.aiStyle = -1; //26
            aiType = ProjectileID.BabySlime;
            projectile.alpha = 50;
            projectile.width = 38;
            projectile.height = 40;
        }

        public override bool PreAI()
        {
            //Player player = Main.player[projectile.owner];
            //player.slime = false; // Relic from aiType
            return true;
        }

        private void Draw()
        {
            if (projectile.ai[0] != 0)
            {
                if (projectile.velocity.X > 0.5f)
                {
                    projectile.spriteDirection = -1;
                }
                else if (projectile.velocity.X < -0.5f)
                {
                    projectile.spriteDirection = 1;
                }

                projectile.frameCounter++;
                if (projectile.frameCounter > 6)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame < 2 || projectile.frame > 5)
                {
                    projectile.frame = 2;
                }
                projectile.rotation = projectile.velocity.X * 0.1f;
            }
            else
            {
                if (projectile.direction == -1)
                {
                    projectile.spriteDirection = 1;
                }
                if (projectile.direction == 1)
                {
                    projectile.spriteDirection = -1;
                }

                if (projectile.velocity.Y >= 0f && projectile.velocity.Y <= 0.8f)
                {
                    if (projectile.velocity.X == 0f)
                    {
                        projectile.frameCounter++;
                    }
                    else
                    {
                        projectile.frameCounter += 3;
                    }
                }
                else
                {
                    projectile.frameCounter += 5;
                }
                if (projectile.frameCounter >= 20)
                {
                    projectile.frameCounter -= 20;
                    projectile.frame++;
                }
                if (projectile.frame > 1)
                {
                    projectile.frame = 0;
                }
                if (projectile.wet && Main.player[projectile.owner].position.Y + Main.player[projectile.owner].height < projectile.position.Y + projectile.height && projectile.localAI[0] == 0f)
                {
                    if (projectile.velocity.Y > -4f)
                    {
                        projectile.velocity.Y -= 0.2f;
                    }
                    if (projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y *= 0.95f;
                    }
                }
                else
                {
                    projectile.velocity.Y += 0.4f;
                }
                if (projectile.velocity.Y > 10f)
                {
                    projectile.velocity.Y = 10f;
                }
            }
        }

        public override void AI()
        {
            PetPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            if (Main.player[projectile.owner].dead)
            {
                modPlayer.Abeemination = false;
            }
            if (modPlayer.Abeemination)
            {
                projectile.timeLeft = 2;
            }

            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            //int num = 85;

            //num = 60 + 30 * minionPos;

            int num = 10;
            int num2 = 40 * (projectile.minionPos + 1) * Main.player[projectile.owner].direction;
            if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) < projectile.position.X + (float)(projectile.width / 2) - (float)num + (float)num2)
            {
                flag = true;
            }
            else if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) > projectile.position.X + (float)(projectile.width / 2) + (float)num + (float)num2)
            {
                flag2 = true;
            }

            if (projectile.ai[1] == 0f)
            {
                int num38 = 500;
                num38 += 40 * projectile.minionPos;
                if (projectile.localAI[0] > 0f)
                {
                    num38 += 600;
                }

                if (Main.player[projectile.owner].rocketDelay2 > 0)
                {
                    projectile.ai[0] = 1f;
                }

                Vector2 vector6 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num39 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector6.X;
                float num40 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector6.Y;
                float num41 = (float)Math.Sqrt((double)(num39 * num39 + num40 * num40));
                if (num41 > 2000f)
                {
                    projectile.position.X = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - (float)(projectile.width / 2);
                    projectile.position.Y = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - (float)(projectile.height / 2);
                }
                else if (num41 > (float)num38 || (Math.Abs(num40) > 300f && (projectile.localAI[0] <= 0f)))
                {
                    if (num40 > 0f && projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                    if (num40 < 0f && projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                    projectile.ai[0] = 1f;
                }
            }

            if (projectile.ai[0] != 0f)
            {
                float num42 = 0.2f;
                int num43 = 200;

                projectile.tileCollide = false;
                Vector2 vector7 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num44 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector7.X;

                num44 -= (float)(40 * Main.player[projectile.owner].direction);
                float num45 = 700f;

                bool flag6 = false;
                int num46 = -1;
                int num30;

                for (int num47 = 0; num47 < 200; num47 = num30 + 1)
                {
                    if (Main.npc[num47].CanBeChasedBy(this))
                    {
                        float num48 = Main.npc[num47].position.X + (float)(Main.npc[num47].width / 2);
                        float num49 = Main.npc[num47].position.Y + (float)(Main.npc[num47].height / 2);
                        float num50 = Math.Abs(Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - num48) + Math.Abs(Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - num49);
                        if (num50 < num45)
                        {
                            if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num47].position, Main.npc[num47].width, Main.npc[num47].height))
                            {
                                num46 = num47;
                            }
                            flag6 = true;
                            break;
                        }
                    }
                    num30 = num47;
                }

                if (!flag6)
                {
                    num44 -= (float)(40 * projectile.minionPos * Main.player[projectile.owner].direction);
                }
                if (flag6 && num46 >= 0)
                {
                    projectile.ai[0] = 0f;
                }

                float num51 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector7.Y;

                float num52 = (float)Math.Sqrt((double)(num44 * num44 + num51 * num51));
                float num53 = 10f;
                float num54 = num52;

                if (num52 < (float)num43 && Main.player[projectile.owner].velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
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

                //false for baby slime
                //if (type != 499 && type != 398 && type != 390 && type != 391 && type != 392 && type != 127 && type != 200 && type != 208 && type != 210 && type != 236 && type != 266 && type != 268 && type != 269 && type != 313 && type != 314 && type != 319 && type != 324 && type != 334 && type != 353)
                //{
                //    int num60 = Dust.NewDust(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 4f, projectile.position.Y + (float)(projectile.height / 2) - 4f) - velocity, 8, 8, 16, (0f - velocity.X) * 0.5f, velocity.Y * 0.5f, 50, default(Color), 1.7f);
                //    Main.dust[num60].velocity.X = Main.dust[num60].velocity.X * 0.2f;
                //    Main.dust[num60].velocity.Y = Main.dust[num60].velocity.Y * 0.2f;
                //    Main.dust[num60].noGravity = true;
                //}
            }
            else //projectile.ai[0] == 0f)
            {
                Vector2 vector9 = Vector2.Zero;

                float num87 = (float)(40 * projectile.minionPos);
                int num88 = 60;
                projectile.localAI[0]  -= 1f;
                if (projectile.localAI[0] < 0f)
                {
                    projectile.localAI[0] = 0f;
                }
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] -= 1f;
                }
                else
                {
                    float num89 = projectile.position.X;
                    float num90 = projectile.position.Y;
                    float num91 = 100000f;
                    float num92 = num91;
                    int num93 = -1;
                    NPC ownerMinionAttackTargetNPC2 = projectile.OwnerMinionAttackTargetNPC;
                    if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(this))
                    {
                        float x = ownerMinionAttackTargetNPC2.Center.X;
                        float y = ownerMinionAttackTargetNPC2.Center.Y;
                        float num94 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - x) + Math.Abs(projectile.position.Y + (projectile.height / 2) - y);
                        if (num94 < num91)
                        {
                            if (num93 == -1 && num94 <= num92)
                            {
                                num92 = num94;
                                num89 = x;
                                num90 = y;
                            }
                            if (Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
                            {
                                num91 = num94;
                                num89 = x;
                                num90 = y;
                                num93 = ownerMinionAttackTargetNPC2.whoAmI;
                            }
                        }
                    }
                    if (num93 == -1)
                    {
                        int num30;
                        for (int num95 = 0; num95 < 200; num95 = num30 + 1)
                        {
                            if (Main.npc[num95].CanBeChasedBy(this))
                            {
                                float num96 = Main.npc[num95].position.X + (float)(Main.npc[num95].width / 2);
                                float num97 = Main.npc[num95].position.Y + (float)(Main.npc[num95].height / 2);
                                float num98 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num96) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num97);
                                if (num98 < num91)
                                {
                                    if (num93 == -1 && num98 <= num92)
                                    {
                                        num92 = num98;
                                        num89 = num96;
                                        num90 = num97;
                                    }
                                    if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num95].position, Main.npc[num95].width, Main.npc[num95].height))
                                    {
                                        num91 = num98;
                                        num89 = num96;
                                        num90 = num97;
                                        num93 = num95;
                                    }
                                }
                            }
                            num30 = num95;
                        }
                    }

                    if (num93 == -1 && num92 < num91)
                    {
                        num91 = num92;
                    }
                    else if (num93 >= 0) //has target
                    {
                        vector9 = new Vector2(num89, num90) - projectile.Center;
                    }

                    float num104 = 300f;
                    if ((double)projectile.position.Y > Main.worldSurface * 16.0)
                    {
                        num104 = 150f;
                    }

                    if (num91 < num104 + num87 && num93 == -1)
                    {
                        float num105 = num89 - (projectile.position.X + (projectile.width / 2));
                        if (num105 < -5f)
                        {
                            flag = true;
                            flag2 = false;
                        }
                        else if (num105 > 5f)
                        {
                            flag2 = true;
                            flag = false;
                        }
                    }

                    bool flag9 = false;

                    if (num93 >= 0 && num91 < 800f + num87)
                    {
                        projectile.friendly = true;
                        projectile.localAI[0] = (float)num88;
                        float num106 = num89 - (projectile.position.X + (projectile.width / 2));
                        if (num106 < -10f)
                        {
                            flag = true;
                            flag2 = false;
                        }
                        else if (num106 > 10f)
                        {
                            flag2 = true;
                            flag = false;
                        }
                        if (num90 < projectile.Center.Y - 100f && num106 > -50f && num106 < 50f && projectile.velocity.Y == 0f)
                        {
                            float num107 = Math.Abs(num90 - projectile.Center.Y);
                            if (num107 < 120f)
                            {
                                projectile.velocity.Y = -10f;
                            }
                            else if (num107 < 210f)
                            {
                                projectile.velocity.Y = -13f;
                            }
                            else if (num107 < 270f)
                            {
                                projectile.velocity.Y = -15f;
                            }
                            else if (num107 < 310f)
                            {
                                projectile.velocity.Y = -17f;
                            }
                            else if (num107 < 380f)
                            {
                                projectile.velocity.Y = -18f;
                            }
                        }
                        if (flag9)
                        {
                            projectile.friendly = false;
                            if (projectile.velocity.X < 0f)
                            {
                                flag = true;
                            }
                            else if (projectile.velocity.X > 0f)
                            {
                                flag2 = true;
                            }
                        }
                    }
                    else
                    {
                        projectile.friendly = false;
                    }
                }

                if (projectile.ai[1] != 0f)
                {
                    flag = false;
                    flag2 = false;
                }
                else if (projectile.localAI[0] == 0f)
                {
                    projectile.direction = Main.player[projectile.owner].direction;
                }

                projectile.tileCollide = true;

                float num110 = 0.2f;
                float num111 = 6f;

                if (num111 < Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y))
                {
                    num110 = 0.3f;
                    num111 = Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y);
                }

                if (flag)
                {
                    if (projectile.velocity.X > -3.5f)
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
                    if (projectile.velocity.X < 3.5f)
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
                if (Main.player[projectile.owner].position.Y + Main.player[projectile.owner].height - 8f > projectile.position.Y + projectile.height)
                {
                    flag3 = true;
                }
                Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
                if (projectile.velocity.Y == 0f)
                {
                    if (!flag3 && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
                    {
                        int num113 = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                        int j2 = (int)(projectile.position.Y + (projectile.height / 2)) / 16 + 1;
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
                        int num114 = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                        int num115 = (int)(projectile.position.Y + projectile.height) / 16 + 1;
                        if (WorldGen.SolidTile(num114, num115) || Main.tile[num114, num115].halfBrick() || Main.tile[num114, num115].slope() > 0)
                        {
                            try
                            {
                                num114 = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                                num115 = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
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
                    else if (flag | flag2)
                    {
                        projectile.velocity.Y -= 6f;
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
            }

            Draw();
        }
    }
}
