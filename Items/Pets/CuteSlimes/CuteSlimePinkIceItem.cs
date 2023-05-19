using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	public class CuteSlimePinkIceItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimePinkIceProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimePinkIceBuff>();
	}
}
