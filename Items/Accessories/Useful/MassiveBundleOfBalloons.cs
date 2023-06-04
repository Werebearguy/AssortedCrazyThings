using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Balloon)]
	public class MassiveBundleOfBalloons : AccessoryBase
	{
		public override void SafeSetDefaults()
		{
			Item.width = 46;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 3 + 2 + 3 + 3, 54 + 10, 0); //Horseshoe 54s, bundle 3g, honey 2g, fart 3g, sharkron 3g, dreams 10s
			Item.rare = 3;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.noFallDmg = true;
			player.jumpBoost = true;
			player.hasJumpOption_Cloud = true;
			player.hasJumpOption_Sandstorm = true;
			player.hasJumpOption_Blizzard = true;
			player.hasJumpOption_Fart = true;
			player.hasJumpOption_Sail = true;
			player.hasJumpOption_Unicorn = true;
			player.honeyCombItem = Item;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.LuckyHorseshoe, 1).AddIngredient(ItemID.BundleofBalloons, 1).AddIngredient(ItemID.HoneyBalloon, 1).AddIngredient(ItemID.FartInABalloon, 1).AddIngredient(ItemID.SharkronBalloon, 1).AddIngredient<BottledDreams>().AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
