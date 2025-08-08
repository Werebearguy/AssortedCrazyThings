using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class ShortfuseCrabItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<ShortfuseCrabProj>();

		public override int BuffType => ModContent.BuffType<ShortfuseCrabBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;

			Item.value = Item.sellPrice(copper: 10);
		}
	}

	public class ShortfuseCrabItem_AoMM : SimplePetItemBase_AoMM<ShortfuseCrabItem>
	{
		public override int BuffType => ModContent.BuffType<ShortfuseCrabBuff_AoMM>();
	}
}
