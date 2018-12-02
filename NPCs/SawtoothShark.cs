using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	public class SawtoothShark : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Sawtooth Shark");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Shark];
				}
			public override void SetDefaults()
				{
					npc.width = 146;
					npc.height = 74;
					npc.damage = 45;
					npc.defense = 2;
					npc.lifeMax = 400;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 75f;
					npc.knockBackResist = 0.5f;
					npc.aiStyle = 16;
					aiType = NPCID.Shark;
					animationType = NPCID.Shark;
					npc.noGravity = true;
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return SpawnCondition.Ocean.Chance * 0.005f;
				}
			public override void NPCLoot()
				{
					{
						if (Main.rand.NextBool(2))
								Item.NewItem(npc.getRect(), ItemID.SharkFin, 1);
						if (Main.rand.NextBool(97))
								Item.NewItem(npc.getRect(), ItemID.DivingHelmet, 1);
						if (Main.rand.NextBool(98))
								Item.NewItem(npc.getRect(), ItemID.SawtoothShark, 1);
					}
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
							{
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_04_01"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_04_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_04_03"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_04_04"), 1f);
							}
					}
				}
		}
}
