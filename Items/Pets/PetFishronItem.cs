using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetFishronItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetFishronProj>();

		public override int BuffType => ModContent.BuffType<PetFishronBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soggy Fish Cake");
			Tooltip.SetDefault("Summons a friendly Fishron that flies with you"
				+ "\nAppearance can be changed with Costume Suitcase");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
