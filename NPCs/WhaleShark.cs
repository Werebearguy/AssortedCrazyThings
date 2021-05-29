using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class WhaleShark : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whale Shark");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Shark];
        }

        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 74;
            NPC.damage = 0;
            NPC.defense = 4;
            NPC.lifeMax = 1200;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 75f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 16;
            AIType = NPCID.Shark;
            AnimationType = NPCID.Shark;
            NPC.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.005f;
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
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/WhaleSharkGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/WhaleSharkGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/WhaleSharkGore_2").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/WhaleSharkGore_3").Type, 1f);
            }
        }
    }
}
