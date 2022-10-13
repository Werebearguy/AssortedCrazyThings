using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class DemonHeartBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<DemonHeartProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().DemonHeart;
	}

	public class DemonHeartBuff_AoMM : SimplePetBuffBase_AoMM<DemonHeartBuff>
	{

	}
}
