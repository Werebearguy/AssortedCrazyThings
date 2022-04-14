using AssortedCrazyThings.Tiles.Paintings;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable.Paintings
{
	public class PretentiousRockItem : PaintingItemBase<PretentiousRock>
	{
		public override string PaintingName => "A Study of Slate";

		public override string PaintingAuthor => "C. Paigner";

		public override (int item, int amount) RecipeIngredient => (ItemID.StoneBlock, 1);
	}
}
