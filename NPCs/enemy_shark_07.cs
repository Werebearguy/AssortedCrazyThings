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
					npc.width = 406;
					npc.height = 138;
					npc.damage = 450;
					npc.defense = 150;
					npc.lifeMax = 10500;
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
						if (Main.rand.NextBool(2))
								Item.NewItem(npc.getRect(), ItemID.SharkFin, 1);
						if (Main.rand.NextBool(97))
								Item.NewItem(npc.getRect(), ItemID.DivingHelmet, 1);
					}
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
							{
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_07_01"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_07_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_07_03"), 1f);
							}
					}
				}
		}
}
