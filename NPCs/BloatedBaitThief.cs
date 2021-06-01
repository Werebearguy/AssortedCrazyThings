using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class BloatedBaitThief : ModNPC
    {
        public float scareRange = 400f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloated Bait Thief");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Goldfish];
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 42;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = -1;
            AIType = NPCID.Goldfish;
            AnimationType = NPCID.Goldfish;
            NPC.noGravity = true;
            NPC.buffImmune[BuffID.Confused] = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.raining == true)
            {
                return SpawnCondition.TownWaterCritter.Chance * 0.8f;
            }
            else
            {
                return SpawnCondition.TownWaterCritter.Chance * 0.5f;
            }
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.Worm, 2 + Main.rand.Next(7));
            if (Main.rand.NextBool(100)) Item.NewItem(NPC.getRect(), ItemID.GoldWorm);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("BaitThiefGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("BaitThiefGore_0").Type, 1f);
            }
        }

        public override void AI()
        {
            //modified goldfish AI
            if (NPC.direction == 0)
            {
                NPC.TargetClosest();
            }
            if (NPC.wet)
            {
                bool flag12 = false;
                NPC.TargetClosest(faceTarget: false);
                Vector2 centerpos = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                Vector2 playerpos = new Vector2(Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2),
                                                Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2));
                float distancex = playerpos.X - centerpos.X; //positive if player on the right
                float distancey = playerpos.Y - centerpos.Y; //positive if player below
                float distancen = (float)Math.Sqrt((double)(distancex * distancex + distancey * distancey)); //distance between player and fish
                if (Main.player[NPC.target].wet && distancen < scareRange)
                {
                    if (!Main.player[NPC.target].dead)
                    {
                        flag12 = true;
                    }
                }
                if (!flag12)
                {
                    if (NPC.collideX)
                    {
                        NPC.velocity.X = NPC.velocity.X * -1f;
                        NPC.direction *= -1;
                        NPC.netUpdate = true;
                    }
                    if (NPC.collideY)
                    {
                        NPC.netUpdate = true;
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = Math.Abs(NPC.velocity.Y) * -1f;
                            NPC.directionY = -1;
                            NPC.ai[0] = -1f;
                        }
                        else if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y = Math.Abs(NPC.velocity.Y);
                            NPC.directionY = 1;
                            NPC.ai[0] = 1f;
                        }
                    }
                }
                if (flag12) //if target is in water
                {
                    NPC.TargetClosest(faceTarget: false);
                    //face away from the player
                    NPC.direction = (distancex >= 0f) ? -1 : 1;
                    NPC.directionY = (distancey >= 0f) ? -1 : 1;


                    NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.1f;
                    NPC.velocity.Y = NPC.velocity.Y + (float)NPC.directionY * 0.1f;


                    if (NPC.velocity.X > 3f)
                    {
                        NPC.velocity.X = 3f;
                    }
                    if (NPC.velocity.X < -3f)
                    {
                        NPC.velocity.X = -3f;
                    }
                    if (NPC.velocity.Y > 2f)
                    {
                        NPC.velocity.Y = 2f;
                    }
                    if (NPC.velocity.Y < -2f)
                    {
                        NPC.velocity.Y = -2f;
                    }
                }
                else
                {
                    NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.1f;
                    if (NPC.velocity.X < -1f || NPC.velocity.X > 1f)
                    {
                        NPC.velocity.X = NPC.velocity.X * 0.95f;
                    }
                    if (NPC.ai[0] == -1f)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - 0.01f;
                        if ((double)NPC.velocity.Y < -0.3)
                        {
                            NPC.ai[0] = 1f;
                        }
                    }
                    else
                    {
                        NPC.velocity.Y = NPC.velocity.Y + 0.01f;
                        if ((double)NPC.velocity.Y > 0.3)
                        {
                            NPC.ai[0] = -1f;
                        }
                    }
                    int num261 = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
                    int num262 = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
                    if (Main.tile[num261, num262 - 1] == null)
                    {
                        Tile[,] tile3 = Main.tile;
                        int num263 = num261;
                        int num264 = num262 - 1;
                        Tile tile4 = new Tile();
                        tile3[num263, num264] = tile4;
                    }
                    if (Main.tile[num261, num262 + 1] == null)
                    {
                        Tile[,] tile5 = Main.tile;
                        int num265 = num261;
                        int num266 = num262 + 1;
                        Tile tile6 = new Tile();
                        tile5[num265, num266] = tile6;
                    }
                    if (Main.tile[num261, num262 + 2] == null)
                    {
                        Tile[,] tile7 = Main.tile;
                        int num267 = num261;
                        int num268 = num262 + 2;
                        Tile tile8 = new Tile();
                        tile7[num267, num268] = tile8;
                    }
                    if (Main.tile[num261, num262 - 1].LiquidAmount > 128)
                    {
                        if (Main.tile[num261, num262 + 1].IsActive)
                        {
                            NPC.ai[0] = -1f;
                        }
                        else if (Main.tile[num261, num262 + 2].IsActive)
                        {
                            NPC.ai[0] = -1f;
                        }
                    }
                    if ((double)NPC.velocity.Y > 0.4 || (double)NPC.velocity.Y < -0.4)
                    {
                        NPC.velocity.Y = NPC.velocity.Y * 0.95f;
                    }
                }
            }
            else //not wet, frantically jump around
            {
                if (NPC.velocity.Y == 0f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.velocity.Y = (float)Main.rand.Next(-50, -20) * 0.1f;
                        NPC.velocity.X = (float)Main.rand.Next(-20, 20) * 0.1f;
                        NPC.netUpdate = true;
                    }
                }
                NPC.velocity.Y = NPC.velocity.Y + 0.3f;
                if (NPC.velocity.Y > 10f)
                {
                    NPC.velocity.Y = 10f;
                }
                NPC.ai[0] = 1f;
            }
            NPC.rotation = NPC.velocity.Y * (float)NPC.direction * 0.1f;
            if (NPC.rotation < -0.2f)
            {
                NPC.rotation = -0.2f;
            }
            if (NPC.rotation > 0.2f)
            {
                NPC.rotation = 0.2f;
            }
        }
    }
}
