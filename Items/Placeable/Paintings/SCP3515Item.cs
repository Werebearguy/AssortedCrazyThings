using AssortedCrazyThings.Tiles.Paintings;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable.Paintings
{
	public class SCP3515Item : PaintingItemBase<SCP3515>
	{
		public override string PaintingName => "SCP-3515";

		public override string PaintingAuthor => "Unknown";

		public override (int item, int amount) RecipeIngredient => (ItemID.DirtBlock, 1);
	}
}
