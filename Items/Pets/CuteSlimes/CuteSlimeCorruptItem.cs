using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeCorruptNew")]
	public class CuteSlimeCorruptItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeCorruptProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeCorruptBuff>();
	}
}
