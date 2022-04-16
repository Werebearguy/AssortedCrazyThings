using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
	public class CuteSlimeIlluminantBuff : CuteSlimeBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeIlluminantProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeIlluminant;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Cute Illumimant Slime");
			Description.SetDefault("A cute illumimant slime is following you");
		}
	}
}
