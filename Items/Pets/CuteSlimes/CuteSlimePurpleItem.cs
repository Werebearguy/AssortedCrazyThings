using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimePurpleNew")]
	public class CuteSlimePurpleItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimePurpleProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimePurpleBuff>();
	}
}
