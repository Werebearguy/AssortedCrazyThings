using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	public class CuteSlimeHoneyItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeHoneyProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeHoneyBuff>();
	}
}
