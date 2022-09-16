using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class DrumstickElementalBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<DrumstickElementalProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().DrumstickElemental;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Drumstick Elemental");
			Description.SetDefault("Dinner is following you");
		}
	}
}
