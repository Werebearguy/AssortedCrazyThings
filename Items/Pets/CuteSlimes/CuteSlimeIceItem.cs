using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeIceNew")]
	public class CuteSlimeIceItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeIceProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeIceBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Ice Slime");
			Tooltip.SetDefault("Summons a friendly Cute Ice Slime to follow you");
		}
	}
}
