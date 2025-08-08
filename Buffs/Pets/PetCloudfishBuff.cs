using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class PetCloudfishBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetCloudfishProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetCloudfish;
	}

	public class PetCloudfishBuff_AoMM : SimplePetBuffBase_AoMM<PetCloudfishBuff>
	{

	}
}
