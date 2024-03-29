using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	public class GiantGrasshopperGold : GiantGrasshopper
	{
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OverworldDaySlime.Chance * 0.001f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.GoldGrasshopper));
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			//quickUnlock: true so only 1 kill is required to list everything about it
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPC.type], quickUnlock: true);

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
            });
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life > 0)
			{
				for (int i = 0; i < 10; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, Main.rand.Next(232, 234), hit.HitDirection, -1f);
				}
			}
			else
			{
				var entitySource = NPC.GetSource_Death();
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_01").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_02").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_02").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_03").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_03").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_03").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_03").Type, 1f);
				for (int i = 0; i < 20; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, Main.rand.Next(232, 234), 2 * hit.HitDirection, -2f);
				}
			}
		}

		public override void DrawEffects(ref Color drawColor)
		{
			//using Microsoft.Xna.Framework;
			//change the NPC. to Projectile. if you port this to pets
			Color color = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
			if (color.R > 20 || color.B > 20 || color.G > 20)
			{
				int num = color.R;
				if (color.G > num)
				{
					num = color.G;
				}
				if (color.B > num)
				{
					num = color.B;
				}
				num /= 30;
				if (Main.rand.Next(300) < num)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 43, 0f, 0f, 254, new Color(255, 255, 0), 0.5f);
					dust.velocity *= 0f;
				}
			}
		}
	}
}
