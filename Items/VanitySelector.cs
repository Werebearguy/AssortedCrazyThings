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
            Tooltip.SetDefault("Use to switch your pet's appearance");
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

        public override void AddRecipes()
        {
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.Lens, 11);
            //recipe.AddTile(TileID.Anvils);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }
    }
}
