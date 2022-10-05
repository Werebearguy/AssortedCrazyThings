using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("BrainofConfusion")]
	public class BrainofConfusionItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<BrainofConfusionProj>();

		public override int BuffType => ModContent.BuffType<BrainofConfusionBuff>();

		public override void SafeSetDefaults()
		{

		}
	}

	public class BrainofConfusionItem_AoMM : SimplePetItemBase_AoMM<BrainofConfusionItem>
	{
		public override int BuffType => ModContent.BuffType<BrainofConfusionBuff_AoMM>();
	}
}
