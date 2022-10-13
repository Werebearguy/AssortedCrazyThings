using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SkeletronPrimeHandItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<SkeletronPrimeHandProj>();

		public override int BuffType => ModContent.BuffType<SkeletronPrimeHandBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class SkeletronPrimeHandItem_AoMM : SimplePetItemBase_AoMM<SkeletronPrimeHandItem>
	{
		public override int BuffType => ModContent.BuffType<SkeletronPrimeHandBuff_AoMM>();
	}
}
