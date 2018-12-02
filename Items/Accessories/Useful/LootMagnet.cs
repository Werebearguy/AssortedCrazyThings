using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	public class LootMagnet : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Loot Magnet");
					Tooltip.SetDefault("Increased Coin Pickup Range"
						+ "\nIncreased Heart and Mana Pickup Range"
						+ "\nHitting Enemies Will Sometimes Drop Extra Coins"
						+ "\nShops Have Lowered Prices.");
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
					player.manaMagnet = true;						//This is the effect of the Celestial Magnet.
					player.lifeMagnet = true;						//This is the effect of the Heartreach Potion.
					player.goldRing = true;							//This is the effect of the Gold Ring.
					player.discount = true;							//This is the effect of the Discount Card.
					player.coins = true;							//This is the effect of the Lucky Coin.
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.GreedyRing, 1);
					recipe.AddIngredient(ItemID.CelestialMagnet, 1);
					recipe.AddIngredient(ItemID.HeartreachPotion, 1);
					recipe.AddTile(TileID.TinkerersWorkbench);
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
		}
}