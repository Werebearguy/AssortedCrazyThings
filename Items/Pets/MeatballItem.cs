using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.HostileNPCs)]
	public class MeatballItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<MeatballSlimeProj>();

		public override int BuffType => ModContent.BuffType<MeatballSlimeBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Meatball");
			Tooltip.SetDefault("Summons Meatball to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
