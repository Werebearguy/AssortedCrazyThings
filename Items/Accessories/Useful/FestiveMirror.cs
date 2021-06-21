using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [Autoload]
    public class FestiveMirror : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Festive Mirror");
            Tooltip.SetDefault("Provides immunity to Chilled, Frozen, and Stoned");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Chilled] = true;       //Immunity to Chilled debuff.
            player.buffImmune[BuffID.Frozen] = true;        //Immunity to Frozen debuff.
            player.buffImmune[BuffID.Stoned] = true;		//Immunity to Stoned debuff.
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.HandWarmer, 1).AddIngredient(ItemID.PocketMirror, 1).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
