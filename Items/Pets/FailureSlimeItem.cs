using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class FailureSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<FailureSlimeProj>();

		public override int BuffType => ModContent.BuffType<FailureSlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(silver: 40); //TODO value
		}

		//TODO obtain (grab bag?)
	}

	public class FailureSlimeItem_AoMM : SimplePetItemBase_AoMM<FailureSlimeItem>
	{
		public override int BuffType => ModContent.BuffType<FailureSlimeBuff_AoMM>();
	}
}
