using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Vanity
{
	[AutoloadEquip(EquipType.Balloon)]
	public class FriendlySoulVanity : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Soul Companion");
					Tooltip.SetDefault("A soul that you saved from the Dungeon.");
				}
			public override void SetDefaults()
				{
					item.width = 18;
					item.height = 32;
					item.value = 0;
					item.rare = -11;
					item.accessory = true;
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(mod, "CaughtSoul");
					recipe.AddTile(TileID.TinkerersWorkbench);
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
		}
}