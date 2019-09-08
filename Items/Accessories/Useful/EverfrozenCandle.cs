using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    public class EverfrozenCandle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everfrozen Candle");
            Tooltip.SetDefault("Applies frostburn damage to all attacks");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 22;
            item.value = Item.sellPrice(gold: 2);
            item.rare = -11;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AssPlayer>().everfrozenCandleBuff = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MagmaStone, 1);
            recipe.AddIngredient(ItemID.IceBlock, 50);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
