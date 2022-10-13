using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class SuspiciousNuggetBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<SuspiciousNuggetProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().SuspiciousNugget;
	}

	public class SuspiciousNuggetBuff_AoMM : SimplePetBuffBase_AoMM<SuspiciousNuggetBuff>
	{

	}
}
