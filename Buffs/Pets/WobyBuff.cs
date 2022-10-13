using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class WobyBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<WobyProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().Woby;
	}
}
