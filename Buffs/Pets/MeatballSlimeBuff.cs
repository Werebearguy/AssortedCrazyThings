using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.HostileNPCs)]
	public class MeatballSlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<MeatballSlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().MeatballSlime;
	}

	[Content(ContentType.AommSupport | ContentType.HostileNPCs)]
	public class MeatballSlimeBuff_AoMM : SimplePetBuffBase_AoMM<MeatballSlimeBuff>
	{

	}
}
