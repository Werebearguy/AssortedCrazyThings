using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class BloatedBaitThief : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Bloated Bait Thief");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Goldfish];
				}
			public override void SetDefaults()
				{
					npc.width = 48;
					npc.height = 42;
					npc.damage = 0;
					npc.defense = 0;
					npc.lifeMax = 5;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 0f;
					npc.knockBackResist = 0.25f;
					npc.aiStyle = 16;
					aiType = NPCID.Goldfish;
					animationType = NPCID.Goldfish;
					npc.noGravity = true;
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					if(Main.raining == true)
					{
						return SpawnCondition.TownWaterCritter.Chance * 0.8f;
					}
					else
					{
						return SpawnCondition.TownWaterCritter.Chance * 0.5f;
					}
				}
			public override void NPCLoot()
				{
					{
						Item.NewItem(npc.getRect(), ItemID.Worm, 2 + Main.rand.Next(7));
						if (Main.rand.NextBool(100))
							Item.NewItem(npc.getRect(), ItemID.GoldWorm);
					}
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
							{
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_fish_01_02"), 1f);
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_fish_01_01"), 1f);
							}
					}
				}
		}
}
