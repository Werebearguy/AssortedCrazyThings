using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Shoes)]
	public class SlipperySoles : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Slippery Soles");
					Tooltip.SetDefault("You slip and slide on all blocks");
				}
			public override void SetDefaults()
				{
					item.width = 20;
					item.height = 28;
					item.value = 0;
					item.rare = -11;
					item.accessory = true;
				}
			public override void UpdateAccessory(Player player, bool hideVisual)
				{
					player.slippy2 = true;
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.FrozenSlimeBlock, 2);
					recipe.AddIngredient(ItemID.Leather, 2);
					recipe.AddTile(TileID.TinkerersWorkbench);
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
		}
}