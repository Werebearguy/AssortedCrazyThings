using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetGolemHeadItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetGolemHeadProj>();

		public override int BuffType => ModContent.BuffType<PetGolemHeadBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PetGolemHeadItem_AoMM : SimplePetItemBase_AoMM<PetGolemHeadItem>
	{
		public override int BuffType => ModContent.BuffType<PetGolemHeadBuff_AoMM>();
	}
}
