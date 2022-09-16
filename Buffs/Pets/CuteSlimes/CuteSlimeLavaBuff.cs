using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
	public class CuteSlimeLavaBuff : CuteSlimeBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeLavaProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeLava;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Cute Lava Slime");
			Description.SetDefault("A cute lava slime is following you");
		}
	}
}
