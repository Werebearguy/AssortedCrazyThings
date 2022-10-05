using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class TrueObservingEyeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<TrueObservingEyeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().TrueObservingEye;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class TrueObservingEyeBuff_AoMM : SimplePetBuffBase_AoMM<TrueObservingEyeBuff>
	{

	}
}
