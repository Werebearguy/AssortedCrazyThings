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
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Goldfish];
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 24;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = 16;
            AIType = NPCID.Goldfish;
            AnimationType = NPCID.Goldfish;
            NPC.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.015f;
        }

        public override void OnKill()
        {
            if (Main.rand.NextBool(2))
                Item.NewItem(NPC.getRect(), ItemID.SharkFin, 1);
            if (Main.rand.NextBool(98))
                Item.NewItem(NPC.getRect(), ItemID.WoodenCrate, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/WhaleSharkPupGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/WhaleSharkPupGore_1").Type, 1f);
            }
        }
    }
}
