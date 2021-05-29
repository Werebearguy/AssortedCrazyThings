using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Vanity
{
    [AutoloadEquip(EquipType.Balloon)]
    public class GreenEyelloonMetalFractured : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Green Metal Toothy Eye-lloon");
            Tooltip.SetDefault("'A Demon Eye balloon, for your Demon Eye needs!'");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;
            Item.value = 0;
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Lens, 1).AddIngredient(ItemID.ShinyRedBalloon, 1).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
