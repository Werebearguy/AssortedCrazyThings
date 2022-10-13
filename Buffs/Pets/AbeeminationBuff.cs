using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class AbeeminationBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<AbeeminationProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().Abeemination;
	}

	public class AbeeminationBuff_AoMM : SimplePetBuffBase_AoMM<AbeeminationBuff>
	{

	}
}
