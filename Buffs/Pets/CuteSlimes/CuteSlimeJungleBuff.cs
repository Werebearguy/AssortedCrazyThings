using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
	public class CuteSlimeJungleBuff : CuteSlimeBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeJungleProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeJungle;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Cute Jungle Slime");
			Description.SetDefault("A cute jungle slime is following you");
		}
	}
}
