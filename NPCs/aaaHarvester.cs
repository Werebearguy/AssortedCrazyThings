using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class aaaHarvester : ModNPC
    {
        public static string name = "aaaHarvester";
        //only one is alive at all times so I can use field variables instead of npc.ai[]

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DemonEye];
        }

        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 46;
            npc.damage = 18;
            npc.defense = 2;
            npc.lifeMax = 60;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = -1; //91;
            aiType = NPCID.Zombie; //91
            animationType = NPCID.DemonEye;
            npc.buffImmune[BuffID.Confused] = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((bool)resting);
            writer.Write((bool)aiTargetType);
            writer.Write((byte)stuckTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            resting = reader.ReadBoolean();
            aiTargetType = reader.ReadBoolean();
            stuckTimer = reader.ReadByte();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.ZoneDungeon && !NPC.AnyNPCs(mod.NPCType(AssWorld.harvesterName)))
            {
                //only spawns when in dungeon and when no other is alive atm
                return 0.1f;
            }
            return 0f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(33))
                Item.NewItem(npc.getRect(), ItemID.Lens, 1);
            if (Main.rand.NextBool(100))
                Item.NewItem(npc.getRect(), ItemID.BlackLens, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
            }
        }

        protected const int AI_State_Slot = 0;
        protected const int AI_X_Timer_Slot = 1;
        protected const int AI_Y_Slot = 2;
        protected const int AI_Timer_Slot = 3;
        protected const bool Target_Player = false;
        protected const bool Target_Soul = true;

        protected const float State_Distribute = 0f;
        protected const float State_FoundMove = 1f;
        protected const float State_Noclip = 2f;
        protected const float State_IdleMove = 3f;
        protected const float State_Recalculate = 4f;
        protected const float State_Stop = 5f;
        protected const float State_Transform = 6f;

        public int target = 0;
        public byte stuckTimer = 0;
        private static readonly float maxVeloScale = 1.3f; //2f default
        private static readonly float maxAccScale = 0.04f; //0.07f default
        private static readonly byte stuckTime = 5; //*60 for ticks
        private static readonly float eatTime = 30f;
        private static readonly float idleTime = 180f;
        private static readonly float aiFighterLimit = 240f;
        public float stopTime = idleTime;
        public bool resting = false;
        public bool aiTargetType = Target_Soul;

        public float AI_State
        {
            get
            {
                return npc.ai[AI_State_Slot];
            }
            set
            {
                npc.ai[AI_State_Slot] = value;
            }
        }
		
		public float AI_X_Timer
        {
            get
            {
                return npc.ai[AI_X_Timer_Slot];
            }
            set
            {
                npc.ai[AI_X_Timer_Slot] = value;
            }
        }
		
		public float AI_Y
        {
            get
            {
                return npc.ai[AI_Y_Slot];
            }
            set
            {
                npc.ai[AI_Y_Slot] = value;
            }
        }

        public float AI_Timer
        {
            get
            {
                return npc.ai[AI_Timer_Slot];
            }
            set
            {
                npc.ai[AI_Timer_Slot] = value;
            }
        }

        public float AI_Local_Timer
        {
            get
            {
                return npc.localAI[0];
            }
            set
            {
                npc.localAI[0] = value;
            }
        }

        public float AI_Init
        {
            get
            {
                return npc.localAI[1];
            }
            set
            {
                npc.localAI[1] = value;
            }
        }

        protected int SelectTarget(string select = "n")
        {
            //0 is player, 1 is soul
            if (select != "n") {
                if (select == "s")
                {
                    target = SoulTargetClosest();
                    if (target == 200)
                    {
                        //some new Idle state
                        AI_State = State_Stop;
                    }
                }
                else if (select == "p") //Target_Player
                {
                    npc.TargetClosest();
                    target = npc.target;
                }
            }
            else
            {
                if (aiTargetType == Target_Soul)
                {
                    target = SoulTargetClosest();
                    if (target == 200)
                    {
                        //some new Idle state
                        AI_State = State_Stop;
                    }
                }
                else if (aiTargetType == Target_Player) //Target_Player
                {
                    npc.TargetClosest();
                    target = npc.target;
                }
            }
            return target;
        }

        protected int SoulTargetClosest()
        {
            int closest = 200;
            Vector2 soulPos = Vector2.Zero;
            float oldDistance = 1000000000f;
            float newDistance = oldDistance;
            //return index of closest soul
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && Main.npc[j].type == mod.NPCType(AssWorld.soulName))
                {
                    soulPos = Main.npc[j].Center - npc.Center;
                    newDistance = soulPos.Length();
                    if(newDistance < oldDistance)
                    {
                        oldDistance = newDistance;
                        closest = j;
                    }
                }
            }
            //NEED TO CATCH "==200" WHEN CALLING THIS 
            //Main.NewText("target " + closest + " " + Main.npc[closest].TypeName);
            return closest; //to self
        }

        protected Entity GetTarget(string select = "n")
        {
            if (select != "n")
            {
                if (select == "s")
                {
                    return Main.npc[target];
                }
                else if (select == "p")
                {
                    return Main.player[target];
                }
                return this.npc;
            }
            else
            {
                if (aiTargetType == Target_Soul)
                {
                    return Main.npc[target];
                }
                else //Target_Player
                {
                    return Main.player[target];
                }
            }
        }

        protected bool IsTargetActive()
        {
            return GetTarget().active;
        }

        protected void SetTimeLeft()
        {
            //only for Soul
            if (aiTargetType == Target_Soul)
            {
                NPC tar = (NPC)GetTarget();
                if (tar.active && tar.type == mod.NPCType(AssWorld.soulName)) //type check since souls might despawn and index changes
                {
                    tar.timeLeft = (int)eatTime + 5;
                    tar.netUpdate = true;
                }
            }
        }

        protected void UpdateStuck(bool closeToSoulVar)
        {
            //TODO make it so it compares the distance of its last position to current (once every second, Main.time) and checks if distance changed significantly
            //something like
            //if distance.Y < 60f : //5 tiles
            //  if(distance.X < 60f :
            //      some timer + 1
            //if timer > threshold
            //  goto noclip
            Vector2 between = new Vector2(0f, GetTarget(select: "s").Center.Y - npc.Center.Y);
            //Main.NewText((npc.collideX || npc.collideY) &&
            //    !closeToSoulVar &&
            //    (!Collision.CanHit(npc.Center, 1, 1, GetTarget().Center, 1, 1) || between.Y < 0f && between.Y > -300f));
            if (Main.time % 60 == 0 &&
                (npc.collideX || npc.collideY) &&
                !closeToSoulVar &&
                (!Collision.CanHit(npc.Center, 1, 1, GetTarget().Center, 1, 1) || between.Y > 0f))
            {
                
                Main.NewText("TICK TOCK " + npc.collideX + " " + npc.collideY);
                between = new Vector2(Math.Abs(npc.position.X - AI_X_Timer), Math.Abs(npc.position.Y - AI_Y));
                Main.NewText(between);
                if (between.Y > 60f || between.X > 60f)
                {
                    Main.NewText("NOT stuck actually");
                    stuckTimer = 0;
                }
                else if (between.Y < 60f)
                {
                    if (between.X < 60f)
                    {
                        Main.NewText("stucktimer++");
                        stuckTimer++;
                        npc.netUpdate = true;
                    }
                }
                if (stuckTimer >= stuckTime)
                {
                    Main.NewText("DOOR STUCK");
                    stuckTimer = 0;
                }
            }
            if (Main.time % 60 == 0) //do these always
            {
                AI_X_Timer = npc.position.X;
                AI_Y = npc.position.Y;
            }
        }

        protected void UpdateVelocity()
        {
            Vector2 between = new Vector2(Math.Abs(GetTarget(select: "s").Center.X - npc.Center.X), GetTarget(select: "s").Center.Y - npc.Center.Y);
            bool lockedX = false;

            if (between.X < 1f && Collision.CanHit(npc.Center, 1, 1, GetTarget().Center, 1, 1) && between.Y < 0f && between.Y > -300f)
            {
                //actually only locked when direct LOS and not too high
                //npc.position.X = GetTarget(select: "s").Center.X - GetTarget(select: "s").width; //centered on the center of the soul
                lockedX = true;
            }

            float veloScale = maxVeloScale; //2f default
            float accScale = maxAccScale; //0.07f default

            //VELOCITY CALCULATIONS HERE
            if (!lockedX)
            {

                if (between.X < 50f && Math.Abs(between.Y) < 24f)
                {
                    veloScale = maxVeloScale * 0.4f; //when literally near the soul
                }

                if (npc.velocity.X < -veloScale || npc.velocity.X > veloScale)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity *= 0.8f;
                    }
                }
                else if (npc.velocity.X < veloScale && npc.direction == 1)
                {
                    npc.velocity.X += accScale;
                    if (npc.velocity.X > veloScale)
                    {
                        npc.velocity.X = veloScale;
                    }
                }
                else if (npc.velocity.X > -veloScale && npc.direction == -1)
                {
                    npc.velocity.X -= accScale;
                    if (npc.velocity.X < -veloScale)
                    {
                        npc.velocity.X = -veloScale;
                    }
                }
            }
            else
            {
                npc.velocity.X = Vector2.Zero.X;
                if (npc.velocity.Y == 0 && between.Y < -32f) //jump when below two tiles
                {
                    npc.velocity.Y = (float)(Math.Sqrt((double)-between.Y) * -0.84f);
                    npc.direction = -1;
                    npc.netUpdate = true;
                }
            }
        }

        protected void UpdateOtherMovement(bool flag3var)
        {
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
                if ((float)(num183 * 16) < position2.X + (float)npc.width &&
                    (float)(num183 * 16 + 16) > position2.X &&
                    ((Main.tile[num183, num184].nactive() &&
                    !Main.tile[num183, num184].topSlope() &&
                    !Main.tile[num183, num184 - 1].topSlope() &&
                    Main.tileSolid[Main.tile[num183, num184].type] &&
                    !Main.tileSolidTop[Main.tile[num183, num184].type]) ||
                    (Main.tile[num183, num184 - 1].halfBrick() &&
                    Main.tile[num183, num184 - 1].nactive())) &&

                    (!Main.tile[num183, num184 - 1].nactive() ||
                    !Main.tileSolid[Main.tile[num183, num184 - 1].type] ||
                    Main.tileSolidTop[Main.tile[num183, num184 - 1].type] ||
                    (Main.tile[num183, num184 - 1].halfBrick() &&
                    (!Main.tile[num183, num184 - 4].nactive() ||
                    !Main.tileSolid[Main.tile[num183, num184 - 4].type] ||
                    Main.tileSolidTop[Main.tile[num183, num184 - 4].type]))) &&

                    (!Main.tile[num183, num184 - 2].nactive() ||
                    !Main.tileSolid[Main.tile[num183, num184 - 2].type] ||
                    Main.tileSolidTop[Main.tile[num183, num184 - 2].type]) &&

                    (!Main.tile[num183, num184 - 3].nactive() ||
                    !Main.tileSolid[Main.tile[num183, num184 - 3].type] ||
                    Main.tileSolidTop[Main.tile[num183, num184 - 3].type]) &&

                    (!Main.tile[num183 - num182, num184 - 3].nactive() ||
                    !Main.tileSolid[Main.tile[num183 - num182, num184 - 3].type]))
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

                            //go up slopes/halfbricks
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





                //adjusted here


                //int num200 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                //int num201 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
                if (!(Main.tile[num200, num201 - 1].nactive() && (TileLoader.IsClosedDoor(Main.tile[num200, num201 - 1]) || Main.tile[num200, num201 - 1].type == 388)))
                {
                    //Main.NewText("" + num200 + " " + num201);
                    //int num219 = npc.spriteDirection;
                    if ((npc.velocity.X < 0f && npc.spriteDirection == -1) || (npc.velocity.X > 0f && npc.spriteDirection == 1))
                    {
                        //if (npc.height >= 32 && Main.tile[num200, num201 - 2].nactive() && Main.tileSolid[Main.tile[num200, num201 - 2].type])
                        //{
                        //    if (Main.tile[num200, num201 - 3].nactive() && Main.tileSolid[Main.tile[num200, num201 - 3].type])
                        //    {
                        //        Main.NewText("1111");
                        //        npc.velocity.Y = -8f;
                        //        npc.netUpdate = true;
                        //    }
                        //    else
                        //    {
                        //        Main.NewText("2222");
                        //        npc.velocity.Y = -7f;
                        //        npc.netUpdate = true;
                        //    }
                        //}
                        //else if (Main.tile[num200, num201 - 1].nactive() && Main.tileSolid[Main.tile[num200, num201 - 1].type])
                        //{
                        //    Main.NewText("3333");
                        //    npc.velocity.Y = -6f;
                        //    npc.netUpdate = true;
                        //}
                        //else if (npc.position.Y + (float)npc.height - (float)(num201 * 16) > 20f && Main.tile[num200, num201].nactive() && !Main.tile[num200, num201].topSlope() && Main.tileSolid[Main.tile[num200, num201].type])
                        //{
                        //    Main.NewText("4444");
                        //    npc.velocity.Y = -5f;
                        //    npc.netUpdate = true;
                        //}
                        //else if (npc.directionY < 0 && (!Main.tile[num200, num201 + 1].nactive() || !Main.tileSolid[Main.tile[num200, num201 + 1].type]) && (!Main.tile[num200 + npc.direction, num201 + 1].nactive() || !Main.tileSolid[Main.tile[num200 + npc.direction, num201 + 1].type]))
                        //{
                        //    //this is for when player stands on an elevation and it just jumped aswell
                        //    Main.NewText("5555");
                        //    npc.velocity.Y = -8f;
                        //    npc.velocity.X *= 1.5f;
                        //    npc.netUpdate = true;
                        //}
                        //if (npc.velocity.Y == 0f && flag3 && false/* && aiFighter == 1f*/)
                        //{
                        //    Main.NewText("6666");
                        //    npc.velocity.Y = -5f;
                        //}

                        //heck this ima do this MY way
                        if (npc.velocity.Y == 0f && flag3var)
                        {
                            float rnd = (float)Main.rand.Next(5, 30);
                            if (rnd < 7f)
                            {
                                npc.velocity.Y = -rnd - 0.5f;
                                npc.netUpdate = true;
                            }
                        }
                    }

                    //this bit is for "frantically jump when close to the player
                    //if (npc.velocity.Y == 0f && Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) < 100f && Math.Abs(npc.position.Y + (float)(npc.height / 2) - (Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2))) < 50f && ((npc.direction > 0 && npc.velocity.X >= 1f) || (npc.direction < 0 && npc.velocity.X <= -1f)))
                    //{
                    //    npc.velocity.X *= 2f;
                    //    if (npc.velocity.X > 3f)
                    //    {
                    //        npc.velocity.X = 3f;
                    //    }
                    //    if (npc.velocity.X < -3f)
                    //    {
                    //        npc.velocity.X = -3f;
                    //    }
                    //    npc.velocity.Y = -4f;
                    //    npc.netUpdate = true;
                    //}
                }
            }
        }

        protected void GenericGraniteAI()
        {
            //Collision.CanHit == is there direct line of sight from a to b
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.dontTakeDamage = false;

            if (AI_Init == 0)
            {
                //initialize it to go for souls first
                aiTargetType = Target_Soul;
                stopTime = idleTime;
                resting = false;
                SelectTarget();
                AI_Init = 1;
            }

            if (!IsTargetActive())
            {
                stopTime = idleTime;
                AI_State = State_Stop;
            }

            if (AI_State == State_Distribute/*0*/)
            {
                //Main.NewText("distribute");

                SelectTarget();
                //npc.netUpdate = true;
                if (AI_State == State_Stop)
                {
                    return;
                }

                //npc.TargetClosest();
                if (Collision.CanHit(npc.Center, 1, 1, GetTarget().Center, 1, 1))
                {
                    if (stopTime == eatTime)
                    {
                        Main.NewText("distribute to stop");
                        //sitting on soul, go into stop/eat mode
                        AI_State = State_Stop; //start to eat
                    }
                    else
                    {
                        //go into regular mode
                        AI_State = State_FoundMove;
                    }
                }
                else
                {
                    Vector2 between = GetTarget().Center - npc.Center;
                    between.Y -= (float)(GetTarget().height / 4);
                    float distance = between.Length();
                    if (distance > 800f)
                    {
                        //go into notilecollide mode
                        AI_State = State_Noclip;
                    }
                    else
                    {
                        Vector2 centerAlignedX = npc.Center;
                        centerAlignedX.X = GetTarget().Center.X;
                        Vector2 vector235 = centerAlignedX - npc.Center;
                        if (vector235.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, centerAlignedX, 1, 1))
                        {
                            //player unreachable now
                            AI_State = State_IdleMove;
                            AI_X_Timer = centerAlignedX.X;
                            AI_Y = centerAlignedX.Y;
                            Vector2 centerAlignedY = npc.Center;
                            centerAlignedY.Y = GetTarget().Center.Y;
                            if (vector235.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, centerAlignedY, 1, 1) && Collision.CanHit(centerAlignedY, 1, 1, GetTarget().position, 1, 1))
                            {
                                //player unreachable now
                                AI_State = State_IdleMove;
                                AI_X_Timer = centerAlignedY.X;
                                AI_Y = centerAlignedY.Y;
                            }
                        }
                        else
                        {
                            Vector2 centerAlignedY = npc.Center;
                            centerAlignedY.Y = GetTarget().Center.Y;
                            if ((centerAlignedY - npc.Center).Length() > 8f && Collision.CanHit(npc.Center, 1, 1, centerAlignedY, 1, 1))
                            {
                                //player unreachable now 
                                AI_State = State_IdleMove;
                                AI_X_Timer = centerAlignedY.X;
                                AI_Y = centerAlignedY.Y;
                            }
                        }
                        if (AI_State == State_Distribute)
                        {
                            npc.localAI[0] = 0f;
                            between.Normalize();
                            between *= 0.5f;
                            npc.velocity += between;
                            //just collided / cant find player
                            AI_State = State_Recalculate;
                            AI_X_Timer = 0f;
                        }
                    }
                }
            }
            else if (AI_State == State_FoundMove/*1*/)
            {
                //Main.NewText("regular, found target");
                Vector2 between = GetTarget().Center - npc.Center;
                float distance = between.Length();
                float factor = 2f;
                factor += distance / 200f;
                int acc = 50;
                between.Normalize();
                between *= factor;
                npc.velocity = (npc.velocity * (acc - 1) + between) / acc;
                if (!Collision.CanHit(npc.Center, 1, 1, GetTarget().Center, 1, 1))
                {
                    AI_State = State_Distribute;
                    AI_X_Timer = 0f;
                }
                if (distance < 0.1f && Collision.CanHit(npc.Center, 1, 1, GetTarget().Center, 1, 1))
                {
                    Main.NewText("set do distribute");
                    SetTimeLeft();
                    stopTime = eatTime;
                    AI_State = State_Distribute;
                    AI_X_Timer = 0f;
                }

            }
            else if (AI_State == State_Noclip/*2*/)
            {
                //Main.NewText("thru wall");
                npc.noTileCollide = true;
                Vector2 between = GetTarget().Center - npc.Center;
                float distance = between.Length();
                float factor = 2f; //2f
                int acc = 4; //4
                between.Normalize();
                between *= factor;
                npc.velocity = (npc.velocity * (acc - 1) + between) / acc;
                if (distance < 50f /*600f*/ && !Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    AI_State = State_Distribute;
                }
            }
            else if (AI_State == State_IdleMove/*3*/)
            {
                npc.noGravity = false;
                //player unreachable now but distance less than 800f, gets parameters AI_X_Timer, AI_Y passed by State_Distribute and State_Recalculate
                //Main.NewText("unreachable");
                Vector2 value42 = new Vector2(AI_X_Timer, AI_Y);
                Vector2 between = value42 - npc.Center;
                float distance = between.Length();
                //distance == distance from the X axis of the player
                float factor = 1f;
                float acc = 3f;
                between.Normalize();
                between *= factor;
                npc.velocity = (npc.velocity * (acc - 1f) + between) / acc;
                if (npc.collideX || npc.collideY)
                {
                    //just collided
                    AI_State = State_Recalculate;
                    AI_X_Timer = 0f;
                }
                if (distance < factor || distance > 800f || Collision.CanHit(npc.Center, 1, 1, GetTarget().Center, 1, 1))
                {
                    AI_State = State_Distribute;
                }
            }
            else if (AI_State == State_Recalculate/*4*/)
            {
                npc.noGravity = false;
                //just collided
                //Main.NewText("just collided");
                Vector2 between = GetTarget().Center - npc.Center;
                npc.direction = (between.X < 0f) ? -1 : 1;
                if (npc.collideX)
                {
                    if (npc.velocity.X < 0.3f && npc.direction == -1)
                    {
                        npc.velocity.X = -0.3f;
                    }
                    else if (npc.velocity.X > -0.3f && npc.direction == 1)
                    {
                        npc.velocity.X = 0.3f;
                    }
                    npc.velocity.X = npc.velocity.X * -0.8f;
                }
                if (npc.collideY)
                {
                    npc.velocity.Y = npc.velocity.Y * -0.8f;
                }
                Vector2 vel;
                if (npc.velocity.X == 0f && npc.velocity.Y == 0f)
                {
                    vel = GetTarget().Center - npc.Center;
                    vel.Y -= (float)(GetTarget().height / 4);
                    vel.Normalize();
                    npc.velocity = vel * 0.1f;
                }
                float scaleFactor21 = 1.5f; //1.5f
                float acc = 20f; //20f
                vel = npc.velocity;
                vel.Normalize();
                vel *= scaleFactor21;
                npc.velocity = (npc.velocity * (acc - 1f) + vel) / acc;
                AI_X_Timer += 1f;
                if (AI_X_Timer > 180f)
                {
                    AI_State = State_Distribute;
                    AI_X_Timer = 0f;
                }
                if (Collision.CanHit(npc.Center, 1, 1, GetTarget().Center, 1, 1))
                {
                    AI_State = State_Distribute;
                }
                AI_Local_Timer += 1f;
                //Adjust these values in the if() depending on your NPCs dimensions (Granite elemental is 20x30)
                if (AI_Local_Timer >= 5f && !Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    AI_Local_Timer = 0f;
                    Vector2 centerAlignedX = npc.Center;
                    centerAlignedX.X = GetTarget().Center.X;
                    if (Collision.CanHit(npc.Center, 1, 1, centerAlignedX, 1, 1) && Collision.CanHit(GetTarget().Center, 1, 1, centerAlignedX, 1, 1))
                    {
                        //player unreachable now
                        AI_State = State_IdleMove;
                        AI_X_Timer = centerAlignedX.X;
                        AI_Y = centerAlignedX.Y;
                    }
                    else
                    {
                        Vector2 centerAlignedY = npc.Center;
                        centerAlignedY.Y = GetTarget().Center.Y;
                        if (Collision.CanHit(npc.Center, 1, 1, centerAlignedY, 1, 1) && Collision.CanHit(GetTarget().Center, 1, 1, centerAlignedY, 1, 1))
                        {
                            //player unreachable now
                            AI_State = State_IdleMove;
                            AI_X_Timer = centerAlignedY.X;
                            AI_Y = centerAlignedY.Y;
                        }
                    }
                }
            }
            else if (AI_State == State_Stop/*5*/)
            {
                if (AI_X_Timer == 0f && stopTime == eatTime)
                {
                    resting = true;
                    npc.netUpdate = true;
                }
                else if (stopTime == idleTime) resting = false;
                if (resting) npc.noGravity = false;

                npc.velocity = Vector2.Zero;
                AI_X_Timer += 1f;
                if (AI_X_Timer > stopTime)
                {
                    AI_State = State_Distribute;
                    AI_X_Timer = 0f;

                    if (stopTime == idleTime)
                    {
                        SelectTarget(); //to retarget for the IsActive check (otherwise it gets stuck in this state)
                    }
                    else if (stopTime == eatTime)
                    {
                        //soul eaten and soul still there: initialize transformation
                        Main.NewText("finished eating");
                        stopTime = idleTime;
                        AI_State = State_Stop; //transform
                        resting = false;
                    }
                }
                else if (AI_State == State_Transform/*6*/)
                {
                    //if (AI_X_Timer == 0f && stopTime == eatTime)
                    //{
                    //    resting = true;
                    //}
                    //else if (stopTime == idleTime) resting = false;
                    //if (resting) npc.noGravity = false;

                    //npc.velocity = Vector2.Zero;
                    //AI_X_Timer += 1f;
                    //if (AI_X_Timer > stopTime)
                    //{
                    //    AI_State = State_Distribute;
                    //    AI_X_Timer = 0f;

                    //    if (stopTime == idleTime)
                    //    {
                    //        SelectTarget();
                    //    }
                    //    else if (stopTime == eatTime)
                    //    {
                    //        //soul eaten and soul still there: initialize transformation
                    //        Main.NewText("finished eating");
                    //        //if the "catch souls to deny" idea works out then add a check during timer increment if souls is alive, and increment a saturation counter
                    //        stopTime = idleTime;
                    //        resting = false;
                    //    }
                    //}
                }
            }
        }

        protected void GenericFighterAI(bool agressive = true)
        {
            //if(npc.velocity.Y != 0f) Main.NewText("veloy " + npc.velocity.Y); //use that in findframe to animate the wings


            bool flag3 = false;
            bool closeToSoul = false;
            bool flag4 = false;

            if (npc.velocity.X == 0f)
            {
                flag3 = true;
            }
            if (npc.justHit)
            {
                flag3 = false;
            }

            if (AI_Init == 0) //1
            {
                AI_Timer = 0f;
                stuckTimer = 0;
                AI_X_Timer = npc.position.X;
                AI_Y = npc.position.Y;

                if (npc.direction == 0)
                {
                    npc.direction = 1;
                }
                AI_Init = 1; //0
            }
            //
            if (agressive /*&& AI_Timer < (float)aiFighterLimit*/)
            {
                if (Main.time % 60 == 0)
                {
                    SelectTarget(select: "s");
                }
                //npc.TargetClosest();
                //Main.NewText("" + SelectTarget(select: "p") + " " + npc.target); //if p, automatically in npc.target
                //GetTarget();
            }

            if (!IsTargetActive())
            {
                stopTime = idleTime;
                AI_State = State_Stop;
                npc.velocity = Vector2.Zero;
                return;
            }
            else
            {
                Vector2 between = GetTarget(select: "s").Center - npc.Center;
                if(between.Length() < 20f)
                {
                    AI_Timer = 0f; //when literally near the soul
                    closeToSoul = true; //used to prevent the stuck timer to run
                }
                npc.direction = (between.X <= 0f) ? -1 : 1;
            }

            if (npc.velocity.Y == 0f && ((npc.velocity.X > 0f && npc.direction < 0) || (npc.velocity.X < 0f && npc.direction > 0)))
            {
                flag4 = true;
            }
            if ((npc.position.X == npc.oldPosition.X || AI_Timer >= (float)aiFighterLimit) | flag4)
            {
                AI_Timer += 1f;
            }
            //else if (/*(double)Math.Abs(npc.velocity.X) > (veloScale*0.4f) && */AI_Timer > 0f) //0.9
            //{
            //    //near 
            //    //AI_Timer -= 1f;
            //}
            if (AI_Timer > (float)(aiFighterLimit * 2))
            {
                AI_Timer = 0f;
            }
            if (npc.justHit)
            {
                AI_Timer = 0f;
            }
            if (AI_Timer == (float)aiFighterLimit)
            {
                npc.netUpdate = true;
            }

            UpdateStuck(closeToSoul);

            UpdateVelocity();

            UpdateOtherMovement(flag3);
        }

        public override void AI()
        {
            GenericFighterAI();
            return;
            GenericGraniteAI();
            return;
        }
    }
}
