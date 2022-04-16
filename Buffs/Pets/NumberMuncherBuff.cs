using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class NumberMuncherBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<NumberMuncherProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().NumberMuncher;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Number Muncher");
			Description.SetDefault("An educated amphibian is following you");
		}
	}
}
