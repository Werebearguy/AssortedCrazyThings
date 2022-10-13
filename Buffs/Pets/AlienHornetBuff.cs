using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class AlienHornetBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<AlienHornetProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().AlienHornet;
	}

	public class AlienHornetBuff_AoMM : SimplePetBuffBase_AoMM<AlienHornetBuff>
	{

	}
}
