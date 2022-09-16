using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
	public class CuteSlimeGreenBuff : CuteSlimeBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeGreenProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeGreen;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Cute Green Slime");
			Description.SetDefault("A cute green slime is following you");
		}
	}
}
