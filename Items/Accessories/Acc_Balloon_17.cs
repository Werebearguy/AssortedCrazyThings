using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Accessories
{
	[AutoloadEquip(EquipType.Balloon)]
	public class Acc_Balloon_17 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wisp in a Balloon");
			Tooltip.SetDefault("Glows in the dark and increases jump height.");
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
            Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 0.7f, 1.3f, 1.6f);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WispinaBottle, 1);
			recipe.AddIngredient(ItemID.ShinyRedBalloon, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}