using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class FailureSlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<FailureSlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().FailureSlime;
	}

	public class FailureSlimeBuff_AoMM : SimplePetBuffBase_AoMM<FailureSlimeBuff>
	{

	}
}
