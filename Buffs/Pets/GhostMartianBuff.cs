using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class GhostMartianBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<GhostMartianProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().GhostMartian;
	}

	public class GhostMartianBuff_AoMM : SimplePetBuffBase_AoMM<GhostMartianBuff>
	{

	}
}
