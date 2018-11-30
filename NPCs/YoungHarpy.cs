using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class YoungHarpy : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Young Harpy");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.FlyingSnake];
				}
			public override void SetDefaults()
				{
					npc.width = 50;
					npc.height = 44;
					npc.damage = 0;
					npc.defense = 1;
					npc.lifeMax = 5;
					npc.friendly = true;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 60f;
					npc.knockBackResist = 0.5f;
					npc.aiStyle = 14;
					aiType = NPCID.FlyingSnake;
					animationType = NPCID.FlyingSnake;
					Main.npcCatchable[mod.NPCType("YoungHarpy")] = true;
					npc.catchItem = (short)mod.ItemType("YoungHarpy");
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return SpawnCondition.Sky.Chance * 0.05f;
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
