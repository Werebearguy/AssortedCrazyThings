using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class DocileDemonEyeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<DocileDemonEyeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().DocileDemonEye;
	}

	public class DocileDemonEyeBuff_AoMM : SimplePetBuffBase_AoMM<DocileDemonEyeBuff>
	{

	}
}
