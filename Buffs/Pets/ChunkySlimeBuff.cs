using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class ChunkySlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<ChunkySlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().ChunkySlime;
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class ChunkySlimeBuff_AoMM : SimplePetBuffBase_AoMM<ChunkySlimeBuff>
	{

	}
}
