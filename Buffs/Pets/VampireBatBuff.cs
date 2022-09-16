using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class VampireBatBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<VampireBatProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().VampireBat;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Vampire Bat");
			Description.SetDefault("A particularly dashing vampire is following you");
		}
	}
}
