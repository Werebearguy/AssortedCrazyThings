using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class YoungWyvernBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<YoungWyvernProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().YoungWyvern;
	}

	public class YoungWyvernBuff_AoMM : SimplePetBuffBase_AoMM<YoungWyvernBuff>
	{

	}
}
