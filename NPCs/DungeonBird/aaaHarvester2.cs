using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public class aaaHarvester2 : BaseHarvester
    {
        public const string typeName = "aaaHarvester2";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[npc.type] = 17;
        }

        public override void SetDefaults()
        {
            maxVeloScale = 1.3f; //1.3f default
            maxAccScale = 0.04f; //0.04f default
            stuckTime = 6; //*30 for ticks, *0.5 for seconds
            afterEatTime = 60;
            eatTime = EatTimeConst + 60;
            idleTime = IdleTimeConst;
            hungerTime = 3600; //AI_Timer
            maxSoulsEaten = 6;
            jumpRange = 300; //also noclip detect range
            restrictedSoulSearch = false;
            noDamage = true;


            transformTime = 120;
            soulsEaten = 0;
            stopTime = idleTime;
            aiTargetType = Target_Soul;
            target = 0;
            stuckTimer = 0;
            rndJump = 0;
            transformServer = false;
            transformTo = AssWorld.harvesterTypes[2];


            npc.npcSlots = 5f; //takes 5 npc slots out of 200 when alive
            npc.width = 50;
            npc.height = 60; //100 or 98 when flying
            npc.damage = 0;
            npc.defense = 11;
            npc.lifeMax = maxSoulsEaten + 1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.aiStyle = -1; //91;
            aiType = -1; //91
            animationType = -1;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.Confused] = false;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.velocity.X < 0f ? 1 : -1; //flipped in the sprite
            if (AI_State == State_Approach || AI_State == State_Distribute) //5 to 12
            {
                if (npc.velocity.X != 0)
                {
                    npc.frameCounter += (double)Math.Abs(npc.velocity.X / 1.5);
                    if (AI_State == State_Approach && (npc.velocity.Y == 0 || npc.velocity.Y < 3f && npc.velocity.Y > 0f) ||
                        AI_State == State_Distribute && npc.velocity.Y == 0) //fuck
                    {
                        if (npc.frameCounter <= 8.0)
                        {
                            npc.frame.Y = frameHeight * 5;
                        }
                        else if (npc.frameCounter <= 16.0)
                        {
                            npc.frame.Y = frameHeight * 6;
                        }
                        else if (npc.frameCounter <= 24.0)
                        {
                            npc.frame.Y = frameHeight * 7;
                        }
                        else if (npc.frameCounter <= 32.0)
                        {
                            npc.frame.Y = frameHeight * 8;
                        }
                        else if (npc.frameCounter <= 40.0)
                        {
                            npc.frame.Y = frameHeight * 9;
                        }
                        else if (npc.frameCounter <= 48.0)
                        {
                            npc.frame.Y = frameHeight * 10;
                        }
                        else if (npc.frameCounter <= 56.0)
                        {
                            npc.frame.Y = frameHeight * 11;
                        }
                        else if (npc.frameCounter <= 64.0)
                        {
                            npc.frame.Y = frameHeight * 12;
                        }
                        else
                        {
                            npc.frameCounter = 0;
                        }
                    }
                    else
                    {
                        if (npc.frameCounter <= 8.0)
                        {
                            npc.frame.Y = frameHeight * 13;
                        }
                        else if (npc.frameCounter <= 16.0)
                        {
                            npc.frame.Y = frameHeight * 14;
                        }
                        else if (npc.frameCounter <= 24.0)
                        {
                            npc.frame.Y = frameHeight * 15;
                        }
                        else if (npc.frameCounter <= 32.0)
                        {
                            npc.frame.Y = frameHeight * 16;
                        }
                        else
                        {
                            npc.frameCounter = 0;
                        }
                    }
                }
                else
                {
                    npc.frameCounter += 1;
                    if (npc.velocity.Y == 0 || npc.velocity.Y < 3f && npc.velocity.Y > 0f)
                    {
                        npc.frame.Y = frameHeight * 5;
                    }
                    else
                    {
                        if (npc.frameCounter <= 8.0)
                        {
                            npc.frame.Y = frameHeight * 13;
                        }
                        else if (npc.frameCounter <= 16.0)
                        {
                            npc.frame.Y = frameHeight * 14;
                        }
                        else if (npc.frameCounter <= 24.0)
                        {
                            npc.frame.Y = frameHeight * 15;
                        }
                        else if (npc.frameCounter <= 32.0)
                        {
                            npc.frame.Y = frameHeight * 16;
                        }
                        else
                        {
                            npc.frameCounter = 0;
                        }
                    }
                }
            }
            else if (AI_State == State_Noclip)
            {
                npc.frameCounter++;
                if (npc.frameCounter <= 8.0)
                {
                    npc.frame.Y = frameHeight * 13;
                }
                else if (npc.frameCounter <= 16.0)
                {
                    npc.frame.Y = frameHeight * 14;
                }
                else if (npc.frameCounter <= 24.0)
                {
                    npc.frame.Y = frameHeight * 15;
                }
                else if (npc.frameCounter <= 32.0)
                {
                    npc.frame.Y = frameHeight * 16;
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            else if (AI_State == State_Stop)
            {
                if (stopTime == eatTime)
                {
                    npc.frameCounter++;
                    if (npc.velocity.Y == 0 || npc.velocity.Y < 3f && npc.velocity.Y > 0f)
                    {
                        if (npc.frameCounter <= 8.0)
                        {
                            npc.frame.Y = 0;
                        }
                        else if (npc.frameCounter <= 16.0)
                        {
                            npc.frame.Y = frameHeight * 1;
                        }
                        else if (npc.frameCounter <= 24.0)
                        {
                            npc.frame.Y = frameHeight * 2;
                        }
                        else if (npc.frameCounter <= 32.0)
                        {
                            npc.frame.Y = frameHeight * 3;
                        }
                        else if (npc.frameCounter <= 40.0)
                        {
                            npc.frame.Y = frameHeight * 4;
                        }
                        else
                        {
                            npc.frameCounter = 0;
                        }
                    }
                    else
                    {

                        if (npc.frameCounter <= 8.0)
                        {
                            npc.frame.Y = frameHeight * 13;
                        }
                        else if (npc.frameCounter <= 16.0)
                        {
                            npc.frame.Y = frameHeight * 14;
                        }
                        else if (npc.frameCounter <= 24.0)
                        {
                            npc.frame.Y = frameHeight * 15;
                        }
                        else if (npc.frameCounter <= 32.0)
                        {
                            npc.frame.Y = frameHeight * 16;
                        }
                        else
                        {
                            npc.frameCounter = 0;
                        }

                    }
                }
                else //idleTime
                {
                    npc.frame.Y = 0;
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            //from server to client
            writer.Write((bool)aiTargetType);
            writer.Write((byte)soulsEaten);
            writer.Write((byte)stuckTimer);
            writer.Write((byte)rndJump);
            writer.Write((short)target);
            writer.Write((bool)transformServer);
            //Print("send: " + AI_State + " " + soulsEaten.ToString() + " " + stuckTimer.ToString() + " " + rndJump.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            //from client to server
            aiTargetType = reader.ReadBoolean();
            soulsEaten = reader.ReadByte();
            stuckTimer = reader.ReadByte();
            rndJump = reader.ReadByte();
            target = reader.ReadInt16();
            transformServer = reader.ReadBoolean();
            //Print("recv: " + AI_State + " " + soulsEaten.ToString() + " " + stuckTimer.ToString() + " " + rndJump.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void AI()
        {
            HarvesterAI(allowNoclip: !restrictedSoulSearch);
        }
    }
}
