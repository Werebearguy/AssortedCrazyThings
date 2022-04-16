using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SwarmofCthulhuItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<SwarmofCthulhuProj>();

		public override int BuffType => ModContent.BuffType<SwarmofCthulhuBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottle of Restless Eyes");
			Tooltip.SetDefault("Summons several eyes to swarm around you");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
