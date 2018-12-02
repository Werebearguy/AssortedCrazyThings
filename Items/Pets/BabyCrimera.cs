using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class BabyCrimera : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Juicy Vertebrae");
					Tooltip.SetDefault("Summons a baby Crimera to follow you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("BabyCrimera");
					item.buffType = mod.BuffType("BabyCrimera");
					item.rare = -11;
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.Vertebrae, 30);
					recipe.AddTile(TileID.DemonAltar);
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
			public override void UseStyle(Player player)
				{
					if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
						{
							player.AddBuff(item.buffType, 3600, true);
						}
				}
		}
}