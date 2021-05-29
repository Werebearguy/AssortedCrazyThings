using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class FlyingSpider : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Spider");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.FlyingSnake];
        }

        public override void SetDefaults()
        {
            NPC.width = 76;
            NPC.height = 48;
            NPC.damage = 45;
            NPC.defense = 17;
            NPC.lifeMax = 195;
            NPC.HitSound = SoundID.NPCHit29;
            NPC.DeathSound = SoundID.NPCDeath31;
            NPC.value = 25f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = 14;
            AIType = NPCID.FlyingSnake;
            AnimationType = NPCID.FlyingSnake;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode == true)
            {
                return SpawnCondition.SpiderCave.Chance * 0.005f;
            }
            else
            {
                return SpawnCondition.SpiderCave.Chance * 0f;
            }
        }

        public override void OnKill()
        {
            if (Main.expertMode)
                Item.NewItem(NPC.getRect(), ItemID.SpiderFang);
            if (Main.rand.Next(4) < 3)
                Item.NewItem(NPC.getRect(), ItemID.SpiderFang, Main.rand.Next(1, 4));
            if (Main.rand.NextBool(50))
                Item.NewItem(NPC.getRect(), ItemID.PoisonStaff, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/FlyingSpiderGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/FlyingSpiderGore_2").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/FlyingSpiderGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/FlyingSpiderGore_0").Type, 1f);
            }
        }
    }
}
