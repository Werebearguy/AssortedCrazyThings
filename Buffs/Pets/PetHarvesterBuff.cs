using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.Bosses)]
	public class PetHarvesterBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetHarvesterProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetHarvester;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Stubborn Bird");
			Description.SetDefault("A stubborn bird is following you");
		}
	}
}
