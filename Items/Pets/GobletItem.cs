using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class GobletItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<GobletProj>();

		public override int BuffType => ModContent.BuffType<GobletBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(silver: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class GobletItem_AoMM : SimplePetItemBase_AoMM<GobletItem>
	{
		public override int BuffType => ModContent.BuffType<GobletBuff_AoMM>();
	}
}
