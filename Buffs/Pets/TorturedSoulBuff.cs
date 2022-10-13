using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class TorturedSoulBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<TorturedSoulProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().TorturedSoul;
	}

	public class TorturedSoulBuff_AoMM : SimplePetBuffBase_AoMM<TorturedSoulBuff>
	{

	}
}
