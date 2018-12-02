using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class BabyOcram : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Baby Ocram");
					Tooltip.SetDefault("Summons a miniature Ocram that follows you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("BabyOcram");
					item.buffType = mod.BuffType("BabyOcram");
					item.rare = -11;
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.SoulofFright, 10);
					recipe.AddIngredient(ItemID.SoulofMight, 10);
					recipe.AddIngredient(ItemID.SoulofSight, 10);
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