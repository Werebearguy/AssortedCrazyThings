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
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Zombie];
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 56;
            NPC.damage = 14;
            NPC.defense = 6;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 3;
            AIType = NPCID.Zombie;
            AnimationType = NPCID.Zombie;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.dayTime) return SpawnCondition.UndergroundJungle.Chance * 0.05f;
            else return 0f;
        }

        public override void OnKill()
        {
            if (Main.rand.NextBool(50))
                Item.NewItem(NPC.getRect(), ItemID.Shackle, prefixGiven: -1);
            if (Main.rand.NextBool(250))
                Item.NewItem(NPC.getRect(), ItemID.FlowerBoots, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/ZombieFloristGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/ZombieFloristGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/ZombieFloristGore_1").Type, 1f);
            }
        }
    }
}
