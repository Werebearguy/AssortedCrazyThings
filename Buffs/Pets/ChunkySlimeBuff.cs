using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.HostileNPCs)]
	public class ChunkySlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<ChunkySlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().ChunkySlime;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Chunky");
			Description.SetDefault("Chunky is following you");
		}
	}
}
