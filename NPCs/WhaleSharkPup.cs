using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class WhaleSharkPup : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whale Shark Pup");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Goldfish];
        }

        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 24;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 100;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 16;
            aiType = NPCID.Goldfish;
            animationType = NPCID.Goldfish;
            npc.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.015f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(2))
                Item.NewItem(npc.getRect(), ItemID.SharkFin, 1);
            if (Main.rand.NextBool(98))
                Item.NewItem(npc.getRect(), ItemID.WoodenCrate, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/WhaleSharkPupGore_0"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/WhaleSharkPupGore_1"), 1f);
            }
        }
    }
}
