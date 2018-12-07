using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Akhult : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Akhult");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.CreatureFromTheDeep];
        }

        public override void SetDefaults()
        {
            npc.width = 98;
            npc.height = 52;
            npc.damage = 40;
            npc.defense = 18;
            npc.lifeMax = 200;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = -1; //3
            aiType = NPCID.CreatureFromTheDeep;
            animationType = -1;//NPCID.CreatureFromTheDeep;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.025f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(2))
                Item.NewItem(npc.getRect(), ItemID.Shrimp, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
            }
        }

        public override void FindFrame(int frameHeight)
        {
            //base.FindFrame(frameHeight);
            //return;
            int num2 = 0;
            //if (npc.aiAction == 0)
            //{
            //    num2 = ((npc.velocity.Y < 0f) ? 2 : ((npc.velocity.Y > 0f) ? 3 : ((npc.velocity.X != 0f) ? 1 : 0)));
            //}
            //else if (npc.aiAction == 1)
            //{
            //    num2 = 4;
            //}
            if (npc.wet)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.direction = -1;
                }
                if (npc.velocity.X > 0f)
                {
                    npc.direction = 1;
                }
                if (npc.spriteDirection != npc.direction)
                {
                    npc.rotation *= -1f;
                    npc.spriteDirection = npc.direction;
                }
                float num219 = (float)Math.Atan2((double)(npc.velocity.Y * (float)npc.direction), (double)(npc.velocity.X * (float)npc.direction));
                if ((double)Math.Abs(npc.rotation - num219) >= 3.14)
                {
                    if (num219 < npc.rotation)
                    {
                        npc.rotation -= 6.28f;
                    }
                    else
                    {
                        npc.rotation += 6.28f;
                    }
                }
                npc.rotation = (npc.rotation * 4f + num219) / 5f;
                npc.frameCounter += (double)Math.Abs(npc.velocity.Length());
                npc.frameCounter += 1.0;
                if (npc.frameCounter > 8.0)
                {
                    npc.frame.Y += frameHeight;
                    npc.frameCounter = 0.0;
                }
                if (npc.frame.Y / frameHeight > 20)
                {
                    npc.frame.Y = frameHeight * 16;
                }
                else if (npc.frame.Y / frameHeight < 16)
                {
                    npc.frame.Y = frameHeight * 19;
                }
            }
            else
            {
                if ((double)npc.rotation > 3.14)
                {
                    npc.rotation -= 6.28f;
                }
                if ((double)npc.rotation > -0.01 && (double)npc.rotation < 0.01)
                {
                    npc.rotation = 0f;
                }
                else
                {
                    npc.rotation *= 0.9f;
                }
                if (npc.velocity.Y != 0f)
                {
                    npc.frameCounter = 0.0;
                    npc.frame.Y = frameHeight;
                }
                else
                {
                    if (npc.direction == 1)
                    {
                        npc.spriteDirection = 1;
                    }
                    if (npc.direction == -1)
                    {
                        npc.spriteDirection = -1;
                    }
                    if (npc.velocity.X == 0f)
                    {
                        npc.frame.Y = 0;
                        npc.frameCounter = 0.0;
                    }
                    else
                    {
                        npc.frameCounter += (double)(Math.Abs(npc.velocity.X) * 1.5f); //2f
                        npc.frameCounter += 1.0;
                        if (npc.frameCounter > 6.0)
                        {
                            npc.frame.Y += frameHeight;
                            npc.frameCounter = 0.0;
                        }
                        if (npc.frame.Y / frameHeight > 15)
                        {
                            npc.frame.Y = frameHeight * 2;
                        }
                    }
                }
            }
        }

        public override void AI()
        {
            bool flag3 = false;
            if (npc.velocity.X == 0f)
            {
                flag3 = true;
            }
            if (npc.justHit)
            {
                flag3 = false;
            }
            int num43 = 60;

            if (npc.wet)
            {
                npc.knockBackResist = 0f;
                npc.ai[3] = -0.10101f;
                npc.noGravity = true;
                Vector2 center = npc.Center;
                npc.width = 98;
                npc.height = 52;
                npc.position.X = center.X - (float)(npc.width / 2);
                npc.position.Y = center.Y - (float)(npc.height / 2);
                npc.TargetClosest();
                if (npc.collideX)
                {
                    npc.velocity.X = 0f - npc.oldVelocity.X;
                }
                if (npc.velocity.X < 0f)
                {
                    npc.direction = -1;
                }
                if (npc.velocity.X > 0f)
                {
                    npc.direction = 1;
                }
                if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].Center, 1, 1))
                {
                    Vector2 vector = Main.player[npc.target].Center - npc.Center;
                    vector.Normalize();
                    vector *= 5f;
                    npc.velocity = (npc.velocity * 19f + vector) / 20f;
                }
                else
                {
                    float num2 = 5f;
                    if (npc.velocity.Y > 0f)
                    {
                        num2 = 3f;
                    }
                    if (npc.velocity.Y < 0f)
                    {
                        num2 = 8f;
                    }
                    Vector2 vector2 = new Vector2((float)npc.direction, -1f);
                    vector2.Normalize();
                    vector2 *= num2;
                    if (num2 < 5f)
                    {
                        npc.velocity = (npc.velocity * 24f + vector2) / 25f;
                    }
                    else
                    {
                        npc.velocity = (npc.velocity * 9f + vector2) / 10f;
                    }
                }
                return;
            }
            npc.knockBackResist = 0.4f * Main.knockBackMultiplier;
            npc.noGravity = false;
            Vector2 center2 = npc.Center;
            npc.width = 98;
            npc.height = 52;
            npc.position.X = center2.X - (float)(npc.width / 2);
            npc.position.Y = center2.Y - (float)(npc.height / 2);
            if (npc.ai[3] == -0.10101f)
            {
                npc.ai[3] = 0f;
                float num3 = npc.velocity.Length();
                num3 *= 2f;
                if (num3 > 10f)
                {
                    num3 = 10f;
                }
                npc.velocity.Normalize();
                npc.velocity *= num3;
                if (npc.velocity.X < 0f)
                {
                    npc.direction = -1;
                }
                if (npc.velocity.X > 0f)
                {
                    npc.direction = 1;
                }
                npc.spriteDirection = npc.direction;
            }


            //
            if (npc.ai[3] < (float)num43 && (Main.eclipse || !Main.dayTime || (double)npc.position.Y > Main.worldSurface * 16.0))
            {
                npc.TargetClosest();
            }
            else if (npc.ai[2] <= 0f)
            {
                if (Main.dayTime && (double)(npc.position.Y / 16f) < Main.worldSurface && npc.timeLeft > 10)
                {
                    npc.timeLeft = 10;
                }
                if (npc.velocity.X == 0f)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.ai[0] += 1f;
                        if (npc.ai[0] >= 2f)
                        {
                            npc.direction *= -1;
                            npc.spriteDirection = npc.direction;
                            npc.ai[0] = 0f;
                        }
                    }
                }
                else
                {
                    npc.ai[0] = 0f;
                }
                if (npc.direction == 0)
                {
                    npc.direction = 1;
                }
            }




            bool flag4 = false;
            bool flag5 = false;
            bool flag6 = false;
            bool flag7 = true;
            if (npc.ai[2] > 0f)
            {
                flag7 = false;
            }

            if (!flag6 && flag7)
            {
                if (npc.velocity.Y == 0f && ((npc.velocity.X > 0f && npc.direction < 0) || (npc.velocity.X < 0f && npc.direction > 0)))
                {
                    flag4 = true;
                }
                if ((npc.position.X == npc.oldPosition.X || npc.ai[3] >= (float)num43) | flag4)
                {
                    npc.ai[3] += 1f;
                }
                else if ((double)Math.Abs(npc.velocity.X) > 1.8 && npc.ai[3] > 0f) //0.9
                {
                    npc.ai[3] -= 1f;
                }
                if (npc.ai[3] > (float)(num43 * 10))
                {
                    npc.ai[3] = 0f;
                }
                if (npc.justHit)
                {
                    npc.ai[3] = 0f;
                }
                if (npc.ai[3] == (float)num43)
                {
                    npc.netUpdate = true;
                }
            }

            //VELOCITY CALCULATIONS HERE
            if (npc.velocity.X < -3f || npc.velocity.X > 3f) //all 2 or -2
            {
                if (npc.velocity.Y == 0f)
                {
                    npc.velocity *= 0.8f;
                }
            }
            else if (npc.velocity.X < 3f && npc.direction == 1)
            {
                npc.velocity.X += 0.06f; //0.07f
                if (npc.velocity.X > 3f)
                {
                    npc.velocity.X = 3f;
                }
            }
            else if (npc.velocity.X > -3f && npc.direction == -1)
            {
                npc.velocity.X -= 0.06f;
                if (npc.velocity.X < -3f)
                {
                    npc.velocity.X = -3f;
                }
            }
            
            bool flag22 = false;
            if (npc.velocity.Y == 0f)
            {
                int num178 = (int)(npc.position.Y + (float)npc.height + 7f) / 16;
                int num179 = (int)npc.position.X / 16;
                int num180 = (int)(npc.position.X + (float)npc.width) / 16;
                int num28;
                for (int num181 = num179; num181 <= num180; num181 = num28 + 1)
                {
                    if (Main.tile[num181, num178] == null)
                    {
                        return;
                    }
                    if (Main.tile[num181, num178].nactive() && Main.tileSolid[Main.tile[num181, num178].type])
                    {
                        flag22 = true;
                        break;
                    }
                    num28 = num181;
                }
            }
            if (npc.velocity.Y >= 0f)
            {
                int num182 = 0;
                if (npc.velocity.X < 0f)
                {
                    num182 = -1;
                }
                if (npc.velocity.X > 0f)
                {
                    num182 = 1;
                }
                Vector2 position2 = npc.position;
                position2.X += npc.velocity.X;
                int num183 = (int)((position2.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 1) * num182)) / 16f);
                int num184 = (int)((position2.Y + (float)npc.height - 1f) / 16f);
                if (Main.tile[num183, num184] == null)
                {
                    Tile[,] tile3 = Main.tile;
                    int num185 = num183;
                    int num186 = num184;
                    Tile tile4 = new Tile();
                    tile3[num185, num186] = tile4;
                }
                if (Main.tile[num183, num184 - 1] == null)
                {
                    Tile[,] tile5 = Main.tile;
                    int num187 = num183;
                    int num188 = num184 - 1;
                    Tile tile6 = new Tile();
                    tile5[num187, num188] = tile6;
                }
                if (Main.tile[num183, num184 - 2] == null)
                {
                    Tile[,] tile7 = Main.tile;
                    int num189 = num183;
                    int num190 = num184 - 2;
                    Tile tile8 = new Tile();
                    tile7[num189, num190] = tile8;
                }
                if (Main.tile[num183, num184 - 3] == null)
                {
                    Tile[,] tile9 = Main.tile;
                    int num191 = num183;
                    int num192 = num184 - 3;
                    Tile tile10 = new Tile();
                    tile9[num191, num192] = tile10;
                }
                if (Main.tile[num183, num184 + 1] == null)
                {
                    Tile[,] tile11 = Main.tile;
                    int num193 = num183;
                    int num194 = num184 + 1;
                    Tile tile12 = new Tile();
                    tile11[num193, num194] = tile12;
                }
                if (Main.tile[num183 - num182, num184 - 3] == null)
                {
                    Tile[,] tile13 = Main.tile;
                    int num195 = num183 - num182;
                    int num196 = num184 - 3;
                    Tile tile14 = new Tile();
                    tile13[num195, num196] = tile14;
                }
                if ((float)(num183 * 16) < position2.X + (float)npc.width && (float)(num183 * 16 + 16) > position2.X && ((Main.tile[num183, num184].nactive() && !Main.tile[num183, num184].topSlope() && !Main.tile[num183, num184 - 1].topSlope() && Main.tileSolid[Main.tile[num183, num184].type] && !Main.tileSolidTop[Main.tile[num183, num184].type]) || (Main.tile[num183, num184 - 1].halfBrick() && Main.tile[num183, num184 - 1].nactive())) && (!Main.tile[num183, num184 - 1].nactive() || !Main.tileSolid[Main.tile[num183, num184 - 1].type] || Main.tileSolidTop[Main.tile[num183, num184 - 1].type] || (Main.tile[num183, num184 - 1].halfBrick() && (!Main.tile[num183, num184 - 4].nactive() || !Main.tileSolid[Main.tile[num183, num184 - 4].type] || Main.tileSolidTop[Main.tile[num183, num184 - 4].type]))) && (!Main.tile[num183, num184 - 2].nactive() || !Main.tileSolid[Main.tile[num183, num184 - 2].type] || Main.tileSolidTop[Main.tile[num183, num184 - 2].type]) && (!Main.tile[num183, num184 - 3].nactive() || !Main.tileSolid[Main.tile[num183, num184 - 3].type] || Main.tileSolidTop[Main.tile[num183, num184 - 3].type]) && (!Main.tile[num183 - num182, num184 - 3].nactive() || !Main.tileSolid[Main.tile[num183 - num182, num184 - 3].type]))
                {
                    float num197 = (float)(num184 * 16);
                    if (Main.tile[num183, num184].halfBrick())
                    {
                        num197 += 8f;
                    }
                    if (Main.tile[num183, num184 - 1].halfBrick())
                    {
                        num197 -= 8f;
                    }
                    if (num197 < position2.Y + (float)npc.height)
                    {
                        float num198 = position2.Y + (float)npc.height - num197;
                        float num199 = 16.1f;
                        if (num198 <= num199)
                        {
                            npc.gfxOffY += npc.position.Y + (float)npc.height - num197;
                            npc.position.Y = num197 - (float)npc.height;
                            if (num198 < 9f)
                            {
                                npc.stepSpeed = 1f;
                            }
                            else
                            {
                                npc.stepSpeed = 2f;
                            }
                        }
                    }
                }
            }
            if (flag22)
            {
                int num200 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                int num201 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
                if (Main.tile[num200, num201] == null)
                {
                    Tile[,] tile15 = Main.tile;
                    int num202 = num200;
                    int num203 = num201;
                    Tile tile16 = new Tile();
                    tile15[num202, num203] = tile16;
                }
                if (Main.tile[num200, num201 - 1] == null)
                {
                    Tile[,] tile17 = Main.tile;
                    int num204 = num200;
                    int num205 = num201 - 1;
                    Tile tile18 = new Tile();
                    tile17[num204, num205] = tile18;
                }
                if (Main.tile[num200, num201 - 2] == null)
                {
                    Tile[,] tile19 = Main.tile;
                    int num206 = num200;
                    int num207 = num201 - 2;
                    Tile tile20 = new Tile();
                    tile19[num206, num207] = tile20;
                }
                if (Main.tile[num200, num201 - 3] == null)
                {
                    Tile[,] tile21 = Main.tile;
                    int num208 = num200;
                    int num209 = num201 - 3;
                    Tile tile22 = new Tile();
                    tile21[num208, num209] = tile22;
                }
                if (Main.tile[num200, num201 + 1] == null)
                {
                    Tile[,] tile23 = Main.tile;
                    int num210 = num200;
                    int num211 = num201 + 1;
                    Tile tile24 = new Tile();
                    tile23[num210, num211] = tile24;
                }
                if (Main.tile[num200 + npc.direction, num201 - 1] == null)
                {
                    Tile[,] tile25 = Main.tile;
                    int num212 = num200 + npc.direction;
                    int num213 = num201 - 1;
                    Tile tile26 = new Tile();
                    tile25[num212, num213] = tile26;
                }
                if (Main.tile[num200 + npc.direction, num201 + 1] == null)
                {
                    Tile[,] tile27 = Main.tile;
                    int num214 = num200 + npc.direction;
                    int num215 = num201 + 1;
                    Tile tile28 = new Tile();
                    tile27[num214, num215] = tile28;
                }
                if (Main.tile[num200 - npc.direction, num201 + 1] == null)
                {
                    Tile[,] tile29 = Main.tile;
                    int num216 = num200 - npc.direction;
                    int num217 = num201 + 1;
                    Tile tile30 = new Tile();
                    tile29[num216, num217] = tile30;
                }
                Main.tile[num200, num201 + 1].halfBrick();
                if ((Main.tile[num200, num201 - 1].nactive() && (TileLoader.IsClosedDoor(Main.tile[num200, num201 - 1]) || Main.tile[num200, num201 - 1].type == 388)) & flag5)
                {
                    npc.ai[2] += 1f;
                    npc.ai[3] = 0f;
                    if (npc.ai[2] >= 60f)
                    {
                        npc.velocity.X = 0.5f * (0f - (float)npc.direction);
                        int num218 = 5;
                        if (Main.tile[num200, num201 - 1].type == 388)
                        {
                            num218 = 2;
                        }
                        npc.ai[1] += (float)num218;
                        npc.ai[2] = 0f;
                        bool flag23 = false;
                        if (npc.ai[1] >= 10f)
                        {
                            flag23 = true;
                            npc.ai[1] = 10f;
                        }
                        WorldGen.KillTile(num200, num201 - 1, true);
                        if (((Main.netMode != 1 || !flag23) & flag23) && Main.netMode != 1)
                        {
                            if (TileLoader.OpenDoorID(Main.tile[num200, num201 - 1]) >= 0)
                            {
                                bool flag24 = WorldGen.OpenDoor(num200, num201 - 1, npc.direction);
                                if (!flag24)
                                {
                                    npc.ai[3] = (float)num43;
                                    npc.netUpdate = true;
                                }
                                if (Main.netMode == 2 && flag24)
                                {
                                    NetMessage.SendData(19, -1, -1, null, 0, (float)num200, (float)(num201 - 1), (float)npc.direction);
                                }
                            }
                            if (Main.tile[num200, num201 - 1].type == 388)
                            {
                                bool flag25 = WorldGen.ShiftTallGate(num200, num201 - 1, false);
                                if (!flag25)
                                {
                                    npc.ai[3] = (float)num43;
                                    npc.netUpdate = true;
                                }
                                if (Main.netMode == 2 && flag25)
                                {
                                    NetMessage.SendData(19, -1, -1, null, 4, (float)num200, (float)(num201 - 1));
                                }
                            }
                        }
                    }
                }
                else
                {
                    int num219 = npc.spriteDirection;
                    if ((npc.velocity.X < 0f && num219 == -1) || (npc.velocity.X > 0f && num219 == 1))
                    {
                        if (npc.height >= 32 && Main.tile[num200, num201 - 2].nactive() && Main.tileSolid[Main.tile[num200, num201 - 2].type])
                        {
                            if (Main.tile[num200, num201 - 3].nactive() && Main.tileSolid[Main.tile[num200, num201 - 3].type])
                            {
                                npc.velocity.Y = -8f;
                                npc.netUpdate = true;
                            }
                            else
                            {
                                npc.velocity.Y = -7f;
                                npc.netUpdate = true;
                            }
                        }
                        else if (Main.tile[num200, num201 - 1].nactive() && Main.tileSolid[Main.tile[num200, num201 - 1].type])
                        {
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                        else if (npc.position.Y + (float)npc.height - (float)(num201 * 16) > 20f && Main.tile[num200, num201].nactive() && !Main.tile[num200, num201].topSlope() && Main.tileSolid[Main.tile[num200, num201].type])
                        {
                            npc.velocity.Y = -5f;
                            npc.netUpdate = true;
                        }
                        else if (npc.directionY < 0 && (!Main.tile[num200, num201 + 1].nactive() || !Main.tileSolid[Main.tile[num200, num201 + 1].type]) && (!Main.tile[num200 + npc.direction, num201 + 1].nactive() || !Main.tileSolid[Main.tile[num200 + npc.direction, num201 + 1].type]))
                        {
                            npc.velocity.Y = -8f;
                            npc.velocity.X *= 1.5f;
                            npc.netUpdate = true;
                        }
                        else if (flag5)
                        {
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                        }
                        if (npc.velocity.Y == 0f && flag3 && npc.ai[3] == 1f)
                        {
                            npc.velocity.Y = -5f;
                        }
                    }
                }
            }
            else if (flag5)
            {
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
            }
        }
    }
}
