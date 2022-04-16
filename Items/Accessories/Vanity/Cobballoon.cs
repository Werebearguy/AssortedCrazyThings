using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Vanity
{
	[AutoloadEquip(EquipType.Balloon)]
	public class Cobballoon : VanityAccessoryBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobballoon");
			Tooltip.SetDefault("'A clump of stones that manages to float, much to your confusion'");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 18;
			Item.height = 32;
			Item.value = 0;
			Item.rare = -11;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.StoneBlock, 25).AddIngredient(ItemID.WhiteString, 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
