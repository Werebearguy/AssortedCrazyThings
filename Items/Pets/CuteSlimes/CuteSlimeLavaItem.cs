using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeLavaNew")]
	public class CuteSlimeLavaItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeLavaProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeLavaBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Lava Slime");
			Tooltip.SetDefault("Summons a friendly Cute Lava Slime to follow you");
		}
	}
}
