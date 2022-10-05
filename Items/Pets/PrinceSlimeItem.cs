using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PrinceSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PrinceSlimeProj>();

		public override int BuffType => ModContent.BuffType<PrinceSlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PrinceSlimeItem_AoMM : SimplePetItemBase_AoMM<PrinceSlimeItem>
	{
		public override int BuffType => ModContent.BuffType<PrinceSlimeBuff_AoMM>();
	}
}
