using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
	public class acc_bottle_01 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Dreams");
			Tooltip.SetDefault("Allows the holder to double jump.");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 26;
			item.value = 0;
			item.rare = -11;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.doubleJumpUnicorn = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bottle, 1);
			recipe.AddIngredient(ItemID.PixieDust, 10);
            recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}