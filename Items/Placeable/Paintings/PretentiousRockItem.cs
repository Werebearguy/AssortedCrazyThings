using AssortedCrazyThings.Tiles.Paintings;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable.Paintings
{
	//TODO aquisition
	[Autoload]
	public class PretentiousRockItem : PaintingItemBase<PretentiousRock>
	{
		public override string PaintingName => "A Study of Slate";

		public override string PaintingAuthor => "C. Paigner";
	}
}
