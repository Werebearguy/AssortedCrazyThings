using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetDreadnautilusItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetDreadnautilusProj>();

		public override int BuffType => ModContent.BuffType<PetDreadnautilusBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;

			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PetDreadnautilushItem_AoMM : SimplePetItemBase_AoMM<PetDreadnautilusItem>
	{
		public override int BuffType => ModContent.BuffType<PetDreadnautilusBuff_AoMM>();
	}
}
