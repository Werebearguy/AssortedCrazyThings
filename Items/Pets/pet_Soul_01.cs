using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Pets
{
	public class pet_Soul_01 : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Tortured Soul");
					Tooltip.SetDefault("Summons an unfriendly Tortured Soul to follow you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("pet_Soul_01");
					item.buffType = mod.BuffType("pet_Soul_01");
					item.rare = -11;
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.TaxCollectorsStickOfDoom, 1);
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