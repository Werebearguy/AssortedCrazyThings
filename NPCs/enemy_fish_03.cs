using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_fish_03 : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Mini Sharkron");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Goldfish];
				}

			public virtual void ScaleExpertStats(NPC npc, int numPlayers, float bossLifeScale)
				{
				}
				
			public override void SetDefaults()
				{
					npc.width = 54;
					npc.height = 26;
					npc.damage = 85;
					npc.defense = 50;
					npc.lifeMax = 50;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 75f;
					npc.knockBackResist = 1f;
					npc.aiStyle = 16;
					aiType = NPCID.Goldfish;
					animationType = NPCID.Goldfish;
					npc.noGravity = true;
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return !NPC.downedFishron ? 0f :
					SpawnCondition.Ocean.Chance * 0.025f;
				}
			public override void NPCLoot()
				{
					{
						if (Main.rand.NextBool(2))
								Item.NewItem(npc.getRect(), ItemID.SharkFin, 1);
						if (Main.rand.NextBool(98))
								Item.NewItem(npc.getRect(), ItemID.DivingHelmet, 1);
					}
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
						{
							Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_fish_03_01"), 1f);
							Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_fish_03_02"), 1f);
						}
					}
				}
		}
}
