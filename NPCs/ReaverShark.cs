using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class ReaverShark : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reaver Shark");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Shark];
        }

        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 74;
            NPC.damage = 45;
            NPC.defense = 2;
            NPC.lifeMax = 400;
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
            if (Main.rand.NextBool(97))
                Item.NewItem(NPC.getRect(), ItemID.DivingHelmet, prefixGiven: -1);
            if (Main.rand.NextBool(98))
                Item.NewItem(NPC.getRect(), ItemID.ReaverShark, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/ReaverSharkGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/ReaverSharkGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/ReaverSharkGore_2").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/ReaverSharkGore_3").Type, 1f);
            }
        }
    }
}
