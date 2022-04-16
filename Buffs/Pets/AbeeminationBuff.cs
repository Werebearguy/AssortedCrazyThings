using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class AbeeminationBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<AbeeminationProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().Abeemination;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Abeemination");
			Description.SetDefault("An Abeemination is following you");
		}
	}
}
