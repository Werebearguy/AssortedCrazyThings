using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class AlienHornetBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<AlienHornetProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().AlienHornet;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Alien Hornet");
			Description.SetDefault("An Alien Hornet is following you");
		}
	}
}
