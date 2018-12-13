using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class PocketSand : ModItem
    {
        public override void SetDefaults()
        {
			item.CloneDefaults(ItemID.ThrowingKnife);
            item.damage = 1;
            item.useTime = 10;
            item.shootSpeed = 10f;
            item.shoot = mod.ProjectileType("PocketSand");
            item.useAnimation = 35;
			item.value = 0;
            item.rare = -11;
			item.noUseGraphic = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("{Pocket Sand");
            Tooltip.SetDefault("Throw a clump of sand at an enemy to confuse it.");
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SandBlock, 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
