using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SkeletronHandItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<SkeletronHandProj>();

		public override int BuffType => ModContent.BuffType<SkeletronHandBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletron's Spare Hand");
			Tooltip.SetDefault("Summons Skeletron's Hand attached to you");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
