using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class MetroidPetBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<MetroidPetProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().MetroidPet;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class MetroidPetBuff_AoMM : SimplePetBuffBase_AoMM<MetroidPetBuff>
	{

	}
}
