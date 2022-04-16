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

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Tortured Soul");
			Tooltip.SetDefault("Summons an unfriendly Tortured Soul to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(silver: 50);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.TaxCollectorsStickOfDoom, 1).AddTile(TileID.DemonAltar).Register();
		}
	}
}
