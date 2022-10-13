using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeGreenNew")]
	public class CuteSlimeGreenItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeGreenProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeGreenBuff>();
	}
}
