using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public class aaaHarvester1 : HarvesterBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[npc.type] = 9;
        }

        public override void SetDefaults()
        {
            maxVeloScale = 1.3f; //1.3f default
            maxAccScale = 0.04f; //0.04f default
            stuckTime = 6; //*30 for ticks, *0.5 for seconds
            afterEatTime = 60;
            eatTime = EatTimeConst + 60;
            idleTime = IdleTimeConst;
            hungerTime = 1000; //AI_Timer
            maxSoulsEaten = 5; //3
            jumpRange = 100;//also noclip detect range //100 for restricted v
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
            transformTo = AssWorld.harvesterTypes[1];


            npc.dontTakeDamage = true;  //if true, it wont show hp count while mouse over
            npc.chaseable = false;
            npc.npcSlots = 1f;
            npc.width = aaaDungeonSoulBase.wid;
            npc.height = aaaDungeonSoulBase.hei;
            npc.damage = 0;
            npc.defense = 1;
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
            npc.gfxOffY = 0f;
            if (AI_State == State_Approach)
            {
                npc.frameCounter++;
                if(npc.velocity.X != 0)
                {
                    if(npc.velocity.Y == 0)
                    {
                        if (npc.frameCounter <= 8.0)
                        {
                            npc.frame.Y = frameHeight * 3;
                        }
                        else if (npc.frameCounter <= 16.0)
                        {
                            npc.frame.Y = frameHeight * 4;
                        }
                        else if (npc.frameCounter <= 24.0)
                        {
                            npc.frame.Y = frameHeight * 3;
                        }
                        else if (npc.frameCounter <= 32.0)
                        {
                            npc.frame.Y = frameHeight * 5;
                        }
                        else
                        {
                            npc.frameCounter = 0;
                        }
                    }
                    else
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                }
                else
                {
                    npc.frame.Y = frameHeight * 3;
                }
            }
            else if(AI_State == State_Noclip)
            {
                npc.frameCounter++;
                if (npc.frameCounter <= 3.0)
                {
                    npc.frame.Y = frameHeight * 7; //"fly"
                }
                else if (npc.frameCounter <= 6.0)
                {
                    npc.frame.Y = frameHeight * 8;
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            else if(AI_State == State_Transform)
            {
                npc.gfxOffY += 1f;
                npc.frame.Y = 0;
            }
            else if (AI_State == State_Stop)
            {
                npc.gfxOffY += 1f;
                if(stopTime == eatTime)
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
                            npc.frame.Y = frameHeight * 1;
                        }
                        else
                        {
                            npc.frameCounter = 0;
                        }
                    }
                    else if (!SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10))
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                }
                else
                {
                    npc.frame.Y = 0;
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (AI_State == State_Stop && stopTime == eatTime)
            {
                Texture2D texture = mod.GetTexture("Glowmasks/Harvester/aaaHarvester1_" + "souleat");
                Vector2 stupidOffset = new Vector2(0f, 3f + npc.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
                SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
                Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;

                texture = mod.GetTexture("Glowmasks/Harvester/aaaHarvester1_" + "souleat");
                spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
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
            //Print("send: " + AI_State + " " + stuckTimer.ToString() + " " + target.ToString() + " " + transformServer.ToString());
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
            //Print("recv: " + AI_State + " " + stuckTimer.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        //public override float SpawnChance(NPCSpawnInfo spawnInfo)
        //{
        //    bool shouldSpawn = true;
        //    for (short j = 0; j < 200; j++)
        //    {
        //        if (Main.npc[j].active && Array.IndexOf(AssWorld.harvesterTypes, Main.npc[j].type) != -1)
        //        {
        //            shouldSpawn = false;
        //            break;
        //        }
        //    }

        //    if (spawnInfo.player.ZoneDungeon && shouldSpawn)
        //    {
        //        //only spawns when in dungeon and when no other is alive atm
        //        return 0.04f;
        //    }
        //    else
        //    {
        //        return 0f;
        //    }
        //}

        public override void AI()
        {
            HarvesterAI(allowNoclip: !restrictedSoulSearch);
        }
    }
}
