using AssortedCrazyThings.Tiles.Paintings;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable.Paintings
{
	public class SCP099Item : PaintingItemBase<SCP099>
	{
		public override (int item, int amount) RecipeIngredient => (ItemID.Lens, 1);
	}
}
