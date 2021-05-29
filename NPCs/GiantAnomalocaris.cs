using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class GiantAnomalocaris : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Anomalocaris");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Piranha];
        }

        public override void SetDefaults()
        {
            NPC.width = 76;
            NPC.height = 24;
            NPC.damage = 30;
            NPC.defense = 1;
            NPC.lifeMax = 225;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 75f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 16;
            AIType = NPCID.Piranha;
            AnimationType = NPCID.Piranha;
            NPC.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.0075f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.Shrimp);
            if (Main.rand.NextBool(100)) // a 1 in 100 chance
                Item.NewItem(NPC.getRect(), Mod.Find<ModItem>("AnomalousWings").Type, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantAnomalocarisGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantAnomalocarisGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantAnomalocarisGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantAnomalocarisGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantAnomalocarisGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantAnomalocarisGore_3").Type, 1f);
            }
        }
    }
}