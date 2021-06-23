using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
    [Content(ContentType.CuteSlimes)]
    public class CuteSlimeStatueItem : PlaceableItem<CuteSlimeStatueTile>
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Slime Statue");
            Tooltip.SetDefault("You can't catch statue spawned creatures");
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlacableTile(TileType);
            Item.width = 24;
            Item.height = 32;
            Item.maxStack = 99;
            Item.rare = -11;
            Item.value = Item.sellPrice(0, 0, 0, 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Gel, 200).AddIngredient(ItemID.StoneBlock, 50).AddRecipeGroup("ACT:RegularCuteSlimes", 1).AddTile(TileID.HeavyWorkBench).Register();
        }
    }
}
