using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class StrangeRobotItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<StrangeRobotProj>();

		public override int BuffType => ModContent.BuffType<StrangeRobotBuff>();

		public override void SafeSetDefaults()
		{
			Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(1)); //Same as other travelling merchant pets
		}
	}

	public class StrangeRobotItem_AoMM : SimplePetItemBase_AoMM<StrangeRobotItem>
	{
		public override int BuffType => ModContent.BuffType<StrangeRobotBuff_AoMM>();
	}
}
