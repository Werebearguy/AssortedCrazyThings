using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeCrimsonNew")]
	public class CuteSlimeCrimsonItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeCrimsonProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeCrimsonBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Crimson Slime");
			Tooltip.SetDefault("Summons a friendly Cute Crimson Slime to follow you");
		}
	}
}
