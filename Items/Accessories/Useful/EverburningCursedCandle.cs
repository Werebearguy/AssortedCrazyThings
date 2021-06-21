using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [Autoload]
    public class EverburningCursedCandle : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everburning Cursed Candle");
            Tooltip.SetDefault("Applies cursed fire damage to all attacks");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AssPlayer>().everburningCursedCandleBuff = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.MagmaStone, 1).AddIngredient(ItemID.CursedFlame, 50).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
