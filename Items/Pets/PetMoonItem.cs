using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class PetMoonItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetMoonProj>();

		public override int BuffType => ModContent.BuffType<PetMoonBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.value = Item.sellPrice(gold: 7);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bottle).AddIngredient(ItemID.MoonStone).AddIngredient(ItemID.Sextant).AddTile(TileID.CrystalBall).Register();
		}
	}

	//Light pet, no Aomm form
}
