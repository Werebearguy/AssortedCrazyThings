using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Fun
{
	public class StarForge : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("StarForge");
					Tooltip.SetDefault("An endless supply of the cosmos");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.FallenStar);
					item.maxStack = 1;
					item.consumable = false;
					item.value = 0;
					item.rare = -11;
					item.shoot = mod.ProjectileType("proj_star_01");   //The projectile shoot when your weapon using this ammo
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.FallenStar, 3996);
					recipe.AddTile(TileID.CrystalBall);  //WorkBenches, Anvils, MythrilAnvil, Furnaces, DemonAltar, or TinkerersWorkbench
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
		}
}
