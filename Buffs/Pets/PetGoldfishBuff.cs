using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class PetGoldfishBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetGoldfishProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetGoldfish;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Possessed Goldfish");
			Description.SetDefault("A possessed goldfish is following you");
		}
	}
}
