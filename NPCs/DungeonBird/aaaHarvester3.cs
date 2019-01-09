using System;
using System.IO;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.VanityArmor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    //[AutoloadBossHead]
    public class aaaHarvester3 : ModNPC
    {
        public static string name = "aaaHarvester3";

        public static float sinY = 0;
        public static int talonDamage = 30;
        public static int Wid = 110;
        public static int Hei = 110;

        public static int TalonOffsetLeftX = -Wid / 4/* - 14*/; //-84
        public static int TalonOffsetRightX = Wid / 4/* + 8*/; // 78
        public static int TalonOffsetY = Hei/2 - 7;              //-9 //normally its negative

        public static int TalonDirectionalOffset = 10;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name); //defined above since its used in CaughtDungeonSoul
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            //npc.SetDefaults(NPCID.QueenBee);
            npc.boss = true;
            npc.npcSlots = 10f; //takes 10 npc slots , so no other npcs can spawn during the fight
            //actual body hitbox
            npc.width = Wid; //302 texture //104
            npc.height = Hei; //176 texture //110
            npc.damage = 20; //contact damage
            npc.defense = 8;
            npc.lifeMax = 1111;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = Item.buyPrice(0, 15);
            npc.knockBackResist = 0f;
            npc.aiStyle = -1; //91;
            aiType = -1; //91
            animationType = -1;
            npc.timeLeft = NPC.activeTime * 30;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.Confused] = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.alpha = 255;

            //queenbee setdefaults
            //width = 66;
            //height = 66;
            //damage = 30;
            //defense = 8;
            //npc.lifeMax = 3400;
            //HitSound = SoundID.NPCHit1;
            //DeathSound = SoundID.NPCDeath1;
            //knockBackResist = 0f;
            //noGravity = true;
            //noTileCollide = true;
            //timeLeft = activeTime * 30;
            //boss = true;
            //value = 100000f;
            //npcSlots = 7f;

            //golem body setdefaults
            //width = 140;
            //height = 140;
            //aiStyle = 45;
            //damage = 72;
            //defense = 26;
            //lifeMax = 9000;
            //HitSound = SoundID.NPCHit4;
            //DeathSound = SoundID.NPCDeath14;
            //knockBackResist = 0f;
            //value = (float)Item.buyPrice(0, 15);
            //alpha = 255;
            //boss = true;
            //buffImmune[20] = true;
            //buffImmune[24] = true;
        }

        public override void FindFrame(int frameHeight)
        {
            //npc.spriteDirection = npc.velocity.X <= 0f ? 1 : -1; //flipped in the sprite
            npc.spriteDirection = -npc.direction;
            npc.frameCounter++;

            if(npc.alpha > 0)
            {
                npc.frame.Y = frameHeight * 4;
                npc.frameCounter = 40.0;
                return;
            }

            //0 1 2 3 4 | 3 2 1 0
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
            else if (npc.frameCounter <= 48.0)
            {
                npc.frame.Y = frameHeight * 3;
            }
            else if (npc.frameCounter <= 56.0)
            {
                npc.frame.Y = frameHeight * 2;
            }
            else if (npc.frameCounter <= 64.0)
            {
                npc.frame.Y = frameHeight * 1;
            }
            else
            {
                npc.frameCounter = 0;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("Glowmasks/Harvester/aaaHarvester3_" + "wings");
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);

            Vector2 stupidOffset = new Vector2(0, -29f + npc.gfxOffY);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;

            spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), Color.White * ((255 - npc.alpha) / 255f), npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - npc.alpha)/255f);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Bone, Main.rand.Next(40, 61));
            Item.NewItem(npc.getRect(), mod.ItemType<SoulHarvesterMask>());
            Item.NewItem(npc.getRect(), mod.ItemType<DesiccatedLeather>(), Main.rand.Next(15, 26));
            //you need to kill it two times to craft the whole armor set
            // (15+15 == 10 + 10 + 10)

            Vector2 randVector = new Vector2(1, 1);
            float randFactor = 0f;

            int npcTypeOld = mod.NPCType<aaaDungeonSoul>();
            int npcTypeNew = mod.NPCType<aaaDungeonSoulAwakened>();  //version that doesnt get eaten by harvesters

            int itemTypeOld = mod.ItemType<CaughtDungeonSoul>();
            int itemTypeNew = mod.ItemType<CaughtDungeonSoulAwakened>(); //version that is used in crafting

            for (int i = 0; i < 15; i++) //spawn souls when dies, 15 total
            {
                randVector = randVector.RotatedByRandom(MathHelper.ToRadians(359f));
                randFactor = Main.rand.NextFloat(2f, 8f);
                int index = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, npcTypeNew);
                Main.npc[index].SetDefaults(npcTypeNew);
                Main.npc[index].velocity = randVector * randFactor;
                Main.npc[index].ai[2] = Main.rand.Next(1, aaaDungeonSoulBase.offsetYPeriod); //doesnt get synced properly to clients idk
            }

            //"convert" NPC souls
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && Main.npc[j].type == npcTypeOld)
                {
                    Main.npc[j].active = false;
                    int index = NPC.NewNPC((int)Main.npc[j].position.X, (int)Main.npc[j].position.Y, npcTypeNew);
                    Main.npc[index].SetDefaults(npcTypeNew);
                    Main.npc[index].ai[2] = Main.rand.Next(1, aaaDungeonSoulBase.offsetYPeriod); //doesnt get synced properly to clients idk

                    //poof visual
                    for (int i = 0; i < 20; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(Main.npc[index].Center, 59, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
                        dust.noLight = true;
                        dust.noGravity = true;
                        dust.fadeIn = Main.rand.NextFloat(0f, 0.5f);
                    }
                }
            }

            //"convert" Item souls
            for (int j = 0; j < Main.player.Length; j++)
            {
                if(Main.player[j].active/* && !Main.player[j].dead*/)
                {
                    int tempStackCount = 0;

                    Item[][] inventoryArray = {Main.player[j].inventory, Main.player[j].bank.item, Main.player[j].bank2.item, Main.player[j].bank3.item }; //go though player inv
                    for (int y = 0; y < inventoryArray.Length; y++)
                    {
                        for (int e = 0; e < inventoryArray[y].Length; e++)
                        {
                            if (inventoryArray[y][e].type == itemTypeOld) //find inert soul
                            {
                                tempStackCount = inventoryArray[y][e].stack;
                                inventoryArray[y][e].SetDefaults(itemTypeNew); //override with awakened
                                inventoryArray[y][e].stack = tempStackCount;
                            }
                        }
                    }
                }
            }

            AssWorld.downedHarvester = true;
        }

        private const int AI_State_Slot = 0;
        private const int AI_Timer_Slot = 1;
        private const int AI_Counter_Slot = 2;
        private const int AI_Unused_Slot = 3;

        private const float State_Distribute = -1f;
        private const float State_Dash = 0f;
        private const float State_Hover = 1f;
        private const float State_Retarget = 2f;
        private const float State_Main = 3f;

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

        public float AI_Counter
        {
            get
            {
                return npc.ai[AI_Counter_Slot];
            }
            set
            {
                npc.ai[AI_Counter_Slot] = value;
            }
        }

        public float AI_Unused
        {
            get
            {
                return npc.ai[AI_Unused_Slot];
            }
            set
            {
                npc.ai[AI_Unused_Slot] = value;
            }
        }

        public float AI_Local1
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

        public float AI_Local2
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

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Vector3(0.3f, 0.3f, 0.7f));

            npc.gfxOffY = npc.height / 2;
            if (Main.netMode != NetmodeID.Server)
            {
                sinY = (float)((Math.Sin(((Main.time % 120.0) / 120.0) * 2.0 * Math.PI) - 1) * 6);
            }
            npc.gfxOffY += sinY;
            return true;
        }

        public override void AI()
        {

            //int num603 = 0;
            //int someIndex;

            if (AI_Local2 == 0)
            {
                AssWorld.harvesterIndex = npc.whoAmI;
                if(Main.netMode != 1)
                {
                    Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                    int index1 = NPC.NewNPC((int)npc.Center.X + TalonOffsetLeftX, (int)npc.Center.Y + TalonOffsetY, AssWorld.harvesterTalonLeft);
                    int index2 = NPC.NewNPC((int)npc.Center.X + TalonOffsetRightX, (int)npc.Center.Y + TalonOffsetY, AssWorld.harvesterTalonRight);

                    if (index1 < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, index1);
                    }
                    if (index2 < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, index2);
                    }
                }
                npc.netUpdate = true;
                AI_Local2 = 1;
                AI_State = State_Main;
            }

            if (npc.alpha > 0)
            {
                npc.alpha -= 5;
                if (npc.alpha < 4)
                {
                    npc.alpha = 0;
                    if(Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                    }
                }
                return;
            }

            //for (int playerIndex = 0; playerIndex < 255; playerIndex = someIndex + 1)
            //{
            //    if (Main.player[playerIndex].active && !Main.player[playerIndex].dead && (npc.Center - Main.player[playerIndex].Center).Length() < 1000f)
            //    {
            //        someIndex = num603;
            //        num603 = someIndex + 1;
            //    }
            //    someIndex = playerIndex;
            //}
            //if (Main.expertMode)
            //{
            //    int num605 = (int)(20f * (1f - (float)npc.life / (float)npc.lifeMax));
            //    npc.defense = npc.defDefense + num605;
            //}
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest();
            }

            if (Main.player[npc.target].dead)
            {
                npc.velocity.Y += 0.04f;
                if (npc.timeLeft > 10)
                {
                    npc.timeLeft = 10;
                }
            }
            else if (AI_State == State_Distribute)
            {
                //if (Main.netMode != 1)
                //{
                //    float num606 = AI_Timer; //parameter whenever switching to State_Distribute
                //    int nextState;
                //    do
                //    {
                //        nextState = Main.rand.Next(3);
                //        switch (nextState)
                //        {
                //            //case 1:
                //            //    nextState = 2;
                //            //    break;
                //            //case 2:
                //            //    nextState = 3;
                //            //    break;
                //            //only 0, 2 or 3
                //            /* State_Dash = 0f;
                //               State_Retarget = 2f;
                //               State_Main = 3f;
                //             */
                //            case 0:
                //            case 1:
                //                nextState = 2;
                //                break;
                //            case 2:
                //                nextState = 3;
                //                break;
                //        }
                //    }
                //    while ((float)nextState == num606); //only switch to state different than the previous
                //    AI_State = (float)nextState;
                //    AI_Timer = 0f;
                //    AI_Counter = 0f;
                //}
            }
            else if (AI_State == State_Dash)
            {
                //int num608 = 2;
                //if (Main.expertMode)
                //{
                //    if (npc.life < npc.lifeMax / 2)
                //    {
                //        someIndex = num608;
                //        num608 = someIndex + 1;
                //    }
                //    if (npc.life < npc.lifeMax / 3)
                //    {
                //        someIndex = num608;
                //        num608 = someIndex + 1;
                //    }
                //    if (npc.life < npc.lifeMax / 5)
                //    {
                //        someIndex = num608;
                //        num608 = someIndex + 1;
                //    }
                //}
                //if (AI_Timer > (float)(2 * num608) && AI_Timer % 2f == 0f)
                //{
                //    AI_State = State_Distribute;
                //    AI_Timer = State_Dash;
                //    AI_Counter = 0f;
                //    npc.netUpdate = true;
                //}
                //else if (AI_Timer % 2f == 0f)
                //{
                //    npc.TargetClosest();
                //    if (Math.Abs(npc.position.Y + (float)(npc.height / 2) - (Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2))) < 20f)
                //    {
                //        AI_Local1 = 1f;
                //        AI_Timer += 1f;
                //        AI_Counter = 0f;
                //        float num609 = 12f;
                //        if (Main.expertMode)
                //        {
                //            num609 = 16f;
                //            if ((double)npc.life < (double)npc.lifeMax * 0.75)
                //            {
                //                num609 += 2f;
                //            }
                //            if ((double)npc.life < (double)npc.lifeMax * 0.5)
                //            {
                //                num609 += 2f;
                //            }
                //            if ((double)npc.life < (double)npc.lifeMax * 0.25)
                //            {
                //                num609 += 2f;
                //            }
                //            if ((double)npc.life < (double)npc.lifeMax * 0.1)
                //            {
                //                num609 += 2f;
                //            }
                //        }
                //        Vector2 vector73 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                //        float num610 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector73.X;
                //        float num611 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector73.Y;
                //        float num612 = (float)Math.Sqrt((double)(num610 * num610 + num611 * num611));
                //        num612 = num609 / num612;
                //        npc.velocity.X = num610 * num612;
                //        npc.velocity.Y = num611 * num612;
                //        npc.spriteDirection = npc.direction;
                //        //Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                //    }
                //    else
                //    {
                //        AI_Local1 = 0f;
                //        float num613 = 12f;
                //        float num614 = 0.15f;
                //        if (Main.expertMode)
                //        {
                //            if ((double)npc.life < (double)npc.lifeMax * 0.75)
                //            {
                //                num613 += 1f;
                //                num614 += 0.05f;
                //            }
                //            if ((double)npc.life < (double)npc.lifeMax * 0.5)
                //            {
                //                num613 += 1f;
                //                num614 += 0.05f;
                //            }
                //            if ((double)npc.life < (double)npc.lifeMax * 0.25)
                //            {
                //                num613 += 2f;
                //                num614 += 0.05f;
                //            }
                //            if ((double)npc.life < (double)npc.lifeMax * 0.1)
                //            {
                //                num613 += 2f;
                //                num614 += 0.1f;
                //            }
                //        }
                //        if (npc.position.Y + (float)(npc.height / 2) < Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2))
                //        {
                //            npc.velocity.Y = npc.velocity.Y + num614;
                //        }
                //        else
                //        {
                //            npc.velocity.Y = npc.velocity.Y - num614;
                //        }
                //        if (npc.velocity.Y < -12f)
                //        {
                //            npc.velocity.Y = 0f - num613;
                //        }
                //        if (npc.velocity.Y > 12f)
                //        {
                //            npc.velocity.Y = num613;
                //        }
                //        if (Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) > 600f)
                //        {
                //            npc.velocity.X = npc.velocity.X + 0.15f * (float)npc.direction;
                //        }
                //        else if (Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) < 300f)
                //        {
                //            npc.velocity.X = npc.velocity.X - 0.15f * (float)npc.direction;
                //        }
                //        else
                //        {
                //            npc.velocity.X = npc.velocity.X * 0.8f;
                //        }
                //        if (npc.velocity.X < -16f)
                //        {
                //            npc.velocity.X = -16f;
                //        }
                //        if (npc.velocity.X > 16f)
                //        {
                //            npc.velocity.X = 16f;
                //        }
                //        npc.spriteDirection = npc.direction;
                //    }
                //}
                //else
                //{
                //    ////ADDED HERE
                //    //npc.TargetClosest();
                //    ////
                //    if (npc.velocity.X < 0f)
                //    {
                //        npc.direction = -1;
                //    }
                //    else
                //    {
                //        npc.direction = 1;
                //    }
                //    npc.spriteDirection = npc.direction;
                //    int num615 = 600;
                //    if (Main.expertMode)
                //    {
                //        if ((double)npc.life < (double)npc.lifeMax * 0.1)
                //        {
                //            num615 = 300;
                //        }
                //        else if ((double)npc.life < (double)npc.lifeMax * 0.25)
                //        {
                //            num615 = 450;
                //        }
                //        else if ((double)npc.life < (double)npc.lifeMax * 0.5)
                //        {
                //            num615 = 500;
                //        }
                //        else if ((double)npc.life < (double)npc.lifeMax * 0.75)
                //        {
                //            num615 = 550;
                //        }
                //    }
                //    int num616 = 1;
                //    if (npc.position.X + (float)(npc.width / 2) < Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))
                //    {
                //        num616 = -1;
                //    }
                //    if (npc.direction == num616 && Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) > (float)num615)
                //    {
                //        AI_Counter = 1f;
                //    }
                //    if (AI_Counter != 1f)
                //    {
                //        AI_Local1 = 1f;
                //    }
                //    else
                //    {
                //        npc.TargetClosest();
                //        npc.spriteDirection = npc.direction;
                //        AI_Local1 = 0f;
                //        npc.velocity *= 0.9f;
                //        float num617 = 0.1f;
                //        if (Main.expertMode)
                //        {
                //            if (npc.life < npc.lifeMax / 2)
                //            {
                //                npc.velocity *= 0.9f;
                //                num617 += 0.05f;
                //            }
                //            if (npc.life < npc.lifeMax / 3)
                //            {
                //                npc.velocity *= 0.9f;
                //                num617 += 0.05f;
                //            }
                //            if (npc.life < npc.lifeMax / 5)
                //            {
                //                npc.velocity *= 0.9f;
                //                num617 += 0.05f;
                //            }
                //        }
                //        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num617)
                //        {
                //            AI_Counter = 0f;
                //            AI_Timer += 1f;
                //        }
                //    }
                //}
            }
            else if (AI_State == State_Retarget)
            {
                //npc.TargetClosest();
                //npc.spriteDirection = npc.direction;
                //float num618 = 12f;
                //float num619 = 0.07f;
                //if (Main.expertMode)
                //{
                //    num619 = 0.1f;
                //}
                //Vector2 vector74 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                //float num620 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector74.X;
                //float num621 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 200f - vector74.Y;
                //float num622 = (float)Math.Sqrt((double)(num620 * num620 + num621 * num621));
                //if (num622 < 200f)
                //{
                //    AI_State = State_Main; //State_Hover;
                //    AI_Timer = 0f;
                //    npc.netUpdate = true;
                //}
                //else
                //{
                //    num622 = num618 / num622;
                //    if (npc.velocity.X < num620)
                //    {
                //        npc.velocity.X = npc.velocity.X + num619;
                //        if (npc.velocity.X < 0f && num620 > 0f)
                //        {
                //            npc.velocity.X = npc.velocity.X + num619;
                //        }
                //    }
                //    else if (npc.velocity.X > num620)
                //    {
                //        npc.velocity.X = npc.velocity.X - num619;
                //        if (npc.velocity.X > 0f && num620 < 0f)
                //        {
                //            npc.velocity.X = npc.velocity.X - num619;
                //        }
                //    }
                //    if (npc.velocity.Y < num621)
                //    {
                //        npc.velocity.Y = npc.velocity.Y + num619;
                //        if (npc.velocity.Y < 0f && num621 > 0f)
                //        {
                //            npc.velocity.Y = npc.velocity.Y + num619;
                //        }
                //    }
                //    else if (npc.velocity.Y > num621)
                //    {
                //        npc.velocity.Y = npc.velocity.Y - num619;
                //        if (npc.velocity.Y > 0f && num621 < 0f)
                //        {
                //            npc.velocity.Y = npc.velocity.Y - num619;
                //        }
                //    }
                //}
            }
            else if (AI_State == State_Hover) //where it shoots bees
            {
                //AI_Local1 = 0f;
                //npc.TargetClosest();
                //Vector2 vector75 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);
                //Vector2 vector76 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                //float num623 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector76.X;
                //float num624 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector76.Y;
                //float num625 = (float)Math.Sqrt((double)(num623 * num623 + num624 * num624));
                //AI_Timer += 1f;
                //if (Main.expertMode)
                //{
                //    AI_Timer += (float)(num603 / 2);
                //    if ((double)npc.life < (double)npc.lifeMax * 0.75)
                //    {
                //        AI_Timer += 0.25f;
                //    }
                //    if ((double)npc.life < (double)npc.lifeMax * 0.5)
                //    {
                //        AI_Timer += 0.25f;
                //    }
                //    if ((double)npc.life < (double)npc.lifeMax * 0.25)
                //    {
                //        AI_Timer += 0.25f;
                //    }
                //    if ((double)npc.life < (double)npc.lifeMax * 0.1)
                //    {
                //        AI_Timer += 0.25f;
                //    }
                //}
                //bool flag36 = false;
                //if (AI_Timer > 40f)
                //{
                //    AI_Timer = 0f;
                //    AI_Counter += 1f;
                //    flag36 = true;
                //}
                //if (Collision.CanHit(vector75, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && flag36)
                //{
                //    //Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y);
                //    //if (Main.netMode != 1)
                //    //{
                //    //    int num626 = Main.rand.Next(210, 212);
                //    //    int num627 = NPC.NewNPC((int)vector75.X, (int)vector75.Y, num626);
                //    //    Main.npc[num627].velocity.X = (float)Main.rand.Next(-200, 201) * 0.002f;
                //    //    Main.npc[num627].velocity.Y = (float)Main.rand.Next(-200, 201) * 0.002f;
                //    //    Main.npc[num627].localAI[0] = 60f;
                //    //    Main.npc[num627].netUpdate = true;
                //    //}
                //}
                //if (num625 > 400f || !Collision.CanHit(new Vector2(vector75.X, vector75.Y - 30f), 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                //{
                //    float num628 = 14f;
                //    float num629 = 0.1f;
                //    vector76 = vector75;
                //    num623 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector76.X;
                //    num624 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector76.Y;
                //    num625 = (float)Math.Sqrt((double)(num623 * num623 + num624 * num624));
                //    num625 = num628 / num625;
                //    if (npc.velocity.X < num623)
                //    {
                //        npc.velocity.X = npc.velocity.X + num629;
                //        if (npc.velocity.X < 0f && num623 > 0f)
                //        {
                //            npc.velocity.X = npc.velocity.X + num629;
                //        }
                //    }
                //    else if (npc.velocity.X > num623)
                //    {
                //        npc.velocity.X = npc.velocity.X - num629;
                //        if (npc.velocity.X > 0f && num623 < 0f)
                //        {
                //            npc.velocity.X = npc.velocity.X - num629;
                //        }
                //    }
                //    if (npc.velocity.Y < num624)
                //    {
                //        npc.velocity.Y = npc.velocity.Y + num629;
                //        if (npc.velocity.Y < 0f && num624 > 0f)
                //        {
                //            npc.velocity.Y = npc.velocity.Y + num629;
                //        }
                //    }
                //    else if (npc.velocity.Y > num624)
                //    {
                //        npc.velocity.Y = npc.velocity.Y - num629;
                //        if (npc.velocity.Y > 0f && num624 < 0f)
                //        {
                //            npc.velocity.Y = npc.velocity.Y - num629;
                //        }
                //    }
                //}
                //else
                //{
                //    npc.velocity *= 0.9f;
                //}
                //npc.spriteDirection = npc.direction;
                //if (AI_Counter > 5f)
                //{
                //    AI_State = State_Distribute;
                //    AI_Timer = State_Hover;
                //    npc.netUpdate = true;
                //}
            }
            else if (AI_State == State_Main) //where it shoots stingers
            {
                float num630 = 4f;
                float num631 = 0.05f;
                //if (Main.expertMode)
                //{
                //    num631 = 0.075f;
                //    num630 = 6f;
                //}
                Vector2 vector77 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);
                Vector2 vector78 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num632 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector78.X;
                float num633 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 200f - vector78.Y; //300f
                float num634 = (float)Math.Sqrt((double)(num632 * num632 + num633 * num633));
                AI_Timer += 1f;
                if (false)
                {
                    //bool canShootStinger = false;
                    //if (Main.expertMode)
                    //{
                    //    if ((double)npc.life < (double)npc.lifeMax * 0.1)
                    //    {
                    //        if (AI_Timer % 15f == 14f)
                    //        {
                    //            canShootStinger = true;
                    //        }
                    //    }
                    //    else if (npc.life < npc.lifeMax / 3)
                    //    {
                    //        if (AI_Timer % 25f == 24f)
                    //        {
                    //            canShootStinger = true;
                    //        }
                    //    }
                    //    else if (npc.life < npc.lifeMax / 2)
                    //    {
                    //        if (AI_Timer % 30f == 29f)
                    //        {
                    //            canShootStinger = true;
                    //        }
                    //    }
                    //    else if (AI_Timer % 35f == 34f)
                    //    {
                    //        canShootStinger = true;
                    //    }
                    //}
                    //else if (AI_Timer % 40f == 39f)
                    //{
                    //    canShootStinger = true;
                    //}
                    //if (flag37 && npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y && Collision.CanHit(vector77, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                    //{
                    //    //Main.PlaySound(SoundID.Item17, npc.position);
                    //    if (Main.netMode != 1)
                    //    {
                    //        float num635 = 8f;
                    //        if (Main.expertMode)
                    //        {
                    //            num635 += 2f;
                    //        }
                    //        if (Main.expertMode && (double)npc.life < (double)npc.lifeMax * 0.1)
                    //        {
                    //            num635 += 3f;
                    //        }
                    //        float num636 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector77.X + (float)Main.rand.Next(-80, 81);
                    //        float num637 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector77.Y + (float)Main.rand.Next(-40, 41);
                    //        float num638 = (float)Math.Sqrt((double)(num636 * num636 + num637 * num637));
                    //        num638 = num635 / num638;
                    //        num636 *= num638;
                    //        num637 *= num638;
                    //        int num639 = 11;
                    //        int num640 = 55;
                    //        int num641 = Projectile.NewProjectile(vector77.X, vector77.Y, num636, num637, num640, num639, 0f, Main.myPlayer);
                    //        Main.projectile[num641].timeLeft = 300;
                    //    }
                    //}
                }
                if (!Collision.CanHit(new Vector2(vector77.X, vector77.Y - 30f), 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    num630 = 14f;
                    num631 = 0.1f;
                    vector78 = vector77;
                    num632 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector78.X;
                    num633 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector78.Y;
                    num634 = (float)Math.Sqrt((double)(num632 * num632 + num633 * num633));
                    num634 = num630 / num634;


                    ////WHEN NO DIRECT CAN HIT LINE
                    if (npc.velocity.X < num632)
                    {
                        npc.velocity.X = npc.velocity.X + num631;
                        if (npc.velocity.X < 0f && num632 > 0f)
                        {
                            npc.velocity.X = npc.velocity.X + num631 * 2.5f; //1f all
                        }
                    }
                    else if (npc.velocity.X > num632)
                    {
                        npc.velocity.X = npc.velocity.X - num631;
                        if (npc.velocity.X > 0f && num632 < 0f)
                        {
                            npc.velocity.X = npc.velocity.X - num631 * 2.5f;
                        }
                    }
                    if (npc.velocity.Y < num633)
                    {
                        npc.velocity.Y = npc.velocity.Y + num631;
                        if (npc.velocity.Y < 0f && num633 > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y + num631 * 2.5f;
                        }
                    }
                    else if (npc.velocity.Y > num633)
                    {
                        npc.velocity.Y = npc.velocity.Y - num631;
                        if (npc.velocity.Y > 0f && num633 < 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y - num631 * 2.5f;
                        }
                    }
                }
                else if (num634 > 100f)
                {
                    npc.TargetClosest();
                    npc.spriteDirection = npc.direction;
                    num634 = num630 / num634;
                    if (npc.velocity.X < num632)
                    {
                        npc.velocity.X = npc.velocity.X + num631;
                        if (npc.velocity.X < 0f && num632 > 0f)
                        {
                            npc.velocity.X = npc.velocity.X + num631 * 2f; //2f all
                        }
                    }
                    else if (npc.velocity.X > num632)
                    {
                        npc.velocity.X = npc.velocity.X - num631;
                        if (npc.velocity.X > 0f && num632 < 0f)
                        {
                            npc.velocity.X = npc.velocity.X - num631 * 2f;
                        }
                    }
                    if (npc.velocity.Y < num633)
                    {
                        npc.velocity.Y = npc.velocity.Y + num631;
                        if (npc.velocity.Y < 0f && num633 > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y + num631 * 2f;
                        }
                    }
                    else if (npc.velocity.Y > num633)
                    {
                        npc.velocity.Y = npc.velocity.Y - num631;
                        if (npc.velocity.Y > 0f && num633 < 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y - num631 * 2f;
                        }
                    }

                }
                //if (AI_Timer > 800f)
                //{
                //    AI_State = State_Distribute;
                //    AI_Timer = State_Main;
                //    npc.netUpdate = true;
                //}
                if (AI_Timer > 120f)
                {
                    AI_Timer = 0;
                    npc.netUpdate = true;
                }
            }
        }

        public override void PostAI()
        {
            if (npc.direction == 1)
            {
                TalonOffsetLeftX = -Wid / 4 + TalonDirectionalOffset;
                TalonOffsetRightX = Wid / 4 + TalonDirectionalOffset;
            }
            else
            {
                TalonOffsetLeftX = -Wid / 4 - TalonDirectionalOffset;
                TalonOffsetRightX = Wid / 4 - TalonDirectionalOffset;
            }
        }


        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
    }
}
