using AssortedCrazyThings.Projectiles.Tools;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Tools
{
    public class ExtendoNetGolden : ExtendoNetBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Extendo-Net");
            Tooltip.SetDefault("'Catches those REALLY hard to reach critters'");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.useAnimation = 18;
            item.useTime = 24;
            item.value = Item.sellPrice(gold: 5, silver: 90);
            item.shoot = mod.ProjectileType<ExtendoNetGoldenProj>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wire, 10);
            recipe.AddRecipeGroup("ACT:GoldPlatinum", 10);
            recipe.AddIngredient(ItemID.GoldenBugNet, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
