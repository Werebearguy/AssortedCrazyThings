using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	public class CuteSlimeCrimsonJungleItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeCrimsonJungleProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeCrimsonJungleBuff>();
	}
}
