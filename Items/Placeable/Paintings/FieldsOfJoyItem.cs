using AssortedCrazyThings.Tiles.Paintings;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable.Paintings
{
	public class FieldsOfJoyItem : PaintingItemBase<FieldsOfJoy>
	{
		public override string PaintingName => "Fields of Joy";

		public override string PaintingAuthor => "C. Paigner";

		public override (int item, int amount) RecipeIngredient => (ItemID.Gel, 1);
	}
}
