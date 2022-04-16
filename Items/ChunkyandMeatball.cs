using Terraria;

namespace AssortedCrazyThings.Items
{
	[Content(ContentType.HostileNPCs)]
	public abstract class ChunkyandMeatballItem : AssItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 2);
			Item.rare = -11;
		}
	}

	public class ChunkysEyeItem : ChunkyandMeatballItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chunky's Eye");
			Tooltip.SetDefault("'Find Meatball's Eye and combine the two at a Demon Altar'");
		}
	}

	public class MeatballsEyeItem : ChunkyandMeatballItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meatball's Eye");
			Tooltip.SetDefault("'Find Chunky's Eye and combine the two at a Demon Altar'");
		}
	}
}
