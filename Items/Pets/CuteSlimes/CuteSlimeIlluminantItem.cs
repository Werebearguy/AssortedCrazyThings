using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeIlluminantNew")]
	public class CuteSlimeIlluminantItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeIlluminantProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeIlluminantBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Illuminant Slime");
			Tooltip.SetDefault("Summons a friendly Cute Illuminant Slime to follow you");
		}
	}
}
