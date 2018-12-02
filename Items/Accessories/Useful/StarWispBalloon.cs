using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Balloon)]
	public class StarWispBalloon : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Star Wisp Balloon");
					Tooltip.SetDefault("Increased mana regeneration and jump height" +
									   "\nGlows in the dark");
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
					Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 0.7f, 1.3f, 1.6f);
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(mod, "StarInABalloon");
					recipe.AddIngredient(mod, "WispInABalloon");
					recipe.AddTile(TileID.TinkerersWorkbench);
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
		}
}