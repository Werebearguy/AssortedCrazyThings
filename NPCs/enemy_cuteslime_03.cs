using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_cuteslime_03 : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Cute Slime");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
				}

			public virtual void ScaleExpertStats(NPC npc, int numPlayers, float bossLifeScale)
				{
				}
				
			public override void SetDefaults()
				{
					npc.width = 42;
					npc.height = 52;
					npc.scale = 0.6f;
					npc.friendly = true;
					npc.damage = 7;
					npc.defense = 2;
					npc.lifeMax = 5;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 25f;
					npc.knockBackResist = 1.4f;
					npc.aiStyle = 1;
					aiType = NPCID.ToxicSludge;
					animationType = NPCID.ToxicSludge;
					npc.alpha = 125;
					npc.color = new Color(250, 30, 90, 90);
					Main.npcCatchable[mod.NPCType("enemy_cuteslime_03")] = true;
					npc.catchItem = (short)mod.ItemType("pet_cuteslime_03");
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return SpawnCondition.OverworldDaySlime.Chance * 0.025f;
				}
			public override void NPCLoot()
				{
					{
						Item.NewItem(npc.getRect(), ItemID.PinkGel);
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
