using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [AutoloadEquip(EquipType.Shoes)]
    public class SolesOfFireAndIce : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soles of Fire and Ice");
            Tooltip.SetDefault("Allows you to walk on water, lava, and thin ice");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.fireWalk = true;
            player.waterWalk = true;
            player.iceSkate = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.LavaWaders, 1).AddIngredient(ItemID.IceSkates, 1).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
