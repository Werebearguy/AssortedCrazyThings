using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class AnimatedTomeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<AnimatedTomeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().AnimatedTome;
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class AnimatedTomeBuff_AoMM : SimplePetBuffBase_AoMM<AnimatedTomeBuff>
	{

	}
}
