using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Balloon)]
	public class StarInABalloon : AccessoryBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star in a Balloon");
			Tooltip.SetDefault("Increased mana regeneration and jump height");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 18;
			Item.height = 32;
			Item.value = Item.sellPrice(silver: 5);
			Item.rare = -11;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//player.manaRegenDelayBonus++;
			//player.manaRegenBonus += 25;
			if (!player.HasBuff(BuffID.StarInBottle))
			{
				player.manaRegenBonus += 2;
			}
			player.jumpBoost = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.StarinaBottle, 1).AddIngredient(ItemID.ShinyRedBalloon, 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
