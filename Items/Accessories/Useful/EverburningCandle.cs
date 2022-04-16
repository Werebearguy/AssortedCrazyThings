using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	public class EverburningCandle : AccessoryBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Everburning Candle");
			Tooltip.SetDefault("Applies fire damage to all attacks");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 2, 41, 0);
			Item.rare = -11;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<AssPlayer>().everburningCandleBuff = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.WaterCandle, 1) //1s
				.AddIngredient(ItemID.MagmaStone, 1) //2g
				.AddIngredient(ItemID.SoulofLight, 20) //2s * 20
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
