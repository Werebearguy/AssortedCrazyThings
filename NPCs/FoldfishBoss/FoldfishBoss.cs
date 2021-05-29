using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.FoldfishBoss
{
    [AutoloadBossHead]
    public class FoldfishBoss : ModNPC
    {
        public static float scaleFactor = 1f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Foldfish");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.KingSlime];
        }

        public override void SetDefaults()
        {
            //npc.CloneDefaults(NPCID.KingSlime);
            NPC.aiStyle = -1;
            NPC.lifeMax = 4000;
            NPC.damage = 20;
            NPC.defense = 5;
            NPC.knockBackResist = 0f;
            NPC.width = 76;
            NPC.height = 38;
            NPC.scale = scaleFactor;
            NPC.value = Item.buyPrice(0, 1, 0, 0);
            NPC.npcSlots = 15f;
            NPC.alpha = 0;
            NPC.boss = true;
            NPC.lavaImmune = true;
            //npc.noGravity = true;
            //npc.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.buffImmune[24] = true;
            NPC.buffImmune[BuffID.Confused] = false;
            //music = MusicID.Boss1; //TODO music
            AnimationType = NPCID.KingSlime;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public override void OnKill()
        {
            if (Main.rand.NextBool(4))
            {
                Item.NewItem(NPC.getRect(), Mod.Find<ModItem>("OrigamiCrane").Type);
            }
            Item.NewItem(NPC.getRect(), Mod.Find<ModItem>("OrigamiHat").Type, prefixGiven: -1);
        }

        public override void AI()
        {
            //type == 50
            //aiStyle == 15
            float num238 = scaleFactor; //1f
            bool flag8 = false;
            bool flag9 = false;
            NPC.aiAction = 0;
            if (NPC.ai[3] == 0f && NPC.life > 0)
            {
                NPC.ai[3] = (float)NPC.lifeMax;
            }
            if (NPC.localAI[3] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0] = -60; //-100f
                NPC.localAI[3] = 1f;
                NPC.TargetClosest();
                NPC.netUpdate = true;
            }
            if (Main.player[NPC.target].dead)
            {
                NPC.TargetClosest();
                if (Main.player[NPC.target].dead)
                {
                    NPC.timeLeft = 0;
                    if (Main.player[NPC.target].Center.X < NPC.Center.X)
                    {
                        NPC.direction = 1;
                    }
                    else
                    {
                        NPC.direction = -1;
                    }
                }
            }
            if (!Main.player[NPC.target].dead && NPC.ai[2] >= 300f && NPC.ai[1] < 5f && NPC.velocity.Y == 0f)
            {
                NPC.ai[2] = 0f;
                NPC.ai[0] = 0f;
                NPC.ai[1] = 5f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.TargetClosest(faceTarget: false);
                    Point point3 = NPC.Center.ToTileCoordinates();
                    Point point4 = Main.player[NPC.target].Center.ToTileCoordinates();
                    Vector2 vector29 = Main.player[NPC.target].Center - NPC.Center;
                    int num239 = 10;
                    int num240 = 0;
                    int num241 = 7;
                    int num242 = 0;
                    bool flag10 = false;
                    if (vector29.Length() > 2000f)
                    {
                        flag10 = true;
                        num242 = 100;
                    }
                    while (!flag10 && num242 < 100)
                    {
                        int num2 = num242;
                        num242 = num2 + 1;
                        int num243 = Main.rand.Next(point4.X - num239, point4.X + num239 + 1);
                        int num244 = Main.rand.Next(point4.Y - num239, point4.Y + 1);
                        if ((num244 < point4.Y - num241 || num244 > point4.Y + num241 || num243 < point4.X - num241 || num243 > point4.X + num241) && (num244 < point3.Y - num240 || num244 > point3.Y + num240 || num243 < point3.X - num240 || num243 > point3.X + num240) && !Main.tile[num243, num244].IsActiveUnactuated)
                        {
                            int num245 = num244;
                            int num246 = 0;
                            if (!Main.tile[num243, num245].IsActiveUnactuated || !Main.tileSolid[Main.tile[num243, num245].type] || Main.tileSolidTop[Main.tile[num243, num245].type])
                            {
                                while (num246 < 150 && num245 + num246 < Main.maxTilesY)
                                {
                                    int num247 = num245 + num246;
                                    if (Main.tile[num243, num247].IsActiveUnactuated && Main.tileSolid[Main.tile[num243, num247].type] && !Main.tileSolidTop[Main.tile[num243, num247].type])
                                    {
                                        num2 = num246;
                                        num246 = num2 - 1;
                                        break;
                                    }
                                    num2 = num246;
                                    num246 = num2 + 1;
                                }
                            }
                            else
                            {
                                num246 = 1;
                            }
                            num244 += num246;
                            bool flag11 = true;
                            if (flag11 && Main.tile[num243, num244].LiquidType == LiquidID.Lava)
                            {
                                flag11 = false;
                            }
                            if (flag11 && !Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
                            {
                                flag11 = false;
                            }
                            if (flag11)
                            {
                                NPC.localAI[1] = num243 * 16 + 8;
                                NPC.localAI[2] = num244 * 16 + 16;
                                break;
                            }
                        }
                    }
                    if (num242 >= 100)
                    {
                        Vector2 bottom = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].Bottom;
                        NPC.localAI[1] = bottom.X;
                        NPC.localAI[2] = bottom.Y;
                    }
                }
            }
            if (!Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
            {
                NPC.ai[2] += 1f;
            }
            if (Math.Abs(NPC.Top.Y - Main.player[NPC.target].Bottom.Y) > 320f)
            {
                NPC.ai[2] += 1f;
            }
            Dust dust3;
            if (NPC.ai[1] == 5f)
            {
                flag8 = true;
                NPC.aiAction = 1;
                NPC.ai[0] += 1f;
                num238 = MathHelper.Clamp((40f - NPC.ai[0]) / 40f, 0f, 1f); //60f to 40f
                num238 = 0.5f + num238 * 0.5f; //0.5f
                if (NPC.ai[0] >= 60f)
                {
                    flag9 = true;
                }
                if (NPC.ai[0] == 60f)
                {
                    //Gore.NewGore(npc.Center + new Vector2(-40f, (0f - (float)npc.height) / 2f), npc.velocity, 734);
                }
                if (NPC.ai[0] >= 40f && Main.netMode != NetmodeID.MultiplayerClient) //60f to 40f
                {
                    NPC.Bottom = new Vector2(NPC.localAI[1], NPC.localAI[2]); //teleport to position of player
                    NPC.ai[1] = 6f;
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && NPC.ai[0] >= 80f) //120f to 80f, jump frequency, but 80 is the minimum, below and shit fucks up
                {
                    NPC.ai[1] = 6f;
                    NPC.ai[0] = 0f;
                }
                if (!flag9)
                {
                    int num2;
                    for (int num248 = 0; num248 < 10; num248 = num2 + 1)
                    {
                        //dust for when before it teleports, change 79 to whatever type
                        //int num249 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                        Dust dust = Dust.NewDustDirect(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, 79, NPC.velocity.X, NPC.velocity.Y, 150, new Color(255, 255, 255), 1.2f);
                        dust.noGravity = true;
                        dust.velocity *= 0.5f;
                        num2 = num248;
                    }
                }
            }
            else if (NPC.ai[1] == 6f)
            {
                flag8 = true;
                NPC.aiAction = 0;
                NPC.ai[0] += 1f;
                num238 = MathHelper.Clamp(NPC.ai[0] / 30f, 0f, 1f);
                num238 = 0.5f + num238 * 0.5f;
                if (NPC.ai[0] >= 30f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                    NPC.TargetClosest();
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && NPC.ai[0] >= 40f) //60f to 40f, cant remember what that does, maybe the jumping
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 0f;
                    NPC.TargetClosest();
                }
                int num2;
                for (int num250 = 0; num250 < 10; num250 = num2 + 1)
                {
                    //dust for when after it teleported
                    Dust dust = Dust.NewDustDirect(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, 79, NPC.velocity.X, NPC.velocity.Y, 150, new Color(255, 255, 255), 1.2f);
                    dust.noGravity = true;
                    dust.velocity *= 2f;
                    num2 = num250;
                }
            }
            NPC.dontTakeDamage = (NPC.hide = flag9);
            if (NPC.velocity.Y == 0f)
            {
                NPC.velocity.X = NPC.velocity.X * 0.8f;
                if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                {
                    NPC.velocity.X = 0f;
                }
                if (!flag8)
                {
                    NPC.ai[0] += 2f;
                    if (NPC.life < NPC.lifeMax * 0.8)
                    {
                        NPC.ai[0] += 1f;
                    }
                    if (NPC.life < NPC.lifeMax * 0.6)
                    {
                        NPC.ai[0] += 1f;
                    }
                    if (NPC.life < NPC.lifeMax * 0.4)
                    {
                        NPC.ai[0] += 2f;
                    }
                    if (NPC.life < NPC.lifeMax * 0.2)
                    {
                        NPC.ai[0] += 3f;
                    }
                    if (NPC.life < NPC.lifeMax * 0.1)
                    {
                        NPC.ai[0] += 4f;
                    }
                    if (NPC.ai[0] >= 0f)
                    {
                        NPC.netUpdate = true;
                        NPC.TargetClosest();
                        if (NPC.ai[1] == 3f) //jump heights here in velo.Y
                        {
                            NPC.velocity.Y = -10f; //-13f
                            NPC.velocity.X = NPC.velocity.X + 3.5f * NPC.direction;
                            NPC.ai[0] = -110f; //-200f
                            NPC.ai[1] = 0f;
                        }
                        else if (NPC.ai[1] == 2f)
                        {
                            NPC.velocity.Y = -6f; //-8f
                            NPC.velocity.X = NPC.velocity.X + 4.5f * NPC.direction;
                            NPC.ai[0] = -80f; //-120f
                            NPC.ai[1] += 1f;
                        }
                        else
                        {
                            NPC.velocity.Y = -6f; //-8f
                            NPC.velocity.X = NPC.velocity.X + 4f * NPC.direction;
                            NPC.ai[0] = -80f; //-120f
                            NPC.ai[1] += 1f;
                        }
                    }
                    else if (NPC.ai[0] >= -30f)
                    {
                        NPC.aiAction = 1;
                    }
                }
            }
            else if (NPC.target < Main.maxPlayers && ((NPC.direction == 1 && NPC.velocity.X < 3f) || (NPC.direction == -1 && NPC.velocity.X > -3f)))
            {
                if ((NPC.direction == -1 && NPC.velocity.X < 0.1) || (NPC.direction == 1 && NPC.velocity.X > -0.1))
                {
                    NPC.velocity.X = NPC.velocity.X + 0.2f * NPC.direction;
                }
                else
                {
                    NPC.velocity.X = NPC.velocity.X * 0.93f;
                }
            }
            //some dust idk

            //int num252 = Dust.NewDust(npc.position, npc.width, npc.height, 4, npc.velocity.X, npc.velocity.Y, 255, new Color(0, 80, 255, 80), npc.scale * 1.2f);
            ////Main.NewText("dust 3");
            //Main.dust[num252].noGravity = true;
            //dust3 = Main.dust[num252];
            //dust3.velocity *= 0.5f;
            if (NPC.life > 0)
            {
                float num253 = NPC.life / (float)NPC.lifeMax; //without npc.scale
                num253 = num253 * 0.5f + 0.75f;
                num253 *= num238;
                if (num253 != NPC.scale)
                {
                    NPC.position.X = NPC.position.X + (NPC.width / 2);
                    NPC.position.Y = NPC.position.Y + NPC.height;
                    NPC.scale = num253;
                    NPC.width = (int)(76f * NPC.scale); //96f those are the hitbox adjusted scales for when the boss takes dammage (it gets smaller)
                    NPC.height = (int)(38f * NPC.scale); //92f
                    NPC.position.X = NPC.position.X - (NPC.width / 2);
                    NPC.position.Y = NPC.position.Y - NPC.height;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int num254 = (int)(NPC.lifeMax * 0.05);
                    if ((NPC.life + num254) < NPC.ai[3])
                    {
                        NPC.ai[3] = NPC.life;
                        int num255 = Main.rand.Next(1, 4);
                        int num2;
                        for (int num256 = 0; num256 < num255; num256 = num2 + 1) //spawn slimes when hit
                        {
                            int x = (int)(NPC.position.X + Main.rand.Next(NPC.width - 16)); //from 32 down to 16
                            int y = (int)(NPC.position.Y + Main.rand.Next(NPC.height - 16)); //from 32 down to 16
                            int num257 = Mod.Find<ModNPC>("FoldfishBaby").Type;
                            int num258 = NPC.NewNPC(x, y, num257);
                            Main.npc[num258].SetDefaults(num257);
                            Main.npc[num258].velocity.X = Main.rand.Next(-15, 16) * 0.1f;
                            Main.npc[num258].velocity.Y = Main.rand.Next(-30, 1) * 0.1f;
                            Main.npc[num258].ai[0] = (-1000 * Main.rand.Next(3));
                            Main.npc[num258].ai[1] = 0f;
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num258);
                            }
                            num2 = num256;
                        }
                    }
                }
            }
            NPC.spriteDirection = NPC.direction;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
    }
}