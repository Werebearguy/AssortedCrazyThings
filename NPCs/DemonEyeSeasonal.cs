using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class DemonEyeSeasonal : ModNPC
    {
        private const int TotalNumberOfThese = 4;

        /*OG = 0
        * OP = 1
        * UG = 2
        * UP = 3
        */
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/DemonEyeSeasonal_0"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Eye");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.DemonEye];
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DemonEye);
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 18;
            NPC.defense = 2;
            NPC.lifeMax = 60;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 75f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 2;
            AIType = NPCID.DemonEye;
            AnimationType = NPCID.DemonEye;
            Banner = Item.NPCtoBanner(NPCID.DemonEye);
            BannerItem = Item.BannerToItem(Banner);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.halloween)
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.025f;
            }
            return 0f;
        }

        public override void OnKill()
        {
            if (Main.rand.NextBool(33))
                Item.NewItem(NPC.getRect(), ItemID.Lens, 1);
            if (Main.rand.NextBool(100))
                Item.NewItem(NPC.getRect(), ItemID.BlackLens, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                switch ((int)AiTexture)
                {
                    case 0:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_1").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_0").Type, 1f);
                        break;
                    case 1:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_1").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_0").Type, 1f);
                        break;
                    case 2:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_1").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_0").Type, 1f);
                        break;
                    case 3:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_1").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_0").Type, 1f);
                        break;
                    default:
                        break;
                }
            }
        }

        public float AiTexture
        {
            get
            {
                return NPC.ai[3];
            }
            set
            {
                NPC.ai[3] = value;
            }
        }

        public override bool PreAI()
        {
            if (AiTexture == 0 && NPC.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                AiTexture = Main.rand.Next(TotalNumberOfThese);

                NPC.localAI[0] = 1;
                NPC.netUpdate = true;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/DemonEyeSeasonal_" + AiTexture).Value;
            Vector2 stupidOffset = new Vector2(0f, NPC.height / 3); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }
    }
}
