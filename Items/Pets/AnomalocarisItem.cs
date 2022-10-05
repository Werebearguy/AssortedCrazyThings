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

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;

			Item.value = Item.sellPrice(gold: 3) / 2; //Zephyr fish price halved because we double the catch rate of it
		}
	}

	public class AnomalocarisItem_AoMM : SimplePetItemBase_AoMM<AnomalocarisItem>
	{
		public override int BuffType => ModContent.BuffType<AnomalocarisBuff_AoMM>();
	}
}
