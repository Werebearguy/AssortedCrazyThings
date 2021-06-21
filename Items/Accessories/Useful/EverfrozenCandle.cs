using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [Autoload]
    public class EverfrozenCandle : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everfrozen Candle");
            Tooltip.SetDefault("Applies frostburn damage to all attacks");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AssPlayer>().everfrozenCandleBuff = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.MagmaStone, 1).AddIngredient(ItemID.IceBlock, 50).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
