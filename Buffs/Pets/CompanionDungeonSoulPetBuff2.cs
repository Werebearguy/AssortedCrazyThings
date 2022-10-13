using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.Bosses)]
	public class CompanionDungeonSoulPetBuff2 : SimplePetBuffBase
	{
		public override string Texture => "AssortedCrazyThings/Buffs/Pets/CompanionDungeonSoulPetBuff";

		public override int PetType => ModContent.ProjectileType<CompanionDungeonSoulPetProj2>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().SoulLightPet2;
	}

	[Content(ContentType.AommSupport | ContentType.Bosses)]
	public class CompanionDungeonSoulPetBuff2_AoMM : SimplePetBuffBase_AoMM<CompanionDungeonSoulPetBuff2>
	{

	}
}
