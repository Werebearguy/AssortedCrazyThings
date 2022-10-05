using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class StrangeRobotBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<StrangeRobotProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().StrangeRobot;
	}

	public class StrangeRobotBuff_AoMM : SimplePetBuffBase_AoMM<StrangeRobotBuff>
	{

	}
}
