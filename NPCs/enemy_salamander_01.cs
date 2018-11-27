using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_salamander_01 : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Deep Salamander");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Salamander];
				}
			public override void SetDefaults()
				{
					npc.width = 56;
					npc.height = 66;
					npc.damage = 36;
					npc.defense = 20;
					npc.lifeMax = 130;
					npc.HitSound = SoundID.NPCHit50;
					npc.DeathSound = SoundID.NPCDeath53;
					npc.value = 240f;
					npc.knockBackResist = 0.2f;
					npc.aiStyle = 3;
					aiType = NPCID.Salamander;
					animationType = NPCID.Salamander;
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					if(Main.hardMode == true)
						{
							return SpawnCondition.Cavern.Chance * 0.005f;
						}
					else
						{
							return SpawnCondition.Cavern.Chance * 0f;
						}
				}
			public override void NPCLoot()
				{
					{
						if (Main.rand.NextBool(90))
							Item.NewItem(npc.getRect(), ItemID.DepthMeter, 1);
					}
					{
						if (Main.rand.NextBool(95))
							Item.NewItem(npc.getRect(), ItemID.Compass, 1);
					}
					{
						if (Main.rand.NextBool(97))
							Item.NewItem(npc.getRect(), ItemID.Gradient, 1);
					}
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
							{
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_salamander_01_03"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_salamander_01_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_salamander_01_01"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_salamander_01_03"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_salamander_01_02"), 1f);
							}		
					}
				}
		}
}
