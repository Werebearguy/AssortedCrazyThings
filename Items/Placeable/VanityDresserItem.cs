using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
    public class VanityDresserItem : PlaceableItem<VanityDresserTile>
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Costume Dresser");
            Tooltip.SetDefault("Left Click to change your Pet's appearance"
                 + "\nRight Click to change your Light Pet's appearance");
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = -11;
            Item.createTile = TileType;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Dresser).AddIngredient(ModContent.ItemType<VanitySelector>()).Register();
        }
    }
}
