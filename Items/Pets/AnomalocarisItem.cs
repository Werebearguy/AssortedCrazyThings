using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class AnomalocarisItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<AnomalocarisProj>();

		public override int BuffType => ModContent.BuffType<AnomalocarisBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Ornery Shrimp");
			Tooltip.SetDefault("Summons a prehistoric shrimp to follow you"
				+ "\nAppearance can be changed with Costume Suitcase");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;

			Item.value = Item.sellPrice(gold: 3); //Zephyr fish price
		}
	}
}
