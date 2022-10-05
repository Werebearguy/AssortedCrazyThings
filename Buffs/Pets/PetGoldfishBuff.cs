using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class PetGoldfishBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetGoldfishProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetGoldfish;
	}

	public class PetGoldfishBuff_AoMM : SimplePetBuffBase_AoMM<PetGoldfishBuff>
	{

	}
}
