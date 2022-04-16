using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("YoungWyvern")]
	public class YoungWyvernItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<YoungWyvernProj>();

		public override int BuffType => ModContent.BuffType<YoungWyvernBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Wyverntail");
			Tooltip.SetDefault("Summons a friendly Young Wyvern that flies with you"
				+ "\nAppearance can be changed with Costume Suitcase");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Wyverntail, 1).AddTile(TileID.WorkBenches).Register();
		}
	}
}
