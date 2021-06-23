using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
    public class WyvernCampfireItem : PlaceableItem<WyvernCampfireTile>
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wyvern Campfire");
            Tooltip.SetDefault("'Makes Wyverns go poof!'");
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlacableTile(TileType);
            Item.width = 32;
            Item.height = 18;
            Item.maxStack = 99;
            Item.rare = -11;
            Item.value = Item.buyPrice(0, 11, 50, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.GiantHarpyFeather).AddRecipeGroup("ACT:AdamantiteTitanium", 12).Register();
        }
    }
}
