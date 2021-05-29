using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [AutoloadEquip(EquipType.Balloon)]
    public class WaterCandleInABalloon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Candle in a Balloon");
            Tooltip.SetDefault("Increased enemy spawn rate and jump height");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;
            Item.value = 0;
            Item.rare = -11;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpBoost = true;
            player.enemySpawns = true;
            Lighting.AddLight(player.Center, 0.1f, 0.4f, 0.7f);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.WaterCandle, 1).AddIngredient(ItemID.ShinyRedBalloon, 1).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}