using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.VanityArmor
{
    [Autoload]
    [AutoloadEquip(EquipType.Head)]
    public class PrettyMask : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pretty Mask");
            Tooltip.SetDefault("'Goes well with a red scarf, red gloves, red boots, and your birthday suit'");
        }
        public override void SetDefaults()
        {
            //item.CloneDefaults(ItemID.BallaHat);
            Item.width = 26;
            Item.height = 26;
            Item.rare = -11;
            Item.value = 0;
            Item.vanity = true;
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Silk, 50).AddIngredient(ItemID.Feather, 1).AddTile(TileID.Loom).Register();
        }
    }
}
