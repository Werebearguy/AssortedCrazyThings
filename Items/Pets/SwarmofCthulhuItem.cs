using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SwarmofCthulhuItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<SwarmofCthulhuProj>();

		public override int BuffType => ModContent.BuffType<SwarmofCthulhuBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class SwarmofCthulhuItem_AoMM : SimplePetItemBase_AoMM<SwarmofCthulhuItem>
	{
		public override int BuffType => ModContent.BuffType<SwarmofCthulhuBuff_AoMM>();
	}
}
