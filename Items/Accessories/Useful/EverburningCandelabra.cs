using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    public class EverburningCandelabra : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everburning Candelabra");
            Tooltip.SetDefault("Applies various forms of fire damage to all attacks");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 36;
            Item.value = Item.sellPrice(gold: 13);
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AssPlayer>().everburningCandleBuff = true;
            player.GetModPlayer<AssPlayer>().everburningShadowflameCandleBuff = true;
            player.GetModPlayer<AssPlayer>().everfrozenCandleBuff = true;
            player.GetModPlayer<AssPlayer>().everburningCursedCandleBuff = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<EverburningCandle>().AddIngredient<EverburningCursedCandle>().AddIngredient<EverburningShadowflameCandle>().AddIngredient<EverfrozenCandle>().AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
