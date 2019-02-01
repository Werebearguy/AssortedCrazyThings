using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class LegendaryWoodenSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Legendary Wooden Sword");
		}

		public override void SetDefaults()
		{
            item.CloneDefaults(ItemID.IronShortsword);
            item.width = 32;
            item.height = 32;
            item.rare = -11;
            item.value = Item.sellPrice(0, 0, 75, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 777);
            recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}
