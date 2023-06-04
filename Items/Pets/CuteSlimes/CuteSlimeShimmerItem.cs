using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	public class CuteSlimeShimmerItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeShimmerProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeShimmerBuff>();

		public override bool CanShimmerItem => false;
	}
}
