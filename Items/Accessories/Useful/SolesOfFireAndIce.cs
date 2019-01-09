using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [AutoloadEquip(EquipType.Shoes)]
    public class SolesOfFireAndIce : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soles of Fire and Ice");
            Tooltip.SetDefault("Allows you to walk on water, lava, and thin ice.");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 28;
            item.value = Item.sellPrice(gold: 10);
            item.rare = -11;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.fireWalk = true;
            player.waterWalk = true;
            player.iceSkate = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LavaWaders, 1);
            recipe.AddIngredient(ItemID.IceSkates, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
