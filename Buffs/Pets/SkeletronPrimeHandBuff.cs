using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SkeletronPrimeHandBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<SkeletronPrimeHandProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().SkeletronPrimeHand;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class SkeletronPrimeHandBuff_AoMM : SimplePetBuffBase_AoMM<SkeletronPrimeHandBuff>
	{

	}
}
