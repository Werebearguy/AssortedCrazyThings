using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Pets
{
	public class DocileMechanicalEyeGreen : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Unconscious Mechanical Eye");
					Tooltip.SetDefault("Summons a docile green Mechanical Eye to follow you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("DocileMechanicalEyeGreen");
					item.buffType = mod.BuffType("DocileMechanicalEyeGreen");
					item.rare = -11;
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.BlackLens, 1);
					recipe.AddIngredient(ItemID.Lens, 2);
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