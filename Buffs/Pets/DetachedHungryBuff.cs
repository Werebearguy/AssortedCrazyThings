using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class DetachedHungryBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<DetachedHungryProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().DetachedHungry;
	}

	public class DetachedHungryBuff_AoMM : SimplePetBuffBase_AoMM<DetachedHungryBuff>
	{

	}
}
