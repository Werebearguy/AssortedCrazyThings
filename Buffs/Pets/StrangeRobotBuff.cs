using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class StrangeRobotBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<StrangeRobotProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().StrangeRobot;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Robot");
			Description.SetDefault("A strange robot is following you");
		}
	}
}
