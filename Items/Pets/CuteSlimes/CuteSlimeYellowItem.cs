using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeYellowNew")]
	public class CuteSlimeYellowItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeYellowProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeYellowBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Yellow Slime");
			Tooltip.SetDefault("Summons a friendly Cute Yellow Slime to follow you");
		}
	}
}
