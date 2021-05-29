using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class WalkingChest : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Walking Chest");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Crab];
        }

        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 34;
            NPC.damage = 0;
            NPC.defense = 10;
            NPC.lifeMax = 40;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 75f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 3;
            AIType = NPCID.Crab;
            AnimationType = NPCID.Crab;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Underground.Chance * 0.025f * 0.083f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.Chest);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/WalkingTombstoneGore_01").Type, 1f);
            }
        }

        public override void PostAI()
        {
            if (Main.dayTime)
            {
                if (NPC.velocity.X > 0 || NPC.velocity.X < 0)
                {
                    NPC.velocity.X = 0;
                }
                if (NPC.velocity.Y < 0)
                {
                    NPC.velocity.Y = 0;
                }
            }
        }
    }
}
