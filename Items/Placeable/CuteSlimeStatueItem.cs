using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
    public class CuteSlimeStatueItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Slime Statue");
            Tooltip.SetDefault("You can't catch statue spawned creatures");
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
            item.value = Item.sellPrice(0, 0, 0, 60);
            item.createTile = ModContent.TileType<CuteSlimeStatueTile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Gel, 200);
            recipe.AddIngredient(ItemID.StoneBlock, 50);
            recipe.AddRecipeGroup("ACT:RegularCuteSlimes", 1);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
