using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Accessories
{
	[AutoloadEquip(EquipType.Balloon)]
	public class Acc_Balloon_21 : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Bipolar Candle in a Balloon");
					Tooltip.SetDefault("Increased enemy spawn rate, reduced enemy spawn rate, and increased jump height");
				}
			public override void SetDefaults()
				{
					item.width = 18;
					item.height = 32;
					item.value = 0;
					item.rare = -11;
					item.accessory = true;
				}
			public override void UpdateAccessory(Player player, bool hideVisual)
				{
					player.jumpBoost = true;
					player.enemySpawns = true;
					player.calmed = true;
					Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 0.7f, 0.7f, 0.7f);
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(mod, "Acc_Balloon_19");
					recipe.AddIngredient(mod, "Acc_Balloon_20");
					recipe.AddTile(TileID.TinkerersWorkbench);
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
		}
}