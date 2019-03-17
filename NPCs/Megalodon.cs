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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Shark];
        }

        public override void SetDefaults()
        {
            npc.width = 300;
            npc.height = 98;
            npc.damage = 500;
            npc.defense = 75;
            npc.lifeMax = 9999;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 10000f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 16;
            aiType = NPCID.Shark;
            animationType = NPCID.Shark;
            npc.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode && !NPC.AnyNPCs(mod.NPCType(name)))
            {
                return SpawnCondition.Ocean.Chance * 0.00001f;
            }
            else
            {
                return 0f;
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), mod.ItemType("SmallMegalodon"));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MegalodonGore_0"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MegalodonGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MegalodonGore_2"), 1f);
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
            if (npc.velocity.X > 3.85f)
            {
                npc.velocity.X = 3.85f;
            }
            if (npc.velocity.X < -3.85f)
            {
                npc.velocity.X = -3.85f;
            }
            if (npc.velocity.Y > 1.85f)
            {
                npc.velocity.Y = 1.85f;
            }
            if (npc.velocity.Y < -1.85f)
            {
                npc.velocity.Y = -1.85f;
            }
            return true;
        }
    }
}
