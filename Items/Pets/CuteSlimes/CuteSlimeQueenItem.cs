using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[Content(ContentType.CuteSlimes)]
	public class CuteSlimeQueenItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeQueenProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeQueenBuff>();
	}
}
