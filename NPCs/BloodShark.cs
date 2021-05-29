using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class BloodShark : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Shark");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Shark];
        }

        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 74;
            NPC.damage = 65;
            NPC.defense = 2;
            NPC.lifeMax = 700;
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
            return !Main.bloodMoon ? 0f :
                SpawnCondition.Ocean.Chance * 0.1f;
        }

        public override void OnKill()
        {
            if (Main.rand.NextBool(2))
                Item.NewItem(NPC.getRect(), ItemID.SharkFin);
            if (Main.rand.NextBool(98))
                Item.NewItem(NPC.getRect(), ItemID.SharkToothNecklace, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/BloodSharkGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/BloodSharkGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/BloodSharkGore_2").Type, 1f);
            }
        }
    }
}
