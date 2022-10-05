using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetFishronBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetFishronProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetFishron;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PetFishronBuff_AoMM : SimplePetBuffBase_AoMM<PetFishronBuff>
	{

	}
}
