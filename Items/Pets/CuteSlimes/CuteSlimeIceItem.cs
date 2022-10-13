using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeIceNew")]
	public class CuteSlimeIceItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeIceProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeIceBuff>();
	}
}
