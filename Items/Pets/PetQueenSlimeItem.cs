using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetQueenSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetQueenSlimeAirProj>();

		public override int BuffType => ModContent.BuffType<PetQueenSlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.buyPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PetQueenSlimeItem_AoMM : SimplePetItemBase_AoMM<PetQueenSlimeItem>
	{
		public override int BuffType => ModContent.BuffType<PetQueenSlimeBuff_AoMM>();
	}
}
