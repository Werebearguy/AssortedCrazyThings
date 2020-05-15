using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
    public class WyvernCampfireItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wyvern Campfire");
            Tooltip.SetDefault("'Makes Wyverns go poof!'");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 18;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.rare = -11;
            item.value = Item.buyPrice(0, 11, 50, 0);
            item.createTile = ModContent.TileType<WyvernCampfireTile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GiantHarpyFeather);
            recipe.AddRecipeGroup("ACT:AdamantiteTitanium", 12);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
