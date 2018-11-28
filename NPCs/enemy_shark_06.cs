using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_shark_06 : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Little Megalodon");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Shark];
				}
			public override void SetDefaults()
				{
					npc.width = 150;
					npc.height = 52;
					npc.damage = 250;
					npc.defense = 75;
					npc.lifeMax = 5000;
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
					return SpawnCondition.Ocean.Chance * 0.0005f;
				}
			public override void NPCLoot()
				{
					{
						Item.NewItem(npc.getRect(), mod.ItemType("pet_megalodon_02"));
					}
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
							{
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_06_01"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_06_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_06_03"), 1f);
							}
					}
				}
		}
}
