using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.HostileNPCs | ContentType.DroppedPets)]
	public class MiniMegalodonBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<MiniMegalodonProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().MiniMegalodon;
	}

	[Content(ContentType.AommSupport | ContentType.HostileNPCs | ContentType.DroppedPets)]
	public class MiniMegalodonBuff_AoMM : SimplePetBuffBase_AoMM<MiniMegalodonBuff>
	{

	}
}
