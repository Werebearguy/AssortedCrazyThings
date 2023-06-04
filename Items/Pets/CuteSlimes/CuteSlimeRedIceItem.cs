using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	public class CuteSlimeRedIceItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeRedIceProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeRedIceBuff>();
	}
}
