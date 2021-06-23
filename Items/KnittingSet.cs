using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items
{
    [Content(ContentType.CuteSlimes)]
    public class KnittingSet : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Knitting Set");
            Tooltip.SetDefault("'A set of tools used in crafting cute clothing and accessories'");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 22;
            Item.height = 22;
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 30);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Silk, 15).AddTile(TileID.Loom).Register();
        }
    }
}
