using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeBlueNew")]
	public class CuteSlimeBlueItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeBlueProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeBlueBuff>();
	}
}
