using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class LifelikeMechanicalFrogBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<LifelikeMechanicalFrogProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().LifelikeMechanicalFrog;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Lifelike Mechanical Frog");
			Description.SetDefault("Whatever happened to this frog at the anvil is a mystery");
		}
	}
}
