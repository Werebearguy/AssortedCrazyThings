using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class MiniAntlionBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<MiniAntlionProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().MiniAntlion;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Baby Antlion");
			Description.SetDefault("A Baby Antlion is following you");
		}
	}
}
