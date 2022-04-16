using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Fun
{
	[Content(ContentType.Weapons)]
	public class StarForge : AssItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Forge");
			Tooltip.SetDefault("'An endless supply of the cosmos'");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.FallenStar);
			Item.maxStack = 1;
			Item.consumable = false;
			Item.value = Item.sellPrice(0, 25, 0, 0);
			Item.rare = -11;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).
				AddIngredient(ItemID.FallenStar, 500)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
