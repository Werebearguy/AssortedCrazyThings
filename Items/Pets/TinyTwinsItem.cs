using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class TinyTwinsItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<TinySpazmatismProj>();

		public override int BuffType => ModContent.BuffType<TinyTwinsBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Twins");
			Tooltip.SetDefault("Summons a tiny pair of The Twins to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
