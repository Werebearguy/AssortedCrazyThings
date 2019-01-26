using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//NOT BEING USED
namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Balloon)]
	public class MassiveBundleOfBalloons : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Massive Bundle of Balloons");
					Tooltip.SetDefault("Allows you to jump six more times"
						+ "\nNegates fall damage and increases jump height");
				}
			public override void SetDefaults()
				{
					item.width = 46;
					item.height = 42;
					item.value = 0;
					item.rare = -11;
					item.accessory = true;
				}
			public override void UpdateAccessory(Player player, bool hideVisual)
				{
					player.noFallDmg = true;
					player.jumpBoost = true;
					player.doubleJumpCloud = true;
					player.doubleJumpSandstorm = true;
					player.doubleJumpBlizzard = true;
					player.doubleJumpFart = true;
					player.doubleJumpSail = true;
					player.doubleJumpUnicorn = true;
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.LuckyHorseshoe, 1);
					recipe.AddIngredient(ItemID.BundleofBalloons, 1);
					recipe.AddIngredient(ItemID.HoneyBalloon, 1);
					recipe.AddIngredient(ItemID.FartInABalloon, 1);
					recipe.AddIngredient(ItemID.SharkronBalloon, 1);
					recipe.AddIngredient(mod, "BottledDreams");
					recipe.AddTile(TileID.TinkerersWorkbench);
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
		}
}