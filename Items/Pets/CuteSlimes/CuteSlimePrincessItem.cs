using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[LegacyName("CuteSlimePrincessNew")]
	public class CuteSlimePrincessItem : CuteSlimeItem
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimePrincessProj>();

		public override int BuffType => ModContent.BuffType<CuteSlimePrincessBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Cute Princess Slime");
			Tooltip.SetDefault("Summons a friendly Cute Princess Slime to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 20);
		}
	}
}
