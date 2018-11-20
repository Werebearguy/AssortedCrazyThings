using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_slime_02 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Turtle Slime");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
		}

			public virtual void ScaleExpertStats(NPC npc, int numPlayers, float bossLifeScale)
				{
				}
				

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 30;
			npc.damage = 7;
			npc.defense = 2;
			npc.lifeMax = 25;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 25f;
			npc.knockBackResist = 0.25f;
			npc.aiStyle = 1;
			aiType = NPCID.ToxicSludge;
			animationType = NPCID.ToxicSludge;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.SurfaceJungle.Chance * 0.1f;
		}
        public override void NPCLoot()
        {
            {
				Item.NewItem(npc.getRect(), ItemID.Gel);
			}
        }
		public override void HitEffect(int hitDirection, double damage)
		{
			{
				if (npc.life <= 0)
				{
					
				}
			}
		}
	}
}
