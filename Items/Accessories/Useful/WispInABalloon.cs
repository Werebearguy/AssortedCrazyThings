using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [AutoloadEquip(EquipType.Balloon)]
    public class WispInABalloon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wisp in a Balloon");
            Tooltip.SetDefault("Glows in the dark and increases jump height");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 32;
            item.value = Item.sellPrice(gold: 5);
            item.rare = -11;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpBoost = true;
            Lighting.AddLight(player.Center, 0.7f, 1.3f, 1.6f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WispinaBottle, 1);
            recipe.AddIngredient(ItemID.ShinyRedBalloon, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
