using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetDreadnautilusBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetDreadnautilusProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetDreadnautilus;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PetDreadnautilusBuff_AoMM : SimplePetBuffBase_AoMM<PetDreadnautilusBuff>
	{

	}
}
