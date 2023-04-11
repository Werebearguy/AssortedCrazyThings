using AssortedCrazyThings.Projectiles.Tools;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Tools
{
	public class ExtendoNetRegular : ExtendoNetBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useAnimation = 24;
			Item.useTime = 32;
			Item.value = Item.sellPrice(silver: 45);
			Item.rare = 1;
			Item.shoot = ModContent.ProjectileType<ExtendoNetRegularProj>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Wire, 10).AddRecipeGroup("IronBar", 10).AddIngredient(ItemID.BugNet, 1).AddTile(TileID.Anvils).Register();
		}
	}
}
