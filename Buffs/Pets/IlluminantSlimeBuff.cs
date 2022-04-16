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

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Illuminant Slime");
			Description.SetDefault("An Illuminant Slime is following you");
		}
	}
}
