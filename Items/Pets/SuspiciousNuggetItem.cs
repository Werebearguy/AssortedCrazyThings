using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class SuspiciousNuggetItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<SuspiciousNuggetProj>();

		public override int BuffType => ModContent.BuffType<SuspiciousNuggetBuff>();

		public override void SafeSetDefaults()
		{
			Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(50)); //10 times more expensive then cube
		}
	}

	public class SuspiciousNuggetItem_AoMM : SimplePetItemBase_AoMM<SuspiciousNuggetItem>
	{
		public override int BuffType => ModContent.BuffType<SuspiciousNuggetBuff_AoMM>();
	}
}
