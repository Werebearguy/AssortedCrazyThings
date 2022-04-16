using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class PigronataItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PigronataProj>();

		public override int BuffType => ModContent.BuffType<PigronataBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pigronata");
			Tooltip.SetDefault("Summons a friendly Pigronata to follow you"
				+ "\nAppearance can be changed with Costume Suitcase");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(gold: 2, silver: 20);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Pigronata, 1).AddIngredient(ItemID.LifeFruit, 1).AddTile(TileID.DemonAltar).Register();
		}
	}
}
