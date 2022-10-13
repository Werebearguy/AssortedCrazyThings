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

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			Item.value = Item.sellPrice(copper: 20);
		}
	}
}
