using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class YoungHarpyBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<YoungHarpyProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().YoungHarpy;
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class YoungHarpyBuff_AoMM : SimplePetBuffBase_AoMM<YoungHarpyBuff>
	{

	}
}
