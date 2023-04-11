using AssortedCrazyThings.Tiles.Paintings;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable.Paintings
{
	public class PretentiousRockItem : PaintingItemBase<PretentiousRock>
	{
		public override (int item, int amount) RecipeIngredient => (ItemID.StoneBlock, 1);
	}
}
