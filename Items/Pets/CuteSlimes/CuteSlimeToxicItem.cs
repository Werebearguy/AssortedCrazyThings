using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeToxicNew")]
	public class CuteSlimeToxicItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeToxicProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeToxicBuff>();
	}
}
