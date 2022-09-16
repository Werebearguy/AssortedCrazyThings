using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.HostileNPCs)]
	public class AnimatedTomeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<AnimatedTomeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().AnimatedTome;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Animated Tome");
			Description.SetDefault("An animated tome is following you");
		}
	}
}
