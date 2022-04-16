using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.HostileNPCs)]
	[LegacyName("StingSlimeBlackItem", "StingSlimeOrangeItem")]
	public class StingSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<StingSlimeProj>();

		public override int BuffType => ModContent.BuffType<StingSlimeBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Sting Slime");
			Tooltip.SetDefault("Summons a friendly black Sting Slime to follow you"
				+ "\nAppearance can be changed with Costume Suitcase");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
