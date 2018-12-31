using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    public class FestiveMirror : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Festive Mirror");
            Tooltip.SetDefault("Provides immunity to Chilled, Frozen, and Stoned");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 32;
            item.value = 0;
            item.rare = -11;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Chilled] = true;       //Immunity to Chilled debuff.
            player.buffImmune[BuffID.Frozen] = true;        //Immunity to Frozen debuff.
            player.buffImmune[BuffID.Stoned] = true;		//Immunity to Stoned debuff.
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HandWarmer, 1);
            recipe.AddIngredient(ItemID.PocketMirror, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
