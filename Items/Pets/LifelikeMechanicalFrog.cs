using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class LifelikeMechanicalFrog : ModItem
	{
		public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Lifelike Mechanical Frog");
				Tooltip.SetDefault("Summons a friendly Frog to follow you.");
			}
		public override void SetDefaults()
			{
				item.CloneDefaults(ItemID.ZephyrFish);
				item.shoot = mod.ProjectileType("LifelikeMechanicalFrog");
				item.buffType = mod.BuffType("LifelikeMechanicalFrog");
				item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }
		public override void AddRecipes()
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddIngredient(ItemID.Frog, 1);
				recipe.AddTile(TileID.Anvils);
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