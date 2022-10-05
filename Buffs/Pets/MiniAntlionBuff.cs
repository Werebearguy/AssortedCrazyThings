using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class MiniAntlionBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<MiniAntlionProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().MiniAntlion;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class MiniAntlionBuff_AoMM : SimplePetBuffBase_AoMM<MiniAntlionBuff>
	{

	}
}
