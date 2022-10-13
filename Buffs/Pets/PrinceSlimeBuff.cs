using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PrinceSlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PrinceSlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PrinceSlime;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PrinceSlimeBuff_AoMM : SimplePetBuffBase_AoMM<PrinceSlimeBuff>
	{

	}
}
