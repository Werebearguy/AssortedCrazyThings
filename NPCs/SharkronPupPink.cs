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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Piranha];
        }

        public override void SetDefaults()
        {
            npc.width = 54;
            npc.height = 26;
            npc.damage = 85;
            npc.defense = 50;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 1f;
            npc.aiStyle = 16;
            aiType = NPCID.Piranha;
            animationType = NPCID.Piranha;
            npc.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !NPC.downedFishron ? 0f :
            SpawnCondition.Ocean.Chance * 0.005f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(2))
                Item.NewItem(npc.getRect(), ItemID.SharkFin, 1);
            if (Main.rand.NextBool(98))
                Item.NewItem(npc.getRect(), ItemID.DivingHelmet, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PinkSharkronPupGore_0"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PinkSharkronPupGore_1"), 1f);
            }
        }
    }
}
