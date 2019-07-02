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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Piranha];
        }

        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 36;
            npc.damage = 5;
            npc.defense = 0;
            npc.lifeMax = 25;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 16;
            aiType = NPCID.Piranha;
            animationType = NPCID.Piranha;
            npc.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.015f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(2))
                Item.NewItem(npc.getRect(), ItemID.SharkFin, 1);
            if (Main.rand.NextBool(97))
                Item.NewItem(npc.getRect(), ItemID.DivingHelmet, prefixGiven: -1);
            if (Main.rand.NextBool(98))
                Item.NewItem(npc.getRect(), ItemID.ReaverShark, prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReaverPupGore_0"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReaverPupGore_1"), 1f);
            }
        }
    }
}
