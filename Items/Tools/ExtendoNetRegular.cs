using AssortedCrazyThings.Projectiles.Tools;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Tools
{
    public class ExtendoNetRegular : ExtendoNetBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Extendo-Net");
            Tooltip.SetDefault("'Catches those hard to reach critters'");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.useAnimation = 24;
            item.useTime = 32;
            item.value = Item.sellPrice(silver: 45);
            item.shoot = mod.ProjectileType<ExtendoNetRegularProj>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wire, 10);
            recipe.AddRecipeGroup("IronBar", 10);
            recipe.AddIngredient(ItemID.BugNet, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
