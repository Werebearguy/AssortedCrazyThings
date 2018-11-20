using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_crabgrave_06 : ModNPC
	{
		public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Walking Obelisk");
				Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Crab];
			}
				
			public virtual void ScaleExpertStats(NPC npc, int numPlayers, float bossLifeScale)
				{
				}
				
		public override void SetDefaults()
			{
				npc.width = 36;
				npc.height = 46;
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
				return SpawnCondition.OverworldNight.Chance * 0.025f;
			}
        public override void NPCLoot()
			{
				{
					Item.NewItem(npc.getRect(), ItemID.Obelisk);
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
