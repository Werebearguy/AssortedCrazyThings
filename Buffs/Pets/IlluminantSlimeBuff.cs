using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class IlluminantSlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<IlluminantSlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().IlluminantSlime;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class IlluminantSlimeBuff_AoMM : SimplePetBuffBase_AoMM<IlluminantSlimeBuff>
	{

	}
}
