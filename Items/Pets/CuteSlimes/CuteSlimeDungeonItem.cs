using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeDungeonNew")]
	public class CuteSlimeDungeonItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeDungeonProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeDungeonBuff>();
	}
}
