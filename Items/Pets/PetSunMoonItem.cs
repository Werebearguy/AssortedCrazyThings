using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class PetSunMoonItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetMoonProj>();

		public override int BuffType => ModContent.BuffType<PetSunMoonBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 26;
			Item.value = Item.sellPrice(gold: 16, silver: 20);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PetSunItem>()).AddIngredient(ModContent.ItemType<PetMoonItem>()).AddTile(TileID.CrystalBall).Register();
		}
	}

	//Light pet, no Aomm form
}
