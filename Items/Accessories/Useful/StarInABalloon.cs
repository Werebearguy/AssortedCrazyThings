using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Balloon)]
	public class StarInABalloon : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Star in a Balloon");
					Tooltip.SetDefault("Increased mana regeneration and jump height.");
				}
			public override void SetDefaults()
				{
					item.width = 18;
					item.height = 32;
					item.value = 0;
					item.rare = -11;
					item.accessory = true;
				}
			public override void UpdateAccessory(Player player, bool hideVisual)
				{
					player.manaRegenDelayBonus++;
					player.manaRegenBonus += 25;
					player.jumpBoost = true;
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.StarinaBottle, 1);
					recipe.AddIngredient(ItemID.ShinyRedBalloon, 1);
					recipe.AddTile(TileID.TinkerersWorkbench);
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
		}
}