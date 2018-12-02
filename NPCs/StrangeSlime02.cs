using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	public class StrangeSlime02 : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Strange Slime");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
				}
			public override void SetDefaults()
				{
					npc.width = 36;
					npc.height = 62;
					npc.damage = 7;
					npc.defense = 2;
					npc.lifeMax = 25;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 25f;
					npc.knockBackResist = 0.25f;
					npc.aiStyle = 1;
					aiType = NPCID.ToxicSludge;
					animationType = NPCID.ToxicSludge;
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return SpawnCondition.OverworldDaySlime.Chance * 0.001f;
				}
			public override void NPCLoot()
				{
					{
						Item.NewItem(npc.getRect(), ItemID.StrangePlant2);
					}
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
