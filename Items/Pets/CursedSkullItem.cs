using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("CursedSkull")]
	public class CursedSkullItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<CursedSkullProj>();

		public override int BuffType => ModContent.BuffType<CursedSkullBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Inert Skull");
			Tooltip.SetDefault("Summons a friendly cursed skull that follows you"
				+ "\nAppearance can be changed with Costume Suitcase");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bone, 10).AddTile(TileID.DemonAltar).Register();
		}
	}
}
