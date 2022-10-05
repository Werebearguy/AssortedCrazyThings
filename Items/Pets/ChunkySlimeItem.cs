using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("ChunkyItem")]
	[Content(ContentType.HostileNPCs)]
	public class ChunkySlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<ChunkySlimeProj>();

		public override int BuffType => ModContent.BuffType<ChunkySlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.HostileNPCs)]
	public class ChunkySlimeItem_AoMM : SimplePetItemBase_AoMM<ChunkySlimeItem>
	{
		public override int BuffType => ModContent.BuffType<ChunkySlimeBuff_AoMM>();
	}
}
