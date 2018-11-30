using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class BloodShark : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Blood Shark");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Shark];
				}
			public override void SetDefaults()
				{
					npc.width = 120;
					npc.height = 74;
					npc.damage = 65;
					npc.defense = 2;
					npc.lifeMax = 700;
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
					return !Main.bloodMoon ? 0f :
						SpawnCondition.Ocean.Chance * 0.1f;
				}
			public override void NPCLoot()
				{
					{
						if (Main.rand.NextBool(2))
								Item.NewItem(npc.getRect(), ItemID.SharkFin, 1);
						if (Main.rand.NextBool(98))
								Item.NewItem(npc.getRect(), ItemID.SharkToothNecklace, 1);
					}
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
							{
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_05_01"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_05_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_shark_05_03"), 1f);
							}
					}
				}
		}
}
