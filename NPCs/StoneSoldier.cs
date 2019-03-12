using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class StoneSoldier : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Soldier");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ArmedZombie];
        }

        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 40;
            npc.damage = 30;
            npc.defense = 16;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.value = 60f;
            npc.knockBackResist = 1f;
            npc.aiStyle = 3;
            aiType = NPCID.ArmedZombie;
            animationType = NPCID.ArmedZombie;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Cavern.Chance * 0.04f;
        }

        public override void NPCLoot()
        {
            if (npc.Center == new Vector2(1000, 1000)) //RecipeBrowser fix
            {
                npc.ai[1] = Main.rand.Next(1, 7);
            }

            Item.NewItem(npc.getRect(), ItemID.StoneBlock, 10 + Main.rand.Next(20));
            if (npc.ai[1] <= 1) Item.NewItem(npc.getRect(), ItemID.Amethyst, 1); //sorted by rarity
            else if (npc.ai[1] <= 2) Item.NewItem(npc.getRect(), ItemID.Topaz, 1);
            else if (npc.ai[1] <= 3) Item.NewItem(npc.getRect(), ItemID.Sapphire, 1);
            else if (npc.ai[1] <= 4) Item.NewItem(npc.getRect(), ItemID.Emerald, 1);
            else if (npc.ai[1] <= 5) Item.NewItem(npc.getRect(), ItemID.Ruby, 1);
            else if (npc.ai[1] <= 6) Item.NewItem(npc.getRect(), ItemID.Diamond, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/StoneSoldierGore_01"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/StoneSoldierGore_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/StoneSoldierGore_03"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/StoneSoldierGore_04"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/StoneSoldierGore_04"), 1f);
			}

            for (int i = 0; i < 15; i++)
            {
                if (Main.rand.NextFloat() < 0.6f)
                {
                    Dust.NewDust(npc.position - new Vector2(-20, 0), (npc.width - 10) / 2, npc.height, 1, 0f, 0f, 50, new Color(255, 255, 255), 1f);
                }
            }
        }

        private float Gaussian(float x, float mean, float var = 1f)
        {
            return (float)((1/Math.Sqrt(Math.PI * 2 * var)) * Math.Exp(-(x - mean) * (x - mean) / (2 * var)));
        }

        public override bool PreAI()
        {
            if (npc.ai[1] == 0 && npc.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                float heightFactor = (float)(npc.position.Y - (16f * Main.rockLayer)) / (float)((Main.maxTilesY - 200 - Main.rockLayer) * 16f) * 6;
                //0f == above rock layer
                //3f == falfway down a hellavator
                //6f == hell start
                heightFactor = heightFactor > 6f ? 6f : heightFactor;
                heightFactor = heightFactor < 0f ? 0f : heightFactor;

                float[] arr = new float[6]; // 0 to 5
                for (int j = 1; j < 7; j++) //1 to 6
                {
                    arr[j - 1] = Gaussian(j, heightFactor, 2f);
                    if (j > 1) arr[j - 1] += arr[j - 2]; //cumulative sum
                }

                float sum = arr[arr.Length - 1];

                for (int j = 1; j < 7; j++)
                {
                    arr[j - 1] /= sum;
                }

                float rand = Main.rand.NextFloat();
                for (int j = 1; j < 7; j++)
                {
                    if (rand <= arr[j - 1]) //arr[6] is 1 
                    {
                        npc.ai[1] = j;
                        break;
                    }
                }

                npc.localAI[0] = 1;
                npc.netUpdate = true;
            }

            if (npc.ai[1] != 0 && npc.ai[3] == 1)
            {
                if (npc.direction == 1) npc.velocity.X += 0.09f; //0.02
                else npc.velocity.X -= 0.09f;
            }

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //base sprite is 80x66
            //hitbox is 18x40
            Texture2D texture = mod.GetTexture("NPCs/StoneSoldier_" + npc.ai[1]);
            Vector2 stupidOffset = new Vector2(0f, -4f + npc.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            drawColor = new Color((int)(drawColor.R * 1.2f + 40), (int)(drawColor.G * 1.2f + 40), (int)(drawColor.B * 1.2f + 40));
            drawColor.R = Math.Max(drawColor.R, (byte)100);
            drawColor.G = Math.Max(drawColor.G, (byte)100);
            drawColor.B = Math.Max(drawColor.B, (byte)100);
            spriteBatch.Draw(texture, drawPos, new Rectangle?(texture.Bounds), drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
        }
	}
}
