using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class LifelikeMechanicalFrogBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<LifelikeMechanicalFrogProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().LifelikeMechanicalFrog;
	}

	public class LifelikeMechanicalFrogBuff_AoMM : SimplePetBuffBase_AoMM<LifelikeMechanicalFrogBuff>
	{

	}
}
