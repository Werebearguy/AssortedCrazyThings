using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;

namespace AssortedCrazyThings
{
    //contains AI for stuff that only uses ai[], used with thing.aiStyle = -1
    public static class AssAI
    {
        #region Flickerwick
        public static void FlickerwickPetDraw(Projectile projectile, int frameCounterMaxFar, int frameCounterMaxClose)
        {
            if (projectile.velocity.Length() > 6f)
            {
                if (++projectile.frameCounter >= frameCounterMaxFar)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= Main.projFrames[projectile.type])
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
                    if (++projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 0;
                    }
                }
            }
        }

        public static void FlickerwickPetAI(Projectile projectile, bool lightPet = true, bool lightDust = true, bool staticDirection = false, bool reverseSide = false, bool vanityPet = false, float veloXToRotationFactor = 1f, float veloSpeed = 1f, float lightFactor = 1f, Vector3 lightColor = default(Vector3), float offsetX = 0f, float offsetY = 0f)
        {
            //veloSpeed not bigger than veloDistanceChange * 0.5f
            Player player = Main.player[projectile.owner];
            float veloDistanceChange = 2f; //6f

            int dir = player.direction;
            if (staticDirection)
            {
                if (reverseSide)
                {
                    dir = -1;
                }
                else
                {
                    dir = 1;
                }
            }
            else
            {
                if (reverseSide)
                {
                    dir = -dir;
                }
            }

            //up and down bobbing
            //projectile.localAI[0] += 1f;
            //if (projectile.localAI[0] > 120f)
            //{
            //    projectile.localAI[0] = 0f;
            //}
            //value.Y += (float)Math.Cos((double)(projectile.localAI[0] * 0.05235988f)) * 2f;

            Vector2 dustOffset = new Vector2((projectile.spriteDirection == -1) ? -6 : -2, -20f).RotatedBy(projectile.rotation);

            Vector2 desiredCenterRelative = new Vector2(dir * (offsetX + 30), -20f + offsetY);

            projectile.direction = projectile.spriteDirection = dir;

            //if (reverseSide)
            //{
            //    desiredCenterRelative.X = -desiredCenterRelative.X;
            //    //value2.X = -value2.X;
            //    projectile.direction = -projectile.direction;
            //    projectile.spriteDirection = -projectile.spriteDirection;
            //}

            if (lightDust && Main.rand.Next(24) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.Center + dustOffset, 4, 4, 135, 0f, 0f, 100);
                if (Main.rand.Next(3) != 0)
                {
                    dust.noGravity = true;
                    dust.velocity.Y = dust.velocity.Y - 3f;
                    dust.noLight = true;
                }
                else if (Main.rand.Next(2) != 0)
                {
                    dust.noLight = true;
                }
                dust.velocity *= 0.5f;
                dust.velocity.Y = dust.velocity.Y - 0.9f;
                dust.scale += 0.1f + Main.rand.NextFloat() * 0.6f;
                dust.shader = GameShaders.Armor.GetSecondaryShader(!vanityPet ? player.cLight : player.cPet, player);
            }

            if (lightPet)
            {
                if (lightColor == default(Vector3)) lightColor = new Vector3(0.3f, 0.5f, 1f);
                //flickerwick is new Vector3(0.3f, 0.5f, 1f)
                Vector3 vector = DelegateMethods.v3_1 = lightColor * lightFactor;
                Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * 6f, 20f, DelegateMethods.CastLightOpen);
                Utils.PlotTileLine(projectile.Left, projectile.Right, 20f, DelegateMethods.CastLightOpen);
                Utils.PlotTileLine(player.Center, player.Center + player.velocity * 6f, 40f, DelegateMethods.CastLightOpen);
                Utils.PlotTileLine(player.Left, player.Right, 40f, DelegateMethods.CastLightOpen);
            }

            Vector2 desiredCenter = player.MountedCenter + desiredCenterRelative;
            float between = Vector2.Distance(projectile.Center, desiredCenter);
            if (between > 1000f)
            {
                projectile.Center = player.Center + desiredCenterRelative;
            }
            Vector2 betweenDirection = desiredCenter - projectile.Center;
            if (between < veloDistanceChange)
            {
                projectile.velocity *= 0.25f;
            }
            if (betweenDirection != Vector2.Zero)
            {
                if (betweenDirection.Length() < veloDistanceChange * 0.5f)
                {
                    projectile.velocity = betweenDirection * veloSpeed;
                }
                else
                {
                    projectile.velocity = betweenDirection * 0.1f * veloSpeed;
                }
            }
            if (projectile.velocity.Length() > 6f)
            {
                float rotationVelo = projectile.velocity.X * 0.08f * veloXToRotationFactor + projectile.velocity.Y * projectile.spriteDirection * 0.02f;
                if (Math.Abs(projectile.rotation - rotationVelo) >= 3.14159274f)
                {
                    if (rotationVelo < projectile.rotation)
                    {
                        projectile.rotation -= 6.28318548f;
                    }
                    else
                    {
                        projectile.rotation += 6.28318548f;
                    }
                }
                float rotationAcc = 12f;
                projectile.rotation = (projectile.rotation * (rotationAcc - 1f) + rotationVelo) / rotationAcc;
            }
            else
            {
                if (projectile.rotation > 3.14159274f)
                {
                    projectile.rotation -= 6.28318548f;
                }
                if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
                {
                    projectile.rotation = 0f;
                }
                else
                {
                    projectile.rotation *= 0.96f;
                }
            }
        }
        #endregion

        #region EyeSpring
        public static void EyeSpringAI(Projectile projectile, bool flyForever = false)
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
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
                    if (player.position.Y + player.height - 8f > projectile.position.Y + projectile.height)
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
                            int num114 = (int)(projectile.position.X + projectile.width / 2) / 16;
                            int num115 = (int)(projectile.position.Y + projectile.height) / 16 + 1;
                            if (WorldGen.SolidTile(num114, num115) || Main.tile[num114, num115].halfBrick() || Main.tile[num114, num115].slope() > 0)
                            {
                                try
                                {
                                    num114 = (int)(projectile.position.X + projectile.width / 2) / 16;
                                    num115 = (int)(projectile.position.Y + projectile.height / 2) / 16;
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


                    //fix cause im dumb and didnt copy ai code correctly
                    if (!flag && !flag2 && projectile.ai[0] == 0f)
                    {
                        projectile.direction = (player.Center - projectile.Center).X > 0 ? 1 : -1;
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
                                    /*int num119 = 0; Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + (float)projectile.height - 2f), projectile.width, 4, 5)*/
                                    ;
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
        #endregion

        #region Zephyrfish
        public static void ZephyrfishDraw(Projectile projectile, int frameCounter = 5)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > frameCounter)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= Main.projFrames[projectile.type]) //3
            {
                projectile.frame = 0;
            }
        }

        public static void ZephyrfishAI(Projectile projectile, Entity parent = null, float velocityFactor = 1f, float sway = 1f, bool random = true, byte swapSides = 0, float offsetX = 0f, float offsetY = 0f)
        {
            //velocityFactor: 
            //kinda wonky, leave at 1f

            //sway: 
            //tells by how much increase/decrease the left/right sway of the idle pet

            //swapSides:
            // 0: always behind
            //-1: always left
            // 1: always right

            //offsetX/Y
            //offsetting the desired center the pet hovers around

            Player player = Main.player[projectile.owner];
            if (!player.active)
            {
                projectile.active = false;
                return;
            }

            if (parent == null) parent = player;
            Vector2 parentCenter = parent.Center;
            if (parent is Player)
            {
                parentCenter = ((Player)parent).MountedCenter;
            }

            float veloDelta = 0.3f;
            projectile.tileCollide = false; //false
            int someDistance = 100;
            Vector2 between = parentCenter - projectile.Center;

            Vector2 desiredCenter = random? new Vector2(Main.rand.Next(-10, 21), Main.rand.Next(-10, 21)) : Vector2.Zero;

            Vector2 offset = new Vector2(60f + offsetX, -60f + offsetY);

            if (swapSides == 1)
            {
                offset.X = -offset.X;
            }
            else if (swapSides == 0)
            {
                offset.X = offset.X * -parent.direction;
            }

            //desiredCenter += new Vector2(60f * -player.direction, -60f);
            between += desiredCenter + offset;
            
            //added in manually since its kinda useful
            if (Vector2.Distance(projectile.Center, parentCenter) > 3000f)
            {
                projectile.Center = parentCenter + desiredCenter + offset;
            }

            float distance = between.Length();
            //float magnitude = 6f;
            if (distance < someDistance && parent.velocity.Y == 0f && projectile.position.Y + projectile.height <= parent.position.Y + parent.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                if (projectile.velocity.Y < -6f)
                {
                    projectile.velocity.Y = -6f;
                }
            }
            float swayDistance = 50 * sway;
            if (distance < swayDistance) //50
            {
                if (Math.Abs(projectile.velocity.X) > 2f || Math.Abs(projectile.velocity.Y) > 2f)
                {
                    projectile.velocity *= 0.99f;
                }
                veloDelta = 0.01f;
            }
            else
            {
                if (distance < swayDistance * 2) //100
                {
                    veloDelta = 0.1f;
                }
                //between: 0.3f
                if (distance > swayDistance * 6) //300
                {
                    veloDelta = 0.4f;
                }
                between.Normalize();
                between *= 6f;
            }
            veloDelta *= velocityFactor;
            if (projectile.velocity.X < between.X)
            {
                projectile.velocity.X = projectile.velocity.X + veloDelta;
                if (veloDelta > 0.05f && projectile.velocity.X < 0f)
                {
                    projectile.velocity.X = projectile.velocity.X + veloDelta;
                }
            }
            if (projectile.velocity.X > between.X)
            {
                projectile.velocity.X = projectile.velocity.X - veloDelta;
                if (veloDelta > 0.05f && projectile.velocity.X > 0f)
                {
                    projectile.velocity.X = projectile.velocity.X - veloDelta;
                }
            }
            if (projectile.velocity.Y < between.Y)
            {
                projectile.velocity.Y = projectile.velocity.Y + veloDelta;
                if (veloDelta > 0.05f && projectile.velocity.Y < 0f)
                {
                    projectile.velocity.Y = projectile.velocity.Y + veloDelta * 2f;
                }
            }
            if (projectile.velocity.Y > between.Y)
            {
                projectile.velocity.Y = projectile.velocity.Y - veloDelta;
                if (veloDelta > 0.05f && projectile.velocity.Y > 0f)
                {
                    projectile.velocity.Y = projectile.velocity.Y - veloDelta * 2f;
                }
            }
            //if ((double)projectile.velocity.X > 0.25)
            //{
            //    projectile.direction = -1;
            //}
            //else if ((double)projectile.velocity.X < -0.25)
            //{
            //    projectile.direction = 1;
            //}
            //projectile.spriteDirection = projectile.direction;

            //fix, direction gets set automatically by tmodloader based on velocity.X for some reason
            if (projectile.velocity.X > 0.25f)
            {
                projectile.ai[0] = -1;
            }
            else if (projectile.velocity.X < -0.25f)
            {
                projectile.ai[0] = 1;
            }
            projectile.direction = (int)projectile.ai[0];
            projectile.spriteDirection = projectile.direction;
            projectile.rotation = projectile.velocity.X * 0.05f;
        }
        #endregion
    }
}
