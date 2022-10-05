using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class CuteLamiaPetBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteLamiaPetProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteLamiaPet;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class CuteLamiaPetBuff_AoMM : SimplePetBuffBase_AoMM<CuteLamiaPetBuff>
	{

	}
}
