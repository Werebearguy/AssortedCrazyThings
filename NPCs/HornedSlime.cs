using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	public class HornedSlime : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Horned Slime");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
				}
			public override void SetDefaults()
				{
					npc.width = 36;
					npc.height = 32;
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
					npc.alpha = 175;
					npc.color = new Color(240, 54, 115, 100);
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return SpawnCondition.Underworld.Chance * 0.015f;
				}
			public override void NPCLoot()
				{
					{
						Item.NewItem(npc.getRect(), ItemID.Gel);
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
