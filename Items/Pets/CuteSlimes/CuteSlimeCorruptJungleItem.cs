using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	public class CuteSlimeCorruptJungleItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeCorruptJungleProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeCorruptJungleBuff>();
	}
}
