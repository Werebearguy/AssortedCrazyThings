using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Pets
{
	public class pet_DemonHeart_01 : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Demon Heart");
					Tooltip.SetDefault("Summons a friendly Demon Heart to follow you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("pet_DemonHeart_01");
					item.buffType = mod.BuffType("pet_DemonHeart_01");
					item.rare = -11;
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.DemonHeart, 1);
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