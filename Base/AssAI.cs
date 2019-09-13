using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace AssortedCrazyThings.Base
{
    /// <summary>
    /// contains AI for stuff that only uses ai[], used with thing.aiStyle = -1
    /// </summary>
    public static class AssAI
    {
        /// <summary>
        /// Makes the projectile teleport if it is too far away from the player
        /// </summary>
        public static void TeleportIfTooFar(Projectile projectile, Vector2 desiredCenter, int distance = 2000)
        {
            if (projectile.DistanceSQ(desiredCenter) > distance * distance)
            {
                projectile.Center = desiredCenter;
                if (Main.myPlayer == projectile.owner && Main.netMode == NetmodeID.MultiplayerClient) projectile.netUpdate = true;
            }
        }

        //Credit to Itorius
        /// <summary>
        /// Uses ray tracking as an alternative to Collision.CanHitLine
        /// </summary>
        public static bool CheckLineOfSight(Vector2 center, Vector2 target)
        {
            Ray ray = new Ray(new Vector3(center, 0), new Vector3(target - center, 0));

            List<Vector2> tiles = new List<Vector2>();
            Utils.PlotTileLine(center, target, 42, (i, j) =>
            {
                tiles.Add(i * 16 > center.X ? new Vector2(i, j + 1) * 16 : new Vector2(i, j) * 16);
                return true;
            });

            return tiles
                .Where(tile => WorldGen.SolidTile((int)(tile.X / 16), (int)(tile.Y / 16)))
                .All(tile => new BoundingBox(new Vector3(tile - new Vector2(2), 0), new Vector3(tile + new Vector2(20), 0)).Intersects(ray) == null);
        }

        /// <summary>
        /// Finds target in range of relativeCenter. Returns index of target
        /// </summary>
        public static int FindTarget(Projectile projectile, Vector2 relativeCenter, float range = 300f, bool ignoreTiles = false, bool useSlowLOS = false)
        {
            int targetIndex = -1;
            float distanceFromTarget = 10000000f;
            Vector2 targetCenter = relativeCenter;
            range *= range;
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy())
                {
                    //Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height)
                    float between = Vector2.DistanceSquared(npc.Center, relativeCenter);
                    if ((between < range && Vector2.DistanceSquared(relativeCenter, targetCenter) > between && between < distanceFromTarget) || targetIndex == -1)
                    {
                        if (ignoreTiles || (useSlowLOS ? CheckLineOfSight(relativeCenter, npc.Center) : Collision.CanHitLine(relativeCenter, projectile.width, projectile.height, npc.position, npc.width, npc.height)))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            targetIndex = k;
                        }
                    }
                }
            }
            return distanceFromTarget < range ? targetIndex : -1;
        }

        #region Flickerwick
        public static void FlickerwickPetDraw(Projectile projectile, int frameCounterMaxFar, int frameCounterMaxClose)
        {
            if (++projectile.frameCounter >= ((projectile.velocity.Length() > 6f) ? frameCounterMaxFar : frameCounterMaxClose))
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }

        /// <summary>
        /// No use of ai[] or LocalAI[].
        /// Default offset is x = 30, y = -20
        /// </summary>
        public static void FlickerwickPetAI(Projectile projectile, bool lightPet = true, bool lightDust = true, bool staticDirection = false, bool reverseSide = false, bool vanityPet = false, float veloXToRotationFactor = 1f, float veloSpeed = 1f, float lightFactor = 1f, Vector3 lightColor = default(Vector3), float offsetX = 0f, float offsetY = 0f)
        {
            //veloSpeed not bigger than veloDistanceChange * 0.5f
            Player player = projectile.GetOwner();
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
                    dust.velocity.Y += -3f;
                    dust.noLight = true;
                }
                else if (Main.rand.Next(2) != 0)
                {
                    dust.noLight = true;
                }
                dust.velocity *= 0.5f;
                dust.velocity.Y += -0.9f;
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
            //if (between > 1000f)
            //{
            //    projectile.Center = player.Center + desiredCenterRelative;
            //}
            TeleportIfTooFar(projectile, desiredCenter, 1000);
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

        /// <summary>
        /// Almost proper working Eye Spring clone
        /// </summary>
        public static void EyeSpringAI(Projectile projectile, bool flyForever = false)
        {
            Player player = projectile.GetOwner();
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
                        TeleportIfTooFar(projectile, player.Center);
                        //projectile.position.X = player.position.X + (float)(player.width / 2) - (float)(projectile.width / 2);
                        //projectile.position.Y = player.position.Y + (float)(player.height / 2) - (float)(projectile.height / 2);
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
                    //float num54 = num52;
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
                    projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
                }
                else
                {
                    //Vector2 vector9 = Vector2.Zero;
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

        /// <summary>
        /// Stays around a certain offset position around the parent.
        /// </summary>
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

            Player player = projectile.GetOwner();
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

            Vector2 desiredCenter = random ? new Vector2(Main.rand.Next(-10, 21), Main.rand.Next(-10, 21)) : Vector2.Zero;

            Vector2 offset = new Vector2(60f + offsetX, -60f + offsetY);

            if (swapSides == 1)
            {
                offset.X = -offset.X;
            }
            else if (swapSides == 0)
            {
                offset.X *= -parent.direction;
            }

            //desiredCenter += new Vector2(60f * -player.direction, -60f);
            between += desiredCenter + offset;

            ////added in manually since its kinda useful
            //if (Vector2.Distance(projectile.Center, parentCenter) > 2000f)
            //{
            //    projectile.Center = parentCenter + desiredCenter + offset;
            //}

            TeleportIfTooFar(projectile, parentCenter + desiredCenter + offset);

            float distance = between.Length();
            //float magnitude = 6f;
            if (distance < someDistance && parent.velocity.Y == 0f && projectile.position.Y + projectile.height <= parent.position.Y + parent.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                //projectile.ai[0] = 0;
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
                between *= velocityFactor;
            }
            veloDelta *= velocityFactor;
            if (projectile.velocity.X < between.X)
            {
                projectile.velocity.X += veloDelta;
                if (veloDelta > 0.05f && projectile.velocity.X < 0f)
                {
                    projectile.velocity.X += +veloDelta;
                }
            }
            if (projectile.velocity.X > between.X)
            {
                projectile.velocity.X += -veloDelta;
                if (veloDelta > 0.05f && projectile.velocity.X > 0f)
                {
                    projectile.velocity.X += -veloDelta;
                }
            }
            if (projectile.velocity.Y < between.Y)
            {
                projectile.velocity.Y += veloDelta;
                if (veloDelta > 0.05f && projectile.velocity.Y < 0f)
                {
                    projectile.velocity.Y += veloDelta * 2f;
                }
            }
            if (projectile.velocity.Y > between.Y)
            {
                projectile.velocity.Y += -veloDelta;
                if (veloDelta > 0.05f && projectile.velocity.Y > 0f)
                {
                    projectile.velocity.Y += -veloDelta * 2f;
                }
            }
            projectile.manualDirectionChange = true;
            if (projectile.velocity.X > 0.25f && projectile.direction == 1)
            {
                projectile.direction = -1;
            }
            else if (projectile.velocity.X < -0.25f && projectile.direction != 1)
            {
                projectile.direction = 1;
            }
            projectile.spriteDirection = projectile.direction;

            //fix, direction gets set automatically by "manualDirectionChange = false" projectiled on velocity.X
            //if (projectile.velocity.X > 0.25f)
            //{
            //    projectile.ai[0] = -1;
            //}
            //else if (projectile.velocity.X < -0.25f)
            //{
            //    projectile.ai[0] = 1;
            //}
            //projectile.direction = (int)projectile.ai[0];
            //projectile.spriteDirection = projectile.direction;
            projectile.rotation = projectile.velocity.X * 0.05f;
        }
        #endregion

        #region BabyEater
        public static void BabyEaterDraw(Projectile projectile, int frameCounter = 6)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > frameCounter)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= Main.projFrames[projectile.type]) //2
            {
                projectile.frame = 0;
            }
        }

        public static void BabyEaterAI(Projectile projectile, Entity parent = null, Vector2 originOffset = default(Vector2), float velocityFactor = 1f, float sway = 1f)
        {
            //velocityFactor: 
            //kinda wonky, leave at 1f

            //sway: 
            //tells by how much increase/decrease the left/right sway radius of the idle pet

            Player player = projectile.GetOwner();

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
            parentCenter += originOffset;

            float veloDelta = 0.1f;
            projectile.tileCollide = false;
            int someDistance = 300;
            Vector2 between = parentCenter - projectile.Center;
            float distance = between.Length();
            float magnitude = 7f;

            TeleportIfTooFar(projectile, parentCenter, 1380);

            if (distance < someDistance && parent.velocity.Y == 0f && projectile.position.Y + projectile.height <= parent.position.Y + parent.height + originOffset.Y && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                if (projectile.velocity.Y < -6f)
                {
                    projectile.velocity.Y = -6f;
                }
            }
            float swayDistance = 150f * sway;
            if (distance < swayDistance)
            {
                if (Math.Abs(projectile.velocity.X) > 2f || Math.Abs(projectile.velocity.Y) > 2f)
                {
                    projectile.velocity *= 0.99f;
                }
                veloDelta = 0.01f;
                if (between.X < -2f)
                {
                    between.X = -2f;
                }
                if (between.X > 2f)
                {
                    between.X = 2f;
                }
                if (between.Y < -2f)
                {
                    between.Y = -2f;
                }
                if (between.Y > 2f)
                {
                    between.Y = 2f;
                }
            }
            else
            {
                if (distance > swayDistance * 2f)
                {
                    veloDelta = 0.2f;
                }
                between.Normalize();
                between *= magnitude;
            }

            veloDelta *= velocityFactor;

            if (Math.Abs(between.X) > Math.Abs(between.Y))
            {
                if (projectile.velocity.X < between.X)
                {
                    projectile.velocity.X += veloDelta;
                    if (veloDelta > 0.05f && projectile.velocity.X < 0f)
                    {
                        projectile.velocity.X += veloDelta;
                    }
                }
                if (projectile.velocity.X > between.X)
                {
                    projectile.velocity.X += -veloDelta;
                    if (veloDelta > 0.05f && projectile.velocity.X > 0f)
                    {
                        projectile.velocity.X += -veloDelta;
                    }
                }
            }
            if (Math.Abs(between.X) <= Math.Abs(between.Y) || veloDelta == 0.05f)
            {
                if (projectile.velocity.Y < between.Y)
                {
                    projectile.velocity.Y += veloDelta;
                    if (veloDelta > 0.05f && projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y += veloDelta;
                    }
                }
                if (projectile.velocity.Y > between.Y)
                {
                    projectile.velocity.Y += -veloDelta;
                    if (veloDelta > 0.05f && projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y += -veloDelta;
                    }
                }
            }
            projectile.rotation = projectile.velocity.ToRotation() - 1.57f;
        }
        #endregion

        #region StardustDragon


        //ProjectileID.Sets.
        //NeedsUUID = true;
        //DontAttachHideToAlpha =true;

        //if minion = true:
        //ProjectileID.Sets.MinionSacrificable[projectile.type] = false, cause the replacing code for worm minions is complicated
        //damage set in NewProjectile/item
        //scales in size with the amount of segments
        public static void StardustDragonSetDefaults(Projectile projectile, bool minion = true, WormType wormType = WormType.None)
        {
            if (minion)
            {
                //if (projectile.type == 625 || projectile.type == 628)
                //{
                //    projectile.netImportant = true;
                //}
                if (wormType == WormType.Body1 || wormType == WormType.Body2)
                {
                    projectile.minionSlots = 0.5f;
                }
                projectile.minion = true;
                //projectile.hide = true;
                projectile.netImportant = true;
            }
            projectile.width = 24;
            projectile.height = 24;
            projectile.penetrate = -1;
            projectile.timeLeft *= 5;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.alpha = 255;
        }
        //wormTypes = new int[] {head, body1, body2, tail} //projectiletype

        //if minion = true:
        //float scaleFactor = MathHelper.Clamp(projectile.localAI[0], 0f, 50f);
        //projectile.scale = 1f + scaleFactor * 0.01f;
        public static void StardustDragonAI(Projectile projectile, int[] wormTypes)
        {
            Player player = projectile.GetOwner();

            if (projectile.minion && (int)Main.time % 120 == 0)
            {
                projectile.netUpdate = true;
            }
            if (!player.active)
            {
                projectile.active = false;
                return;
            }
            bool head = projectile.type == wormTypes[0];
            int defScaleFactor = 30;
            //if (Main.rand.Next(30) == 0)
            //{
            //    int num1049 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, 0f, 0f, 0, default(Color), 2f);
            //    Main.dust[num1049].noGravity = true;
            //    Main.dust[num1049].fadeIn = 2f;
            //    Point point4 = Main.dust[num1049].position.ToTileCoordinates();
            //    if (WorldGen.InWorld(point4.X, point4.Y, 5) && WorldGen.SolidTile(point4.X, point4.Y))
            //    {
            //        Main.dust[num1049].noLight = true;
            //    }
            //}

            if (projectile.alpha > 0)
            {
                //for (int i = 0; i < 2; i++)
                //{
                //    int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 135, 0f, 0f, 100, default(Color), 2f);
                //    Main.dust[dust].noGravity = true;
                //    Main.dust[dust].noLight = true;
                //}
                projectile.alpha -= 42;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                    return;
                }
            }

            if (head)
            {
                Vector2 desiredCenter = player.Center;
                int targetIndex = -1;
                TeleportIfTooFar(projectile, desiredCenter);
                if (projectile.minion)
                {
                    float maxProjDistance = 490000f;
                    float maxPlayerDistance = 1000000f;
                    NPC ownerMinionAttackTargetNPC5 = projectile.OwnerMinionAttackTargetNPC;
                    if (ownerMinionAttackTargetNPC5 != null && ownerMinionAttackTargetNPC5.CanBeChasedBy())
                    {
                        float distance1 = projectile.DistanceSQ(ownerMinionAttackTargetNPC5.Center);
                        if (distance1 < maxProjDistance * 2f)
                        {
                            targetIndex = ownerMinionAttackTargetNPC5.whoAmI;
                        }
                    }
                    if (targetIndex < 0)
                    {
                        for (int i = 0; i < 200; i++)
                        {
                            NPC nPC14 = Main.npc[i];
                            if (nPC14.CanBeChasedBy() && player.DistanceSQ(nPC14.Center) < maxPlayerDistance)
                            {
                                float distance2 = projectile.DistanceSQ(nPC14.Center);
                                if (distance2 < maxProjDistance)
                                {
                                    targetIndex = i;
                                }
                            }
                        }
                    }
                }
                if (targetIndex != -1)
                {
                    NPC npc = Main.npc[targetIndex];
                    Vector2 betweenNPC = npc.Center - projectile.Center;
                    float veloFactor = 0.4f;
                    if (betweenNPC.Length() < 600f)
                    {
                        veloFactor = 0.6f;
                    }
                    if (betweenNPC.Length() < 300f)
                    {
                        veloFactor = 0.8f;
                    }
                    if (betweenNPC.Length() > npc.Size.Length() * 0.75f)
                    {
                        projectile.velocity += Vector2.Normalize(betweenNPC) * veloFactor * 1.5f;
                        if (Vector2.Dot(projectile.velocity, betweenNPC) < 0.25f)
                        {
                            projectile.velocity *= 0.8f;
                        }
                    }
                    float targetMagnitude = 30f;
                    if (projectile.velocity.Length() > targetMagnitude)
                    {
                        projectile.velocity = Vector2.Normalize(projectile.velocity) * targetMagnitude;
                    }
                }
                else
                {
                    float idleVelo = 0.2f;
                    Vector2 betweenPlayer = desiredCenter - projectile.Center;
                    if (betweenPlayer.Length() < 200f)
                    {
                        idleVelo = 0.12f;
                    }
                    if (betweenPlayer.Length() < 140f)
                    {
                        idleVelo = 0.06f;
                    }
                    if (betweenPlayer.Length() > 100f)
                    {
                        if (Math.Abs(desiredCenter.X - projectile.Center.X) > 20f)
                        {
                            projectile.velocity.X += idleVelo * Math.Sign(desiredCenter.X - projectile.Center.X);
                        }
                        if (Math.Abs(desiredCenter.Y - projectile.Center.Y) > 10f)
                        {
                            projectile.velocity.Y += idleVelo * Math.Sign(desiredCenter.Y - projectile.Center.Y);
                        }
                    }
                    else if (projectile.velocity.Length() > 2f)
                    {
                        projectile.velocity *= 0.96f;
                    }
                    if (Math.Abs(projectile.velocity.Y) < 1f)
                    {
                        projectile.velocity.Y += -0.1f;
                    }
                    float idleMagnitude = 15f;
                    if (projectile.velocity.Length() > idleMagnitude)
                    {
                        projectile.velocity = Vector2.Normalize(projectile.velocity) * idleMagnitude;
                    }
                }
                projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
                int direction = projectile.direction;
                projectile.direction = projectile.spriteDirection = (projectile.velocity.X > 0f) ? 1 : -1;
                if (projectile.minion && direction != projectile.direction)
                {
                    projectile.netUpdate = true;
                }
                float scaleFactor = MathHelper.Clamp(projectile.localAI[0], 0f, 50f);
                if (!projectile.minion) scaleFactor = 0;
                projectile.position = projectile.Center;
                projectile.scale = 1f + scaleFactor * 0.01f;
                projectile.width = projectile.height = (int)(defScaleFactor * projectile.scale);
                projectile.Center = projectile.position;
            }
            else
            {
                Vector2 pCenter = Vector2.Zero;
                float parentRotation = 0f;
                float positionOffset = 0f;
                float scaleOffset = 1f;

                //some custom syncing it seems like, when summoning/replacing it
                if (projectile.ai[1] == 1f)
                {
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }

                Projectile parent = null;
                for (short i = 0; i < 1000; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == projectile.owner && proj.identity == (int)projectile.ai[0]/* && proj.type == projectile.type*/)
                    {
                        parent = proj;
                        break;
                    }
                }
                if (parent != null)
                {
                    if (parent.active && (parent.type == wormTypes[0] || parent.type == wormTypes[1] || parent.type == wormTypes[2]))
                    {
                        pCenter = parent.Center;
                        //Vector2 velocity2 = parent.velocity;
                        parentRotation = parent.rotation;
                        scaleOffset = MathHelper.Clamp(parent.scale, 0f, 50f);
                        if (!projectile.minion) scaleOffset = 1;
                        positionOffset = 16f;
                        parent.localAI[0] = projectile.localAI[0] + 1f;
                        if (parent.type != wormTypes[0])
                        {
                            parent.localAI[1] = projectile.whoAmI;
                        }
                        if (projectile.owner == Main.myPlayer && parent.type == wormTypes[0] && projectile.type == wormTypes[3])
                        {
                            parent.Kill();
                            projectile.Kill();
                            return;
                        }
                    }

                    projectile.velocity = Vector2.Zero;
                    Vector2 newVelocity = pCenter - projectile.Center;
                    if (parentRotation != projectile.rotation)
                    {
                        float rotatedBy = MathHelper.WrapAngle(parentRotation - projectile.rotation);
                        newVelocity = newVelocity.RotatedBy(rotatedBy * 0.1f);
                    }
                    projectile.rotation = newVelocity.ToRotation() + 1.57079637f;
                    projectile.position = projectile.Center;
                    projectile.scale = scaleOffset;
                    projectile.width = projectile.height = (int)(defScaleFactor * projectile.scale);
                    projectile.Center = projectile.position;
                    if (newVelocity != Vector2.Zero)
                    {
                        projectile.Center = pCenter - Vector2.Normalize(newVelocity) * positionOffset * scaleOffset;
                    }
                    projectile.spriteDirection = (newVelocity.X > 0f) ? 1 : -1;
                }
            }
        }

        #endregion
    }

    public enum WormType : byte
    {
        None = 0,
        Head = 1,
        Body1 = 2,
        Body2 = 3,
        Tail = 4
    }
}
