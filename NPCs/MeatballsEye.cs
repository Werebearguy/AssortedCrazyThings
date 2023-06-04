using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	public class MeatballsEye : ChunkysMeatballsEyeBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			NPC.catchItem = ModContent.ItemType<Items.MeatballsEyeItem>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
			});
		}
	}
}
