using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	public class CuteSlimeGoldenItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeGoldenProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeGoldenBuff>();
	}
}
