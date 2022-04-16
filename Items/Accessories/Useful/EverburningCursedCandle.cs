using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	public class EverburningCursedCandle : AccessoryBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Everburning Cursed Candle");
			Tooltip.SetDefault("Applies cursed fire damage to all attacks");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 2, 1, 0);
			Item.rare = -11;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<AssPlayer>().everburningCursedCandleBuff = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.WaterCandle, 1) //1s
				.AddIngredient(ItemID.CursedFlame, 20) //8s * 20
				.AddIngredient(ItemID.SoulofNight, 20) //2s * 20
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
