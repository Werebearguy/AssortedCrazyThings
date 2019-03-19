using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class VanitySelector : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Costume Suitcase");
            Tooltip.SetDefault("Left Click to change your Pet's appearance"
                 + "\nRight Click to change your Light Pet's appearance");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.rare = -11;
            item.useAnimation = 16;
            item.useTime = 16;
            item.UseSound = SoundID.Item1;
            item.consumable = false;
            item.value = Item.sellPrice(silver: 10);
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            //actual recipe here
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("IronBar", 10);
            recipe.AddIngredient(ItemID.Silk, 50);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
