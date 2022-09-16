using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetFishronBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetFishronProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetFishron;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Fishron");
			Description.SetDefault("A Fishron is following you");
		}
	}
}
