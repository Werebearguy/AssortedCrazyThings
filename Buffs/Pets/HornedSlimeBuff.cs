using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class HornedSlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<HornedSlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().HornedSlime;
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class HornedSlimeBuff_AoMM : SimplePetBuffBase_AoMM<HornedSlimeBuff>
	{

	}
}
