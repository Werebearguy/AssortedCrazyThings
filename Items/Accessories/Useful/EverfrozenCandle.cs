using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	public class EverfrozenCandle : AccessoryBase
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 1, 41, 0);
			Item.rare = 3;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<AssPlayer>().everfrozenCandleBuff = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.WaterCandle, 1) //1s
				.AddIngredient(ItemID.FrostCore, 1) //1g
				.AddIngredient(ItemID.SoulofLight, 20) //2s * 20
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
