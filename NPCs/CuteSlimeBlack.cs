using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class CuteSlimeBlack : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Cute Slime");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
				}
			public override void SetDefaults()
				{
					npc.width = 42;
					npc.height = 52;
					npc.scale = 0.9f;
					npc.friendly = true;
					npc.damage = 7;
					npc.defense = 2;
					npc.lifeMax = 5;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 25f;
					npc.knockBackResist = 0.9f;
					npc.aiStyle = 1;
					aiType = NPCID.ToxicSludge;
					animationType = NPCID.ToxicSludge;
					npc.alpha = 125;
					npc.color = new Color(0, 0, 0, 50);
					Main.npcCatchable[mod.NPCType("CuteSlimeBlack")] = true;
					npc.catchItem = (short)mod.ItemType("CuteSlimeBlack");
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return SpawnCondition.OverworldDaySlime.Chance * 0.025f;
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
