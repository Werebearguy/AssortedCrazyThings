using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Opabinia : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Opabinia");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Piranha];
        }

        public override void SetDefaults()
        {
            npc.width = 68;
            npc.height = 18;
            npc.damage = 18;
            npc.defense = 1;
            npc.lifeMax = 35;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 16;
            aiType = NPCID.Piranha;
            animationType = NPCID.Piranha;
            npc.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.025f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(2))
                Item.NewItem(npc.getRect(), ItemID.Shrimp, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OpabiniaGore_0"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OpabiniaGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OpabiniaGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OpabiniaGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OpabiniaGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OpabiniaGore_2"), 1f);
            }
        }
    }
}
