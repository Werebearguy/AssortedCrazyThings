using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    [Content(ContentType.Bosses)]
    public abstract class HarvesterBase : AssNPC
    {
        public const short MaxSouls = 15;

        public const short EatTimeConst = 240; //shouldn't be equal to IdleTimeConst + 120####### //180 //+60
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

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true //Only main boss shows
            });
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
        protected short jumpRange; //also noclip detect range //100 for restricted v
        public byte maxSoulsEaten;
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

        public ref float AI_State => ref NPC.ai[AI_State_Slot];

        public ref float AI_X_Timer => ref NPC.ai[AI_X_Timer_Slot];

        public ref float AI_Y => ref NPC.ai[AI_Y_Slot];

        public ref float AI_Timer => ref NPC.ai[AI_Timer_Slot];

        public ref float AI_Local_Timer => ref NPC.localAI[0];

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
                NPC.TargetClosest();
                target = (short)NPC.target;
            }
            return target;
        }

        protected short SoulTargetClosest(bool restrictedvar = false)
        {
            short closest = 200;
            Vector2 soulPos;
            float oldDistance = 1000000000f;
            float newDistance;
            //return index of closest soul
            for (short j = 0; j < Main.maxNPCs; j++)
            {
                //ignore souls if they are noclipping
                NPC other = Main.npc[j];
                if (other.active && other.type == ModContent.NPCType<DungeonSoul>() && !Collision.SolidCollision(other.position, other.width, other.height))
                {
                    soulPos = other.Center - NPC.Center;
                    newDistance = soulPos.Length();
                    if (newDistance < oldDistance && ((restrictedvar ? (soulPos.Y > -jumpRange) : true) || Collision.CanHitLine(NPC.Center - new Vector2(0f, NPC.height), 1, NPC.height, other.Center, 1, 1)))
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
            int value2 = (int)((Position.X + Width) / 16f) + 2;
            int value3 = (int)(Position.Y / 16f) - 1;
            int value4 = (int)((Position.Y + Height) / 16f) + 2;
            value = Utils.Clamp(value, 0, Main.maxTilesX - 1);
            value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
            value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
            value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
            for (int i = value; i < value2; i++)
            {
                for (int j = value3; j < value4; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile != null && !tile.IsActuated && tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                    {
                        Vector2 vector = default(Vector2);
                        vector.X = i * 16;
                        vector.Y = j * 16;
                        int num = 16;
                        if (tile.IsHalfBlock || tile.Slope != 0)
                        {
                            vector.Y += 8f;
                            num -= 8;
                        }
                        if (Position.X + Width > vector.X && Position.X < vector.X + 16f && Position.Y + Height > vector.Y && Position.Y < vector.Y + num)
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
            //AssUtils.Print("test " + Collision.CanHitLine(tl1, 1, 1, tl2, 1, 1) + " " + Collision.CanHitLine(tr1, 1, 1, tr2, 1, 1) + " " + Collision.CanHitLine(bl1, 1, 1, bl2, 1, 1) + " " + Collision.CanHitLine(br1, 1, 1, br2, 1, 1));
            return (
                Collision.CanHitLine(tl1, 1, 1, tl2, 1, 1) &&
                Collision.CanHitLine(tr1, 1, 1, tr2, 1, 1) &&
                Collision.CanHitLine(bl1, 1, 1, bl2, 1, 1) &&
                Collision.CanHitLine(br1, 1, 1, br2, 1, 1));
        }

        protected void UpdateStuck(bool closeToSoulvar, bool allowNoclipvar)
        {
            Vector2 between = new Vector2(0f, GetTarget().Center.Y - NPC.Center.Y);
            //collideY isnt proper when its on ledges/halfbricks
            if (Main.GameUpdateCount % 30 == 0)
            {
                if ((NPC.collideX || (NPC.collideY || (NPC.velocity.Y == 0 || NPC.velocity.Y < 2f && NPC.velocity.Y > 0f))) &&
                !closeToSoulvar)
                {
                    if (!CanHitLineCombined(NPC, GetTarget()) ||
                        between.Y > 0f ||
                        between.Y <= -jumpRange)
                    {
                        //Main.NewText("TICK TOCK " + npc.collideX + " " + npc.collideY);
                        between = new Vector2(Math.Abs(NPC.Center.X - AI_X_Timer), Math.Abs(NPC.Center.Y - AI_Y));
                        //twice a second, diff is max 39f
                        if (between.Y > 100f || between.X > 35f || (NPC.wet && (between.Y > 50f || between.X > 17.5f)))
                        {
                            NPC.netUpdate = true;
                            //AssUtils.Print("NOT stuck actually");
                            stuckTimer = 0;
                        }
                        else if (between.Y <= 100f)
                        {
                            if (between.X <= 35f)
                            {
                                stuckTimer++;
                                //Base.AssUtils.Print("stucktimer++ " + stuckTimer + " " + between.X);
                                NPC.netUpdate = true;
                            }
                        }
                        if (stuckTimer >= stuckTime)
                        {
                            if (allowNoclipvar)
                            {
                                if (!SolidCollisionNew(GetTarget().position, GetTarget().width, GetTarget().height))
                                {
                                    NPC.netUpdate = true;
                                    //AssUtils.Print("noclipping");
                                    //Main.NewText("DOOR STUCK");
                                    PassCoordinates(GetTarget());
                                    AI_State = STATE_NOCLIP; //pass targets X/Y to noclip
                                }
                            }
                            else
                            {
                                DungeonSoulBase.SetTimeLeft((NPC)GetTarget(), NPC);
                            }
                            stuckTimer = 0;
                            return;
                        }
                    }
                }
            }
            if (Main.GameUpdateCount % 30 == 0) //do these always
            {
                AI_X_Timer = NPC.Center.X;
                AI_Y = NPC.Center.Y;
            }
        }

        protected bool UpdateVelocity()
        {
            Vector2 between = new Vector2(Math.Abs(GetTarget().Center.X - NPC.Center.X), GetTarget().Center.Y - NPC.Center.Y);
            float bottomY = GetTarget().BottomLeft.Y - NPC.BottomLeft.Y;
            bool lockedX = false;
            if (between.X < GetTarget().width / 2/*2f*/ && CanHitLineCombined(NPC, GetTarget())/*Collision.CanHit(npc.Center - new Vector2(2f, 2f), 4, 4, GetTarget().Center - new Vector2(2f, 2f), 4, 4)*/ && bottomY <= 16f && between.Y > -jumpRange)
            {
                //actually only locked when direct LOS and not too high
                //AssUtils.Print("set lockedX");
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

                if (NPC.velocity.X < -veloScale || NPC.velocity.X > veloScale)
                {
                    if (NPC.velocity.Y == 0f)
                    {
                        NPC.velocity *= 0.7f;
                    }
                }
                else if (NPC.velocity.X < veloScale && NPC.direction == 1)
                {
                    NPC.velocity.X += accScale;
                    if (NPC.velocity.X > veloScale)
                    {
                        NPC.velocity.X = veloScale;
                    }
                }
                else if (NPC.velocity.X > -veloScale && NPC.direction == -1)
                {
                    NPC.velocity.X -= accScale;
                    if (NPC.velocity.X < -veloScale)
                    {
                        NPC.velocity.X = -veloScale;
                    }
                }
            }
            else
            {
                NPC.velocity.X = Vector2.Zero.X;
                //  if on ground || if on downward slope
                if ((NPC.velocity.Y == 0 || NPC.velocity.Y < 1.5f && NPC.velocity.Y > 0f) /*SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10)*/ && between.Y < -32f) //jump when below two tiles
                {
                    //AssUtils.Print("jump to get to soul");
                    NPC.velocity.Y = (float)(Math.Sqrt((double)-between.Y) * -0.84f);
                    NPC.netUpdate = true;
                }
                NPC.direction = -1;
                //go to eat mode
                Entity tar = GetTarget();
                NPC tarnpc = new NPC();
                if (tar is NPC)
                {
                    tarnpc = (NPC)tar;
                }
                if (NPC.getRect().Intersects(tarnpc.getRect()))
                {
                    //AssUtils.Print("pass eatTime");
                    stopTime = eatTime;
                }
                AI_State = STATE_DISTRIBUTE;
            }
            return lockedX;
        }

        protected void UpdateOtherMovement(bool flag3var)
        {
            //bool flag3 = false;
            //if (npc.velocity.X == 0f)
            //{
            //    flag3 = true;
            //}
            bool flag22 = false;
            //might scrap it idk
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.GameUpdateCount % 60 == 1 && (NPC.velocity.X == 0f || (NPC.velocity.Y > -3f && NPC.velocity.Y < 3f)))
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



            if (NPC.velocity.Y == 0f)
            {
                int num178 = (int)(NPC.position.Y + NPC.height + 7f) / 16;
                int num179 = (int)NPC.position.X / 16;
                int num180 = (int)(NPC.position.X + NPC.width) / 16;
                int num28;
                for (int num181 = num179; num181 <= num180; num181 = num28 + 1)
                {
                    if (Main.tile[num181, num178] == null)
                    {
                        return;
                    }
                    if (Main.tile[num181, num178].HasUnactuatedTile && Main.tileSolid[Main.tile[num181, num178].TileType])
                    {
                        flag22 = true;
                        break;
                    }
                    num28 = num181;
                }
            }
            if (NPC.velocity.Y >= 0f)
            {
                int num182 = 0;
                if (NPC.velocity.X < 0f)
                {
                    num182 = -1;
                }
                if (NPC.velocity.X > 0f)
                {
                    num182 = 1;
                }
                Vector2 position2 = NPC.position;
                position2.X += NPC.velocity.X;
                int num183 = (int)((position2.X + NPC.width / 2 + ((NPC.width / 2 + 1) * num182)) / 16f);
                int num184 = (int)((position2.Y + NPC.height - 1f) / 16f);

                if (num183 * 16 < position2.X + NPC.width &&
                    num183 * 16 + 16 > position2.X &&
                    ((Main.tile[num183, num184].HasUnactuatedTile &&
                    !Main.tile[num183, num184].topSlope() &&
                    !Main.tile[num183, num184 - 1].topSlope() &&
                    Main.tileSolid[Main.tile[num183, num184].TileType] &&
                    !Main.tileSolidTop[Main.tile[num183, num184].TileType]) ||
                    (Main.tile[num183, num184 - 1].IsHalfBlock &&
                    Main.tile[num183, num184 - 1].HasUnactuatedTile)) &&

                    (!Main.tile[num183, num184 - 1].HasUnactuatedTile ||
                    !Main.tileSolid[Main.tile[num183, num184 - 1].TileType] ||
                    Main.tileSolidTop[Main.tile[num183, num184 - 1].TileType] ||
                    (Main.tile[num183, num184 - 1].IsHalfBlock &&
                    (!Main.tile[num183, num184 - 4].HasUnactuatedTile ||
                    !Main.tileSolid[Main.tile[num183, num184 - 4].TileType] ||
                    Main.tileSolidTop[Main.tile[num183, num184 - 4].TileType]))) &&

                    (!Main.tile[num183, num184 - 2].HasUnactuatedTile ||
                    !Main.tileSolid[Main.tile[num183, num184 - 2].TileType] ||
                    Main.tileSolidTop[Main.tile[num183, num184 - 2].TileType]) &&

                    (!Main.tile[num183, num184 - 3].HasUnactuatedTile ||
                    !Main.tileSolid[Main.tile[num183, num184 - 3].TileType] ||
                    Main.tileSolidTop[Main.tile[num183, num184 - 3].TileType]) &&

                    (!Main.tile[num183 - num182, num184 - 3].HasUnactuatedTile ||
                    !Main.tileSolid[Main.tile[num183 - num182, num184 - 3].TileType]))
                {
                    float num197 = num184 * 16;
                    if (Main.tile[num183, num184].IsHalfBlock)
                    {
                        num197 += 8f;
                    }
                    if (Main.tile[num183, num184 - 1].IsHalfBlock)
                    {
                        num197 -= 8f;
                    }
                    if (num197 < position2.Y + NPC.height)
                    {
                        float num198 = position2.Y + NPC.height - num197;
                        float num199 = 16.1f;
                        if (num198 <= num199)
                        {

                            //go up slopes/halfbricks
                            NPC.gfxOffY += NPC.position.Y + NPC.height - num197;
                            NPC.position.Y = num197 - NPC.height;
                            if (num198 < 9f)
                            {
                                NPC.stepSpeed = 1f;
                            }
                            else
                            {
                                NPC.stepSpeed = 2f;
                            }
                        }
                    }
                }
            }
            if (flag22)
            {
                int num200;
                int num201;
                if (1 == 1)
                {
                    num200 = (int)((NPC.position.X + NPC.width / 2 + 15 * NPC.direction) / 16f);
                    num201 = (int)((NPC.position.Y + NPC.height - 15f) / 16f);
                    //Main.tile[num200, num201 + 1].IsHalfBlock;
                }

                //adjusted here

                //int num200 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                //int num201 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
                if (!(Main.tile[num200, num201 - 1].HasUnactuatedTile && (TileLoader.IsClosedDoor(Main.tile[num200, num201 - 1]) || Main.tile[num200, num201 - 1].TileType == 388)))
                {
                    //Main.NewText("" + num200 + " " + num201);
                    if ((NPC.velocity.X < 0f && NPC.direction == -1) || (NPC.velocity.X > 0f && NPC.direction == 1)) //spritedir instead of dir before
                    {
                        //if (1 == 2)
                        //{
                        //    if (npc.height >= 32 && Main.tile[num200, num201 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num200, num201 - 2].TileType])
                        //    {
                        //        if (Main.tile[num200, num201 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num200, num201 - 3].TileType])
                        //        {
                        //            Main.NewText("1111");
                        //            npc.velocity.Y = -8f;
                        //            npc.netUpdate = true;
                        //        }
                        //        else
                        //        {
                        //            Main.NewText("2222");
                        //            npc.velocity.Y = -7f;
                        //            npc.netUpdate = true;
                        //        }
                        //    }
                        //    else if (Main.tile[num200, num201 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num200, num201 - 1].TileType])
                        //    {
                        //        Main.NewText("3333");
                        //        npc.velocity.Y = -6f;
                        //        npc.netUpdate = true;
                        //    }
                        //    else if (npc.position.Y + (float)npc.height - (float)(num201 * 16) > 20f && Main.tile[num200, num201].HasUnactuatedTile && !Main.tile[num200, num201].topSlope() && Main.tileSolid[Main.tile[num200, num201].TileType])
                        //    {
                        //        Main.NewText("4444");
                        //        npc.velocity.Y = -5f;
                        //        npc.netUpdate = true;
                        //    }
                        //    else if (npc.directionY < 0 && (!Main.tile[num200, num201 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num200, num201 + 1].TileType]) && (!Main.tile[num200 + npc.direction, num201 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num200 + npc.direction, num201 + 1].TileType]))
                        //    {
                        //        //this is for when player stands on an elevation and it just jumped aswell
                        //        Main.NewText("5555");
                        //        npc.velocity.Y = -8f;
                        //        npc.velocity.X *= 1.5f;
                        //        npc.netUpdate = true;
                        //    }
                        //    if (npc.velocity.Y == 0f && flag3 && false/* && aiFighter == 1f*/)
                        //    {
                        //        Main.NewText("6666");
                        //        npc.velocity.Y = -5f;
                        //    }
                        //}


                        //heck this ima do this MY way

                        if (/*npc.velocity.Y == 0f && */flag3var)
                        {
                            if (Main.GameUpdateCount % 60 == 35)
                            {
                                NPC.velocity.Y = -rndJump - 0.5f;
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

            if (NPC.velocity.X == 0f)
            {
                flag3 = true;
            }
            if (NPC.justHit)
            {
                flag3 = false;
            }

            if (true/*&& AI_Timer < (float)aiFighterLimit*/)
            {
                if (Main.GameUpdateCount % 60 == 0)
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
                Vector2 between = GetTarget().Center - NPC.Center;
                if (between.Length() < 20f)
                {
                    AI_Timer = 0f; //when literally near the soul
                    closeToSoul = true; //used to prevent the stuck timer to run
                }
                if (Math.Abs(between.X) > 2f) NPC.direction = (between.X <= 0f) ? -1 : 1;
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
                    //AssUtils.Print("passed to noclip aaaaaaaaaaaaaaaaaaaa");
                    AI_State = STATE_NOCLIP; //this is needed in order for the harvester to keep progressing
                }
                if (target == 200)
                {
                    AI_Timer -= 360; //6 seconds
                }
                NPC.netUpdate = true;
            }

            UpdateStuck(closeToSoul, allowNoclip);

            //if not locked, do othermovement
            if (!UpdateVelocity()) UpdateOtherMovement(flag3);



            //---------------------------------------------------------------------
            //NEW: DROP THROUGH PLATFORMS WHEN SOUL BELOW
            int tilex = (int)(NPC.position.X / 16f);
            int tiley = (int)((NPC.position.Y + NPC.height + 15f) / 16f);

            Point16 point1 = new Point16(tilex, tiley);
            Point16 point2 = new Point16(tilex + 1, tiley);
            Point16 point3 = new Point16(tilex + 2, tiley);

            Tile tile1 = Framing.GetTileSafely(point1);
            Tile tile2 = Framing.GetTileSafely(point2);
            Tile tile3 = Framing.GetTileSafely(point3);

            bool atleastOneSolidBelow = (!tile1.IsActuated && tile1.HasTile && Main.tileSolid[tile1.TileType] && !TileID.Sets.Platforms[tile1.TileType]) ||
                (!tile2.IsActuated && tile2.HasTile && Main.tileSolid[tile2.TileType] && !TileID.Sets.Platforms[tile2.TileType]);

            if (!atleastOneSolidBelow &&
                ((NPC.direction == -1) ? TileID.Sets.Platforms[tile3.TileType] : true) && (GetTarget().Top.Y - NPC.Bottom.Y) > 0f)
            {
                NPC.netUpdate = true;
                NPC.position.Y += 1f;
            }
        }

        protected void HarvesterAI(bool allowNoclip = true)
        {
            NPC.scale = defScale;
            NPC.lifeMax = defLifeMax;

            if (Main.GameUpdateCount % 120 == 2)
            {
                if (NPC.timeLeft > 750) NPC.timeLeft = 750;

                bool shouldDecreaseTime = false;
                bool allPlayersDead = true;
                int closest = 255;
                Vector2 playerPos;
                float oldDistance = 1000000000f;
                float newDistance;

                //return index of closest player
                for (short j = 0; j < Main.maxPlayers; j++)
                {
                    Player player = Main.player[j];
                    if (player.active)
                    {
                        playerPos = player.Center - NPC.Center;
                        newDistance = playerPos.Length();
                        if (newDistance < oldDistance)
                        {
                            oldDistance = newDistance;
                            closest = j;
                            shouldDecreaseTime = true; //atleast one player is found
                        }
                        if (allPlayersDead && !player.dead) allPlayersDead = false;
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
                        NPC.timeLeft -= 4; //check every two seconds, decrease 2 every second from 750 until 0: 6.25 minutes
                    }
                    else
                    {
                        NPC.timeLeft = 750;
                    }

                    if (NPC.timeLeft < 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.life = 0;
                            NPC.active = false;
                            if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
                        }
                    }
                }

                NPC.netUpdate = true;
            }

            NPC.noGravity = false;
            NPC.noTileCollide = false;

            if (Main.GameUpdateCount % 30 == 0)
            {
                for (int k = 0; k < 256; k++)
                {
                    NPC.immune[k] = 40;
                }
            }

            if (!aiInit/* && Main.netMode != 1*/)
            {
                NPC.life = soulsEaten + 1;
                //initialize it to go for souls first
                aiTargetType = Target_Soul;
                stopTime = idleTime;
                SelectTarget(restrictedSoulSearch);
                aiInit = true;
                AI_Timer = 0f;
                //AI_Local_Timer = 0f;
                NPC.netUpdate = true;
            }

            if (NPC.alpha > 0)
            {
                NPC.alpha -= 5;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
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
                if (!IsTargetActive())
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
                    if (IsTargetActive() && target != 200)
                    {
                        AI_Timer = 0;
                        //goto noclip 
                        if (IsTargetActive())
                            PassCoordinates(GetTarget());
                        //AssUtils.Print("passed to noclip aaaaaaaaaaaaaaaaaaaa");
                        AI_State = STATE_NOCLIP; //this is needed in order for the harvester to keep progressing
                    }
                    if (target == 200)
                    {
                        AI_Timer -= 360;
                    }
                    NPC.netUpdate = true;
                }
            }

            //if (IsTargetActive())
            //{
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                AI_Timer++; //count hungerTime up, always
            }
            //}

            //Attack player
            if (AI_Timer % 30 == 0)
            {
                if (AI_State == STATE_STOP && aiTargetType == Target_Player && soulsEaten <= maxSoulsEaten)
                {
                    AttackPlayer(5, 3f, 200, aiTargetType);
                }
                else if (AI_State != STATE_STOP && aiTargetType == Target_Soul)
                {
                    AttackPlayer(5, 3f, NPC.width, aiTargetType);
                }
            }


            if (AI_State == STATE_DISTRIBUTE/*0*/)
            {
                if (Main.GameUpdateCount % 20 == 0)
                {
                    SelectTarget(restrictedSoulSearch);
                }
                //else if (target == 200)
                //{
                //    //SelectTarget(restrictedSoulSearch);
                //}

                //check if atleast one of four tiles underneath exists properly , aka "on the ground"

                if (stopTime == eatTime &&
                    (NPC.velocity.Y == 0 || (NPC.velocity.Y < 2f && NPC.velocity.Y > 0f)) && //still (or on slope)
                    (GetTarget().velocity.Y == 0 || (GetTarget().velocity.Y < 2f && GetTarget().velocity.Y > 0f))//still (or on slope)
                                                                                                                 /*&& !Collision.SolidCollision(npc.position, npc.width, npc.height)*/)
                {
                    //sitting on soul, go into stop/eat mode
                    if (NPC.velocity.Y != 0)
                    {
                        int num200 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
                        int num201 = (int)((NPC.position.Y + (float)NPC.height - 15f) / 16f);
                        //Main.tile[num200, num201 + 1].IsHalfBlock;
                    }

                    NPC.netUpdate = true;

                    //AssUtils.Print("distribute to stop");
                    DungeonSoulBase.SetTimeLeft((NPC)GetTarget(), NPC);
                    aiTargetType = Target_Player;
                    SelectTarget(restrictedSoulSearch); //now player

                    AI_X_Timer = 0f;
                    AI_State = STATE_STOP; //start to eat
                }
                else if (stopTime != eatTime)
                {
                    NPC.netUpdate = true;

                    //go into regular mode
                    PassCoordinates(NPC);
                    stuckTimer = 0;
                    if (NPC.direction == 0)
                    {
                        NPC.direction = 1;
                    }
                    if (AI_Timer > hungerTime - 180) AI_Timer -= 180; //halve hunger timer
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
                NPC.noGravity = true;
                NPC.noTileCollide = true;
                Vector2 between = new Vector2(AI_X_Timer - NPC.Center.X, AI_Y - NPC.Center.Y); //use latest known position from UpdateStuck of target
                Vector2 normBetween = new Vector2(between.X, between.Y);

                NPC.direction = (normBetween.X <= 0f) ? -1 : 1;

                float distance = normBetween.Length();
                float factor = 2.5f; //2f
                int acc = 30; //4
                normBetween.Normalize();
                normBetween *= factor;
                NPC.velocity = (NPC.velocity * (acc - 1) + normBetween) / acc;
                //concider only the bottom half of the hitbox (minus a small bit below)
                //Main.NewText(Collision.SolidCollision(npc.position + new Vector2(-2f, npc.height / 2), npc.width + 4, npc.height / 2 + 4));
                if (distance < NPC.height /*600f*/ && between.Y < 20f && !Collision.SolidCollision(NPC.position + new Vector2(-0f, NPC.height / 2), NPC.width + 0, NPC.height / 2 - 4))
                {
                    NPC.netUpdate = true;
                    AI_State = STATE_DISTRIBUTE;
                }
            }
            else if (AI_State == STATE_STOP/*3*/)
            {

                if (stopTime == eatTime)
                {
                    if (AI_X_Timer == 0f)
                    {
                        AI_Timer = 0;
                        //AssUtils.Print("started eating");
                        NPC.netUpdate = true;
                    }

                    //if alteast one soul intersects with hitbox
                    bool intersects = false;
                    for (int k = 0; k < Main.maxNPCs; k++)
                    {
                        NPC npc = Main.npc[k];
                        if (npc.active && npc.type == ModContent.NPCType<DungeonSoul>())
                        {
                            if (base.NPC.getRect().Intersects(npc.getRect()))
                            {
                                intersects = true;
                                //if (Main.GameUpdateCount % 60 == 0) Main.NewText("intersects with " + k);
                                break;
                            }
                        }
                    }
                    if (!intersects)
                    {
                        NPC.netUpdate = true;
                        AI_State = STATE_DISTRIBUTE;
                        aiInit = false; //reinitialize
                        AI_X_Timer = 0f;
                        //AssUtils.Print("cancelled eating");
                    }
                }
                NPC.noGravity = false;

                NPC.velocity.X = 0f;
                AI_X_Timer += 1f;
                if (AI_X_Timer > stopTime)
                {
                    NPC.netUpdate = true;
                    AI_State = STATE_DISTRIBUTE;
                    AI_X_Timer = 0f;

                    if (stopTime == idleTime)
                    {
                        SelectTarget(restrictedSoulSearch); //to retarget for the IsActive check (otherwise it gets stuck in this state)
                    }
                    else if (stopTime == eatTime)
                    {
                        //AssUtils.Print("finished eating");
                        aiInit = false; //reinitialize
                        soulsEaten++;
                        //npc.HealEffect(npc.lifeMax - 1 - ++soulsEaten); //life gets set manually anyway so it doesnt matter what number is here

                        //poof visual
                        for (int i = 0; i < 20; i++)
                        {
                            Dust dust = Dust.NewDustPerfect(NPC.BottomLeft + new Vector2(NPC.width / 2, -NPC.height / 4), 59, new Vector2(Main.rand.NextFloat(-1.5f, 1.5f) + NPC.velocity.X, Main.rand.NextFloat(-1.5f, 1f)), 26, new Color(255, 255, 255), Main.rand.NextFloat(1.5f, 2.4f));
                            dust.noLight = true;
                            dust.noGravity = true;
                            dust.fadeIn = Main.rand.NextFloat(0f, 0.5f);
                        }

                        if (soulsEaten >= maxSoulsEaten)
                        {
                            //AssUtils.Print("souls eaten max reached");
                            NPC.life = NPC.lifeMax;
                            //soulsEaten = 0;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                aiInit = true; //skip the reinit
                                transformServer = true;
                                //AssUtils.Print("set transform var to tru");
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
                NPC.velocity.X = 0;
                if (transformServer)
                {
                    AI_X_Timer++;

                    int randfactor = Main.rand.Next(2, 6);
                    //aitimer: from 0 to transformtime:
                    //rate: from 0 to 0.8 //((AI_X_Timer * 0.8f) / transformTime)
                    if (Main.GameUpdateCount % 8 == 0 && Main.rand.NextFloat() < ((AI_X_Timer * 0.8f) / transformTime))
                    {
                        SpawnBoneShort(NPC.Center + new Vector2(0f, -NPC.height / 4), new Vector2(Main.rand.NextFloat(-1f, 1f) * randfactor, -Main.rand.NextFloat() * randfactor), 0, 1.5f);
                    }

                    //51 is because 255 is decremented by 5 in the transformTo alpha
                    if (AI_X_Timer == (transformTime - 51))
                    {
                        Transform(transformTo);
                    }

                    if (AI_X_Timer > transformTime)
                    {
                        KillInstantly(NPC);
                    }
                    //handle fade out in GetAlpha scaled on AI_X_Timer,
                    //and fade in via alpha
                }
            }
        }

        private void AttackPlayer(int dmg, float knock, int distance, bool prevTargetType)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                aiTargetType = Target_Player;
                SelectTarget();
                Rectangle rect = new Rectangle((int)NPC.Center.X - distance, (int)NPC.Center.Y - distance, NPC.width + distance * 2, NPC.height + distance * 2);
                if (rect.Intersects(new Rectangle((int)GetTarget().position.X, (int)GetTarget().position.Y, GetTarget().width, GetTarget().height)) &&
                    (Collision.CanHitLine(NPC.Center, 1, 1, GetTarget().Center, 1, 1) || (GetTarget().Center.Y - NPC.Center.Y) <= 0f))
                //if either direct LOS or player above (so it can go around small obstacles)
                {
                    Vector2 between = GetTarget().Center - NPC.Center;
                    between.Normalize();
                    if (between.Y > 0.2f) between *= 1.1f; //small arc
                    if (between.Y > 0.8f) between *= 1.1f; //big arc
                    between *= 7f;
                    SpawnBoneShort(NPC.Center + new Vector2(0f, -NPC.height / 4), between, dmg, knock);
                }

                aiTargetType = prevTargetType;
                SelectTarget();
            }
        }


        private void SpawnBoneShort(Vector2 pos, Vector2 vel, int dmg, float knock)
        {
            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), pos, vel, ModContent.ProjectileType<HarvesterBone>(), dmg, knock, Main.myPlayer);
        }

        public void Transform(int to)
        {
            //set to -1 to not transform
            if (to != -1)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int index = NPC.NewNPC(NPC.GetSpawnSourceForNPCFromNPCAI(), (int)NPC.Center.X, (int)NPC.Center.Y, to, 150); //150 for index 150 atleast
                                                                                           //(so the claws will likely spawn with lower index and rendered infront)
                    Main.npc[index].SetDefaults(to);
                    if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, index);
                    }
                }
            }
        }

        public override void AI()
        {
            //HarvesterAI(allowNoclip: true);
            //if (transformServer) Transform(transformTo);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            //Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[AssortedCrazyThings.harvesterTypes[2]], quickUnlock: true);
        }
    }

    public static class TileExtensions
    {
        //TODO 1.4 Old vanilla method, no tml replacement
        internal static bool topSlope(this Tile tile)
        {
            SlopeType slope = tile.Slope;
            return slope == SlopeType.SlopeDownLeft || slope == SlopeType.SlopeDownRight;
        }
    }
}
