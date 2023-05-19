using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	public class CuteSlimePurpleIceItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimePurpleIceProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimePurpleIceBuff>();
	}
}
