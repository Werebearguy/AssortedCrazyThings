using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetGolemHeadItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetGolemHeadProj>();

		public override int BuffType => ModContent.BuffType<PetGolemHeadBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Replica Golem Head");
			Tooltip.SetDefault("Summons a Replica Golem Head to watch over you"
				+ "\nShoots bouncing fireballs at nearby enemies");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
