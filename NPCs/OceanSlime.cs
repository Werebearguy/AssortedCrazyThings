using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.NPCs.DropConditions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
    [Content(ContentType.HostileNPCs)]
    public class OceanSlime : AssNPC
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
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 36;
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
            NPC.alpha = 175;
            NPC.color = new Color(65, 193, 247, 100);
            NPC.catchItem = (short)ModContent.ItemType<OceanSlimeItem>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.015f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 0), ItemID.Gel));
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 1), ItemID.BlackInk));
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 2), ItemID.SharkFin));
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 3), ItemID.PinkGel));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("Slimes that are lost to the sea are faced with two options; adapt, or be eaten. Luckily, these slimes taste bad.")
            });
        }

        public ref float AiTexture => ref NPC.ai[1];

        public bool DecidedAiTexture
        {
            get => NPC.localAI[0] == 1f;
            set => NPC.localAI[0] = value ? 1f : 0f;
        }

        public override bool PreAI()
        {
            if (AiTexture == 0 && !DecidedAiTexture && Main.netMode != NetmodeID.MultiplayerClient)
            {
                AiTexture = Main.rand.Next(TotalNumberOfThese);

                DecidedAiTexture = true;
                NPC.netUpdate = true;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/OceanSlime_" + AiTexture).Value;
            Vector2 stupidOffset = new Vector2(0f, 4f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }
    }
}
