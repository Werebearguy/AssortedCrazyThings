using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.CuteSlimes)]
	[LegacyName("CuteGastropod")]
	public class CuteGastropodItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<CuteGastropodProj>();

		public override int BuffType => ModContent.BuffType<CuteGastropodBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Mysterious Pod");
			Tooltip.SetDefault("Summons a friendly Cute Gastropod to follow you");
		}

		public override void SafeSetDefaults()
		{

		}
	}
}
