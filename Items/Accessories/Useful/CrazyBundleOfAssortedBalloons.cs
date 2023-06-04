using AssortedCrazyThings.Items.Accessories.Vanity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Balloon)]
	public class CrazyBundleOfAssortedBalloons : AccessoryBase
	{
		public override void SafeSetDefaults()
		{
			Item.width = 46;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 3 + 2 + 3 + 3, 54 + 10, 0) + //bundle price, starwisp 5g5s, silly kit x1g, party ballons 40s, balloon animal 40s
				Item.sellPrice(0, 5, 5, 0) +
				Item.sellPrice(0, 1, 0, 0) +
				Item.sellPrice(0, 0, 40, 0) +
				Item.sellPrice(0, 0, 40, 0);
			Item.rare = 8;
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
			if (!player.HasBuff(BuffID.StarInBottle))
			{
				player.manaRegenBonus += 2;
			}
			Lighting.AddLight(player.Center, 0.7f, 1.3f, 1.6f);
		}

		/*
         * Massive bundle + Bipolar (+ Star) + Star Wisp + Cobballoon + Retinazer + Spaz + Bundled Party + Balloon Animal
         */
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<MassiveBundleOfBalloons>()).AddIngredient(ModContent.ItemType<StarWispBalloon>()).AddIngredient(ModContent.ItemType<SillyBalloonKit>()).AddIngredient(ItemID.PartyBundleOfBalloonsAccessory).AddIngredient(ItemID.PartyBalloonAnimal).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
