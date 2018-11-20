using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_book_02 : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Animated Tome");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.GiantBat];
				}

			public virtual void ScaleExpertStats(NPC npc, int numPlayers, float bossLifeScale)
				{
				}

			public override void SetDefaults()
				{
					npc.width = 44;
					npc.height = 32;
					npc.damage = 13;
					npc.defense = 2;
					npc.lifeMax = 16;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 60f;
					npc.knockBackResist = 0.8f;
					npc.aiStyle = 14;
					npc.noGravity = true;
					aiType = NPCID.GiantBat;
					animationType = NPCID.GiantBat;
				}

			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return SpawnCondition.Dungeon.Chance * 0.005f;
				}
			
			public override void NPCLoot()
				{
					{
					Item.NewItem(npc.getRect(), ItemID.SpellTome);
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
