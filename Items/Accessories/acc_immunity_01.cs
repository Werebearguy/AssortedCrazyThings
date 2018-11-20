using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Accessories
{
	public class acc_immunity_01 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Festive Mirror");
			Tooltip.SetDefault("Provides immunity to Chilled, Frozen, and Stoned");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 32;
			item.value = 0;
			item.rare = -11;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[46] = true;					//Immunity to Chilled debuff.
			player.buffImmune[47] = true;					//Immunity to Frozen debuff.
			player.buffImmune[156] = true;					//Immunity to Stoned debuff.
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HandWarmer, 1);
			recipe.AddIngredient(ItemID.PocketMirror, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}