using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimePinkNew")]
	public class CuteSlimePinkItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimePinkProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimePinkBuff>();
	}
}
