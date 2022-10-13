using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.HostileNPCs)]
	[LegacyName("StingSlimeBlackItem", "StingSlimeOrangeItem")]
	public class StingSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<StingSlimeProj>();

		public override int BuffType => ModContent.BuffType<StingSlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.HostileNPCs)]
	public class StingSlimeItem_AoMM : SimplePetItemBase_AoMM<StingSlimeItem>
	{
		public override int BuffType => ModContent.BuffType<StingSlimeBuff_AoMM>();
	}
}
