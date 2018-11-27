using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_shrimp_02 : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Anomalocaris");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Piranha];
				}	
			public override void SetDefaults()
				{
					npc.width = 48;
					npc.height = 16;
					npc.damage = 30;
					npc.defense = 1;
					npc.lifeMax = 50;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 75f;
					npc.knockBackResist = 0.5f;
					npc.aiStyle = 16;
					aiType = NPCID.Piranha;
					animationType = NPCID.Piranha;
					npc.noGravity = true;
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return SpawnCondition.Ocean.Chance * 0.025f;
				}
			public override void NPCLoot()
				{
					{
						if (Main.rand.NextBool(2))
								Item.NewItem(npc.getRect(), ItemID.Shrimp, 1);
					}
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
							{
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_02_01"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_02_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_02_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_02_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_02_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_02_03"), 1f);
							}
					}
				}
		}
}
