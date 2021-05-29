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
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.rare = -11;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.UseSound = SoundID.Item1;
            Item.consumable = false;
            Item.value = Item.sellPrice(silver: 10);
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            //actual recipe here
            CreateRecipe(1).AddRecipeGroup("IronBar", 10).AddIngredient(ItemID.Silk, 50).AddTile(TileID.Anvils).Register();
        }
    }
}
