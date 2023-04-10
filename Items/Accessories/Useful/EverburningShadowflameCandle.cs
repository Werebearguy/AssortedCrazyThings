using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	public class EverburningShadowflameCandle : AccessoryBase
	{
		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Everburning Shadowflame Candle");
			// Tooltip.SetDefault("Applies shadowflame damage to all attacks");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 1, 41, 0);
			Item.rare = 3;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<AssPlayer>().everburningShadowflameCandleBuff = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.WaterCandle, 1) //1s
				.AddIngredient(ItemID.AncientBattleArmorMaterial, 1) //Forbidden fragment //1g
				.AddIngredient(ItemID.SoulofNight, 20) //2s * 20
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
