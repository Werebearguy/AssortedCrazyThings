using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class RainbowSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<RainbowSlimeProj>();

		public override int BuffType => ModContent.BuffType<RainbowSlimeBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Rainbow Slime");
			Tooltip.SetDefault("Summons a friendly Rainbow Slime to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
