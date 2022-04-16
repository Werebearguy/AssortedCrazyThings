using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class PetSunItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetSunProj>();

		public override int BuffType => ModContent.BuffType<PetSunBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Sun");
			Tooltip.SetDefault("Summons a small sun that provides you with constant sunlight"
				+ "\nShows the current time in the buff tip");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.value = Item.sellPrice(gold: 9, silver: 20);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bottle).AddIngredient(ItemID.SunStone).AddIngredient(ItemID.Hellstone, 25).AddTile(TileID.CrystalBall).Register();
		}
	}
}
