using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Accessories.Useful;
using AssortedCrazyThings.Items.VanityArmor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Chat;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    [AutoloadBossHead]
    public class Harvester : ModNPC
    {
        public static readonly string name = "Soul Harvester";
        public static readonly string deathMessage = "The Dungeon Souls have been freed!"; //on death
        public static Color deathColor = new Color(35, 200, 254);

        public static float sinY = 0;
        public static int talonDamage = 30;
        public static int Wid = 110;
        public static int Hei = 110;

        public static int TalonOffsetLeftX = -Wid / 4/* - 14*/; //-84
        public static int TalonOffsetRightX = Wid / 4/* + 8*/; // 78
        public static int TalonOffsetY = Hei / 2 - 7;              //-9 //normally its negative

        public static int TalonDirectionalOffset = 10;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name); //defined above since its used in CaughtDungeonSoul
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            //npc.SetDefaults(NPCID.QueenBee);
            NPC.boss = true;
            NPC.npcSlots = 10f; //takes 10 npc slots , so no other npcs can spawn during the fight
            //actual body hitbox
            NPC.width = Wid; //302 texture //104
            NPC.height = Hei; //176 texture //110
            NPC.damage = 5; //contact damage
            NPC.defense = 8;
            NPC.lifeMax = 1500;
            NPC.scale = 1f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = Item.buyPrice(0, 10);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1; //91;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.alpha = 255;
            //music = MusicID.Boss5; //TODO music

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
            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter++;

            if (NPC.alpha > 0)
            {
                NPC.frame.Y = frameHeight * 4;
                NPC.frameCounter = 40.0;
                return;
            }

            //0 1 2 3 4 | 3 2 1 0
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
            else if (NPC.frameCounter <= 48.0)
            {
                NPC.frame.Y = frameHeight * 3;
            }
            else if (NPC.frameCounter <= 56.0)
            {
                NPC.frame.Y = frameHeight * 2;
            }
            else if (NPC.frameCounter <= 64.0)
            {
                NPC.frame.Y = frameHeight * 1;
            }
            else
            {
                NPC.frameCounter = 0;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/DungeonBird/HarvesterWings").Value;
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width >> 1, NPC.height >> 1);

            Vector2 stupidOffset = new Vector2(0, -29f + NPC.gfxOffY);
            Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + stupidOffset;

            spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(Color.White), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - NPC.alpha) / 255f);
        }

        /// <summary>
        /// To drop the accessories and the souls multiplied by the number of people present during the fight
        /// </summary>
        private void DropLoot(int npcTypeNew)
        {
            int count = Array.FindAll(NPC.playerInteraction, interacted => interacted).Length;

            for (int i = 0; i < count; i++)
            {
                if (Main.rand.NextBool(3)) //33% chance
                {
                    int[] types = new int[] { ModContent.ItemType<SigilOfRetreat>(), ModContent.ItemType<SigilOfEmergency>(), ModContent.ItemType<SigilOfPainSuppression>() };
                    int itemType = Main.rand.Next(types);
                    Item.NewItem(NPC.getRect(), itemType, prefixGiven: -1);
                }

                Vector2 randVector = Vector2.One;
                float randFactor;
                int index;

                for (int j = 0; j < 15; j++) //spawn souls when dies, 15 total
                {
                    randVector = randVector.RotatedByRandom(MathHelper.ToRadians(359f));
                    randFactor = Main.rand.NextFloat(2f, 8f);
                    index = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, npcTypeNew);
                    Main.npc[index].SetDefaults(npcTypeNew);
                    //Main.npc[index].timeLeft = 3600;
                    Main.npc[index].velocity = randVector * randFactor;
                    Main.npc[index].ai[2] = Main.rand.Next(1, DungeonSoulBase.offsetYPeriod); //doesnt get synced properly to clients idk
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, number: index);
                    }
                }
            }
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.Bone, Main.rand.Next(40, 61));
            if (Main.rand.NextBool(10)) Item.NewItem(NPC.getRect(), ModContent.ItemType<SoulHarvesterMask>());
            Item.NewItem(NPC.getRect(), ModContent.ItemType<DesiccatedLeather>());

            if (Main.rand.NextBool(4)) Item.NewItem(NPC.getRect(), ModContent.ItemType<IdolOfDecay>());

            //RecipeBrowser fix
            if (NPC.Center == new Vector2(1000, 1000))
            {
                Item.NewItem(NPC.getRect(), ModContent.ItemType<CaughtDungeonSoulFreed>());
            }

            int npcTypeOld = ModContent.NPCType<DungeonSoul>();
            int npcTypeNew = ModContent.NPCType<DungeonSoulFreed>();  //version that doesnt get eaten by harvesters

            int itemTypeOld = ModContent.ItemType<CaughtDungeonSoul>();
            int itemTypeNew = ModContent.ItemType<CaughtDungeonSoulFreed>(); //version that is used in crafting

            DropLoot(npcTypeNew);

            //"convert" NPC souls
            for (short j = 0; j < Main.maxNPCs; j++)
            {
                NPC other = Main.npc[j];
                if (other.active && other.type == npcTypeOld)
                {
                    other.active = false;
                    int index = NPC.NewNPC((int)other.position.X, (int)other.position.Y, npcTypeNew);
                    NPC npcnew = Main.npc[index];
                    npcnew.ai[2] = Main.rand.Next(1, DungeonSoulBase.offsetYPeriod); //doesnt get synced properly to clients idk
                    npcnew.timeLeft = 3600;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, number: index);
                    }

                    //poof visual works only in singleplayer
                    for (int i = 0; i < 15; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(npcnew.Center, 59, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
                        dust.noLight = true;
                        dust.noGravity = true;
                        dust.fadeIn = Main.rand.NextFloat(0.1f, 0.6f);
                    }
                }
            }

            //"convert" Item souls that got dropped for some reason
            int tempStackCount;
            for (int j = 0; j < Main.maxItems; j++)
            {
                Item item = Main.item[j];
                if (item.active && item.type == itemTypeOld)
                {
                    tempStackCount = item.stack;
                    item.SetDefaults(itemTypeNew);
                    item.stack = tempStackCount;

                    //poof visual
                    for (int i = 0; i < 15; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(item.Center, 59, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
                        dust.noLight = true;
                        dust.noGravity = true;
                        dust.fadeIn = Main.rand.NextFloat(0.1f, 0.6f);
                    }
                }
            }

            //"convert" Item souls in inventory
            for (int j = 0; j < Main.maxPlayers; j++)
            {
                Player player = Main.player[j];
                if (player.active/* && !Main.player[j].dead*/)
                {
                    AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

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
                Main.NewText(deathMessage, deathColor);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(deathMessage), deathColor);
            }
        }

        private void SendConvertInertSoulsInventory()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)AssMessageType.ConvertInertSoulsInventory);
                packet.Send();
            }
        }

        private const int AI_State_Slot = 0;
        private const int AI_Timer_Slot = 1;
        private const int AI_Counter_Slot = 2;
        private const int AI_Unused_Slot = 3;

        private const float State_Main = 3f;
        //No additional states here

        public float AI_State
        {
            get
            {
                return NPC.ai[AI_State_Slot];
            }
            set
            {
                NPC.ai[AI_State_Slot] = value;
            }
        }

        public float AI_Timer
        {
            get
            {
                return NPC.ai[AI_Timer_Slot];
            }
            set
            {
                NPC.ai[AI_Timer_Slot] = value;
            }
        }

        public float AI_Counter
        {
            get
            {
                return NPC.ai[AI_Counter_Slot];
            }
            set
            {
                NPC.ai[AI_Counter_Slot] = value;
            }
        }

        public float AI_Unused
        {
            get
            {
                return NPC.ai[AI_Unused_Slot];
            }
            set
            {
                NPC.ai[AI_Unused_Slot] = value;
            }
        }

        public float AI_Local1
        {
            get
            {
                return NPC.localAI[0];
            }
            set
            {
                NPC.localAI[0] = value;
            }
        }

        public bool Initialized
        {
            get
            {
                return NPC.localAI[1] == 1f;
            }
            set
            {
                NPC.localAI[1] = value ? 1f : 0f;
            }
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, new Vector3(0.3f, 0.3f, 0.7f));

            NPC.gfxOffY = NPC.height / 2;
            if (Main.netMode != NetmodeID.Server && !Main.gamePaused && Main.hasFocus)
            {
                double freq = 120.0;
                sinY = (float)((Math.Sin(((Main.GameUpdateCount % freq) / freq) * MathHelper.TwoPi) - 1) * 6);
            }
            NPC.gfxOffY += sinY;
            return true;
        }

        public override void AI()
        {
            if (!Initialized)
            {
                AssWorld.harvesterIndex = NPC.whoAmI;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SoundEngine.PlaySound(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 0);
                    int index1 = NPC.NewNPC((int)NPC.Center.X + TalonOffsetLeftX, (int)NPC.Center.Y + TalonOffsetY, AssortedCrazyThings.harvesterTalonLeft);
                    int index2 = NPC.NewNPC((int)NPC.Center.X + TalonOffsetRightX, (int)NPC.Center.Y + TalonOffsetY, AssortedCrazyThings.harvesterTalonRight);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        if (index1 < Main.maxNPCs)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, number: index1);
                        }
                        if (index2 < Main.maxNPCs)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, number: index2);
                        }
                    }
                }
                NPC.scale = 1f;
                AI_State = State_Main;
                NPC.netUpdate = true;
                Initialized = true;
            }

            if (NPC.alpha > 0)
            {
                NPC.alpha -= 5;
                if (NPC.alpha < 4)
                {
                    NPC.alpha = 0;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.netUpdate = true;
                    }
                }
                return;
            }

            if (NPC.target < 0 || NPC.target >= 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player target = Main.player[NPC.target];

            if (target.dead)
            {
                NPC.velocity.Y += 0.04f;
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
            }
            else if (AI_State == State_Main)
            {
                float acceleration = 0.05f;

                Vector2 origin = new Vector2(NPC.Center.X + (float)(Main.rand.Next(20) * NPC.direction), NPC.position.Y + NPC.height * 0.8f);
                float diffX = target.Center.X - NPC.Center.X;
                float diffY = target.Center.Y - 200f - NPC.Center.Y; //300f
                float length = (float)Math.Sqrt(diffX * diffX + diffY * diffY);
                AI_Timer += 1f;

                if (!Collision.CanHit(new Vector2(origin.X, origin.Y - 30f), 1, 1, target.position, target.width, target.height))
                {
                    acceleration = 0.1f;
                    diffX = target.Center.X - NPC.Center.X;
                    diffY = target.Center.Y - NPC.Center.Y;

                    //WHEN NO DIRECT CAN HIT LINE
                    
                    if (Math.Abs(NPC.velocity.X) < 32)
                    {
                        if (NPC.velocity.X < diffX)
                        {
                            NPC.velocity.X = NPC.velocity.X + acceleration;
                            if (NPC.velocity.X < 0f && diffX > 0f)
                            {
                                NPC.velocity.X = NPC.velocity.X + acceleration * 2.5f; //1f all
                            }
                        }
                        else if (NPC.velocity.X > diffX)
                        {
                            NPC.velocity.X = NPC.velocity.X - acceleration;
                            if (NPC.velocity.X > 0f && diffX < 0f)
                            {
                                NPC.velocity.X = NPC.velocity.X - acceleration * 2.5f;
                            }
                        }
                    }
                    if (Math.Abs(NPC.velocity.Y) < 32)
                    {
                        if (NPC.velocity.Y < diffY)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + acceleration;
                            if (NPC.velocity.Y < 0f && diffY > 0f)
                            {
                                NPC.velocity.Y = NPC.velocity.Y + acceleration * 2.5f;
                            }
                        }
                        else if (NPC.velocity.Y > diffY)
                        {
                            NPC.velocity.Y = NPC.velocity.Y - acceleration;
                            if (NPC.velocity.Y > 0f && diffY < 0f)
                            {
                                NPC.velocity.Y = NPC.velocity.Y - acceleration * 2.5f;
                            }
                        }
                    }
                }
                else if (length > 100f)
                {
                    NPC.TargetClosest();
                    NPC.spriteDirection = NPC.direction;
                    if (Math.Abs(NPC.velocity.X) < 32)
                    {
                        if (NPC.velocity.X < diffX)
                        {
                            NPC.velocity.X = NPC.velocity.X + acceleration;
                            if (NPC.velocity.X < 0f && diffX > 0f)
                            {
                                NPC.velocity.X = NPC.velocity.X + acceleration * 2f; //2f all
                            }
                        }
                        else if (NPC.velocity.X > diffX)
                        {
                            NPC.velocity.X = NPC.velocity.X - acceleration;
                            if (NPC.velocity.X > 0f && diffX < 0f)
                            {
                                NPC.velocity.X = NPC.velocity.X - acceleration * 2f;
                            }
                        }
                    }
                    if (Math.Abs(NPC.velocity.Y) < 32)
                    {
                        if (NPC.velocity.Y < diffY)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + acceleration;
                            if (NPC.velocity.Y < 0f && diffY > 0f)
                            {
                                NPC.velocity.Y = NPC.velocity.Y + acceleration * 2f;
                            }
                        }
                        else if (NPC.velocity.Y > diffY)
                        {
                            NPC.velocity.Y = NPC.velocity.Y - acceleration;
                            if (NPC.velocity.Y > 0f && diffY < 0f)
                            {
                                NPC.velocity.Y = NPC.velocity.Y - acceleration * 2f;
                            }
                        }
                    }
                }

                if (AI_Timer > 120f)
                {
                    AI_Timer = 0;
                    NPC.netUpdate = true;
                }
            }
            //additional stages here
        }

        public override void PostAI()
        {
            if (NPC.direction == 1)
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
            if (NPC.life <= 0)
            {
                int first = 1;
                int second = 13 + first;
                int third = 2 + second;
                int fourth = 2 + third;
                int total = fourth;
                for (int i = 0; i < total; i++)
                {
                    string name = "4";
                    if (i < first) name = "1";
                    else if (i < second) name = "2";
                    else if (i < third) name = "3";
                    Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("SoulHarvesterGore_0" + name).Type, 1f);
                }
            }
        }
    }
}
