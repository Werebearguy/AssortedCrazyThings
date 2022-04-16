using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeToxicNew")]
	public class CuteSlimeToxicItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeToxicProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeToxicBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Toxic Slime");
			Tooltip.SetDefault("Summons a friendly Cute Toxic Slime to follow you");
		}
	}
}
