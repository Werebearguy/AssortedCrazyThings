using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class QueenLarvaItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<QueenLarvaProj>();

		public override int BuffType => ModContent.BuffType<QueenLarvaBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Queen Larva");
			Tooltip.SetDefault("Summons a Queen Bee Larva to follow you"
				+ "\nAppearance can be changed with Costume Suitcase");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 32;
			Item.rare = -11;
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
