using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class CuteLamiaPetBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteLamiaPetProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteLamiaPet;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Small Snake");
			Description.SetDefault("A small snake is following you");
		}
	}
}
