using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    public class EverburningCandle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everburning Candle");
            Tooltip.SetDefault("Applies fire damage to all attacks");
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
            player.GetModPlayer<AssPlayer>().everburningCandleBuff = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.MagmaStone, 1).AddIngredient(ItemID.SoulofLight, 50).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
