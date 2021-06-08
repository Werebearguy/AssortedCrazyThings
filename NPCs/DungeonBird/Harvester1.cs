using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public class Harvester1 : HarvesterBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[NPC.type] = 9;

            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true //Only main boss shows
            });
        }

        public override void SetDefaults()
        {
            maxVeloScale = 1.3f; //1.3f default
            maxAccScale = 0.04f; //0.04f default
            stuckTime = 6; //*30 for ticks, *0.5 for seconds
            afterEatTime = 60;
            eatTime = EatTimeConst - 30; // +60
            idleTime = IdleTimeConst;
            hungerTime = 2040; //AI_Timer //1000
            maxSoulsEaten = 5; //3
            jumpRange = 160;//also noclip detect range //100 for restricted v
            restrictedSoulSearch = true;
            noDamage = true;

            transformTime = 120;
            soulsEaten = 0;
            stopTime = idleTime;
            aiTargetType = Target_Soul;
            target = 0;
            stuckTimer = 0;
            rndJump = 0;
            transformServer = false;
            transformTo = AssortedCrazyThings.harvesterTypes[1];

            defLifeMax = maxSoulsEaten + 1;


            NPC.dontTakeDamage = true;  //if true, it wont show hp count while mouse over
            NPC.chaseable = false;
            NPC.npcSlots = 0.5f;
            NPC.width = DungeonSoulBase.wid;
            NPC.height = DungeonSoulBase.hei;
            NPC.damage = 0;
            NPC.defense = 1;
            NPC.scale = defScale;
            NPC.lifeMax = defLifeMax;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = -1; //91;
            AIType = -1; //91
            NPC.alpha = 255;
            AnimationType = -1;
            NPC.lavaImmune = true;
            NPC.buffImmune[BuffID.Confused] = false;
            NPC.timeLeft = NPC.activeTime * 30; //doesnt do jackshit
        }

        public override void FindFrame(int frameHeight)
        {
            //npc.spriteDirection = npc.velocity.X <= 0f ? 1 : -1; //flipped in the sprite
            NPC.spriteDirection = -NPC.direction;
            NPC.gfxOffY = 0f;
            if (AI_State == STATE_APPROACH)
            {
                NPC.frameCounter++;
                if (NPC.velocity.X != 0)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        if (NPC.frameCounter <= 8.0)
                        {
                            NPC.frame.Y = frameHeight * 3;
                        }
                        else if (NPC.frameCounter <= 16.0)
                        {
                            NPC.frame.Y = frameHeight * 4;
                        }
                        else if (NPC.frameCounter <= 24.0)
                        {
                            NPC.frame.Y = frameHeight * 3;
                        }
                        else if (NPC.frameCounter <= 32.0)
                        {
                            NPC.frame.Y = frameHeight * 5;
                        }
                        else
                        {
                            NPC.frameCounter = 0;
                        }
                    }
                    else
                    {
                        NPC.frame.Y = frameHeight * 6;
                    }
                }
                else
                {
                    NPC.frame.Y = frameHeight * 3;
                }
            }
            else if (AI_State == STATE_NOCLIP)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter <= 3.0)
                {
                    NPC.frame.Y = frameHeight * 7; //"fly"
                }
                else if (NPC.frameCounter <= 6.0)
                {
                    NPC.frame.Y = frameHeight * 8;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
            else if (AI_State == STATE_TRANSFORM)
            {
                NPC.gfxOffY += 1f;
                NPC.frame.Y = 0;
            }
            else if (AI_State == STATE_STOP)
            {
                NPC.gfxOffY += 1f;
                if (stopTime == eatTime)
                {
                    NPC.frameCounter++;
                    if (NPC.velocity.Y == 0 || NPC.velocity.Y < 3f && NPC.velocity.Y > 0f)
                    {
                        if (NPC.frameCounter <= 8.0)
                        {
                            NPC.frame.Y = 0;
                        }
                        else if (NPC.frameCounter <= 16.0)
                        {
                            NPC.frame.Y = frameHeight * 1;
                        }
                        else if (NPC.frameCounter <= 24.0)
                        {
                            NPC.frame.Y = frameHeight * 2;
                        }
                        else if (NPC.frameCounter <= 32.0)
                        {
                            NPC.frame.Y = frameHeight * 1;
                        }
                        else
                        {
                            NPC.frameCounter = 0;
                        }
                    }
                    else if (!SolidCollisionNew(NPC.position + new Vector2(-1f, -1f), NPC.width + 2, NPC.height + 10))
                    {
                        NPC.frame.Y = frameHeight * 6;
                    }
                }
                else
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (AI_State == STATE_STOP && stopTime == eatTime)
            {
                Texture2D texture = Mod.GetTexture("NPCs/DungeonBird/Harvester1Souleat").Value;
                Vector2 stupidOffset = new Vector2(0f, 3f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
                SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
                Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;

                drawColor.R = Math.Max(drawColor.R, (byte)200);
                drawColor.G = Math.Max(drawColor.G, (byte)200);
                drawColor.B = Math.Max(drawColor.B, (byte)200);
               spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            //from server to client
            writer.Write((byte)soulsEaten);
            writer.Write((byte)stuckTimer);
            writer.Write((byte)rndJump);
            writer.Write((short)target);
            BitsByte flags = new BitsByte();
            flags[0] = aiInit;
            flags[1] = aiTargetType;
            flags[2] = transformServer;
            writer.Write(flags);
            //Print("send: " + AI_State + " " + stuckTimer.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            //from client to server
            soulsEaten = reader.ReadByte();
            stuckTimer = reader.ReadByte();
            rndJump = reader.ReadByte();
            target = reader.ReadInt16();
            BitsByte flags = reader.ReadByte();
            aiInit = flags[0];
            aiTargetType = flags[1];
            transformServer = flags[2];
            //Print("recv: " + AI_State + " " + stuckTimer.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        public override void AI()
        {
            HarvesterAI(allowNoclip: !restrictedSoulSearch);
        }
    }
}
