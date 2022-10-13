using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.CuteSlimes)]
	[LegacyName("CuteGastropod")]
	public class CuteGastropodItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<CuteGastropodProj>();

		public override int BuffType => ModContent.BuffType<CuteGastropodBuff>();

		public override void SafeSetDefaults()
		{

		}
	}

	[Content(ContentType.AommSupport | ContentType.CuteSlimes)]
	public class CuteGastropodItem_AoMM : SimplePetItemBase_AoMM<CuteGastropodItem>
	{
		public override int BuffType => ModContent.BuffType<CuteGastropodBuff_AoMM>();
	}
}
