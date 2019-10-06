using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
    public class VanityDresserItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Costume Dresser");
            Tooltip.SetDefault("Left Click to change your Pet's appearance"
                 + "\nRight Click to change your Light Pet's appearance");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 15;
            item.useStyle = 1;
            item.consumable = true;
            item.value = Item.sellPrice(silver: 10);
            item.rare = -11;
            item.createTile = ModContent.TileType<VanityDresserTile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Dresser);
            recipe.AddIngredient(ModContent.ItemType<VanitySelector>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
