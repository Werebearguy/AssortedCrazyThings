using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("BabyCrimera")]
	public class BabyCrimeraItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<BabyCrimeraProj>();

		public override int BuffType => ModContent.BuffType<BabyCrimeraBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 20);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Vertebrae, 10).AddTile(TileID.DemonAltar).Register();
		}
	}

	public class BabyCrimeraItem_AoMM : SimplePetItemBase_AoMM<BabyCrimeraItem>
	{
		public override int BuffType => ModContent.BuffType<BabyCrimeraBuff_AoMM>();
	}
}
