using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_shark_07 : ModNPC
		{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megalodon");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Shark];
		}
		public override void SetDefaults()
		{
			npc.width = 300;
			npc.height = 98;
			npc.damage = 999;
			npc.defense = 200;
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
			return SpawnCondition.Ocean.Chance * 0.00001f;
		}
		public override void NPCLoot()
		{
			{
				Item.NewItem(npc.getRect(), mod.ItemType("pet_megalodon_01"));
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_07_01"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_07_02"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_07_03"), 1f);
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
