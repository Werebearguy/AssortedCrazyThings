using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.HostileNPCs | ContentType.DroppedPets)]
	public class BabyOcramBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<BabyOcramProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().BabyOcram;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Baby Ocram");
			Description.SetDefault("What could have been now follows you");
		}
	}
}
