using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.KingSlime];
		}

		public override void SetDefaults()
		{
            //npc.CloneDefaults(NPCID.KingSlime);
			npc.aiStyle = -1; //set to -1 later
			npc.lifeMax = 4000;
			npc.damage = 20;
			npc.defense = 5;
			npc.knockBackResist = 0f;
			npc.width = 76;
			npc.height = 38;
            npc.scale = scaleFactor;
			npc.value = Item.buyPrice(0, 1, 0, 0);
            npc.npcSlots = 15f;
            npc.alpha = 0;
			npc.boss = true;
			npc.lavaImmune = true;
			//npc.noGravity = true;
			//npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.buffImmune[24] = true;
			music = MusicID.Boss1;
            animationType = NPCID.KingSlime;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(4))
            {
                Item.NewItem(npc.getRect(), mod.ItemType("OrigamiCrane"));
            }
            Item.NewItem(npc.getRect(), mod.ItemType("OrigamiHat"));
        }

        public override void AI()
		{
            //type == 50
            //aiStyle == 15
            float num238 = scaleFactor; //1f
            bool flag8 = false;
            bool flag9 = false;
            npc.aiAction = 0;
            if (npc.ai[3] == 0f && npc.life > 0)
            {
                npc.ai[3] = (float)npc.lifeMax;
            }
            if (npc.localAI[3] == 0f && Main.netMode != 1)
            {
                npc.ai[0] = -60; //-100f
                npc.localAI[3] = 1f;
                npc.TargetClosest();
                npc.netUpdate = true;
            }
            if (Main.player[npc.target].dead)
            {
                npc.TargetClosest();
                if (Main.player[npc.target].dead)
                {
                    npc.timeLeft = 0;
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                    {
                        npc.direction = 1;
                    }
                    else
                    {
                        npc.direction = -1;
                    }
                }
            }
            if (!Main.player[npc.target].dead && npc.ai[2] >= 300f && npc.ai[1] < 5f && npc.velocity.Y == 0f)
            {
                npc.ai[2] = 0f;
                npc.ai[0] = 0f;
                npc.ai[1] = 5f;
                if (Main.netMode != 1)
                {
                    npc.TargetClosest(faceTarget: false);
                    Point point3 = npc.Center.ToTileCoordinates();
                    Point point4 = Main.player[npc.target].Center.ToTileCoordinates();
                    Vector2 vector29 = Main.player[npc.target].Center - npc.Center;
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
                        if ((num244 < point4.Y - num241 || num244 > point4.Y + num241 || num243 < point4.X - num241 || num243 > point4.X + num241) && (num244 < point3.Y - num240 || num244 > point3.Y + num240 || num243 < point3.X - num240 || num243 > point3.X + num240) && !Main.tile[num243, num244].nactive())
                        {
                            int num245 = num244;
                            int num246 = 0;
                            if (!Main.tile[num243, num245].nactive() || !Main.tileSolid[Main.tile[num243, num245].type] || Main.tileSolidTop[Main.tile[num243, num245].type])
                            {
                                while (num246 < 150 && num245 + num246 < Main.maxTilesY)
                                {
                                    int num247 = num245 + num246;
                                    if (Main.tile[num243, num247].nactive() && Main.tileSolid[Main.tile[num243, num247].type] && !Main.tileSolidTop[Main.tile[num243, num247].type])
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
                            if (flag11 && Main.tile[num243, num244].lava())
                            {
                                flag11 = false;
                            }
                            if (flag11 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                            {
                                flag11 = false;
                            }
                            if (flag11)
                            {
                                npc.localAI[1] = (float)(num243 * 16 + 8);
                                npc.localAI[2] = (float)(num244 * 16 + 16);
                                break;
                            }
                        }
                    }
                    if (num242 >= 100)
                    {
                        Vector2 bottom = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].Bottom;
                        npc.localAI[1] = bottom.X;
                        npc.localAI[2] = bottom.Y;
                    }
                }
            }
            if (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
            {
                //ref float reference = ref npc.ai[2];
                //reference += 1f;
                npc.ai[2] += 1f;
            }
            if (Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 320f)
            {
                //ref float reference = ref npc.ai[2];
                //reference += 1f;
                npc.ai[2] += 1f;
            }
            Dust dust3;
            if (npc.ai[1] == 5f)
            {
                flag8 = true;
                npc.aiAction = 1;
                //ref float reference = ref npc.ai[0];
                //reference += 1f;
                npc.ai[0] += 1f;
                num238 = MathHelper.Clamp((40f - npc.ai[0]) / 40f, 0f, 1f); //60f to 40f
                num238 = 0.5f + num238 * 0.5f;
                if (npc.ai[0] >= 60f)
                {
                    flag9 = true;
                }
                if (npc.ai[0] == 60f)
                {
                    //Gore.NewGore(npc.Center + new Vector2(-40f, (0f - (float)npc.height) / 2f), npc.velocity, 734);
                }
                if (npc.ai[0] >= 40f && Main.netMode != 1) //60f to 40f
                {
                    npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]); //teleport to position of player
                    npc.ai[1] = 6f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                }
                if (Main.netMode == 1 && npc.ai[0] >= 80f) //120f to 80f, jump frequency, but 80 is the minimum, below and shit fucks up
                {
                    npc.ai[1] = 6f;
                    npc.ai[0] = 0f;
                }
                if (!flag9)
                {
                    int num2;
                    for (int num248 = 0; num248 < 10; num248 = num2 + 1)
                    {
                        //dust for when before it teleports, change 79 to whatever type
                        //int num249 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                        int num249 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 79, npc.velocity.X, npc.velocity.Y, 150, new Color(255, 255, 255), 1.2f);
                        Main.dust[num249].noGravity = true;
                        dust3 = Main.dust[num249];
                        dust3.velocity *= 0.5f;
                        num2 = num248;
                    }
                }
            }
            else if (npc.ai[1] == 6f)
            {
                flag8 = true;
                npc.aiAction = 0;
                //ref float reference = ref npc.ai[0];
                //reference += 1f;
                npc.ai[0] += 1f;
                num238 = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
                num238 = 0.5f + num238 * 0.5f;
                if (npc.ai[0] >= 30f && Main.netMode != 1)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                    npc.TargetClosest();
                }
                if (Main.netMode == 1 && npc.ai[0] >= 40f) //60f to 40f, cant remember what that does, maybe the jumping
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.TargetClosest();
                }
                int num2;
                for (int num250 = 0; num250 < 10; num250 = num2 + 1)
                {
                    //dust for when after it teleported
                    int num251 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 79, npc.velocity.X, npc.velocity.Y, 150, new Color(255, 255, 255), 1.2f);
                    Main.dust[num251].noGravity = true;
                    dust3 = Main.dust[num251];
                    dust3.velocity *= 2f;
                    num2 = num250;
                }
            }
            npc.dontTakeDamage = (npc.hide = flag9);
            if (npc.velocity.Y == 0f)
            {
                npc.velocity.X = npc.velocity.X * 0.8f;
                if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                {
                    npc.velocity.X = 0f;
                }
                if (!flag8)
                {
                    //ref float reference = ref npc.ai[0];
                    //reference += 2f;
                    npc.ai[0] += 2f;
                    if ((double)npc.life < (double)npc.lifeMax * 0.8)
                    {
                        //reference = ref npc.ai[0];
                        //reference += 1f;
                        npc.ai[0] += 1f;
                    }
                    if ((double)npc.life < (double)npc.lifeMax * 0.6)
                    {
                        //reference = ref npc.ai[0];
                        //reference += 1f;
                        npc.ai[0] += 1f;
                    }
                    if ((double)npc.life < (double)npc.lifeMax * 0.4)
                    {
                        //reference = ref npc.ai[0];
                        //reference += 2f;
                        npc.ai[0] += 2f;
                    }
                    if ((double)npc.life < (double)npc.lifeMax * 0.2)
                    {
                        //reference = ref npc.ai[0];
                        //reference += 3f;
                        npc.ai[0] += 3f;
                    }
                    if ((double)npc.life < (double)npc.lifeMax * 0.1)
                    {
                        //reference = ref npc.ai[0];
                        //reference += 4f;
                        npc.ai[0] += 4f;
                    }
                    if (npc.ai[0] >= 0f)
                    {
                        npc.netUpdate = true;
                        npc.TargetClosest();
                        if (npc.ai[1] == 3f) //jump heights here in velo.Y
                        {
                            npc.velocity.Y = -10f; //-13f
                            npc.velocity.X = npc.velocity.X + 3.5f * (float)npc.direction;
                            npc.ai[0] = -110f; //-200f
                            npc.ai[1] = 0f;
                        }
                        else if (npc.ai[1] == 2f)
                        {
                            npc.velocity.Y = -6f; //-8f
                            npc.velocity.X = npc.velocity.X + 4.5f * (float)npc.direction;
                            npc.ai[0] = -80f; //-120f
                            //reference = ref npc.ai[1];
                            //reference += 1f;
                            npc.ai[1] += 1f;
                        }
                        else
                        {
                            npc.velocity.Y = -6f; //-8f
                            npc.velocity.X = npc.velocity.X + 4f * (float)npc.direction;
                            npc.ai[0] = -80f; //-120f
                            //reference = ref npc.ai[1];
                            //reference += 1f;
                            npc.ai[1] += 1f;
                        }
                    }
                    else if (npc.ai[0] >= -30f)
                    {
                        npc.aiAction = 1;
                    }
                }
            }
            else if (npc.target < 255 && ((npc.direction == 1 && npc.velocity.X < 3f) || (npc.direction == -1 && npc.velocity.X > -3f)))
            {
                if ((npc.direction == -1 && (double)npc.velocity.X < 0.1) || (npc.direction == 1 && (double)npc.velocity.X > -0.1))
                {
                    npc.velocity.X = npc.velocity.X + 0.2f * (float)npc.direction;
                }
                else
                {
                    npc.velocity.X = npc.velocity.X * 0.93f;
                }
            }
            //some dust idk

            //int num252 = Dust.NewDust(npc.position, npc.width, npc.height, 4, npc.velocity.X, npc.velocity.Y, 255, new Color(0, 80, 255, 80), npc.scale * 1.2f);
            ////Main.NewText("dust 3");
            //Main.dust[num252].noGravity = true;
            //dust3 = Main.dust[num252];
            //dust3.velocity *= 0.5f;
            if (npc.life > 0)
            {
                float num253 = ((float)npc.life / (float)npc.lifeMax); //without npc.scale
                num253 = num253 * 0.5f + 0.75f;
                num253 *= num238;
                if (num253 != npc.scale)
                {
                    npc.position.X = npc.position.X + (float)(npc.width / 2);
                    npc.position.Y = npc.position.Y + (float)npc.height;
                    npc.scale = num253;
                    npc.width = (int)(76f * npc.scale); //96f those are the hitbox adjusted scales for when the boss takes dammage (it gets smaller)
                    npc.height = (int)(38f * npc.scale); //92f
                    npc.position.X = npc.position.X - (float)(npc.width / 2);
                    npc.position.Y = npc.position.Y - (float)npc.height;
                }
                if (Main.netMode != 1)
                {
                    int num254 = (int)((double)npc.lifeMax * 0.05);
                    if ((float)(npc.life + num254) < npc.ai[3])
                    {
                        npc.ai[3] = (float)npc.life;
                        int num255 = Main.rand.Next(1, 4);
                        int num2;
                        for (int num256 = 0; num256 < num255; num256 = num2 + 1) //spawn slimes when hit
                        {
                            int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 16)); //from 32 down to 16
                            int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 16)); //from 32 down to 16
                            int num257 = mod.NPCType("FoldfishBaby");
                            int num258 = NPC.NewNPC(x, y, num257);
                            Main.npc[num258].SetDefaults(num257);
                            Main.npc[num258].velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
                            Main.npc[num258].velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
                            Main.npc[num258].ai[0] = (float)(-1000 * Main.rand.Next(3));
                            Main.npc[num258].ai[1] = 0f;
                            if (Main.netMode == 2 && num258 < 200)
                            {
                                NetMessage.SendData(23, -1, -1, null, num258);
                            }
                            num2 = num256;
                        }
                    }
                }
            }
            npc.spriteDirection = npc.direction;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            //nothing
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.5f;
			return null;
		}
	}
}