using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Balloon)]
	public class WispInABalloon : AccessoryBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wisp in a Balloon");
			Tooltip.SetDefault("Glows in the dark and increases jump height");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 18;
			Item.height = 32;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = -11;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.jumpBoost = true;
			Lighting.AddLight(player.Center, 0.7f, 1.3f, 1.6f);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.WispinaBottle, 1).AddIngredient(ItemID.ShinyRedBalloon, 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
