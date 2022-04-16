using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeRainbowNew")]
	public class CuteSlimeRainbowItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeRainbowProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeRainbowBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Rainbow Slime");
			Tooltip.SetDefault("Summons a friendly Cute Rainbow Slime to follow you");
		}
	}
}
