using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Waist)]
	public class BottledDreams : AccessoryBase
	{
		public override void SafeSetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.value = Item.sellPrice(silver: 10);
			Item.rare = 4;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.hasJumpOption_Unicorn = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bottle, 1).AddIngredient(ItemID.PixieDust, 10).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
