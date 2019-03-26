using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class OceanSlime : ModNPC
    {
        private const int TotalNumberOfThese = 4;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/OceanSlime_0"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ocean Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            npc.width = 36;
            npc.height = 36;
            npc.damage = 7;
            npc.defense = 2;
            npc.lifeMax = 25;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            npc.alpha = 175;
            npc.color = new Color(65, 193, 247, 100);
            Main.npcCatchable[mod.NPCType("OceanSlime")] = true;
            npc.catchItem = (short)mod.ItemType("OceanSlimeItem");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.015f;
        }

        public override void NPCLoot()
        {
            int itemid = 0;

            if (npc.Center == new Vector2(1000, 1000)) //RecipeBrowser fix
            {
                AiTexture = Main.rand.Next(4);
            }

            switch ((int)AiTexture)
            {
                case 0:
                    itemid = ItemID.Gel;
                    break;
                case 1:
                    itemid = ItemID.BlackInk;
                    break;
                case 2:
                    itemid = ItemID.SharkFin;
                    break;
                case 3:
                    itemid = ItemID.PinkGel;
                    break;
                default:
                    break;
            }

            Item.NewItem(npc.getRect(), itemid);
        }

        public float AiTexture
        {
            get
            {
                return npc.ai[1];
            }
            set
            {
                npc.ai[1] = value;
            }
        }

        public override bool PreAI()
        {
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
            Texture2D texture = mod.GetTexture("NPCs/OceanSlime_" + AiTexture);
            Vector2 stupidOffset = new Vector2(0f, 4f + npc.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
            return false;
        }
    }
}
