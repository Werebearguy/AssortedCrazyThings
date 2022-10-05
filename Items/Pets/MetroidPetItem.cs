using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class MetroidPetItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<MetroidPetProj>();

		public override int BuffType => ModContent.BuffType<MetroidPetBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class MetroidPetItem_AoMM : SimplePetItemBase_AoMM<MetroidPetItem>
	{
		public override int BuffType => ModContent.BuffType<MetroidPetBuff_AoMM>();
	}
}
