using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class DemonEyeWandering : ModNPC
    {
        private const int TotalNumberOfThese = 2;

        /*WG = 0
        * WP = 1
        */
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/DemonEyeWandering_0"; //use fixed texture
            }
        }

        //public override string[] AltTextures
        //{
        //    get
        //    {
        //        return new[] { "AssortedCrazyThings/NPCs/DemonEyeWandering_1" };
        //    }
        //}

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wandering Eye");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.WanderingEye];

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true //Hides this NPC from the Bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 32;
            NPC.damage = 40;
            NPC.defense = 20;
            NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 75f;
            NPC.knockBackResist = 0.8f;
            NPC.aiStyle = 2;
            AIType = NPCID.WanderingEye;
            AnimationType = NPCID.WanderingEye;
            Banner = Item.NPCtoBanner(NPCID.DemonEye);
            BannerItem = Item.BannerToItem(Banner);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.025f;
            }
            return 0f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                switch ((int)AiTexture)//switch ((int)npc.altTexture)
                {
                    case 0:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("WanderingEyeGreenGore").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("WanderingEyeGreenGore").Type, 1f);
                        break;
                    case 1:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("WanderingEyePurpleGore").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("WanderingEyePurpleGore").Type, 1f);
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
            //if (npc.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            //{
            //    //npc.altTexture = Main.rand.Next(TotalNumberOfThese);
            //    //AssUtils.Print("generate texture " + npc.altTexture + " from " + npc.whoAmI);
            //    //AssUtils.Print("type " + Main.npc[npc.whoAmI].type);
            //    //AssUtils.Print("extracount " + NPCID.Sets.ExtraTextureCount[npc.type]);
            //    //AssUtils.Instance.SyncAltTextureNPC(npc);

            //    npc.localAI[0] = 1;
            //    npc.netUpdate = true;
            //}

            //if (Main.time % 60 == 0)
            //{
            //    AssUtils.Print("TEX " + npc.altTexture);
            //}

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
            Texture2D texture = Mod.GetTexture("NPCs/DemonEyeWandering_" + AiTexture).Value;
            Vector2 stupidOffset = new Vector2(0f, 0f); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }
    }
}
