using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items
{
	[Content(ContentType.CuteSlimes)]
	public class KnittingSet : AssItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 10;
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 22;
			Item.height = 22;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 30);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Silk, 15).AddTile(TileID.Loom).Register();
		}
	}
}
