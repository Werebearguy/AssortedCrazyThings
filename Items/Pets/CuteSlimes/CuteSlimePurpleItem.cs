using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimePurpleNew")]
	public class CuteSlimePurpleItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimePurpleProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimePurpleBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Purple Slime");
			Tooltip.SetDefault("Summons a friendly Cute Purple Slime to follow you");
		}
	}
}
