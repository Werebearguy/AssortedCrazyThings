using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable
{
    public class CuteSlimeStatueItem : PlaceableItem<CuteSlimeStatueTile>
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Slime Statue");
            Tooltip.SetDefault("You can't catch statue spawned creatures");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = -11;
            Item.value = Item.sellPrice(0, 0, 0, 60);
            Item.createTile = TileType;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Gel, 200).AddIngredient(ItemID.StoneBlock, 50).AddRecipeGroup("ACT:RegularCuteSlimes", 1).AddTile(TileID.HeavyWorkBench).Register();
        }
    }
}
