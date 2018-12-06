using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class StoneSoldier : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Soldier");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ArmedZombie];
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 50;
            npc.damage = 45;
            npc.defense = 20;
            npc.lifeMax = 100;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.value = 60f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 3;
            aiType = NPCID.ArmedZombie;
            animationType = NPCID.ArmedZombie;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Cavern.Chance * 0.05f;
        }

        public override void NPCLoot()
        {
            {
                Item.NewItem(npc.getRect(), ItemID.StoneBlock, 10 + Main.rand.Next(20));
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 10; i++)
            {
            }
        }
    }
}
