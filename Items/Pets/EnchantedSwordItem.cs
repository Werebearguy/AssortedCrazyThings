using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("EnchantedSword")]
	public class EnchantedSwordItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<EnchantedSwordProj>();

		public override int BuffType => ModContent.BuffType<EnchantedSwordBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(silver: 40);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.EnchantedSword, 1).AddTile(TileID.DemonAltar).Register();
		}
	}
}
