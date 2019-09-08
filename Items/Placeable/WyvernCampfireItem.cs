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
            item.useStyle = 1;
            item.consumable = true;
            item.rare = -11;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.createTile = mod.TileType<WyvernCampfireTile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.anyWood = true;
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulFreed>(), 5);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
