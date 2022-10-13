using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class AnomalocarisBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<AnomalocarisProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetAnomalocaris;
	}

	public class AnomalocarisBuff_AoMM : SimplePetBuffBase_AoMM<AnomalocarisBuff>
	{

	}
}
