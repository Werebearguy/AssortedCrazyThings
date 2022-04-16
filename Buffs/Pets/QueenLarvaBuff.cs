using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class QueenLarvaBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<QueenLarvaProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().QueenLarva;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Queen Larva");
			Description.SetDefault("A Queen Bee Larva is following you");
		}
	}
}
