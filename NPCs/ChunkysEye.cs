using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	public class ChunkysEye : ChunkysMeatballsEyeBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			NPC.catchItem = ModContent.ItemType<Items.ChunkysEyeItem>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
			});
		}
	}
}
