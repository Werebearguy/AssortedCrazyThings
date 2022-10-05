using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class VampireBatBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<VampireBatProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().VampireBat;
	}

	public class VampireBatBuff_AoMM : SimplePetBuffBase_AoMM<VampireBatBuff>
	{

	}
}
