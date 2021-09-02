using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable.Paintings
{
	/// <summary>
	/// Base class for all painting items
	/// </summary>
	public abstract class PaintingItemBase<T> : PlaceableItem<T> where T : ModTile
	{
		/// <summary>
		/// Name of the painting
		/// </summary>
		public abstract string PaintingName { get; }

		/// <summary>
		/// Name of the author displayed in the tooltip
		/// </summary>
		public abstract string PaintingAuthor { get; }

		public sealed override void SetStaticDefaults()
		{
			DisplayName.SetDefault(PaintingName);
			Tooltip.SetDefault($"'{PaintingAuthor}'");
		}

		public sealed override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType);
			Item.value = Item.buyPrice(gold: 1);
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Blue;
		}
	}
}
