using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    public class EverburningShadowflameCandle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everburning Shadowflame Candle");
            Tooltip.SetDefault("Applies shadowflame damage to all attacks");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AssPlayer>().everburningShadowflameCandleBuff = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.MagmaStone, 1).AddIngredient(ItemID.SoulofNight, 50).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
