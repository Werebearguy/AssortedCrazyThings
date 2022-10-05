using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class FairySlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<FairySlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().FairySlime;
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class FairySlimeBuff_AoMM : SimplePetBuffBase_AoMM<FairySlimeBuff>
	{

	}
}
