using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class ShortfuseCrabBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<ShortfuseCrabProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().ShortfuseCrab;
	}

	public class ShortfuseCrabBuff_AoMM : SimplePetBuffBase_AoMM<ShortfuseCrabBuff>
	{

	}
}
