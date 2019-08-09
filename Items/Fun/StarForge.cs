using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Fun
{
    public class StarForge : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Forge");
            Tooltip.SetDefault("'An endless supply of the cosmos'");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.FallenStar);
            item.maxStack = 1;
            item.consumable = false;
            item.value = Item.sellPrice(gold: 1);
            item.rare = -11;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 3996);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
