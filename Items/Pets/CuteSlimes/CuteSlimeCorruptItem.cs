using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeCorruptNew")]
	public class CuteSlimeCorruptItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeCorruptProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeCorruptBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Corrupt Slime");
			Tooltip.SetDefault("Summons a friendly Cute Corrupt Slime to follow you");
		}
	}
}
