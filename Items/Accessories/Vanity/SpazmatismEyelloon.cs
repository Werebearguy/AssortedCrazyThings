using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Vanity
{
	[AutoloadEquip(EquipType.Balloon)]
	public class SpazmatismEyelloon : VanityAccessoryBase
	{
		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Spazmatism Eye-lloon");
			Tooltip.SetDefault("'A Demon Eye balloon, for your Demon Eye needs!'");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 18;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 0, 80 + 1, 0); //lens 1s, sos 80s
			Item.rare = 1;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Lens, 1).AddIngredient(ItemID.SoulofSight, 1).AddIngredient(ItemID.ShinyRedBalloon, 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
