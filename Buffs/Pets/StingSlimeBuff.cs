using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class StingSlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<StingSlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().StingSlime;
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class StingSlimeBuff_AoMM : SimplePetBuffBase_AoMM<StingSlimeBuff>
	{

	}
}
