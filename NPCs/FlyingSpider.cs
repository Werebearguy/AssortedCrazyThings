using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class FlyingSpider : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Spider");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.FlyingSnake];
        }

        public override void SetDefaults()
        {
            npc.width = 76;
            npc.height = 48;
            npc.damage = 45;
            npc.defense = 17;
            npc.lifeMax = 195;
            npc.HitSound = SoundID.NPCHit29;
            npc.DeathSound = SoundID.NPCDeath31;
            npc.value = 25f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 14;
            aiType = NPCID.FlyingSnake;
            animationType = NPCID.FlyingSnake;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode == true)
            {
                return SpawnCondition.SpiderCave.Chance * 0.005f;
            }
            else
            {
                return SpawnCondition.SpiderCave.Chance * 0f;
            }
        }

        public override void NPCLoot()
        {
            if (Main.expertMode)
                Item.NewItem(npc.getRect(), ItemID.SpiderFang);
            if (Main.rand.Next(4) < 3)
                Item.NewItem(npc.getRect(), ItemID.SpiderFang, Main.rand.Next(1, 4));
            if (Main.rand.NextBool(50))
                Item.NewItem(npc.getRect(), ItemID.PoisonStaff, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FlyingSpiderGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FlyingSpiderGore_2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FlyingSpiderGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FlyingSpiderGore_0"), 1f);
            }
        }
    }
}
