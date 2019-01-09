using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
	public class MeatballsEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meatball's Eye");
			Tooltip.SetDefault("Find Chunky's Eye and combine the two at a Demon Altar.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Silk);
			item.rare = -11;
		}
	}
}
