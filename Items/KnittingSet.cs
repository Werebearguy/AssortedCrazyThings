using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class KnittingSet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Knitting Set");
            Tooltip.SetDefault("'A set of tools used in crafting cute clothing and accessories'");
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.rare = -11;
            item.value = Item.sellPrice(silver: 35);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
