using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class aaaHarvester : ModNPC
    {
        public static string name = "aaaHarvester";

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
            aiType = NPCID.GraniteFlyer; //91
            animationType = NPCID.DemonEye;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNightMonster.Chance * 0.025f;
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

        private const int AI_State_Slot = 0;
        private const int AI_X_Timer_Slot = 1;
        private const int AI_Y_Slot = 2;
        private const int AI_Unused_Slot_3 = 3;

        private const float State_Distribute = 0f;
        private const float State_FoundMove = 1f;
        private const float State_Noclip = 2f;
        private const float State_IdleMove = 3f;
        private const float State_Recalculate = 4f;

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
		
		public float AI_Unused
        {
            get
            {
                return npc.ai[AI_Unused_Slot_3];
            }
            set
            {
                npc.ai[AI_Unused_Slot_3] = value;
            }
        }

        public float AI_LocalTimer
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

        private int SoulTargetClosest()
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
            return closest; //to self
        }

        public override void AI()
        {
            //Collision.CanHit == is there direct line of sight from a to b
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.dontTakeDamage = false;

            //-------------------------------------------------------------------
            //This is only necessary for the "drop dead for two seconds" mode
            //if (npc.justHit && Main.netMode != 1 && Main.expertMode && Main.rand.Next(6) == 0)
            //{
            //    npc.netUpdate = true;
            //    AI_State = -1f;

            //    AI_X_Timer = 0f;

            //}
            //if (AI_State == -1f)
            //{
            //    //drops dead for 2 seconds, cba to reg this as a state aswell
            //    npc.dontTakeDamage = true;
            //    npc.noGravity = false;
            //    npc.velocity.X = npc.velocity.X * 0.98f;
            //    AI_X_Timer += 1f;
            //    if (AI_X_Timer >= 120f)
            //    {
            //        AI_State = (AI_X_Timer = (AI_Y = (AI_Unused = 0f)));
            //    }
            //}
            //-----------------------------------------------------------------

            if (AI_State == State_Distribute)
            {
                //Main.NewText("distribute");
                //Main.NewText(SoulTargetClosest());
                npc.TargetClosest();
                if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                {
                    //go into regular mode
                    AI_State = State_FoundMove;
                }
                else
                {
                    Vector2 between = Main.player[npc.target].Center - npc.Center;
                    between.Y -= (float)(Main.player[npc.target].height / 4);
                    float distance = between.Length();
                    if (distance > 800f)
                    {
                        //go into notilecollide mode
                        AI_State = State_Noclip;
                    }
                    else
                    {
                        Vector2 centerAlignedX = npc.Center;
                        centerAlignedX.X = Main.player[npc.target].Center.X;
                        Vector2 vector235 = centerAlignedX - npc.Center;
                        if (vector235.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, centerAlignedX, 1, 1))
                        {
                            //player unreachable now
                            AI_State = State_IdleMove;
                            AI_X_Timer = centerAlignedX.X;
                            AI_Y = centerAlignedX.Y;
                            Vector2 centerAlignedY = npc.Center;
                            centerAlignedY.Y = Main.player[npc.target].Center.Y;
                            if (vector235.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, centerAlignedY, 1, 1) && Collision.CanHit(centerAlignedY, 1, 1, Main.player[npc.target].position, 1, 1))
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
                            centerAlignedY.Y = Main.player[npc.target].Center.Y;
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
            else if (AI_State == State_FoundMove)
            {
                //Main.NewText("regular, found target");
                Vector2 between = Main.player[npc.target].Center - npc.Center;
                float distance = between.Length();
                float factor = 2f;
                factor += distance / 200f;
                int acc = 50;
                between.Normalize();
                between *= factor;
                npc.velocity = (npc.velocity * (acc - 1) + between) / acc;
                if (!Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                {
                    AI_State = State_Distribute;
                    AI_X_Timer = 0f;
                }
            }
            else if (AI_State == State_Noclip)
            {
                //Main.NewText("thru wall");
                npc.noTileCollide = true;
                Vector2 between = Main.player[npc.target].Center - npc.Center;
                float distance = between.Length();
                float factor = 2f;
                int acc = 4;
                between.Normalize();
                between *= factor;
                npc.velocity = (npc.velocity * (acc - 1) + between) / acc;
                if (distance < 600f && !Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    AI_State = State_Distribute;
                }
            }
            else if (AI_State == State_IdleMove)
            {
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
                if (distance < factor || distance > 800f || Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                {
                    AI_State = State_Distribute;
                }
            }
            else if (AI_State == State_Recalculate)
            {
                //just collided
                //Main.NewText("just collided");
                if (npc.collideX)
                {
                    npc.velocity.X = npc.velocity.X * -0.8f;
                }
                if (npc.collideY)
                {
                    npc.velocity.Y = npc.velocity.Y * -0.8f;
                }
                Vector2 vel;
                if (npc.velocity.X == 0f && npc.velocity.Y == 0f)
                {
                    vel = Main.player[npc.target].Center - npc.Center;
                    vel.Y -= (float)(Main.player[npc.target].height / 4);
                    vel.Normalize();
                    npc.velocity = vel * 0.1f;
                }
                float scaleFactor21 = 1.5f;
                float acc = 20f;
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
                if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                {
                    AI_State = State_Distribute;
                }
                AI_LocalTimer += 1f;
                //Adjust these values in the if() depending on your NPCs dimensions (Granite elemental is 20x30)
                if (AI_LocalTimer >= 5f && !Collision.SolidCollision(npc.position - new Vector2(10f, 10f), npc.width + 20, npc.height + 20))
                {
                    AI_LocalTimer = 0f;
                    Vector2 centerAlignedX = npc.Center;
                    centerAlignedX.X = Main.player[npc.target].Center.X;
                    if (Collision.CanHit(npc.Center, 1, 1, centerAlignedX, 1, 1) && Collision.CanHit(Main.player[npc.target].Center, 1, 1, centerAlignedX, 1, 1))
                    {
                        //player unreachable now
                        AI_State = State_IdleMove;
                        AI_X_Timer = centerAlignedX.X;
                        AI_Y = centerAlignedX.Y;
                    }
                    else
                    {
                        Vector2 centerAlignedY = npc.Center;
                        centerAlignedY.Y = Main.player[npc.target].Center.Y;
                        if (Collision.CanHit(npc.Center, 1, 1, centerAlignedY, 1, 1) && Collision.CanHit(Main.player[npc.target].Center, 1, 1, centerAlignedY, 1, 1))
                        {
                            //player unreachable now
                            AI_State = State_IdleMove;
                            AI_X_Timer = centerAlignedY.X;
                            AI_Y = centerAlignedY.Y;
                        }
                    }
                }
            }
        }
    }
}
