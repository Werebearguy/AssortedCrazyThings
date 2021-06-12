using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public class Harvester2 : HarvesterBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault(name);
            Main.npcFrameCount[NPC.type] = 17;
        }

        public override void SetDefaults()
        {
            maxVeloScale = 1.3f; //1.3f default
            maxAccScale = 0.04f; //0.04f default
            stuckTime = 6; //*30 for ticks, *0.5 for seconds
            afterEatTime = 180;
            eatTime = EatTimeConst - 30; // +60
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
            transformTo = AssortedCrazyThings.harvesterTypes[2];

            defLifeMax = maxSoulsEaten + 1;


            NPC.dontTakeDamage = true;  //if true, it wont show hp count while mouse over
            NPC.chaseable = false;
            NPC.npcSlots = 0.5f;
            NPC.width = DungeonSoulBase.wid;
            NPC.height = DungeonSoulBase.hei; //100 or 98 when flying
            NPC.damage = 0;
            NPC.defense = 11;
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

        public void SpawnDust(int frame)
        {
            Rectangle left = new Rectangle();
            Rectangle right = new Rectangle();
            switch (frame)
            {
                case 13:
                    left.X = (int)NPC.Center.X - 60;
                    right.X = (int)NPC.Center.X + 10;
                    left.Y = right.Y = (int)NPC.Top.Y - 62;
                    left.Width = right.Width = 48;
                    left.Height = right.Height = 52;
                    break;
                case 14:
                    left.X = (int)NPC.Center.X - 80;
                    right.X = (int)NPC.Center.X + 10;
                    left.Y = right.Y = (int)NPC.Top.Y - 20;
                    left.Width = right.Width = 60;
                    left.Height = right.Height = 10;
                    break;
                case 15:
                    left.X = (int)NPC.Center.X - 30;
                    right.X = (int)NPC.Center.X - 10;
                    left.Y = right.Y = (int)NPC.Top.Y - 20;
                    left.Width = right.Width = 30;
                    left.Height = right.Height = 60;
                    break;
                case 16:
                    left.X = (int)NPC.Center.X - 80;
                    right.X = (int)NPC.Center.X + 10;
                    left.Y = right.Y = (int)NPC.Top.Y - 8;
                    left.Width = right.Width = 60;
                    left.Height = right.Height = 10;
                    break;
                default:
                    break;
            }

            if (left.X != 0)
            {
                List<Rectangle> list = new List<Rectangle>() { left, right };

                foreach (Rectangle dustBox in list)
                {
                    if (Main.rand.NextFloat() < (float)soulsEaten / maxSoulsEaten)
                    {
                        Dust dust = Dust.NewDustDirect(dustBox.TopLeft(), dustBox.Width, dustBox.Height, 135, 0f, 0f, 0, default(Color), 1.5f);
                        dust.noGravity = true;
                        dust.noLight = true;
                        dust.velocity *= 0.3f;
                        if (Main.rand.NextBool(5))
                        {
                            dust.fadeIn = 1f;
                        }
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            //npc.spriteDirection = npc.velocity.X <= 0f ? 1 : -1; //flipped in the sprite
            NPC.spriteDirection = -NPC.direction;
            if (AI_State == STATE_APPROACH || AI_State == STATE_DISTRIBUTE) //5 to 12
            {
                if (NPC.velocity.X != 0)
                {
                    NPC.frameCounter += Math.Abs(NPC.velocity.X / 1.5);
                    if (AI_State == STATE_APPROACH && (NPC.velocity.Y == 0 || NPC.velocity.Y < 3f && NPC.velocity.Y > 0f) ||
                        AI_State == STATE_DISTRIBUTE && SolidCollisionNew(NPC.position + new Vector2(-1f, -1f), NPC.width + 2, NPC.height + 10)) //fuck
                    {
                        if (NPC.frameCounter <= 8.0)
                        {
                            NPC.frame.Y = frameHeight * 5;
                        }
                        else if (NPC.frameCounter <= 16.0)
                        {
                            NPC.frame.Y = frameHeight * 6;
                        }
                        else if (NPC.frameCounter <= 24.0)
                        {
                            NPC.frame.Y = frameHeight * 7;
                        }
                        else if (NPC.frameCounter <= 32.0)
                        {
                            NPC.frame.Y = frameHeight * 8;
                        }
                        else if (NPC.frameCounter <= 40.0)
                        {
                            NPC.frame.Y = frameHeight * 9;
                        }
                        else if (NPC.frameCounter <= 48.0)
                        {
                            NPC.frame.Y = frameHeight * 10;
                        }
                        else if (NPC.frameCounter <= 56.0)
                        {
                            NPC.frame.Y = frameHeight * 11;
                        }
                        else if (NPC.frameCounter <= 64.0)
                        {
                            NPC.frame.Y = frameHeight * 12;
                        }
                        else
                        {
                            NPC.frameCounter = 0;
                            NPC.frame.Y = frameHeight * 5;
                        }
                    }
                    else
                    {
                        if (NPC.frameCounter <= 8.0)
                        {
                            NPC.frame.Y = frameHeight * 13;
                        }
                        else if (NPC.frameCounter <= 16.0)
                        {
                            NPC.frame.Y = frameHeight * 14;
                        }
                        else if (NPC.frameCounter <= 24.0)
                        {
                            NPC.frame.Y = frameHeight * 15;
                        }
                        else if (NPC.frameCounter <= 32.0)
                        {
                            NPC.frame.Y = frameHeight * 16;
                        }
                        else
                        {
                            NPC.frameCounter = 0;
                            NPC.frame.Y = frameHeight * 13;
                        }
                    }
                }
                else //if velo.x == 0
                {
                    NPC.frameCounter += 1;
                    if (/*npc.velocity.Y == 0 || npc.velocity.Y < 3f && npc.velocity.Y > 0f*/ SolidCollisionNew(NPC.position + new Vector2(-1f, -1f), NPC.width + 2, NPC.height + 10))
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                    else
                    {
                        if (NPC.frameCounter <= 8.0)
                        {
                            NPC.frame.Y = frameHeight * 13;
                        }
                        else if (NPC.frameCounter <= 16.0)
                        {
                            NPC.frame.Y = frameHeight * 14;
                        }
                        else if (NPC.frameCounter <= 24.0)
                        {
                            NPC.frame.Y = frameHeight * 15;
                        }
                        else if (NPC.frameCounter <= 32.0)
                        {
                            NPC.frame.Y = frameHeight * 16;
                        }
                        else
                        {
                            NPC.frameCounter = 0;
                            NPC.frame.Y = frameHeight * 13;
                        }
                    }
                }
            }
            else if (AI_State == STATE_NOCLIP)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter <= 8.0)
                {
                    NPC.frame.Y = frameHeight * 13;
                }
                else if (NPC.frameCounter <= 16.0)
                {
                    NPC.frame.Y = frameHeight * 14;
                }
                else if (NPC.frameCounter <= 24.0)
                {
                    NPC.frame.Y = frameHeight * 15;
                }
                else if (NPC.frameCounter <= 32.0)
                {
                    NPC.frame.Y = frameHeight * 16;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
            else if (AI_State == STATE_STOP)
            {
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
                            NPC.frame.Y = frameHeight * 3;
                        }
                        else if (NPC.frameCounter <= 40.0)
                        {
                            NPC.frame.Y = frameHeight * 4;
                        }
                        else
                        {
                            NPC.frameCounter = 0;
                        }
                    }
                    else if (!SolidCollisionNew(NPC.position + new Vector2(-1f, -1f), NPC.width + 2, NPC.height + 10))
                    {
                        if (NPC.frameCounter <= 8.0)
                        {
                            NPC.frame.Y = frameHeight * 13;
                        }
                        else if (NPC.frameCounter <= 16.0)
                        {
                            NPC.frame.Y = frameHeight * 14;
                        }
                        else if (NPC.frameCounter <= 24.0)
                        {
                            NPC.frame.Y = frameHeight * 15;
                        }
                        else if (NPC.frameCounter <= 32.0)
                        {
                            NPC.frame.Y = frameHeight * 16;
                        }
                        else
                        {
                            NPC.frameCounter = 0;
                        }
                    }
                }
                else //idleTime
                {
                    NPC.frame.Y = 0;
                }
            }

            SpawnDust(NPC.frame.Y / frameHeight);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/DungeonBird/Harvester2Wings").Value;
            Vector2 stupidOffset = new Vector2(0f, -26f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);

            if (soulsEaten > 0)
            {
                if (soulsEaten < maxSoulsEaten / 2)
                {
                    texture = Mod.GetTexture("NPCs/DungeonBird/Harvester2Soulsmall").Value;
                }
                else if (soulsEaten != maxSoulsEaten - 1)
                {
                    texture = Mod.GetTexture("NPCs/DungeonBird/Harvester2Soulpulse").Value;
                }
                else
                {
                    texture = Mod.GetTexture("NPCs/DungeonBird/Harvester2Soulbig").Value;
                }
            }
            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);

            if (AI_State == STATE_STOP && stopTime == eatTime)
            {
                texture = Mod.GetTexture("NPCs/DungeonBird/Harvester2Souleat").Value;

                drawColor.R = Math.Max(drawColor.R, (byte)200);
                drawColor.G = Math.Max(drawColor.G, (byte)200);
                drawColor.B = Math.Max(drawColor.B, (byte)200);
                spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            }

            //Spawn light, add dust
            Lighting.AddLight(NPC.Center, new Vector3(0.25f, 0.25f, 0.5f) * (soulsEaten / (float)maxSoulsEaten));
            if (AI_State != STATE_STOP && AI_State != STATE_TRANSFORM && Main.rand.NextFloat() < ((soulsEaten * 0.1f) / maxSoulsEaten))
            {
                Vector2 position = NPC.position;

                if (AI_State != STATE_NOCLIP)
                {
                    if (NPC.spriteDirection == -1)
                    {
                        position += new Vector2(NPC.width - 2f, 0f);
                    }

                }
                else
                {
                    position += new Vector2(NPC.width / 2, -NPC.height / 4);
                }

                Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-1.5f, 1.5f) + NPC.velocity.X, Main.rand.NextFloat(-1.3f, 0.3f)), 26, new Color(255, 255, 255), Main.rand.NextFloat(1f, 1.6f));
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
