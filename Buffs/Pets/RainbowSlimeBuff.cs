using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class RainbowSlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<RainbowSlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().RainbowSlime;
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class RainbowSlimeBuff_AoMM : SimplePetBuffBase_AoMM<RainbowSlimeBuff>
	{

	}
}
