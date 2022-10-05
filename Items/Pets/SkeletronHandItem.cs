using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SkeletronHandItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<SkeletronHandProj>();

		public override int BuffType => ModContent.BuffType<SkeletronHandBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class SkeletronHandItem_AoMM : SimplePetItemBase_AoMM<SkeletronHandItem>
	{
		public override int BuffType => ModContent.BuffType<SkeletronHandBuff_AoMM>();
	}
}
