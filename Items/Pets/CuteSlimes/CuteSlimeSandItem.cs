using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeSandNew")]
	public class CuteSlimeSandItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeSandProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeSandBuff>();
	}
}
