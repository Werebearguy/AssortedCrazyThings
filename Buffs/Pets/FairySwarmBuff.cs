using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class FairySwarmBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<FairySwarmProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().FairySwarm;
	}

	public class FairySwarmBuff_AoMM : SimplePetBuffBase_AoMM<FairySwarmBuff>
	{

	}
}
