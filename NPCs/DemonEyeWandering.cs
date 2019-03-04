using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.WanderingEye];
        }

        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 46;
            npc.damage = 40;
            npc.defense = 20;
            npc.lifeMax = 300;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.8f;
            npc.aiStyle = 2;
            aiType = NPCID.WanderingEye;
            animationType = NPCID.WanderingEye;
            banner = Item.NPCtoBanner(NPCID.DemonEye);
            bannerItem = Item.BannerToItem(banner);
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
            if (npc.life <= 0)
            {
                switch ((int)AiTexture)//switch ((int)npc.altTexture)
                {
                    case 0:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_eye_biggreen"), 1f);
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_eye_biggreen"), 1f);
                        break;
                    case 1:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_eye_bigpurple"), 1f);
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_eye_bigpurple"), 1f);
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
                return npc.ai[3];
            }
            set
            {
                npc.ai[3] = value;
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

            //if(Main.time % 60 == 0)
            //{
            //    AssUtils.Print("TEX " + npc.altTexture);
            //}

            if (AiTexture == 0 && npc.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                AiTexture = Main.rand.Next(TotalNumberOfThese);

                npc.localAI[0] = 1;
                npc.netUpdate = true;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/DemonEyeWandering_" + AiTexture);
            Vector2 stupidOffset = new Vector2(0f, npc.height / 3); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
            return false;
        }
    }
}
