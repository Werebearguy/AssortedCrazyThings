using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Fun
{
    [Autoload]
    public class StarForge : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Forge");
            Tooltip.SetDefault("'An endless supply of the cosmos'");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FallenStar);
            Item.maxStack = 1;
            Item.consumable = false;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = -11;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.FallenStar, 3996).AddTile(TileID.CrystalBall).Register();
        }
    }
}
