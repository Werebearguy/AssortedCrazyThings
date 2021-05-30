using AssortedCrazyThings.Items.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Megalodon : ModNPC
    {
        public static string name = "Megalodon";
        public static string message = "A Megalodon is approaching! Get out of the ocean!";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Shark];
        }

        public override void SetDefaults()
        {
            NPC.width = 150;
            NPC.height = 52;
            NPC.damage = 150;
            NPC.defense = 50;
            NPC.lifeMax = 1000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 10000f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 16;
            AIType = NPCID.Shark;
            AnimationType = NPCID.Shark;
            NPC.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!NPC.AnyNPCs(NPC.type))
            {
                return SpawnCondition.Ocean.Chance * 0.0005f;
            }
            else return 0f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ModContent.ItemType<MiniMegalodon>());
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/LittleMegalodonGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/LittleMegalodonGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/LittleMegalodonGore_2").Type, 1f);
            }
        }
    }
}
