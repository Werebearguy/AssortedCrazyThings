using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	[LegacyName("YoungHarpy")]
	public class YoungHarpyItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<YoungHarpyProj>();

		public override int BuffType => ModContent.BuffType<YoungHarpyBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class YoungHarpyItem_AoMM : SimplePetItemBase_AoMM<YoungHarpyItem>
	{
		public override int BuffType => ModContent.BuffType<YoungHarpyBuff_AoMM>();
	}
}
