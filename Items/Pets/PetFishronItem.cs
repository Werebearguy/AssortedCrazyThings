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

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PetFishronItem_AoMM : SimplePetItemBase_AoMM<PetFishronItem>
	{
		public override int BuffType => ModContent.BuffType<PetFishronBuff_AoMM>();
	}
}
