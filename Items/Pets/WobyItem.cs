using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class WobyItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<WobyProj>();

		public override int BuffType => ModContent.BuffType<WobyBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	//TODO aomm form
}
