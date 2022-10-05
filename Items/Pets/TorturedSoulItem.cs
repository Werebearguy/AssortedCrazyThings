using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("TorturedSoul")]
	public class TorturedSoulItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<TorturedSoulProj>();

		public override int BuffType => ModContent.BuffType<TorturedSoulBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(silver: 50);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.TaxCollectorsStickOfDoom, 1).AddTile(TileID.DemonAltar).Register();
		}
	}

	public class TorturedSoulItem_AoMM : SimplePetItemBase_AoMM<TorturedSoulItem>
	{
		public override int BuffType => ModContent.BuffType<TorturedSoulBuff_AoMM>();
	}
}
