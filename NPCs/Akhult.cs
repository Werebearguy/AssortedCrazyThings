using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Akhult : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Akhult");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.CreatureFromTheDeep];
        }

        public override void SetDefaults()
        {
            npc.width = 98;
            npc.height = 52;
            npc.damage = 40;
            npc.defense = 18;
            npc.lifeMax = 200;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 3;
            aiType = NPCID.CreatureFromTheDeep;
            animationType = NPCID.CreatureFromTheDeep;
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
            }
        }
    }
}
