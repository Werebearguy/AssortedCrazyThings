using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_crabchest_01 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Walking Chest");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Crab];
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 34;
			npc.damage = 0;
			npc.defense = 10;
			npc.lifeMax = 40;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 75f;
			npc.knockBackResist = 0.5f;
			npc.aiStyle = 7;
			aiType = NPCID.Crab;
			animationType = NPCID.Crab;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Underground.Chance * 0.025f;
		}

        public override void NPCLoot()
		{
            Item.NewItem(npc.getRect(), ItemID.Chest);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
				{
				}
        }

        public override void PostAI()
        {
            if (Main.dayTime)
            {
                if (npc.velocity.X > 0 || npc.velocity.X < 0)
                {
                    npc.velocity.X = 0;
                }
                if (npc.velocity.Y < 0)
                {
                    npc.velocity.Y = 0;
                }
            }
        }
    }
}
