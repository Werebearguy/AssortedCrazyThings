using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public abstract class BaseHarvester : ModNPC
    {
        public const short EatTimeConst = 90; //shouldnt be equal to IdleTimeConst + 60
        public const short IdleTimeConst = 180;
        public static readonly string message = "You hear a faint cawing come from nearby";
        protected const bool Target_Player = false;
        protected const bool Target_Soul = true;
        protected const int AI_State_Slot = 0;
        protected const int AI_X_Timer_Slot = 1;
        protected const int AI_Y_Slot = 2;
        protected const int AI_Timer_Slot = 3;

        protected const float State_Distribute = 0f;
        protected const float State_Approach = 1f;
        protected const float State_Noclip = 2f;
        //protected const float State_IdleMove = 3f;
        //protected const float State_Recalculate = 4f;
        protected const float State_Stop = 5f;
        protected const float State_Transform = 6f;

        protected void Print(string msg)
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
            if (AI_State == State_Transform)
            {
                //Main.NewText(lightColor * ((60 - AI_X_Timer) / 60));
                return Color.White * ((transformTime - AI_X_Timer) / transformTime);
            }
            return Color.White;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (!AssWorld.isPlayerHealthManaBarLoaded)
            {
                if (damage == npc.lifeMax && knockback == 0 && crit) //cheatsheet clear
                {
                    return true;
                }
                if (noDamage)
                {
                    damage = 0;
                    return false;
                }
                return true;
            }
            return base.StrikeNPC(ref damage, defense, ref knockback, hitDirection, ref crit);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
            }
        }

        public static string name = "aaaHarvester";

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
        //those above are all "static", cant make em static in 

        public byte soulsEaten;
        public short stopTime;
        public bool aiTargetType;
        public short target;
        public byte stuckTimer;
        public byte rndJump;
        public bool transformServer;
        public int transformTo;

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

        protected int SelectTarget(bool restricted = false)
        {
            if (aiTargetType == Target_Soul)
            {
                target = SoulTargetClosest(restricted);
                if (target == 200)
                {
                    AI_State = State_Stop;
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
                if (Main.npc[j].active && Main.npc[j].type == mod.NPCType(aaaSoul.name))
                {
                    soulPos = Main.npc[j].Center - npc.Center;
                    newDistance = soulPos.Length();
                    if (newDistance < oldDistance && ((restrictedvar? (soulPos.Y > -jumpRange) : true) || Collision.CanHitLine(npc.Center, 1, 1, Main.npc[j].Center, 1, 1)))
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

        protected void UpdateStuck(bool closeToSoulvar, bool allowNoclipvar)
        {
            Vector2 between = new Vector2(0f, GetTarget().Center.Y - npc.Center.Y);
            //collideY isnt proper when its on ledges/halfbricks
            if (Main.time % 30 == 0)
            {
                if ((npc.collideX || (npc.collideY || (npc.velocity.Y == 0 || npc.velocity.Y < 2f && npc.velocity.Y > 0f))) &&
                !closeToSoulvar)
                {
                    Vector2 tl1 = new Vector2(npc.position.X, npc.position.Y);
                    Vector2 tr1 = new Vector2(npc.position.X + npc.width - 1, npc.position.Y);
                    Vector2 bl1 = new Vector2(npc.position.X, npc.position.Y + npc.height - 1);
                    Vector2 br1 = new Vector2(npc.position.X + npc.width - 1, npc.position.Y + npc.height - 1);

                    Vector2 tl2 = new Vector2(GetTarget().position.X, GetTarget().position.Y);
                    Vector2 tr2 = new Vector2(GetTarget().position.X + GetTarget().width - 1, GetTarget().position.Y);
                    Vector2 bl2 = new Vector2(GetTarget().position.X, GetTarget().position.Y + GetTarget().height - 1);
                    Vector2 br2 = new Vector2(GetTarget().position.X + GetTarget().width - 1, GetTarget().position.Y + GetTarget().height - 1);
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
                    if (
                        !Collision.CanHitLine(tl1, 1, 1, tl2, 1, 1) ||
                        !Collision.CanHitLine(tr1, 1, 1, tr2, 1, 1) ||
                        !Collision.CanHitLine(bl1, 1, 1, bl2, 1, 1) ||
                        !Collision.CanHitLine(br1, 1, 1, br2, 1, 1) ||
                        between.Y > 0f ||
                        between.Y <= -jumpRange)
                    {
                        //Main.NewText("TICK TOCK " + npc.collideX + " " + npc.collideY);
                        between = new Vector2(Math.Abs(npc.Center.X - AI_X_Timer), Math.Abs(npc.Center.Y - AI_Y));
                        //twice a second, diff is max 39f
                        if (between.Y > 100f || between.X > 35f)
                        {
                            npc.netUpdate = true;
                            Print("NOT stuck actually");
                            stuckTimer = 0;
                        }
                        else if (between.Y <= 100f)
                        {
                            if (between.X <= 35f)
                            {
                                stuckTimer++;
                                Print("stucktimer++ " + stuckTimer);
                                npc.netUpdate = true;
                            }
                        }
                        if (stuckTimer >= stuckTime)
                        {
                            if (allowNoclipvar)
                            {
                                npc.netUpdate = true;
                                Print("noclipping");
                                //Main.NewText("DOOR STUCK");
                                PassCoordinates(GetTarget());
                                AI_State = State_Noclip; //pass targets X/Y to noclip
                            }
                            else
                            {
                                aaaSoul.SetTimeLeft((NPC)GetTarget(), npc);
                                KillInstantly((NPC)GetTarget());
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
            bool lockedX = false;
            if (between.X < GetTarget().width/3/*2f*/ && Collision.CanHit(npc.Center - new Vector2(2f, 2f), 4, 4, GetTarget().Center - new Vector2(2f, 2f), 4, 4) && between.Y <= 32f && between.Y > -jumpRange)
            {
                //actually only locked when direct LOS and not too high
                Print("set lockedX");
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
                    Main.NewText("jump to get to soul");
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
                AI_State = State_Distribute;
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
                    npc.netUpdate = true;
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
                int num200 = 0;
                int num201 = 0;
                if (1 == 1)
                {
                    num200 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                    num201 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
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
                }

                //adjusted here

                //int num200 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                //int num201 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
                if (!(Main.tile[num200, num201 - 1].nactive() && (TileLoader.IsClosedDoor(Main.tile[num200, num201 - 1]) || Main.tile[num200, num201 - 1].type == 388)))
                {
                    //Main.NewText("" + num200 + " " + num201);
                    if ((npc.velocity.X < 0f && npc.spriteDirection == -1) || (npc.velocity.X > 0f && npc.spriteDirection == 1))
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
            //if(npc.velocity.Y != 0f) Main.NewText("veloy " + npc.velocity.Y); //use that in findframe to animate the wings


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
                AI_State = State_Stop;
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
            //AI_Timer++ in HarvesterAI when target available;
            if (AI_Timer >= hungerTime && IsTargetActive())
            {
                AI_Timer = 0f;
                //goto noclip 
                PassCoordinates(GetTarget());
                Print("passed to noclip bbbbbbbbbbbbb");
                AI_State = State_Noclip; //this is needed in order for the harvester to keep progressing
                npc.netUpdate = true;
            }

            UpdateStuck(closeToSoul, allowNoclip);

            //if not locked, do othermovement
            if (!UpdateVelocity()) UpdateOtherMovement(flag3);

        }

        protected void HarvesterAI(bool allowNoclip = true)
        {

            if(npc.velocity.Y != 0) Main.NewText(SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10) + " " + AI_State);
            //if(SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10))

            if (Main.time % 120 == 2)
            {
                Print("soulseaten:" + soulsEaten);
            }

            npc.noGravity = false;
            npc.noTileCollide = false;
            if (AssWorld.isPlayerHealthManaBarLoaded)
            {
                npc.dontTakeDamage = true;  //if true, it wont show hp count while mouse over


            }
            else
            {
                for (int k = 0; k < 256; k++)
                {
                    npc.immune[k] = 30;
                }
            }

            if (AI_Init == 0)
            {
                npc.life = soulsEaten + 1;
                //initialize it to go for souls first
                aiTargetType = Target_Soul;
                stopTime = idleTime;
                SelectTarget(restrictedSoulSearch);
                AI_Init = 1;
                AI_Timer = 0f;
                AI_Local_Timer = 0f;
            }

            if (AI_Local_Timer < afterEatTime)
            {
                AI_Local_Timer++;
                return;
            }

            if (!(AI_State == State_Noclip))
            {
                if(!IsTargetActive())
                {
                    if (aiTargetType == Target_Soul)
                    {
                        stopTime = idleTime;
                        AI_State = State_Stop;
                    }
                    else //if target is player, its eating anyways (to prevent it from resetting because of target switch)
                    {
                        stopTime = eatTime;
                    }
                    //AI_X_Timer = 0f;
                }

                if (AI_Timer >= hungerTime && IsTargetActive())
                {
                    AI_Timer = 0;
                    //goto noclip 
                    PassCoordinates(GetTarget());
                    Print("passed to noclip aaaaaaaaaaaaaaaaaaaa");
                    AI_State = State_Noclip; //this is needed in order for the harvester to keep progressing
                    npc.netUpdate = true;
                }
            }

            if (IsTargetActive())
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    AI_Timer++; //count hungerTime up
                }
            }

            if (AI_State == State_Distribute/*0*/)
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

                    Print("distribute to stop");
                    aaaSoul.SetTimeLeft((NPC)GetTarget(), npc);
                    aiTargetType = Target_Player;
                    SelectTarget(restrictedSoulSearch); //now player

                    AI_X_Timer = 0f;
                    AI_State = State_Stop; //start to eat
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
                    AI_State = State_Approach;
                }
                else//keep state
                {
                    AI_State = State_Distribute;
                }
            }
            else if (AI_State == State_Approach/*1*/)
            {
                HarvesterAIGround(allowNoclip);
            }
            else if (AI_State == State_Noclip/*2*/)
            {
                AI_Timer = 0f;
                npc.noGravity = true;
                npc.noTileCollide = true;
                Vector2 between = new Vector2(AI_X_Timer - npc.Center.X, AI_Y - npc.Center.Y); //use latest known position from UpdateStuck of target
                Vector2 normBetween = new Vector2(between.X, between.Y);
                float distance = normBetween.Length();
                float factor = 2.5f; //2f
                int acc = 30; //4
                normBetween.Normalize();
                normBetween *= factor;
                npc.velocity = (npc.velocity * (acc - 1) + normBetween) / acc;
                //concider only the bottom half of the hitbox (plus a small bit below)
                if (distance < 96f /*600f*/ && between.Y < 20f && !Collision.SolidCollision(npc.position + new Vector2(-2f, npc.height / 2), npc.width + 4, npc.height / 2 + 4))
                {
                    npc.netUpdate = true;
                    AI_State = State_Distribute;
                }
            }
            else if (3 == 4 /*idlemove, recalculate*/)
            {
                
            }
            else if (AI_State == State_Stop/*5*/)
            {

                if (AI_X_Timer == 0f && stopTime == eatTime)
                {
                    AI_Timer = 0;
                    Main.NewText("started eating");
                    npc.netUpdate = true;
                }
                npc.noGravity = false;

                npc.velocity.X = 0f;
                AI_X_Timer += 1f;
                if (AI_X_Timer > stopTime)
                {
                    npc.netUpdate = true;
                    AI_State = State_Distribute;
                    AI_X_Timer = 0f;



                    if (stopTime == idleTime)
                    {
                        SelectTarget(restrictedSoulSearch); //to retarget for the IsActive check (otherwise it gets stuck in this state)
                    }
                    else if (stopTime == eatTime)
                    {
                        Print("finished eating");
                        AI_Init = 0; //reinitialize
                        npc.HealEffect(++soulsEaten); //life gets set manually anyway so it doesnt matter what number is here

                        if (soulsEaten >= maxSoulsEaten)
                        {
                            Print("souls eaten max reached");
                            npc.life = npc.lifeMax;
                            //soulsEaten = 0;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                AI_Init = 1; //skip the reinit
                                transformServer = true;
                                Main.NewText("set transform var to tru");
                            }
                            AI_State = State_Transform;
                        }
                        else
                        {
                            AI_State = State_Stop;
                        }
                    }
                }

                if (Main.netMode != NetmodeID.MultiplayerClient && AI_X_Timer % 20 == 0 && aiTargetType == Target_Player && soulsEaten <= maxSoulsEaten)
                {
                    SelectTarget();
                    Rectangle rect = new Rectangle((int)npc.Center.X - 200, (int)npc.Center.Y - 200, npc.width + 200 * 2, npc.height + 200 * 2);
                    if (rect.Intersects(new Rectangle((int)GetTarget().position.X, (int)GetTarget().position.Y, GetTarget().width, GetTarget().height)) &&
                        (Collision.CanHitLine(npc.Center, 1, 1, GetTarget().Center, 1, 1) || (GetTarget().Center.Y - npc.Center.Y) <= 0f)) 
                        //if either direct LOS or player above (so it can around small obstacles)
                    {
                        Vector2 between = GetTarget().Center - npc.Center;
                        between.Normalize();
                        if (between.Y > 0.2f) between *= 1.1f; //small arc
                        if (between.Y > 0.8f) between *= 1.1f; //big arc
                        between *= 7f;
                        SpawnBone(npc.Center + new Vector2(0f, -npc.height / 4), between, 6, 3f);
                    }
                }
            }
            else if (AI_State == State_Transform/*6*/)
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
                        SpawnBone(npc.Center + new Vector2(0f, -npc.height / 4), new Vector2(Main.rand.NextFloat(-1f, 1f) * randfactor, -Main.rand.NextFloat() * randfactor), 0, 1.5f);
                    }

                    if (AI_X_Timer > transformTime)
                    {
                        Transform(transformTo);
                    }
                    //handle fade out in GetAlpha scaled on AI_X_Timer,
                    //and fade in scaled on Local_Timer (when soulseaten == 0)
                }
            }
        }

        protected void SpawnBone(Vector2 pos, Vector2 vel, int dmg, float knock)
        {
            Projectile.NewProjectile(pos, vel, ProjectileID.SkeletonBone, dmg, knock, Main.myPlayer);
        }

        public void Transform(int to)
        {
            //set to zero to not transform
            if(to != -1)
            {
                KillInstantly(npc);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int type = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, to);
                    if (Main.netMode == NetmodeID.Server && type < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, type);
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
