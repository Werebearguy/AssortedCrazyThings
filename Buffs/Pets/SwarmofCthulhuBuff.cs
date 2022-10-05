using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SwarmofCthulhuBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<SwarmofCthulhuProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().SwarmofCthulhu;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class SwarmofCthulhuBuff_AoMM : SimplePetBuffBase_AoMM<SwarmofCthulhuBuff>
	{

	}
}
