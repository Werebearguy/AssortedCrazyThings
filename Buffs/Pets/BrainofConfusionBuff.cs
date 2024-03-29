using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class BrainofConfusionBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<BrainofConfusionProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().BrainofConfusion;
	}

	public class BrainofConfusionBuff_AoMM : SimplePetBuffBase_AoMM<BrainofConfusionBuff>
	{

	}
}
