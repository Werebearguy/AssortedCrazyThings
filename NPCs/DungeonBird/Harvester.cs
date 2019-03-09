using System;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Accessories.Useful;
using AssortedCrazyThings.Items.VanityArmor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    [AutoloadBossHead]
    public class Harvester : ModNPC
    {
        public static readonly string name = "Soul Harvester";
        public static readonly string deathMessage = "The Dungeon Souls have been freed"; //on death

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
            npc.damage = 5; //contact damage
            npc.defense = 8;
            npc.lifeMax = 1500;
            npc.scale = 1f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = Item.buyPrice(0, 10);
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
            music = MusicID.Boss5;

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
            Texture2D texture = mod.GetTexture("NPCs/DungeonBird/HarvesterWings");
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
            if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<SoulHarvesterMask>());
            Item.NewItem(npc.getRect(), mod.ItemType<DesiccatedLeather>());

            if (Main.rand.NextBool(4)) Item.NewItem(npc.getRect(), mod.ItemType<IdolOfDecay>());

            if (Main.rand.NextBool(3)) //33% chance
            {
                int rand = Main.rand.Next(3);
                switch (rand)
                {
                    case 0:
                        Item.NewItem(npc.getRect(), mod.ItemType<SigilOfRetreat>());
                        break;
                    case 1:
                        Item.NewItem(npc.getRect(), mod.ItemType<SigilOfEmergency>());
                        break;
                    case 2:
                        Item.NewItem(npc.getRect(), mod.ItemType<SigilOfPainSuppression>());
                        break;
                }
            }

            //RecipeBrowser fix
            if (npc.Center == new Vector2(1000, 1000))
            {
                Item.NewItem(npc.getRect(), mod.ItemType<CaughtDungeonSoulFreed>());
            }

            Vector2 randVector = new Vector2(1, 1);
            float randFactor = 0f;

            int npcTypeOld = mod.NPCType<DungeonSoul>();
            int npcTypeNew = mod.NPCType<DungeonSoulFreed>();  //version that doesnt get eaten by harvesters

            int itemTypeOld = mod.ItemType<CaughtDungeonSoul>();
            int itemTypeNew = mod.ItemType<CaughtDungeonSoulFreed>(); //version that is used in crafting

            for (int i = 0; i < 15; i++) //spawn souls when dies, 15 total
            {
                randVector = randVector.RotatedByRandom(MathHelper.ToRadians(359f));
                randFactor = Main.rand.NextFloat(2f, 8f);
                int index = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, npcTypeNew);
                Main.npc[index].SetDefaults(npcTypeNew);
                //Main.npc[index].timeLeft = 3600;
                Main.npc[index].velocity = randVector * randFactor;
                Main.npc[index].ai[2] = Main.rand.Next(1, DungeonSoulBase.offsetYPeriod); //doesnt get synced properly to clients idk
            }

            //"convert" NPC souls
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && Main.npc[j].type == npcTypeOld)
                {
                    Main.npc[j].active = false;
                    int index = NPC.NewNPC((int)Main.npc[j].position.X, (int)Main.npc[j].position.Y, npcTypeNew);
                    Main.npc[index].SetDefaults(npcTypeNew);
                    //Main.npc[index].timeLeft = 3600;
                    Main.npc[index].ai[2] = Main.rand.Next(1, DungeonSoulBase.offsetYPeriod); //doesnt get synced properly to clients idk

                    //poof visual
                    for (int i = 0; i < 15; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(Main.npc[index].Center, 59, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
                        dust.noLight = true;
                        dust.noGravity = true;
                        dust.fadeIn = Main.rand.NextFloat(0.1f, 0.6f);
                    }
                }
            }

            //"convert" Item souls that got dropped for some reason
            int tempStackCount = 0;
            for (int j = 0; j < Main.item.Length; j++)
            {
                if (Main.item[j].active && Main.item[j].type == itemTypeOld)
                {
                    tempStackCount = Main.item[j].stack;
                    Main.item[j].SetDefaults(itemTypeNew);
                    Main.item[j].stack = tempStackCount;
                }

                //poof visual
                for (int i = 0; i < 15; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Main.item[j].Center, 59, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
                    dust.noLight = true;
                    dust.noGravity = true;
                    dust.fadeIn = Main.rand.NextFloat(0.1f, 0.6f);
                }
            }

            //"convert" Item souls in inventory
            for (int j = 0; j < Main.player.Length; j++)
            {
                if(Main.player[j].active/* && !Main.player[j].dead*/)
                {
                    AssPlayer mPlayer = Main.player[j].GetModPlayer<AssPlayer>(mod);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        SendConvertInertSoulsInventory();
                    }
                    else //singleplayer
                    {
                        mPlayer.ConvertInertSoulsInventory();
                    }
                }
            }

            if (!AssWorld.downedHarvester)
            {
                AssWorld.downedHarvester = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(deathMessage, 35, 200, 254);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(deathMessage), new Color(35, 200, 254));
            }
        }

        private void SendConvertInertSoulsInventory()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)AssMessageType.ConvertInertSoulsInventory);
                packet.Send();
            }
        }

        private const int AI_State_Slot = 0;
        private const int AI_Timer_Slot = 1;
        private const int AI_Counter_Slot = 2;
        private const int AI_Unused_Slot = 3;

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
            if (AI_Local2 == 0)
            {
                AssWorld.harvesterIndex = npc.whoAmI;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                    int index1 = NPC.NewNPC((int)npc.Center.X + TalonOffsetLeftX, (int)npc.Center.Y + TalonOffsetY, AssWorld.harvesterTalonLeft);
                    int index2 = NPC.NewNPC((int)npc.Center.X + TalonOffsetRightX, (int)npc.Center.Y + TalonOffsetY, AssWorld.harvesterTalonRight);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        if (index1 < 200)
                        {
                            NetMessage.SendData(23, -1, -1, null, index1);
                        }
                        if (index2 < 200)
                        {
                            NetMessage.SendData(23, -1, -1, null, index2);
                        }
                    }
                }
                npc.scale = 1f;
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
            else if (AI_State == State_Main)
            {
                float num630 = 4f;
                float num631 = 0.05f;

                Vector2 vector77 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);
                Vector2 vector78 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num632 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector78.X;
                float num633 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 200f - vector78.Y; //300f
                float num634 = (float)Math.Sqrt((double)(num632 * num632 + num633 * num633));
                AI_Timer += 1f;
                
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
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_01"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_03"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_03"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_04"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SoulHarvesterGore_04"), 1f);
			}
		}
    }
}
