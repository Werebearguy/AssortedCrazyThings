using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class PigronataBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PigronataProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().Pigronata;
	}

	public class PigronataBuff_AoMM : SimplePetBuffBase_AoMM<PigronataBuff>
	{

	}
}
