using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Accessories
{
	[AutoloadEquip(EquipType.Balloon)]
	public class Acc_Balloon_08 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Metal Eye-lloon");
			Tooltip.SetDefault("A Demon Eye balloon, for your Demon Eye needs!");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 32;
			item.value = 0;
			item.rare = -11;
			item.accessory = true;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Lens, 1);
			recipe.AddIngredient(ItemID.ShinyRedBalloon, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}