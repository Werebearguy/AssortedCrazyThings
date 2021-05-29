using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class ReaverSharkPup : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reaver Shark Pup");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Piranha];
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 36;
            NPC.damage = 5;
            NPC.defense = 0;
            NPC.lifeMax = 25;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = 16;
            AIType = NPCID.Piranha;
            AnimationType = NPCID.Piranha;
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
            if (Main.rand.NextBool(97))
                Item.NewItem(NPC.getRect(), ItemID.DivingHelmet, prefixGiven: -1);
            if (Main.rand.NextBool(98))
                Item.NewItem(NPC.getRect(), ItemID.ReaverShark, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/ReaverPupGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/ReaverPupGore_1").Type, 1f);
            }
        }
    }
}
