using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	public class GiantAnomalocaris : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Giant Anomalocaris");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Piranha];
				}
			public override void SetDefaults()
				{
					npc.width = 72;
					npc.height = 24;
					npc.damage = 30;
					npc.defense = 1;
					npc.lifeMax = 225;
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
					return SpawnCondition.Ocean.Chance * 0.005f;
				}
			public override void NPCLoot()
				{
					{
							if(Main.rand.Next(1) < 1) // a 1 in 1 chance
								Item.NewItem(npc.getRect(), ItemID.Shrimp);
							if(Main.rand.Next(200) < 1) // a 1 in 200 chance
								Item.NewItem(npc.getRect(), mod.ItemType("acc_wings_01"));
					}
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
							{
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_01_01"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_01_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_01_03"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_01_04"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_01_05"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shrimp_01_06"), 1f);
							}
					}
				}
		}
}
