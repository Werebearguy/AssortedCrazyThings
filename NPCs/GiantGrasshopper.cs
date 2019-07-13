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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Derpling];
        }

        public override void SetDefaults()
        {
            npc.width = 64;
            npc.height = 44;
            npc.damage = 1;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath4;
            npc.value = 60f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 41;
            aiType = NPCID.Derpling;
            animationType = NPCID.Derpling;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * 0.01f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Grasshopper, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGrasshopperGore_01"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGrasshopperGore_02"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGrasshopperGore_02"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGrasshopperGore_03"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGrasshopperGore_03"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGrasshopperGore_03"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGrasshopperGore_03"), 1f);
            }
        }
    }
}
