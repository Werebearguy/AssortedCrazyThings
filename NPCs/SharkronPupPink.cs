using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class SharkronPupPink : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharkron Pup");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Piranha];
        }

        public override void SetDefaults()
        {
            NPC.width = 54;
            NPC.height = 26;
            NPC.damage = 85;
            NPC.defense = 50;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 75f;
            NPC.knockBackResist = 1f;
            NPC.aiStyle = 16;
            AIType = NPCID.Piranha;
            AnimationType = NPCID.Piranha;
            NPC.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !NPC.downedFishron ? 0f :
            SpawnCondition.Ocean.Chance * 0.005f;
        }

        public override void OnKill()
        {
            if (Main.rand.NextBool(2))
                Item.NewItem(NPC.getRect(), ItemID.SharkFin, 1);
            if (Main.rand.NextBool(98))
                Item.NewItem(NPC.getRect(), ItemID.DivingHelmet, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/PinkSharkronPupGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/PinkSharkronPupGore_1").Type, 1f);
            }
        }
    }
}
