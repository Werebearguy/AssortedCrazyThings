using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class TrueLegendaryWoodenSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Legendary Wooden Sword");
		}

		public override void SetDefaults()
		{
            item.CloneDefaults(ItemID.CobaltSword);
            item.width = 50;
            item.height = 50;
            item.rare = -11;
            item.value = Item.sellPrice(0, 0, 75, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<LegendaryWoodenSword>(), 1);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}
