using System;
using System.IO;
using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public abstract class HarvesterBase : ModNPC
    {
        public const short EatTimeConst = 180; //shouldn't be equal to IdleTimeConst + 60
        public const short IdleTimeConst = 180;
        public static readonly string message = "You hear a faint cawing come from nearby..."; //used for announcing
        protected const bool Target_Player = false;
        protected const bool Target_Soul = true;
        protected const int AI_State_Slot = 0;
        protected const int AI_X_Timer_Slot = 1;
        protected const int AI_Y_Slot = 2;
        protected const int AI_Timer_Slot = 3;

        protected const float STATE_DISTRIBUTE = 0f;
        protected const float STATE_APPROACH = 1f;
        protected const float STATE_NOCLIP = 2f;
        protected const float STATE_STOP = 3f;
        protected const float STATE_TRANSFORM = 4f;

        public static void Print(string msg)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                Console.WriteLine(msg);
            }

            if (Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(msg);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (AI_State == STATE_TRANSFORM)
            {
                return lightColor * ((transformTime - AI_X_Timer) / transformTime);
            }
            return new Color((int)(lightColor.R * 1.2f + 20), (int)(lightColor.G * 1.2f + 20), (int)(lightColor.B * 1.2f + 20));
        }

        public override bool CheckActive()
        {
            //manually decrease timeleft
            return false;
        }

        //doesnt get affected by expert scaling anyway for some reason
        //public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        //{
        //    npc.lifeMax = (int)(npc.lifeMax * 0.5f);
        //}

        //public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        //{
        //    if (!AssWorld.isPlayerHealthManaBarLoaded)
        //    {
        //        if (damage == npc.lifeMax && knockback == 0 && crit) //cheatsheet clear
        //        {
        //            return true;
        //        }
        //        if (noDamage)
        //        {
        //            damage = 0;
        //            return false;
        //        }
        //        return true;
        //    }
        //    return base.StrikeNPC(ref damage, defense, ref knockback, hitDirection, ref crit);
        //}

        public override void SendExtraAI(BinaryWriter writer)
        {
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }

        public static string name = "Soul Harvester";

        protected float maxVeloScale; //2f default //
        protected float maxAccScale; //0.07f default
        protected byte stuckTime; //*30 for ticks, *0.5 for seconds
        protected byte afterEatTime;
        protected short eatTime;
        protected short idleTime;
        protected short hungerTime; //AI_Timer, the "global failsafe", increments when a target exists
        protected byte maxSoulsEaten;
        protected short jumpRange; //also noclip detect range //100 for restricted v
        public bool restrictedSoulSearch;
        public short transformTime;
        public bool noDamage;
        //those above are all "defaults", aka they get defined in setdefaults

        public byte soulsEaten;
        public short stopTime;
        public bool aiTargetType;
        public short target;
        public byte stuckTimer;
        public byte rndJump;
        public bool transformServer;
        public int transformTo;
        public bool aiInit;

        public float defScale = 1.0f;
        public int defLifeMax; //should be the same as maxSoulsEaten

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

        protected int SelectTarget(bool restricted = false)
        {
            if (aiTargetType == Target_Soul)
            {
                target = SoulTargetClosest(restricted);
                if (target == 200)
                {
                    stopTime = idleTime;
                    AI_State = STATE_STOP;
                }
            }
            else if (aiTargetType == Target_Player) //Target_Player
            {
                npc.TargetClosest();
                target = (short)npc.target;
            }
            return target;
        }

        protected short SoulTargetClosest(bool restrictedvar = false)
        {
            short closest = 200;
            Vector2 soulPos = Vector2.Zero;
            float oldDistance = 1000000000f;
            float newDistance = oldDistance;
            //return index of closest soul
            for (short j = 0; j < 200; j++)
            {
                //ignore souls if they are noclipping
                if (Main.npc[j].active && Main.npc[j].type == mod.NPCType<DungeonSoul>() && !Collision.SolidCollision(Main.npc[j].position, Main.npc[j].width, Main.npc[j].height))
                {
                    soulPos = Main.npc[j].Center - npc.Center;
                    newDistance = soulPos.Length();
                    if (newDistance < oldDistance && ((restrictedvar? (soulPos.Y > -jumpRange) : true) || Collision.CanHitLine(npc.Center - new Vector2(0f, npc.height), 1, npc.height, Main.npc[j].Center, 1, 1)))
                    {
                        oldDistance = newDistance;
                        closest = j;
                    }
                }
            }
            //NEED TO CATCH "==200" WHEN CALLING THIS 
            return closest; //to self
        }

        protected Entity GetTarget()
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

        protected bool IsTargetActive()
        {
            return GetTarget().active;
        }

        protected void PassCoordinates(Entity ent)
        {
            AI_X_Timer = ent.Center.X;
            AI_Y = ent.Center.Y - 8f; //buffer up
        }

        protected void KillInstantly(NPC npc)
        {
            npc.life = 0;
            npc.active = false;
            npc.netUpdate = true;
        }

        public static bool SolidCollisionNew(Vector2 Position, int Width, int Height)
        {

            int value = (int)(Position.X / 16f) - 1;
            int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
            int value3 = (int)(Position.Y / 16f) - 1;
            int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
            value = Utils.Clamp(value, 0, Main.maxTilesX - 1);
            value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
            value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
            value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
            for (int i = value; i < value2; i++)
            {
                for (int j = value3; j < value4; j++)
                {
                    if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type] && !Main.tileSolidTop[Main.tile[i, j].type])
                    {
                        Vector2 vector = default(Vector2);
                        vector.X = (float)(i * 16);
                        vector.Y = (float)(j * 16);
                        int num = 16;
                        if (Main.tile[i, j].halfBrick() || Main.tile[i, j].slope() != 0)
                        {
                            vector.Y += 8f;
                            num -= 8;
                        }
                        if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool CanHitLineCombined(Entity npcto, Entity npcfrom)
        {
            //returns true if npcto can reach npcfrom from all to all four corners of the hitbox
            //returns false if atleast one corner isnt reachable
            Vector2 tl1 = new Vector2(npcto.position.X, npcto.position.Y);
            Vector2 tr1 = new Vector2(npcto.position.X + npcto.width - 1, npcto.position.Y);
            Vector2 bl1 = new Vector2(npcto.position.X, npcto.position.Y + npcto.height - 1);
            Vector2 br1 = new Vector2(npcto.position.X + npcto.width - 1, npcto.position.Y + npcto.height - 1);

            Vector2 tl2 = new Vector2(npcfrom.position.X, npcfrom.position.Y);
            Vector2 tr2 = new Vector2(npcfrom.position.X + npcfrom.width - 1, npcfrom.position.Y);
            Vector2 bl2 = new Vector2(npcfrom.position.X, npcfrom.position.Y + npcfrom.height - 1);
            Vector2 br2 = new Vector2(npcfrom.position.X + npcfrom.width - 1, npcfrom.position.Y + npcfrom.height - 1);
            /* * * * * * * * from each corner to each corner. If between all four there are no tiles inbetween,
             *             * then you arent stuck
             *             *
             *             *
             *             *
             *             *
             *             *
             *             *
             * * * * * * * *
             */
            //Print("test " + Collision.CanHitLine(tl1, 1, 1, tl2, 1, 1) + " " + Collision.CanHitLine(tr1, 1, 1, tr2, 1, 1) + " " + Collision.CanHitLine(bl1, 1, 1, bl2, 1, 1) + " " + Collision.CanHitLine(br1, 1, 1, br2, 1, 1));
            return (
                Collision.CanHitLine(tl1, 1, 1, tl2, 1, 1) &&
                Collision.CanHitLine(tr1, 1, 1, tr2, 1, 1) &&
                Collision.CanHitLine(bl1, 1, 1, bl2, 1, 1) &&
                Collision.CanHitLine(br1, 1, 1, br2, 1, 1));
        }

        protected void UpdateStuck(bool closeToSoulvar, bool allowNoclipvar)
        {
            Vector2 between = new Vector2(0f, GetTarget().Center.Y - npc.Center.Y);
            //collideY isnt proper when its on ledges/halfbricks
            if (Main.time % 30 == 0)
            {
                if ((npc.collideX || (npc.collideY || (npc.velocity.Y == 0 || npc.velocity.Y < 2f && npc.velocity.Y > 0f))) &&
                !closeToSoulvar)
                {
                    if (!CanHitLineCombined(npc, GetTarget()) ||
                        between.Y > 0f ||
                        between.Y <= -jumpRange)
                    {
                        //Main.NewText("TICK TOCK " + npc.collideX + " " + npc.collideY);
                        between = new Vector2(Math.Abs(npc.Center.X - AI_X_Timer), Math.Abs(npc.Center.Y - AI_Y));
                        //twice a second, diff is max 39f
                        if ((between.Y > 100f || between.X > 35f) || (npc.wet && (between.Y > 50f || between.X > 17.5f)))
                        {
                            npc.netUpdate = true;
                            //Print("NOT stuck actually");
                            stuckTimer = 0;
                        }
                        else if (between.Y <= 100f)
                        {
                            if (between.X <= 35f)
                            {
                                stuckTimer++;
                                //Print("stucktimer++ " + stuckTimer);
                                npc.netUpdate = true;
                            }
                        }
                        if (stuckTimer >= stuckTime)
                        {
                            if (allowNoclipvar)
                            {
                                if (!SolidCollisionNew(GetTarget().position, GetTarget().width, GetTarget().height + 2))
                                {
                                    npc.netUpdate = true;
                                    //Print("noclipping");
                                    //Main.NewText("DOOR STUCK");
                                    PassCoordinates(GetTarget());
                                    AI_State = STATE_NOCLIP; //pass targets X/Y to noclip
                                }
                            }
                            else
                            {
                                DungeonSoulBase.SetTimeLeft((NPC)GetTarget(), npc);
                            }
                            stuckTimer = 0;
                            return;
                        }
                    }
                }
            }
            if (Main.time % 30 == 0) //do these always
            {
                AI_X_Timer = npc.Center.X;
                AI_Y = npc.Center.Y;
            }
        }

        protected bool UpdateVelocity()
        {
            Vector2 between = new Vector2(Math.Abs(GetTarget().Center.X - npc.Center.X), GetTarget().Center.Y - npc.Center.Y);
            float bottomY = GetTarget().BottomLeft.Y - npc.BottomLeft.Y;
            bool lockedX = false;
            if (between.X < GetTarget().width/2/*2f*/ && CanHitLineCombined(npc, GetTarget())/*Collision.CanHit(npc.Center - new Vector2(2f, 2f), 4, 4, GetTarget().Center - new Vector2(2f, 2f), 4, 4)*/ && bottomY <= 16f && between.Y > -jumpRange)
            {
                //actually only locked when direct LOS and not too high
                //Print("set lockedX");
                lockedX = true;
            }

            float veloScale = maxVeloScale; //2f default
            float accScale = maxAccScale; //0.07f default

            //VELOCITY CALCULATIONS HERE
            if (!lockedX)
            {
                if (between.X < GetTarget().width / 2 && Math.Abs(between.Y) < 24f)
                {
                    veloScale = maxVeloScale * 0.4f; //when literally near the soul
                }
                if (GetTarget().velocity.X != 0) //if it tries to fly away because of borked code in soul AI()
                {
                    veloScale *= 1.75f;
                }

                if (npc.velocity.X < -veloScale || npc.velocity.X > veloScale)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity *= 0.7f;
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
                //  if on ground || if on downward slope
                if ((npc.velocity.Y == 0 || npc.velocity.Y < 1.5f && npc.velocity.Y > 0f) /*SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10)*/ && between.Y < -32f) //jump when below two tiles
                {
                    //Print("jump to get to soul");
                    npc.velocity.Y = (float)(Math.Sqrt((double)-between.Y) * -0.84f);
                    npc.netUpdate = true;
                }
                npc.direction = -1;
                //go to eat mode
                Entity tar = GetTarget();
                NPC tarnpc = new NPC();
                if (tar is NPC)
                {
                    tarnpc = (NPC)tar;
                }
                if (npc.getRect().Intersects(tarnpc.getRect()))
                {
                    stopTime = eatTime;
                }
                AI_State = STATE_DISTRIBUTE;
            }
            return lockedX;
        }

        protected void UpdateOtherMovement(bool flag3var)
        {
            bool flag3 = false;
            if (npc.velocity.X == 0f)
            {
                flag3 = true;
            }
            bool flag22 = false;
            //might scrap it idk
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.time % 60 == 1 && (npc.velocity.X == 0f || (npc.velocity.Y > -3f && npc.velocity.Y < 3f)))
                {
                    rndJump = (byte)Main.rand.Next(5, 8);
                    //if (rndJump >= 7)
                    //{
                    //    rndJump = 0;
                    //}
                    //npc.netUpdate = true;
                }
            }
            //



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
                    Tile tile4 = new Tile();
                    tile3[num183, num184] = tile4;
                }
                if (Main.tile[num183, num184 - 1] == null)
                {
                    Tile[,] tile5 = Main.tile;
                    Tile tile6 = new Tile();
                    tile5[num183, num184 - 1] = tile6;
                }
                if (Main.tile[num183, num184 - 2] == null)
                {
                    Tile[,] tile7 = Main.tile;
                    Tile tile8 = new Tile();
                    tile7[num183, num184 - 2] = tile8;
                }
                if (Main.tile[num183, num184 - 3] == null)
                {
                    Tile[,] tile9 = Main.tile;
                    Tile tile10 = new Tile();
                    tile9[num183, num184 - 3] = tile10;
                }
                if (Main.tile[num183, num184 + 1] == null)
                {
                    Tile[,] tile11 = Main.tile;
                    Tile tile12 = new Tile();
                    tile11[num183, num184 + 1] = tile12;
                }
                if (Main.tile[num183 - num182, num184 - 3] == null)
                {
                    Tile[,] tile13 = Main.tile;
                    Tile tile14 = new Tile();
                    tile13[num183 - num182, num184 - 3] = tile14;
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
                int num200 = 0;
                int num201 = 0;
                if (1 == 1)
                {
                    num200 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                    num201 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
                    if (Main.tile[num200, num201] == null)
                    {
                        Tile[,] tile15 = Main.tile;
                        Tile tile16 = new Tile();
                        tile15[num200, num201] = tile16;
                    }
                    if (Main.tile[num200, num201 - 1] == null)
                    {
                        Tile[,] tile17 = Main.tile;
                        Tile tile18 = new Tile();
                        tile17[num200, num201 - 1] = tile18;
                    }
                    if (Main.tile[num200, num201 - 2] == null)
                    {
                        Tile[,] tile19 = Main.tile;
                        Tile tile20 = new Tile();
                        tile19[num200, num201 - 2] = tile20;
                    }
                    if (Main.tile[num200, num201 - 3] == null)
                    {
                        Tile[,] tile21 = Main.tile;
                        Tile tile22 = new Tile();
                        tile21[num200, num201 - 3] = tile22;
                    }
                    if (Main.tile[num200, num201 + 1] == null)
                    {
                        Tile[,] tile23 = Main.tile;
                        Tile tile24 = new Tile();
                        tile23[num200, num201 + 1] = tile24;
                    }
                    if (Main.tile[num200 + npc.direction, num201 - 1] == null)
                    {
                        Tile[,] tile25 = Main.tile;
                        Tile tile26 = new Tile();
                        tile25[num200 + npc.direction, num201 - 1] = tile26;
                    }
                    if (Main.tile[num200 + npc.direction, num201 + 1] == null)
                    {
                        Tile[,] tile27 = Main.tile;
                        Tile tile28 = new Tile();
                        tile27[num200 + npc.direction, num201 + 1] = tile28;
                    }
                    if (Main.tile[num200 - npc.direction, num201 + 1] == null)
                    {
                        Tile[,] tile29 = Main.tile;
                        Tile tile30 = new Tile();
                        tile29[num200 - npc.direction, num201 + 1] = tile30;
                    }
                    Main.tile[num200, num201 + 1].halfBrick();
                }

                //adjusted here

                //int num200 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                //int num201 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
                if (!(Main.tile[num200, num201 - 1].nactive() && (TileLoader.IsClosedDoor(Main.tile[num200, num201 - 1]) || Main.tile[num200, num201 - 1].type == 388)))
                {
                    //Main.NewText("" + num200 + " " + num201);
                    if ((npc.velocity.X < 0f && npc.direction == -1) || (npc.velocity.X > 0f && npc.direction == 1)) //spritedir instead of dir before
                    {
                        if (1 == 2)
                        {
                            if (npc.height >= 32 && Main.tile[num200, num201 - 2].nactive() && Main.tileSolid[Main.tile[num200, num201 - 2].type])
                            {
                                if (Main.tile[num200, num201 - 3].nactive() && Main.tileSolid[Main.tile[num200, num201 - 3].type])
                                {
                                    Main.NewText("1111");
                                    npc.velocity.Y = -8f;
                                    npc.netUpdate = true;
                                }
                                else
                                {
                                    Main.NewText("2222");
                                    npc.velocity.Y = -7f;
                                    npc.netUpdate = true;
                                }
                            }
                            else if (Main.tile[num200, num201 - 1].nactive() && Main.tileSolid[Main.tile[num200, num201 - 1].type])
                            {
                                Main.NewText("3333");
                                npc.velocity.Y = -6f;
                                npc.netUpdate = true;
                            }
                            else if (npc.position.Y + (float)npc.height - (float)(num201 * 16) > 20f && Main.tile[num200, num201].nactive() && !Main.tile[num200, num201].topSlope() && Main.tileSolid[Main.tile[num200, num201].type])
                            {
                                Main.NewText("4444");
                                npc.velocity.Y = -5f;
                                npc.netUpdate = true;
                            }
                            else if (npc.directionY < 0 && (!Main.tile[num200, num201 + 1].nactive() || !Main.tileSolid[Main.tile[num200, num201 + 1].type]) && (!Main.tile[num200 + npc.direction, num201 + 1].nactive() || !Main.tileSolid[Main.tile[num200 + npc.direction, num201 + 1].type]))
                            {
                                //this is for when player stands on an elevation and it just jumped aswell
                                Main.NewText("5555");
                                npc.velocity.Y = -8f;
                                npc.velocity.X *= 1.5f;
                                npc.netUpdate = true;
                            }
                            if (npc.velocity.Y == 0f && flag3 && false/* && aiFighter == 1f*/)
                            {
                                Main.NewText("6666");
                                npc.velocity.Y = -5f;
                            }
                        }


                        //heck this ima do this MY way

                        if (/*npc.velocity.Y == 0f && */flag3var)
                        {
                            if (Main.time % 60 == 35)
                            {
                                npc.velocity.Y = -(float)rndJump - 0.5f;
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

        protected void HarvesterAIGround(bool allowNoclip = true)
        {
            bool flag3 = false;
            bool closeToSoul = false;
            //bool flag4 = false;

            if (npc.velocity.X == 0f)
            {
                flag3 = true;
            }
            if (npc.justHit)
            {
                flag3 = false;
            }

            if (true/*&& AI_Timer < (float)aiFighterLimit*/)
            {
                if (Main.time % 60 == 0)
                {
                    SelectTarget(restrictedSoulSearch);
                }
            }

            if (!IsTargetActive())
            {
                stopTime = idleTime;
                AI_State = STATE_STOP;
                return;
            }
            else
            {
                Vector2 between = GetTarget().Center - npc.Center;
                if (between.Length() < 20f)
                {
                    AI_Timer = 0f; //when literally near the soul
                    closeToSoul = true; //used to prevent the stuck timer to run
                }
                npc.direction = (between.X <= 0f) ? -1 : 1;
            }

            //if (npc.velocity.Y == 0f && ((npc.velocity.X > 0f && npc.direction < 0) || (npc.velocity.X < 0f && npc.direction > 0)))
            //{
            //    flag4 = true;
            //}
            //if ((npc.position.X == npc.oldPosition.X || AI_Timer >= (float)hungerTimeLimit) | flag4)
            //{
            //    AI_Timer += 1f;
            //}
            ////else if (/*(double)Math.Abs(npc.velocity.X) > (veloScale*0.4f) && */AI_Timer > 0f) //0.9
            ////{
            ////    //near 
            ////    //AI_Timer -= 1f;
            ////}
            //if (AI_Timer > (float)(hungerTimeLimit * 2))
            //{
            //    AI_Timer = 0f;
            //}

            //hungerTimer
            //AI_Timer++ in HarvesterAI always
            if (AI_Timer >= hungerTime && !SolidCollisionNew(GetTarget().position, GetTarget().width, GetTarget().height/* + 2*/))
            {
                SelectTarget(restricted: false);
                if (IsTargetActive() && target != 200)
                {
                    AI_Timer = 0;
                    //goto noclip 
                    if (IsTargetActive())
                        PassCoordinates(GetTarget());
                    //Print("passed to noclip aaaaaaaaaaaaaaaaaaaa");
                    AI_State = STATE_NOCLIP; //this is needed in order for the harvester to keep progressing
                }
                if (target == 200)
                {
                    AI_Timer -= 360; //6 seconds
                }
                npc.netUpdate = true;
            }

            UpdateStuck(closeToSoul, allowNoclip);

            //if not locked, do othermovement
            if (!UpdateVelocity()) UpdateOtherMovement(flag3);



            //---------------------------------------------------------------------
            //NEW: DROP THROUGH PLATFORMS WHEN SOUL BELOW
            //maybe use
            //Framing.GetTileSafely((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16))
            int num = (int)(npc.position.X / 16f);
            int num2 = (int)((npc.position.Y + npc.height + 15f) / 16f);

            //tile under the left corner of the NPC
            if (Main.tile[num, num2] == null)
            {
                Tile[,] tile3 = Main.tile;
                Tile tile4 = new Tile();
                tile3[num, num2] = tile4;
            }
            //tile on the right of that
            if (Main.tile[num + 1, num2] == null)
            {
                Tile[,] tile5 = Main.tile;
                Tile tile6 = new Tile();
                tile5[num + 1, num2] = tile6;
            }
            //tile on the right right of that
            if (npc.direction == -1 && Main.tile[num + 2, num2] == null)
            {
                Tile[,] tile7 = Main.tile;
                Tile tile8 = new Tile();
                tile7[num + 2, num2] = tile8;
            }

            if (TileID.Sets.Platforms[Main.tile[num, num2].type] && TileID.Sets.Platforms[Main.tile[num + 1, num2].type] && ((npc.direction == -1)? TileID.Sets.Platforms[Main.tile[num + 2, num2].type]:true) && (GetTarget().Top.Y - npc.Bottom.Y) > 0f)
            {
                npc.netUpdate = true;
                npc.position.Y += 1f;
            }
        }

        protected void HarvesterAI(bool allowNoclip = true)
        {
            npc.scale = defScale;
            npc.lifeMax = defLifeMax;

            if (Main.time % 120 == 2)
            {
                if (npc.timeLeft > 750) npc.timeLeft = 750;

                bool shouldDecreaseTime = false;
                bool allPlayersDead = true;
                int closest = 255;
                Vector2 playerPos = Vector2.Zero;
                float oldDistance = 1000000000f;
                float newDistance = oldDistance;

                //return index of closest player
                for (short j = 0; j < 255; j++)
                {
                    if (Main.player[j].active)
                    {
                        playerPos = Main.player[j].Center - npc.Center;
                        newDistance = playerPos.Length();
                        if (newDistance < oldDistance)
                        {
                            oldDistance = newDistance;
                            closest = j;
                            shouldDecreaseTime = true; //atleast one player is found
                        }
                        if (allPlayersDead && !Main.player[j].dead) allPlayersDead = false;
                    }
                }

                if (allPlayersDead)
                {
                    AI_X_Timer = 0;
                    aiTargetType = Target_Player;
                    transformTime = 60;
                    transformServer = true;
                    AI_State = STATE_TRANSFORM;
                    transformTo = -1; //fade out, but don't transform
                }

                if (shouldDecreaseTime && !allPlayersDead)
                {
                    //decrease time only when distance is bigger than one and a half screens and player not in dungeon,
                    //otherwise dont decrease time (2880)
                    if (oldDistance > 2880 && !Main.player[closest].ZoneDungeon)
                    {
                        npc.timeLeft -= 4; //check every two seconds, decrease 2 every second from 750 until 0: 6.25 minutes
                    }
                    else
                    {
                        npc.timeLeft = 750;
                    }

                    if(npc.timeLeft < 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            npc.life = 0;
                            npc.active = false;
                            if (Main.netMode == NetmodeID.Server) NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                        }
                    }
                }

                npc.netUpdate = true;
            }

            npc.noGravity = false;
            npc.noTileCollide = false;

            if (Main.time % 30 == 0)
            {
                for (int k = 0; k < 256; k++)
                {
                    npc.immune[k] = 40;
                }
            }

            if (!aiInit/* && Main.netMode != 1*/)
            {
                npc.life = soulsEaten + 1;
                //initialize it to go for souls first
                aiTargetType = Target_Soul;
                stopTime = idleTime;
                SelectTarget(restrictedSoulSearch);
                aiInit = true;
                AI_Timer = 0f;
                //AI_Local_Timer = 0f;
                npc.netUpdate = true;
            }

            if (npc.alpha > 0)
            {
                npc.alpha -= 5;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
                return;
            }

            //if (AI_Local_Timer < afterEatTime)
            //{
            //    AI_Local_Timer++;
            //    return;
            //}

            if (!(AI_State == STATE_NOCLIP))
            {
                if(!IsTargetActive())
                {
                    if (aiTargetType == Target_Soul)
                    {
                        stopTime = idleTime;
                        AI_State = STATE_STOP;
                    }
                    else //if target is player, its eating anyways (to prevent it from resetting because of target switch)
                    {
                        stopTime = eatTime;
                    }
                    //AI_X_Timer = 0f;
                }

                if (AI_Timer >= hungerTime && !SolidCollisionNew(GetTarget().position, GetTarget().width, GetTarget().height/* + 2*/))
                {
                    SelectTarget(restricted: false);
                    if(IsTargetActive() && target != 200)
                    {
                        AI_Timer = 0;
                        //goto noclip 
                        if (IsTargetActive())
                            PassCoordinates(GetTarget());
                        //Print("passed to noclip aaaaaaaaaaaaaaaaaaaa");
                        AI_State = STATE_NOCLIP; //this is needed in order for the harvester to keep progressing
                    }
                    if (target == 200)
                    {
                        AI_Timer -= 360;
                    }
                    npc.netUpdate = true;
                }
            }

            //if (IsTargetActive())
            //{
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    AI_Timer++; //count hungerTime up, always
                }
            //}

            //Attack player
            if (AI_Timer % 20 == 0)
            {
                if(AI_State == STATE_STOP && aiTargetType == Target_Player && soulsEaten <= maxSoulsEaten)
                {
                    AttackPlayer(5, 3f, 200, aiTargetType);
                }
                else if(AI_State != STATE_STOP && aiTargetType == Target_Soul)
                {
                    AttackPlayer(5, 3f, npc.width, aiTargetType);
                }
            }


            if (AI_State == STATE_DISTRIBUTE/*0*/)
            {
                if(Main.time % 20 == 0)
                {
                    SelectTarget(restrictedSoulSearch);
                }
                else if(target == 200)
                {
                    //SelectTarget(restrictedSoulSearch);
                }

                //check if atleast one of four tiles underneath exists properly , aka "on the ground"

                if (stopTime == eatTime &&
                    (npc.velocity.Y == 0 || (npc.velocity.Y < 2f && npc.velocity.Y > 0f)) && //still (or on slope)
                    (GetTarget().velocity.Y == 0 || (GetTarget().velocity.Y < 2f && GetTarget().velocity.Y > 0f))//still (or on slope)
                                                                                                                 /*&& !Collision.SolidCollision(npc.position, npc.width, npc.height)*/)
                {
                    //sitting on soul, go into stop/eat mode
                    if (npc.velocity.Y != 0)
                    {
                        int num200 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                        int num201 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
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
                    }

                    npc.netUpdate = true;

                    //Print("distribute to stop");
                    DungeonSoulBase.SetTimeLeft((NPC)GetTarget(), npc);
                    aiTargetType = Target_Player;
                    SelectTarget(restrictedSoulSearch); //now player

                    AI_X_Timer = 0f;
                    AI_State = STATE_STOP; //start to eat
                }
                else if (stopTime != eatTime)
                {
                    npc.netUpdate = true;

                    //go into regular mode
                    PassCoordinates(npc);
                    stuckTimer = 0;
                    if (npc.direction == 0)
                    {
                        npc.direction = 1;
                    }
                    if(AI_Timer > hungerTime - 180) AI_Timer -= 180; //halve hunger timer
                    //AI_Timer = 0f; //reset hunger timer
                    AI_State = STATE_APPROACH;
                }
                else//keep state
                {
                    AI_State = STATE_DISTRIBUTE;
                }
            }
            else if (AI_State == STATE_APPROACH/*1*/)
            {
                HarvesterAIGround(allowNoclip);
            }
            else if (AI_State == STATE_NOCLIP/*2*/)
            {
                AI_Timer = 0f;
                npc.noGravity = true;
                npc.noTileCollide = true;
                Vector2 between = new Vector2(AI_X_Timer - npc.Center.X, AI_Y - npc.Center.Y); //use latest known position from UpdateStuck of target
                Vector2 normBetween = new Vector2(between.X, between.Y);

                npc.direction = (normBetween.X <= 0f) ? -1 : 1;

                float distance = normBetween.Length();
                float factor = 2.5f; //2f
                int acc = 30; //4
                normBetween.Normalize();
                normBetween *= factor;
                npc.velocity = (npc.velocity * (acc - 1) + normBetween) / acc;
                //concider only the bottom half of the hitbox (plus a small bit below)
                if (distance < npc.height /*600f*/ && between.Y < 20f && !Collision.SolidCollision(npc.position + new Vector2(-2f, npc.height / 2), npc.width + 4, npc.height / 2 + 4))
                {
                    npc.netUpdate = true;
                    AI_State = STATE_DISTRIBUTE;
                }
            }
            else if (AI_State == STATE_STOP/*3*/)
            {

                if (AI_X_Timer == 0f && stopTime == eatTime)
                {
                    AI_Timer = 0;
                    //Print("started eating");
                    npc.netUpdate = true;
                }
                npc.noGravity = false;

                npc.velocity.X = 0f;
                AI_X_Timer += 1f;
                if (AI_X_Timer > stopTime)
                {
                    npc.netUpdate = true;
                    AI_State = STATE_DISTRIBUTE;
                    AI_X_Timer = 0f;

                    if (stopTime == idleTime)
                    {
                        SelectTarget(restrictedSoulSearch); //to retarget for the IsActive check (otherwise it gets stuck in this state)
                    }
                    else if (stopTime == eatTime)
                    {
                        //Print("finished eating");
                        aiInit = false; //reinitialize
                        npc.HealEffect(npc.lifeMax - 1 - ++soulsEaten); //life gets set manually anyway so it doesnt matter what number is here

                        //poof visual
                        for (int i = 0; i < 20; i++)
                        {
                            Dust dust = Dust.NewDustPerfect(npc.BottomLeft + new Vector2(npc.width / 2, -npc.height / 4), 59, new Vector2(Main.rand.NextFloat(-1.5f, 1.5f) + npc.velocity.X, Main.rand.NextFloat(-1.5f, 1f)), 26, new Color(255, 255, 255), Main.rand.NextFloat(1.5f, 2.4f));
                            dust.noLight = true;
                            dust.noGravity = true;
                            dust.fadeIn = Main.rand.NextFloat(0f, 0.5f);
                        }

                        if (soulsEaten >= maxSoulsEaten)
                        {
                            //Print("souls eaten max reached");
                            npc.life = npc.lifeMax;
                            //soulsEaten = 0;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                aiInit = true; //skip the reinit
                                transformServer = true;
                                //Print("set transform var to tru");
                            }
                            AI_State = STATE_TRANSFORM;
                        }
                        else
                        {
                            AI_State = STATE_STOP;
                        }
                    }
                }
            }
            else if (AI_State == STATE_TRANSFORM/*6*/)
            {
                npc.velocity.X = 0;
                if (transformServer)
                {
                    AI_X_Timer++;

                    int randfactor = Main.rand.Next(2, 6);
                    //aitimer: from 0 to transformtime:
                    //rate: from 0 to 0.8 //((AI_X_Timer * 0.8f) / transformTime)
                    if (Main.time % 8 == 0 && Main.rand.NextFloat() < ((AI_X_Timer * 0.8f) / transformTime))
                    {
                        SpawnBoneShort(npc.Center + new Vector2(0f, -npc.height / 4), new Vector2(Main.rand.NextFloat(-1f, 1f) * randfactor, -Main.rand.NextFloat() * randfactor), 0, 1.5f);
                    }

                    //51 is because 255 is decremented by 5 in the transformTo alpha
                    if (AI_X_Timer == (transformTime - 51))
                    {
                        Transform(transformTo);
                    }

                    if (AI_X_Timer > transformTime)
                    {
                        KillInstantly(npc);
                    }
                    //handle fade out in GetAlpha scaled on AI_X_Timer,
                    //and fade in via alpha
                }
            }
        }

        private void AttackPlayer(int dmg, float knock, int distance, bool prevTargetType)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                aiTargetType = Target_Player;
                SelectTarget();
                Rectangle rect = new Rectangle((int)npc.Center.X - distance, (int)npc.Center.Y - distance, npc.width + distance * 2, npc.height + distance * 2);
                if (rect.Intersects(new Rectangle((int)GetTarget().position.X, (int)GetTarget().position.Y, GetTarget().width, GetTarget().height)) &&
                    (Collision.CanHitLine(npc.Center, 1, 1, GetTarget().Center, 1, 1) || (GetTarget().Center.Y - npc.Center.Y) <= 0f))
                //if either direct LOS or player above (so it can go around small obstacles)
                {
                    Vector2 between = GetTarget().Center - npc.Center;
                    between.Normalize();
                    if (between.Y > 0.2f) between *= 1.1f; //small arc
                    if (between.Y > 0.8f) between *= 1.1f; //big arc
                    between *= 7f;
                    SpawnBoneShort(npc.Center + new Vector2(0f, -npc.height / 4), between, dmg, knock);
                }

                aiTargetType = prevTargetType;
                SelectTarget();
            }
        }


        private void SpawnBoneShort(Vector2 pos, Vector2 vel, int dmg, float knock)
        {
            Projectile.NewProjectile(pos, vel, mod.ProjectileType<HarvesterBone>(), dmg, knock, Main.myPlayer);
        }

        public void Transform(int to)
        {
            //set to -1 to not transform
            if(to != -1)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int index = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, to, 150); //150 for index 150 atleast
                                                                                           //(so the claws will likely spawn with lower index and rendered infront)
                    Main.npc[index].SetDefaults(to);
                    if (Main.netMode == NetmodeID.Server && index < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, index);
                    }
                }
            }
        }

        public override void AI()
        {
            //HarvesterAI(allowNoclip: true);
            //if (transformServer) Transform(transformTo);
        }
    }
}
