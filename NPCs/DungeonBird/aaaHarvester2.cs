using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public class aaaHarvester2 : HarvesterBase
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
            afterEatTime = 60;
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


            npc.dontTakeDamage = true;  //if true, it wont show hp count while mouse over
            npc.chaseable = false;
            npc.npcSlots = 1f;
            npc.width = aaaDungeonSoulBase.wid;
            npc.height = aaaDungeonSoulBase.hei; //100 or 98 when flying
            npc.damage = 0;
            npc.defense = 11;
            npc.lifeMax = maxSoulsEaten + 1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.aiStyle = -1; //91;
            aiType = -1; //91
            npc.alpha = 255;
            animationType = -1;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.Confused] = false;
        }

        public override void FindFrame(int frameHeight)
        {
            //npc.spriteDirection = npc.velocity.X <= 0f ? 1 : -1; //flipped in the sprite
            npc.spriteDirection = -npc.direction;
            if (AI_State == State_Approach || AI_State == State_Distribute) //5 to 12
            {
                if (npc.velocity.X != 0)
                {
                    npc.frameCounter += (double)Math.Abs(npc.velocity.X / 1.5);
                    if (AI_State == State_Approach && (npc.velocity.Y == 0 || npc.velocity.Y < 3f && npc.velocity.Y > 0f) ||
                        AI_State == State_Distribute && SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10)) //fuck
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
                    else if(!SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10))
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

            //npc.frame.Y = frameHeight * 14;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("Glowmasks/Harvester/aaaHarvester2_" + "wings");
            Vector2 stupidOffset = new Vector2(0f, -26f + npc.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            //drawColor = new Color((int)(drawColor.R * 1.2f + 20), (int)(drawColor.G * 1.2f + 20), (int)(drawColor.B * 1.2f + 20));
            //drawColor * 2f makes it so its twice as bright as the model itself (capped at Color.White), +20f makes it so its always a bit visible
            spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);

            if (soulsEaten > 0)
            {
                if(soulsEaten < maxSoulsEaten / 2)
                {
                    texture = mod.GetTexture("Glowmasks/Harvester/aaaHarvester2_" + "soulsmall");
                }
                else if (soulsEaten != maxSoulsEaten - 1)
                {
                    texture = mod.GetTexture("Glowmasks/Harvester/aaaHarvester2_" + "soulpulse");
                }
                else
                {
                    texture = mod.GetTexture("Glowmasks/Harvester/aaaHarvester2_" + "soulbig");
                }
            }
            spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);

            if (AI_State == State_Stop && stopTime == eatTime)
            {
                texture = mod.GetTexture("Glowmasks/Harvester/aaaHarvester2_" + "souleat");
                //drawColor = npc.GetAlpha(Color.White) * ((stopTime - AI_X_Timer) / (float)stopTime);
                drawColor = Color.White;
                spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
            }

            //Spawn light, add dust
            Lighting.AddLight(npc.Center, new Vector3(0.25f, 0.25f, 0.5f) * (soulsEaten / (float)maxSoulsEaten));
            if (AI_State != State_Stop && AI_State != State_Transform && Main.rand.NextFloat() < ((soulsEaten * 0.1f) / (float)maxSoulsEaten))
            {
                Vector2 position = npc.position;

                if (AI_State != State_Noclip)
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
