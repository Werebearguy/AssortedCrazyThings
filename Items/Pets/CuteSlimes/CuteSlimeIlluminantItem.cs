using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeIlluminantNew")]
	public class CuteSlimeIlluminantItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeIlluminantProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeIlluminantBuff>();
	}
}
