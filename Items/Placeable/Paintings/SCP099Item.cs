using AssortedCrazyThings.Tiles.Paintings;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable.Paintings
{
	public class SCP099Item : PaintingItemBase<SCP099>
	{
		public override string PaintingName => "The Portrait";

		public override string PaintingAuthor => "R. Magritte";

		public override (int item, int amount) RecipeIngredient => (ItemID.Lens, 1);
	}
}
