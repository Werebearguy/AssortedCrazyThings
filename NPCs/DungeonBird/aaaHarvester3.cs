using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public class aaaHarvester3 : BaseHarvester
    {
        public const string typeName = "aaaHarvester3";

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/SpawnOfOcram"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Corruptor];
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 5f; //takes 5 npc slots out of 200 when alive
            npc.width = 38;
            npc.height = 46;
            npc.damage = 111;
            npc.defense = 11;
            npc.lifeMax = 1111;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = -1; //91;
            aiType = NPCID.Zombie; //91
            animationType = NPCID.Corruptor;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.Confused] = false;

            maxVeloScale = 1.3f; //2f default
            maxAccScale = 0.04f; //0.07f default
            stuckTime = 6; //*30 for ticks, *0.5 for seconds
            afterEatTime = 60;
            eatTime = EatTimeConst - 5;
            idleTime = IdleTimeConst;
            hungerTime = 3600; //AI_Timer
            maxSoulsEaten = 3;
            jumpRange = 300; //also noclip detect range
            restrictedSoulSearch = false;
            soulsEaten = 0;
            stopTime = idleTime;
            aiTargetType = Target_Soul;
            target = 0;
            stuckTimer = 0;
            rndJump = 0;
            transformServer = false;
            transformTo = 0;
        }
    
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            //from server to client
            //writer.Write((bool)aiTargetType);
            //writer.Write((byte)soulsEaten);
            //writer.Write((byte)stuckTimer);
            //writer.Write((byte)rndJump);
            //writer.Write((short)target);
            //writer.Write((bool)transformServer);
            //Print("send: " + AI_State + " " + soulsEaten.ToString() + " " + stuckTimer.ToString() + " " + rndJump.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            //from client to server
            //aiTargetType = reader.ReadBoolean();
            //soulsEaten = reader.ReadByte();
            //stuckTimer = reader.ReadByte();
            //rndJump = reader.ReadByte();
            //target = reader.ReadInt16();
            //transformServer = reader.ReadBoolean();
            //Print("recv: " + AI_State + " " + soulsEaten.ToString() + " " + stuckTimer.ToString() + " " + rndJump.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }
        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.WaterCandle);
            Item.NewItem(npc.getRect(), ItemID.Bone, 250);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (short j = 0; j < 200; j++)
                {
                    if (Main.npc[j].active && Main.npc[j].type == mod.NPCType(AssWorld.soulName))
                    {
                        KillInstantly(Main.npc[j]);
                    }
                }
            }
        }
        
        public override void AI()
        {
            if (1 == 1)
            {
                npc.noTileCollide = true;
                npc.noGravity = true;
                if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
                {
                    npc.TargetClosest();
                }
                float num = 4.2f;
                float num2 = 0.022f;
                Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num4 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2);
                float num5 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2);
                num4 = (float)((int)(num4 / 8f) * 8);
                num5 = (float)((int)(num5 / 8f) * 8);
                vector.X = (float)((int)(vector.X / 8f) * 8);
                vector.Y = (float)((int)(vector.Y / 8f) * 8);
                num4 -= vector.X;
                num5 -= vector.Y;
                float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                float num7 = num6;
                if (num6 == 0f)
                {
                    num4 = npc.velocity.X;
                    num5 = npc.velocity.Y;
                }
                else
                {
                    num6 = num / num6;
                    num4 *= num6;
                    num5 *= num6;
                }
                if (num7 > 100f)
                {
                    npc.ai[0] += 1f;
                    if (npc.ai[0] > 0f)
                    {
                        npc.velocity.Y += 0.023f;
                    }
                    else
                    {
                        npc.velocity.Y -= 0.023f;
                    }
                    if (npc.ai[0] < -100f || npc.ai[0] > 100f)
                    {
                        npc.velocity.X += 0.023f;
                    }
                    else
                    {
                        npc.velocity.X -= 0.023f;
                    }
                    if (npc.ai[0] > 200f)
                    {
                        npc.ai[0] = -200f;
                    }
                }
                if (num7 < 150f)
                {
                    npc.velocity.X += num4 * 0.007f;
                    npc.velocity.Y += num5 * 0.007f;
                }
                if (Main.player[npc.target].dead)
                {
                    num4 = (float)npc.direction * num / 2f;
                    num5 = (0f - num) / 2f;
                }
                if (npc.velocity.X < num4)
                {
                    npc.velocity.X += num2;
                }
                else if (npc.velocity.X > num4)
                {
                    npc.velocity.X -= num2;
                }
                if (npc.velocity.Y < num5)
                {
                    npc.velocity.Y += num2;
                }
                else if (npc.velocity.Y > num5)
                {
                    npc.velocity.Y -= num2;
                }
                npc.rotation = (float)Math.Atan2((double)num5, (double)num4) - 1.57f;

                //doesn't seem to do anything because npc.notilecollide is set to false
                //float num12 = 0.7f;
                //if (npc.collideX)
                //{
                //	npc.netUpdate = true;
                //	npc.velocity.X = npc.oldVelocity.X * (0f - num12);
                //	if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                //	{
                //		npc.velocity.X = 2f;
                //	}
                //	if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                //	{
                //		npc.velocity.X = -2f;
                //	}
                //}
                //if (npc.collideY)
                //{
                //  npc.netUpdate = true;
                //	npc.velocity.Y = npc.oldVelocity.Y * (0f - num12);
                //	if (npc.velocity.Y > 0f && (double)npc.velocity.Y < 1.5)
                //	{
                //		npc.velocity.Y = 2f;
                //	}
                //	if (npc.velocity.Y < 0f && (double)npc.velocity.Y > -1.5)
                //	{
                //		npc.velocity.Y = -2f;
                //	}
                //}
                if (npc.wet)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y *= 0.95f;
                    }
                    npc.velocity.Y -= 0.3f;
                    if (npc.velocity.Y < -2f)
                    {
                        npc.velocity.Y = -2f;
                    }
                }
                if (Main.netMode != 1 && !Main.player[npc.target].dead)
                {
                    //localAI[0] is the timer for the projectile shoot
                    if (npc.justHit)
                    {
                        //makes it so it doesn't shoot projectiles when it's hit
                        //npc.localAI[0] = 0f;
                    }
                    npc.localAI[0] += 1f;
                    float shootDelay = 180f;
                    if (npc.localAI[0] == shootDelay)
                    {
                        int projectileDamage = 21;
                        int projectileType = 44; //Demon Scythe
                        int projectileTravelTime = 70;
                        float num224 = 0.2f;
                        Vector2 vector27 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float num225 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector27.X + (float)Main.rand.Next(-50, 51);
                        float num226 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector27.Y + (float)Main.rand.Next(-50, 51);
                        float num227 = (float)Math.Sqrt((double)(num225 * num225 + num226 * num226));
                        num227 = num224 / num227;
                        num225 *= num227;
                        num226 *= num227;
                        num225 *= 20;
                        num226 *= 20;
                        int leftScythe = Projectile.NewProjectile(vector27.X - npc.width * 0.5f, vector27.Y, num225, num226, projectileType, projectileDamage, 0f, Main.myPlayer);
                        Main.projectile[leftScythe].tileCollide = false;
                        Main.projectile[leftScythe].timeLeft = projectileTravelTime;

                        num225 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector27.X + (float)Main.rand.Next(-50, 51);
                        num226 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector27.Y + (float)Main.rand.Next(-50, 51);
                        num227 = (float)Math.Sqrt((double)(num225 * num225 + num226 * num226));
                        num227 = num224 / num227;
                        num225 *= num227;
                        num226 *= num227;
                        num225 *= 20;
                        num226 *= 20;
                        int rightScythe = Projectile.NewProjectile(vector27.X + npc.width * 0.5f, vector27.Y, num225, num226, projectileType, projectileDamage, 0f, Main.myPlayer);
                        Main.projectile[rightScythe].tileCollide = false;
                        Main.projectile[rightScythe].timeLeft = projectileTravelTime;

                        //NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), 112);
                        //PROJECTILE IS ACTUALLY AN NPC AHAHAHAHAHAHAHAHHAAH
                        //https://terraria.gamepedia.com/Vile_Spit

                        npc.localAI[0] = 0f;
                    }
                }
                //
                //  Main.dayTime || Main.player[npc.target].dead
                //  vvvvvvvvvvvv
                if (Main.player[npc.target].dead)
                {
                    npc.velocity.Y -= num2 * 2f;
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                }
                if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
                {
                    npc.netUpdate = true;
                }
            }
            //HarvesterAI(allowNoclip: false);
        }
    }
}
