using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SkeletronPrimeHandItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<SkeletronPrimeHandProj>();

		public override int BuffType => ModContent.BuffType<SkeletronPrimeHandBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletron Prime's Spare Hand");
			Tooltip.SetDefault("Summons Skeletron Prime's Hand attached to you");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
