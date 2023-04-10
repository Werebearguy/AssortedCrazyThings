using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	public class MeatballsEye : ChunkysMeatballsEyeBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			// DisplayName.SetDefault("Meatball's Eye");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();

			NPC.catchItem = ModContent.ItemType<Items.MeatballsEyeItem>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
				new FlavorTextBestiaryInfoElement("A minion of Cthulhu that was trapped in the Crimson. It hastily seeks out its older brother.")
			});
		}
	}
}
