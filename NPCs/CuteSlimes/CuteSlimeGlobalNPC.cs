using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
	[Content(ContentType.CuteSlimes)]
	public class CuteSlimeGlobalNPC : AssGlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.KingSlime)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CuteSlimePrincessItem>(), chanceDenominator: 10));
			}
			else if (npc.type == NPCID.QueenSlimeBoss)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CuteSlimeQueenItem>(), chanceDenominator: 10));
			}
		}

		public override void SpawnNPC(int npc, int tileX, int tileY)
		{
			NPC slime = Main.npc[npc];
			if (!SlimePets.slimePetReplacedByRareVariantOnSpawnNPCs.Contains(slime.type))
			{
				return;
			}

			if (Main.rand.NextBool(SlimePets.rareVariantSpawnDenominator))
			{
				slime.SetDefaults(ModContent.NPCType<CuteSlimePink>());
			}
			else if (Main.tenthAnniversaryWorld && Main.rand.NextBool(SlimePets.rareVariantSpawnDenominator))
			{
				slime.SetDefaults(ModContent.NPCType<CuteSlimeGolden>());
			}
		}
	}
}
