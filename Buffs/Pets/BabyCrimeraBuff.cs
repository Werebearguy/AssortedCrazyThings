using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class BabyCrimeraBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<BabyCrimeraProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().BabyCrimera;
	}

	public class BabyCrimeraBuff_AoMM : SimplePetBuffBase_AoMM<BabyCrimeraBuff>
	{

	}
}
