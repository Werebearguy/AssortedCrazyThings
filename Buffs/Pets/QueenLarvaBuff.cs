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
	}

	public class QueenLarvaBuff_AoMM : SimplePetBuffBase_AoMM<QueenLarvaBuff>
	{

	}
}
