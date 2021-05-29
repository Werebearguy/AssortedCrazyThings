using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class GiantGrasshopper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Grasshopper");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Derpling];
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 44;
            NPC.damage = 1;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath4;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 41;
            AIType = NPCID.Derpling;
            AnimationType = NPCID.Derpling;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * 0.01f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.Grasshopper, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantGrasshopperGore_01").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantGrasshopperGore_02").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantGrasshopperGore_02").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantGrasshopperGore_03").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantGrasshopperGore_03").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantGrasshopperGore_03").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/GiantGrasshopperGore_03").Type, 1f);
            }
        }
    }
}
