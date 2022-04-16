using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	public class FestiveMirror : AccessoryBase
	{
		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Festive Mirror");
			Tooltip.SetDefault("Provides immunity to Chilled, Frozen, and Stoned");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 32;
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = 4;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.buffImmune[BuffID.Stoned] = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HandWarmer, 1).AddIngredient(ItemID.PocketMirror, 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
