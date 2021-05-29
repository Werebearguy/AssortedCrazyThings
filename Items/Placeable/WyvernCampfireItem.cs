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
            Item.width = 32;
            Item.height = 18;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = -11;
            Item.value = Item.buyPrice(0, 11, 50, 0);
            Item.createTile = ModContent.TileType<WyvernCampfireTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.GiantHarpyFeather).AddRecipeGroup("ACT:AdamantiteTitanium", 12).Register();
        }
    }
}
