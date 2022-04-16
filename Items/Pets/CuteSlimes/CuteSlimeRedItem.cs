using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimeRedNew")]
	public class CuteSlimeRedItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeRedProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimeRedBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Red Slime");
			Tooltip.SetDefault("Summons a friendly Cute Red Slime to follow you");
		}
	}
}
