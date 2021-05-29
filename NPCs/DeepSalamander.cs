using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class DeepSalamander : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deep Salamander");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Salamander];
        }

        public override void SetDefaults()
        {
            NPC.width = 56;
            NPC.height = 66;
            NPC.damage = 36;
            NPC.defense = 20;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit50;
            NPC.DeathSound = SoundID.NPCDeath53;
            NPC.value = 240f;
            NPC.knockBackResist = 0.2f;
            NPC.aiStyle = 3;
            AIType = NPCID.Salamander;
            AnimationType = NPCID.Salamander;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {
                return SpawnCondition.Cavern.Chance * 0.0075f;
            }
            else
            {
                return 0f;
            }
        }

        public override void OnKill()
        {
            if (Main.rand.NextBool(45))
                Item.NewItem(NPC.getRect(), ItemID.DepthMeter, prefixGiven: -1);
            if (Main.rand.NextBool(48))
                Item.NewItem(NPC.getRect(), ItemID.Compass, prefixGiven: -1);
            if (Main.rand.NextBool(49))
                Item.NewItem(NPC.getRect(), ItemID.Gradient, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/DeepSalamanderGore_2").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/DeepSalamanderGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/DeepSalamanderGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/DeepSalamanderGore_2").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/DeepSalamanderGore_1").Type, 1f);
            }
        }

        public override bool PreAI()
        {
            //ai[1] is the internal timer for the attack delay, it naturally starts at 70,
            //and at roughly 30 it shoots the projectile, which takes 30 ticks
            //here the timer is now capped at 40 instead of 70, more than halving the delay
            if (NPC.ai[1] > 40) NPC.ai[1] = 40; //attack speed roughly doubled
            //Main.NewText("newline");
            //Main.NewText(npc.ai[0]);
            //Main.NewText(npc.ai[1]);
            //Main.NewText(npc.ai[2]);
            //Main.NewText(npc.ai[3]);
            return true;
        }
    }
}
