using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class DrumstickElementalBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<DrumstickElementalProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().DrumstickElemental;
	}

	public class DrumstickElementalBuff_AoMM : SimplePetBuffBase_AoMM<DrumstickElementalBuff>
	{

	}
}
