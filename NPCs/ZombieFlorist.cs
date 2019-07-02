using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class ZombieFlorist : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zombie Florist");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Zombie];
        }

        public override void SetDefaults()
        {
            npc.width = 34;
            npc.height = 56;
            npc.damage = 14;
            npc.defense = 6;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 60f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 3;
            aiType = NPCID.Zombie;
            animationType = NPCID.Zombie;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.dayTime) return SpawnCondition.UndergroundJungle.Chance * 0.05f;
            else return 0f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(50))
                Item.NewItem(npc.getRect(), ItemID.Shackle, prefixGiven: -1);
            if (Main.rand.NextBool(250))
                Item.NewItem(npc.getRect(), ItemID.FlowerBoots, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZombieFloristGore_0"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZombieFloristGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZombieFloristGore_1"), 1f);
            }
        }
    }
}
