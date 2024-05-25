using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class OceanSlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<OceanSlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().OceanSlime;
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class OceanSlimeBuff_AoMM : SimplePetBuffBase_AoMM<OceanSlimeBuff>
	{

	}
}
