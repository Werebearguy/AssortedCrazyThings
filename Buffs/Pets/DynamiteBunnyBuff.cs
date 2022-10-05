using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class DynamiteBunnyBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<DynamiteBunnyProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().DynamiteBunny;
	}

	public class DynamiteBunnyBuff_AoMM : SimplePetBuffBase_AoMM<DynamiteBunnyBuff>
	{

	}
}
