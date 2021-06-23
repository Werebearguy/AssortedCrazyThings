using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [AutoloadEquip(EquipType.Shoes)]
    public class SlipperySoles : AccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slippery Soles");
            Tooltip.SetDefault("You slip and slide on all blocks");
        }
        public override void SafeSetDefaults()
        {
            Item.width = 20;
            Item.height = 28;
            Item.value = 0;
            Item.rare = -11;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.slippy2 = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.FrozenSlimeBlock, 2).AddIngredient(ItemID.Leather, 2).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}