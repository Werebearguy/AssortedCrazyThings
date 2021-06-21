using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [Autoload]
    public class EverburningCandelabra : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everburning Candelabra");
            Tooltip.SetDefault("Applies various forms of fire damage to all attacks");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 13);
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            mPlayer.everburningCandleBuff = true;
            mPlayer.everburningShadowflameCandleBuff = true;
            mPlayer.everfrozenCandleBuff = true;
            mPlayer.everburningCursedCandleBuff = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<EverburningCandle>().AddIngredient<EverburningCursedCandle>().AddIngredient<EverburningShadowflameCandle>().AddIngredient<EverfrozenCandle>().AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
