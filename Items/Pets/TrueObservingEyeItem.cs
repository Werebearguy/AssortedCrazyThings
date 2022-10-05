using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class TrueObservingEyeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<TrueObservingEyeProj>();

		public override int BuffType => ModContent.BuffType<TrueObservingEyeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class TrueObservingEyeItem_AoMM : SimplePetItemBase_AoMM<TrueObservingEyeItem>
	{
		public override int BuffType => ModContent.BuffType<TrueObservingEyeBuff_AoMM>();
	}
}
