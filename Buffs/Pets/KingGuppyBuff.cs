using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class KingGuppyBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<KingGuppyProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().KingGuppy;
	}

	public class KingGuppyBuff_AoMM : SimplePetBuffBase_AoMM<KingGuppyBuff>
	{

	}
}
