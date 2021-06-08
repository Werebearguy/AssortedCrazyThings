using AssortedCrazyThings.NPCs.DropConditions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class StrangeSlime : ModNPC
    {
        private const int TotalNumberOfThese = 4;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/StrangeSlime_0"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 62;
            NPC.damage = 7;
            NPC.defense = 2;
            NPC.lifeMax = 25;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 25f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = 1;
            AIType = NPCID.ToxicSludge;
            AnimationType = NPCID.ToxicSludge;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * 0.001f;
        }

        //public override void OnKill()
        //{
        //    /*          public const short StrangePlant1 = 3385;
        //                public const short StrangePlant2 = 3386;
        //                public const short StrangePlant3 = 3387;
        //                public const short StrangePlant4 = 3388;
        //    */

        //    if (NPC.Center == new Vector2(1000, 1000)) //RecipeBrowser fix
        //    {
        //        AiTexture = Main.rand.Next(4);
        //    }

        //    Item.NewItem(NPC.getRect(), 3385 + (int)AiTexture);
        //}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 0), ItemID.StrangePlant1));
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 1), ItemID.StrangePlant2));
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 2), ItemID.StrangePlant3));
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 3), ItemID.StrangePlant4));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("These peculiar slimes are host to strange plants, which grow for life. The slime always makes sure to keep it upright.")
            });
        }

        public float AiTexture
        {
            get
            {
                return NPC.ai[1];
            }
            set
            {
                NPC.ai[1] = value;
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

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/StrangeSlime_" + AiTexture).Value;
            Vector2 stupidOffset = new Vector2(0f, 4f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }
    }
}
