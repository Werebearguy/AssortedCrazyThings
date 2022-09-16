using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class PigronataBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PigronataProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().Pigronata;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Pigronata");
			Description.SetDefault("A Pigronata is thankful that you did not bust it");
		}
	}
}
