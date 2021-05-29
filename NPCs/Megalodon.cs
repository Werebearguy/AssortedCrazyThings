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
            NPC.width = 300;
            NPC.height = 98;
            NPC.damage = 500;
            NPC.defense = 75;
            NPC.lifeMax = 9999;
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
            if (Main.hardMode && !NPC.AnyNPCs(NPC.type))
            {
                return SpawnCondition.Ocean.Chance * 0.00001f;
            }
            else
            {
                return 0f;
            }
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), Mod.Find<ModItem>("SmallMegalodon").Type);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/MegalodonGore_0").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/MegalodonGore_1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/MegalodonGore_2").Type, 1f);
            }
        }

        public override bool PreAI()
        {
            //while printing out Main.NewText(npc.velocity) I noticed that the speed caps at 5x and 3y
            //then I looked into the AI code and found this piece here

            //reduces max speed from 5 to 4 in X direction
            //and from 3 to 2 in Y direction
            //in vanilla, it was 5 and 3 respectively
            //doing this here needs to be 0.15 less since this is by how much it increases per frame
            if (NPC.velocity.X > 3.85f)
            {
                NPC.velocity.X = 3.85f;
            }
            if (NPC.velocity.X < -3.85f)
            {
                NPC.velocity.X = -3.85f;
            }
            if (NPC.velocity.Y > 1.85f)
            {
                NPC.velocity.Y = 1.85f;
            }
            if (NPC.velocity.Y < -1.85f)
            {
                NPC.velocity.Y = -1.85f;
            }
            return true;
        }
    }
}
