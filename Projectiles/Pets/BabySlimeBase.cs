using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    /// <summary>
    /// localAI[1] freely available
    /// </summary>
    public abstract class BabySlimeBase : ModProjectile
    {
        public bool shootSpikes = false;
        private static readonly byte shootDelay = 60; //either +1 or +0 every tick, so effectively every 90 ticks
        public byte flyingFrameSpeed = 6;
        public byte walkingFrameSpeed = 20;
        public float customMinionSlots = 1f;

        public sealed override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabySlime);
            projectile.aiStyle = -1; //26
            //aiType = ProjectileID.BabySlime;
            projectile.alpha = 50;

            //set those in moresetdefaults in the projectile that inherits from this
            //projectile.width = 38;
            //projectile.height = 40;

            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;

            flyingFrameSpeed = 6;
            walkingFrameSpeed = 20;

            MoreSetDefaults();

            projectile.minionSlots = projectile.minion ? customMinionSlots : 0f;
        }

        public virtual void MoreSetDefaults()
        {
            //used to set dimensions (if necessary)
            //also used to set projectile.minion
        }

        public override bool MinionContactDamage()
        {
            return projectile.minion ? true : false;
        }

        public override void AI()
        {
            BabySlimeAI();
            Draw();
        }

        public void Draw()
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
                if (projectile.frameCounter > flyingFrameSpeed)
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
                if (projectile.frameCounter >= walkingFrameSpeed)
                {
                    projectile.frameCounter -= walkingFrameSpeed;
                    projectile.frame++;
                }
                if (projectile.frame > 1)
                {
                    projectile.frame = 0;
                }
                if (projectile.wet && projectile.GetOwner().Bottom.Y < projectile.Bottom.Y && JumpTimer == 0f)
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
                projectile.rotation = 0f;
            }
        }

        public byte pickedTexture = 1;

        public byte PickedTexture
        {
            get
            {
                return (byte)(pickedTexture - 1);
            }
            set
            {
                pickedTexture = (byte)(value + 1);
            }
        }

        public bool HasTexture
        {
            get
            {
                return PickedTexture != 0;
            }
        }

        public int JumpTimer
        {
            get
            {
                return (int)projectile.localAI[0];
            }
            set
            {
                projectile.localAI[0] = value;
            }
        }

        public byte ShootTimer
        {
            get
            {
                return (byte)projectile.localAI[1];
            }
            set
            {
                projectile.localAI[1] = value;
            }
        }


        public void BabySlimeAI()
        {
            bool left = false;
            bool right = false;
            bool flag3 = false;
            bool flag4 = false;

            Player player = projectile.GetOwner();

            int initialOffset = projectile.minion ? 10 : 25;
            if (!projectile.minion) projectile.minionPos = 0;
            int directionalOffset = 40 * (projectile.minionPos + 1) * player.direction;
            if (player.Center.X < projectile.Center.X - initialOffset + directionalOffset)
            {
                left = true;
            }
            else if (player.Center.X > projectile.Center.X + initialOffset + directionalOffset)
            {
                right = true;
            }

            if (projectile.ai[1] == 0f)
            {
                int num38 = 500;
                num38 += 40 * projectile.minionPos;
                if (JumpTimer > 0)
                {
                    num38 += 600;
                }

                if (player.rocketDelay2 > 0)
                {
                    projectile.ai[0] = 1f;
                }

                Vector2 center = projectile.Center;
                float x = player.Center.X - center.X;
                float y = player.Center.Y - center.Y;
                float distance = (float)Math.Sqrt(x * x + y * y);
                if (distance > 2000f)
                {
                    projectile.position.X = player.position.X;
                    projectile.position.Y = player.position.Y;
                }
                else if (distance > num38 || (Math.Abs(y) > 300f && JumpTimer <= 0))
                {
                    if (y > 0f && projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                    if (y < 0f && projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                    projectile.ai[0] = 1f;
                }
            }

            if (projectile.ai[0] != 0f)
            {
                float veloDelta = 0.2f;
                int playerRange = 200;

                projectile.tileCollide = false;
                float desiredVeloX = player.Center.X - projectile.Center.X;

                desiredVeloX -= 40 * player.direction;

                bool foundCloseTarget = false;
                int targetIndex = -1;

                if (projectile.minion)
                {
                    float range = 700f;
                    for (int k = 0; k < 200; k++)
                    {
                        NPC npc = Main.npc[k];
                        if (npc.CanBeChasedBy())
                        {
                            float distance = Math.Abs(player.Center.X - npc.Center.X) + Math.Abs(player.Center.Y - npc.Center.Y);
                            if (distance < range)
                            {
                                if (Collision.CanHit(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                                {
                                    targetIndex = k;
                                }
                                foundCloseTarget = true;
                                break;
                            }
                        }
                    }
                }

                if (!foundCloseTarget)
                {
                    desiredVeloX -= 40 * projectile.minionPos * player.direction;
                }
                if (foundCloseTarget && targetIndex >= 0)
                {
                    projectile.ai[0] = 0f;
                }

                float desiredVeloY = player.Center.Y - projectile.Center.Y;

                float between = (float)Math.Sqrt(desiredVeloX * desiredVeloX + desiredVeloY * desiredVeloY);
                //float num54 = num52;

                if (between < playerRange && player.velocity.Y == 0f && projectile.position.Y + projectile.height <= player.position.Y + player.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.ai[0] = 0f;
                    if (projectile.velocity.Y < -6f)
                    {
                        projectile.velocity.Y = -6f;
                    }
                }
                if (between < 60f)
                {
                    desiredVeloX = projectile.velocity.X;
                    desiredVeloY = projectile.velocity.Y;
                }
                else
                {
                    between = 10f / between;
                    desiredVeloX *= between;
                    desiredVeloY *= between;
                }

                if (projectile.velocity.X < desiredVeloX)
                {
                    projectile.velocity.X += veloDelta;
                    if (projectile.velocity.X < 0f)
                    {
                        projectile.velocity.X += veloDelta * 1.5f;
                    }
                }
                if (projectile.velocity.X > desiredVeloX)
                {
                    projectile.velocity.X -= veloDelta;
                    if (projectile.velocity.X > 0f)
                    {
                        projectile.velocity.X -= veloDelta * 1.5f;
                    }
                }
                if (projectile.velocity.Y < desiredVeloY)
                {
                    projectile.velocity.Y += veloDelta;
                    if (projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y += veloDelta * 1.5f;
                    }
                }
                if (projectile.velocity.Y > desiredVeloY)
                {
                    projectile.velocity.Y -= veloDelta;
                    if (projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y -= veloDelta * 1.5f;
                    }
                }
            }
            else //projectile.ai[0] == 0f
            {
                Vector2 toTarget = Vector2.Zero;

                float offset = 40 * projectile.minionPos;
                JumpTimer -= 1;
                if (JumpTimer < 0)
                {
                    JumpTimer = 0;
                }
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] -= 1f;
                }
                else
                {
                    float targetX = projectile.position.X;
                    float targetY = projectile.position.Y;
                    float distance = 100000f;
                    float otherDistance = distance;
                    int targetNPC = -1;

                    //------------------------------------------------------------------------------------
                    //DISABLE MINION TARGETING------------------------------------------------------------
                    //------------------------------------------------------------------------------------

                    if (projectile.minion)
                    {
                        NPC ownerMinionAttackTargetNPC2 = projectile.OwnerMinionAttackTargetNPC;
                        if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy())
                        {
                            float x = ownerMinionAttackTargetNPC2.Center.X;
                            float y = ownerMinionAttackTargetNPC2.Center.Y;
                            float num94 = Math.Abs(projectile.Center.X - x) + Math.Abs(projectile.Center.Y - y);
                            if (num94 < distance)
                            {
                                if (targetNPC == -1 && num94 <= otherDistance)
                                {
                                    otherDistance = num94;
                                    targetX = x;
                                    targetY = y;
                                }
                                if (Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
                                {
                                    distance = num94;
                                    targetX = x;
                                    targetY = y;
                                    targetNPC = ownerMinionAttackTargetNPC2.whoAmI;
                                }
                            }
                        }
                        if (targetNPC == -1)
                        {
                            for (int k = 0; k < 200; k++)
                            {
                                if (Main.npc[k].CanBeChasedBy())
                                {
                                    float npcX = Main.npc[k].Center.X;
                                    float npcY = Main.npc[k].Center.Y;
                                    float between = Math.Abs(projectile.Center.X - npcX) + Math.Abs(projectile.Center.Y - npcY);
                                    if (between < distance)
                                    {
                                        if (targetNPC == -1 && between <= otherDistance)
                                        {
                                            otherDistance = between;
                                            targetX = npcX;
                                            targetY = npcY;
                                        }
                                        if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[k].position, Main.npc[k].width, Main.npc[k].height))
                                        {
                                            distance = between;
                                            targetX = npcX;
                                            targetY = npcY;
                                            targetNPC = k;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (targetNPC == -1 && otherDistance < distance)
                    {
                        distance = otherDistance;
                    }
                    else if (targetNPC >= 0) //has target
                    {
                        toTarget = new Vector2(targetX, targetY) - projectile.Center;
                    }

                    float num104 = 300f;
                    if (projectile.position.Y > Main.worldSurface * 16.0)
                    {
                        num104 = 150f;
                    }

                    if (distance < num104 + offset && targetNPC == -1)
                    {
                        float num105 = targetX - projectile.Center.X;
                        if (num105 < -5f)
                        {
                            left = true;
                            right = false;
                        }
                        else if (num105 > 5f)
                        {
                            right = true;
                            left = false;
                        }
                    }

                    //bool flag9 = false;

                    if (targetNPC >= 0 && distance < 800f + offset)
                    {
                        projectile.friendly = true;
                        JumpTimer = 60;
                        float distanceX = targetX - projectile.Center.X;
                        if (distanceX < -10f)
                        {
                            left = true;
                            right = false;
                        }
                        else if (distanceX > 10f)
                        {
                            right = true;
                            left = false;
                        }
                        if (targetY < projectile.Center.Y - 100f && distanceX > -50f && distanceX < 50f && projectile.velocity.Y == 0f)
                        {
                            float distanceAbsY = Math.Abs(targetY - projectile.Center.Y);
                            //jumping velocities
                            if (distanceAbsY < 100f) //120f
                            {
                                projectile.velocity.Y = -10f;
                            }
                            else if (distanceAbsY < 210f)
                            {
                                projectile.velocity.Y = -13f;
                            }
                            else if (distanceAbsY < 270f)
                            {
                                projectile.velocity.Y = -15f;
                            }
                            else if (distanceAbsY < 310f)
                            {
                                projectile.velocity.Y = -17f;
                            }
                            else if (distanceAbsY < 380f)
                            {
                                projectile.velocity.Y = -18f;
                            }
                        }

                        //---------------------------------------------------------------------
                        //drop through platforms
                        //only drop if targets center y is lower than the minions bottom y, and only if its less than 300 away from the target horizontally
                        if (Main.npc[targetNPC].Center.Y > projectile.Bottom.Y && Math.Abs(toTarget.X) < 300)
                        {
                            int tilex = (int)(projectile.position.X / 16f);
                            int tiley = (int)((projectile.position.Y + projectile.height + 15f) / 16f);

                            if (TileID.Sets.Platforms[Framing.GetTileSafely(tilex, tiley).type] &&
                                TileID.Sets.Platforms[Framing.GetTileSafely(tilex + 1, tiley).type] &&
                                ((projectile.direction == -1) ? TileID.Sets.Platforms[Framing.GetTileSafely(tilex + 2, tiley).type] : true))
                            {
                                //AssUtils.Print("drop " + Main.time);
                                //Main.NewText("drop");
                                projectile.netUpdate = true;
                                projectile.position.Y += 1f;
                            }
                        }

                        if (shootSpikes)
                        {
                            //PickedTexture * 3 makes it so theres a small offset for minion shooting based on their texture, which means that if you have different slimes out,
                            //they don't all shoot in sync
                            if (ShootTimer > (shootDelay + PickedTexture * 3) && distance < 200f &&
                                Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[targetNPC].position, Main.npc[targetNPC].width, Main.npc[targetNPC].height))
                            {
                                if (Main.netMode != NetmodeID.Server && projectile.owner == Main.myPlayer)
                                {
                                    for (int k = 0; k < 3; k++)
                                    {
                                        Vector2 velo = new Vector2(k - 1, -2f);
                                        velo.X *= 1f + Main.rand.Next(-40, 41) * 0.02f;
                                        velo.Y *= 1f + Main.rand.Next(-40, 41) * 0.02f;
                                        velo.Normalize();
                                        velo *= 3f + Main.rand.Next(-40, 41) * 0.01f;
                                        Projectile.NewProjectile(projectile.Center.X, projectile.Bottom.Y - 8f, velo.X, velo.Y, ModContent.ProjectileType<SlimePackMinionSpike>(), projectile.damage / 2, 0f, Main.myPlayer, ai1: PickedTexture);
                                        ShootTimer = (byte)(PickedTexture * 3);
                                    }
                                }
                            }
                            if (ShootTimer <= shootDelay + PickedTexture * 3) ShootTimer = (byte)(ShootTimer + Main.rand.Next(2));
                        }
                    }
                    else
                    {
                        projectile.friendly = false;
                    }
                }

                if (projectile.ai[1] != 0f)
                {
                    left = false;
                    right = false;
                }
                else if (JumpTimer == 0)
                {
                    projectile.direction = player.direction;
                }

                projectile.tileCollide = true;

                float veloXthreshold = 0.2f;
                float maxVeloXthreshold = 6f;

                if (maxVeloXthreshold < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                {
                    veloXthreshold = 0.3f;
                    maxVeloXthreshold = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
                }

                if (left)
                {
                    if (projectile.velocity.X > -3.5f)
                    {
                        projectile.velocity.X -= veloXthreshold;
                    }
                    else
                    {
                        projectile.velocity.X -= veloXthreshold * 0.25f;
                    }
                }
                else if (right)
                {
                    if (projectile.velocity.X < 3.5f)
                    {
                        projectile.velocity.X += veloXthreshold;
                    }
                    else
                    {
                        projectile.velocity.X += veloXthreshold * 0.25f;
                    }
                }
                else
                {
                    projectile.velocity.X *= 0.9f;
                    if (projectile.velocity.X >= 0f - veloXthreshold && projectile.velocity.X <= veloXthreshold)
                    {
                        projectile.velocity.X = 0f;
                    }
                }

                if (left | right)
                {
                    int i = (int)projectile.Center.X / 16;
                    int j = (int)projectile.Center.Y / 16;
                    if (left)
                    {
                        i--;
                    }
                    if (right)
                    {
                        i++;
                    }
                    i += (int)projectile.velocity.X;
                    if (WorldGen.SolidTile(i, j))
                    {
                        flag4 = true;
                    }
                }
                if (player.position.Y + player.height - 8f > projectile.position.Y + projectile.height)
                {
                    flag3 = true;
                }
                Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
                if (projectile.velocity.Y == 0f)
                {
                    if (!flag3 && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
                    {
                        int i2 = (int)projectile.Center.X / 16;
                        int j2 = (int)projectile.Center.Y / 16 + 1;
                        if (left)
                        {
                            i2--;
                        }
                        if (right)
                        {
                            i2++;
                        }
                        WorldGen.SolidTile(i2, j2);
                    }
                    if (flag4)
                    {
                        int i = (int)projectile.Center.X / 16;
                        int j = (int)projectile.Bottom.Y / 16 + 1;
                        if (WorldGen.SolidTile(i, j) || Main.tile[i, j].halfBrick() || Main.tile[i, j].slope() > 0)
                        {
                            try
                            {
                                i = (int)projectile.Center.X / 16;
                                j = (int)projectile.Center.Y / 16;
                                if (left)
                                {
                                    i--;
                                }
                                if (right)
                                {
                                    i++;
                                }
                                i += (int)projectile.velocity.X;
                                if (!WorldGen.SolidTile(i, j - 1) && !WorldGen.SolidTile(i, j - 2))
                                {
                                    projectile.velocity.Y = -5.1f;
                                }
                                else if (!WorldGen.SolidTile(i, j - 2))
                                {
                                    projectile.velocity.Y = -7.1f;
                                }
                                else if (WorldGen.SolidTile(i, j - 5))
                                {
                                    projectile.velocity.Y = -11.1f;
                                }
                                else if (WorldGen.SolidTile(i, j - 4))
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
                    else if (left | right)
                    {
                        projectile.velocity.Y -= 6f;
                    }
                }
                if (projectile.velocity.X > maxVeloXthreshold)
                {
                    projectile.velocity.X = maxVeloXthreshold;
                }
                if (projectile.velocity.X < -maxVeloXthreshold)
                {
                    projectile.velocity.X = -maxVeloXthreshold;
                }
                if (projectile.velocity.X != 0f) projectile.direction = (projectile.velocity.X > 0f).ToDirectionInt();
                if (projectile.velocity.X > veloXthreshold && right)
                {
                    projectile.direction = 1;
                }
                if (projectile.velocity.X < -veloXthreshold && left)
                {
                    projectile.direction = -1;
                }
            }
        }
    }
}
