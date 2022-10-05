using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetCultistItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetCultistProj>();

		public override int BuffType => ModContent.BuffType<PetCultistBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	//Light pet, no Aomm form
}
