using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class LittleMegalodon : ModNPC
    {
        public static string name = "LittleMegalodon";
        public static string message = "A Megalodon is approaching! Get out of the ocean!";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Shark];
        }

        public override void SetDefaults()
        {
            npc.width = 150;
            npc.height = 52;
            npc.damage = 150;
            npc.defense = 50;
            npc.lifeMax = 5000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 10000f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 16;
            aiType = NPCID.Shark;
            animationType = NPCID.Shark;
            npc.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.0005f;
        }

        public override void NPCLoot()
        {
            {
                Item.NewItem(npc.getRect(), mod.ItemType("MiniMegalodon"));
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            {
                if (npc.life <= 0)
                {
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_06_01"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_06_02"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_06_03"), 1f);
                }
            }
        }
    }
}
