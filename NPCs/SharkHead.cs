using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class SharkHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shark Head");
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
            return SpawnCondition.Ocean.Chance * 0.005f;
        }

        public override void OnKill()
        {
            if (Main.rand.NextBool(2))
                Item.NewItem(NPC.getRect(), ItemID.SharkFin, 1);
            if (Main.rand.NextBool(50))
                Item.NewItem(NPC.getRect(), ItemID.Shackle, prefixGiven: -1);
            if (Main.rand.NextBool(2500))
                Item.NewItem(NPC.getRect(), ItemID.ZombieArm, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/SharkheadGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/SharkheadGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/SharkheadGore_1").Type, 1f);
            }
        }
    }
}
