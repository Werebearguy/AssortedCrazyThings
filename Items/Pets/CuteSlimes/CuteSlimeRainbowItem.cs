using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeRainbowNew")]
	public class CuteSlimeRainbowItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeRainbowProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeRainbowBuff>();
	}
}
