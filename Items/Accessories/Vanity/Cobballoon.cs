using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Vanity
{
    [AutoloadEquip(EquipType.Balloon)]
    public class Cobballoon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cobballoon");
            Tooltip.SetDefault("'A clump of stones that manages to float, much to your confusion'");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 32;
            item.value = 0;
            item.rare = -11;
            item.accessory = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 25);
            recipe.AddIngredient(ItemID.WhiteString, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
