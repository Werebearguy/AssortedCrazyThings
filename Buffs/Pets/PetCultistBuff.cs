using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetCultistBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetCultistProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetCultist;

		public override void SafeSetStaticDefaults()
		{
			Main.vanityPet[Type] = false;
			Main.lightPet[Type] = true;
		}
	}
}
