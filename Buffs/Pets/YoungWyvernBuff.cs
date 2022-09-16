using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class YoungWyvernBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<YoungWyvernProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().YoungWyvern;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Young Wyvern");
			Description.SetDefault("A young Wyvern is following you");
		}
	}
}
