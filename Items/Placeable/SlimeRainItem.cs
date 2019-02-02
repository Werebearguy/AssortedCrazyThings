using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
	public class SlimeRainItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Rain Item");
            Tooltip.SetDefault("'Slimes shall rain from the sky'"); //This is what the party machine says but with "Balloons" instead of "Slimes"
        }

        public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = -11;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.createTile = mod.TileType<SlimeRainItemTile>();
		}

		public override void AddRecipes()
		{
			//ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.LunarBar, 12);
			//recipe.AddTile(TileID.LunarCraftingStation);
			//recipe.SetResult(this);
			//recipe.AddRecipe();
		}
	}
}