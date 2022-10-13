using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.Bosses)]
	public class PetHarvesterBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetHarvesterProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetHarvester;
	}

	[Content(ContentType.AommSupport | ContentType.Bosses)]
	public class PetHarvesterBuff_AoMM : SimplePetBuffBase_AoMM<PetHarvesterBuff>
	{

	}
}
