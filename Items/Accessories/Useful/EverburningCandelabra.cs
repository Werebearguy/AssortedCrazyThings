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
            Tooltip.SetDefault("Inflicts various forms of fire damage on all attacks.");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 36;
            item.value = 0;
            item.rare = -11;
            item.accessory = true;
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "EverburningCandle");
            recipe.AddIngredient(mod, "EverburningCursedCandle");
            recipe.AddIngredient(mod, "EverburningShadowflameCandle");
            recipe.AddIngredient(mod, "EverfrozenCandle");
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
