using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public class Harvester2 : HarvesterBase
    {
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
            afterEatTime = 180;
            eatTime = EatTimeConst + 60;
            idleTime = IdleTimeConst;
            hungerTime = 3600; //AI_Timer
            maxSoulsEaten = 10; //10
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

            defLifeMax = maxSoulsEaten + 1;


            npc.dontTakeDamage = true;  //if true, it wont show hp count while mouse over
            npc.chaseable = false;
            npc.npcSlots = 0.5f;
            npc.width = DungeonSoulBase.wid;
            npc.height = DungeonSoulBase.hei; //100 or 98 when flying
            npc.damage = 0;
            npc.defense = 11;
            npc.scale = defScale;
            npc.lifeMax = defLifeMax;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.aiStyle = -1; //91;
            aiType = -1; //91
            npc.alpha = 255;
            animationType = -1;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.Confused] = false;
            npc.timeLeft = NPC.activeTime * 30; //doesnt do jackshit
        }

        public override void FindFrame(int frameHeight)
        {
            //npc.spriteDirection = npc.velocity.X <= 0f ? 1 : -1; //flipped in the sprite
            npc.spriteDirection = -npc.direction;
            if (AI_State == STATE_APPROACH || AI_State == STATE_DISTRIBUTE) //5 to 12
            {
                if (npc.velocity.X != 0)
                {
                    npc.frameCounter += (double)Math.Abs(npc.velocity.X / 1.5);
                    if (AI_State == STATE_APPROACH && (npc.velocity.Y == 0 || npc.velocity.Y < 3f && npc.velocity.Y > 0f) ||
                        AI_State == STATE_DISTRIBUTE && SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10)) //fuck
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
                else //if velo.x == 0
                {
                    npc.frameCounter += 1;
                    if (/*npc.velocity.Y == 0 || npc.velocity.Y < 3f && npc.velocity.Y > 0f*/ SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10))
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
            else if (AI_State == STATE_NOCLIP)
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
            else if (AI_State == STATE_STOP)
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
                    else if (!SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10))
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

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/DungeonBird/Harvester2Wings");
            Vector2 stupidOffset = new Vector2(0f, -26f + npc.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);

            if (soulsEaten > 0)
            {
                if (soulsEaten < maxSoulsEaten / 2)
                {
                    texture = mod.GetTexture("NPCs/DungeonBird/Harvester2Soulsmall");
                }
                else if (soulsEaten != maxSoulsEaten - 1)
                {
                    texture = mod.GetTexture("NPCs/DungeonBird/Harvester2Soulpulse");
                }
                else
                {
                    texture = mod.GetTexture("NPCs/DungeonBird/Harvester2Soulbig");
                }
            }
            spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);

            if (AI_State == STATE_STOP && stopTime == eatTime)
            {
                texture = mod.GetTexture("NPCs/DungeonBird/Harvester2Souleat");

                drawColor.R = Math.Max(drawColor.R, (byte)200);
                drawColor.G = Math.Max(drawColor.G, (byte)200);
                drawColor.B = Math.Max(drawColor.B, (byte)200);
                spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
            }

            //Spawn light, add dust
            Lighting.AddLight(npc.Center, new Vector3(0.25f, 0.25f, 0.5f) * (soulsEaten / (float)maxSoulsEaten));
            if (AI_State != STATE_STOP && AI_State != STATE_TRANSFORM && Main.rand.NextFloat() < ((soulsEaten * 0.1f) / maxSoulsEaten))
            {
                Vector2 position = npc.position;

                if (AI_State != STATE_NOCLIP)
                {
                    if (npc.spriteDirection == -1)
                    {
                        position += new Vector2(npc.width - 2f, 0f);
                    }

                }
                else
                {
                    position += new Vector2(npc.width / 2, -npc.height / 4);
                }
    
                Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-1.5f, 1.5f) + npc.velocity.X, Main.rand.NextFloat(-1.3f, 0.3f)), 26, new Color(255, 255, 255), Main.rand.NextFloat(1f, 1.6f));
                dust.noLight = true;
                dust.noGravity = true;
                dust.fadeIn = Main.rand.NextFloat(0f + soulsEaten / (float)maxSoulsEaten, 1f + soulsEaten / (float)maxSoulsEaten);
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
            //Print("send: " + AI_State + " " + soulsEaten.ToString() + " " + stuckTimer.ToString() + " " + rndJump.ToString() + " " + target.ToString() + " " + transformServer.ToString());
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
