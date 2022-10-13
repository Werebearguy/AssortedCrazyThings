using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeJungleNew")]
	public class CuteSlimeJungleItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeJungleProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeJungleBuff>();
	}
}
