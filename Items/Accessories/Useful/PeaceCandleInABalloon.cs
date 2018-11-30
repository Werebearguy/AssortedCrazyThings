using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Balloon)]
	public class PeaceCandleInABalloon : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Peace Candle in a Balloon");
					Tooltip.SetDefault("Reduces enemy spawn rate and increases jump height");
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
					player.calmed = true;
					Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 0.7f, 0.1f, 0.7f);
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.PeaceCandle, 1);
					recipe.AddIngredient(ItemID.ShinyRedBalloon, 1);
					recipe.AddTile(TileID.TinkerersWorkbench);
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
		}
}