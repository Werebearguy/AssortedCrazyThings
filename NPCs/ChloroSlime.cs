using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	public class ChloroSlime : MudSlime
	{
		public override void Load()
		{
			base.Load();

			On_NPC.TransformVisuals += On_NPC_TransformVisuals;
		}

		private static void On_NPC_TransformVisuals(On_NPC.orig_TransformVisuals orig, NPC self, int oldType, int newType)
		{
			orig(self, oldType, newType);

			if (newType == ModContent.NPCType<ChloroSlime>())
			{
				for (int i = 0; i < 20; i++)
				{
					Dust dust = Dust.NewDustDirect(self.position, self.width, self.height, DustID.Chlorophyte, Scale: 1.2f);
					dust.noGravity = true;
				}
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			base.HitEffect(hit);

			if (NPC.life > 0)
			{
				for (int i = 0; i < hit.Damage / (float)NPC.lifeMax * 25f; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Chlorophyte, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int i = 0; i < 20; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Chlorophyte, 2 * hit.HitDirection, -2f);
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.ChlorophyteOre, minimumDropped: 5, maximumDropped: 10));
		}

		public override void PostAI()
		{
			//no-op
		}
	}
}
